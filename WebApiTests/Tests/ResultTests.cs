using Commons.Data.Results;
using WebApiTests.Extensions;
using Xunit;

namespace WebApiTests.Tests

{
    public class ResultTests
    {
        [Fact]
        public void Constructor_WithData_ShouldSetDataCorrectly()
        {
            // Arrange
            var data = "test data";

            // Act
            var result = new Result<string>(data);

            // Assert
            Assert.Equal(data, result.Data);
            Assert.True(result.Valid);
        }

        [Fact]
        public void Constructor_WithDataAndErrors_ShouldSetDataAndErrorsCorrectly()
        {
            // Arrange
            var data = "test data";
            var errors = new List<Error> { new Error("E001", "Test error") };

            // Act
            var result = new Result<string>(data, errors);

            // Assert
            Assert.Equal(data, result.Data);
            Assert.False(result.Valid);
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void Constructor_WithErrors_ShouldSetErrorsCorrectly()
        {
            // Arrange
            var errors = new List<Error> { new Error("E001", "Test error") };

            // Act
            var result = new Result<string>(errors);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Valid);
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void Constructor_WithSingleError_ShouldSetErrorsCorrectly()
        {
            // Arrange
            var error = new Error("E001", "Single error");

            // Act
            var result = new Result<string>(new List<Error> { error });

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Valid);
            Assert.Single(result.Errors);
            Assert.Equal(error, result.Errors.First());
        }

        [Fact]
        public void ImplicitConversion_FromResultT_ToResult_ShouldWorkCorrectly()
        {
            // Arrange
            var data = "test data";
            var resultWithData = new Result<string>(data);
            var resultWithError = new Result<string>(new List<Error> { new Error("E002", "Implicit conversion error") });

            // Act
            Result implicitResultOk = resultWithData.ToResult();
            Result implicitResultFail = resultWithError.ToResult();

            // Assert
            Assert.True(implicitResultOk.Valid);
            Assert.False(implicitResultFail.Valid);
        }


        [Fact]
        public void ImplicitConversion_FromResult_ToResultT_ShouldWorkCorrectly()
        {
            // Arrange
            var errors = new List<Error> { new Error("E003", "Conversion error") };
            var result = new Result(errors);

            // Act
            Result<string> implicitResult = result;

            // Assert
            Assert.Null(implicitResult.Data);
            Assert.False(implicitResult.Valid);
            Assert.Equal(errors, implicitResult.Errors);
        }

        [Fact]
        public void HasErrors_WhenErrorsAreEmpty_ShouldReturnFalse()
        {
            // Arrange
            var result = new Result();

            // Act & Assert
            Assert.True(result.Valid);
        }

        [Fact]
        public void HasErrors_WhenErrorsExist_ShouldReturnTrue()
        {
            // Arrange
            var result = new Result(new List<Error> { new Error("E004", "Error exists") });

            // Act & Assert
            Assert.False(result.Valid);
        }
    }
}

