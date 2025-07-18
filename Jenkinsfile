pipeline {
    agent any

    environment {
        SONARQUBE_SERVER = 'Sonar'
        DOTNET_SONAR_SCANNER = '/var/lib/jenkins/.dotnet/tools/dotnet-sonarscanner'
        DOCKER_COMPOSE_FILE = 'Container/docker-compose.yml'
        PATH = "${PATH}:/var/lib/jenkins/.dotnet/tools"
    }

    stages {
        stage('Checkout') {
            steps {
                git credentialsId: 'pcampos2', url: 'https://github.com/Hempada/pipeline-jenkins.git', branch: 'ApiTest'
            }
        }

        stage('Start Docker Containers') {
            steps {
                sh """
                docker-compose -f ${DOCKER_COMPOSE_FILE} down || true
                timeout 60s docker-compose -f ${DOCKER_COMPOSE_FILE} up -d
                """
            }
        }

        stage('Clean & Build') {
            steps {
                sh 'rm -rf WebApi/bin WebApi/obj'
                sh 'dotnet build WebApi/WebApi.csproj --no-incremental'
            }
        }

        stage('Run Tests & Collect Coverage') {
            steps {
                sh """
                mkdir -p TestResults
                dotnet test WebApi/WebApi.csproj --collect:"XPlat Code Coverage" --logger:trx --results-directory=TestResults
                """
            }
        }

        stage('Validate Coverage File') {
            steps {
                script {
                    def coverageFile = sh(script: "find ${WORKSPACE} -name 'coverage.opencover.xml' | head -n 1", returnStdout: true).trim()
                    if (coverageFile && coverageFile != "${WORKSPACE}/TestResults/coverage.opencover.xml") {
                        sh "mv ${coverageFile} ${WORKSPACE}/TestResults/coverage.opencover.xml"
                    } else if (!coverageFile) {
                        error "Arquivo de cobertura não encontrado!"
                    }
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                withCredentials([string(credentialsId: 'Token-Sonar', variable: 'SONARQUBE_TOKEN')]) {
                    withSonarQubeEnv(SONARQUBE_SERVER) {
                        sh """
                        ${DOTNET_SONAR_SCANNER} begin /k:"TMS" \
                        /d:sonar.login=${SONARQUBE_TOKEN} \
                        /d:sonar.host.url="http://localhost:9000" \
                        /d:sonar.cs.opencover.reportsPaths="TestResults/coverage.opencover.xml" \
                        /d:sonar.verbose=true
                        dotnet build WebApi/WebApi.csproj --no-incremental
                        ${DOTNET_SONAR_SCANNER} end /d:sonar.login=${SONARQUBE_TOKEN}
                        """
                    }
                }
            }
        }

        stage('Update SonarQube Dashboard') {
            steps {
                withCredentials([string(credentialsId: 'Token-Sonar', variable: 'SONARQUBE_TOKEN')]) {
                    sh "curl -s -u ${SONARQUBE_TOKEN}: 'http://localhost:9000/api/measures/component?component=TMS&metricKeys=coverage,lines,duplicated_lines_density'"
                }
            }
        }
    }

    post {
        always { echo 'Pipeline finalizada.' }
        success { echo 'Pipeline executada com sucesso!' }
        failure { echo 'Falha na execução da pipeline.' }
    }
}
