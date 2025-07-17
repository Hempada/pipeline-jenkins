using Commons.Data.Results;
using WebApiTests.Extensions;
using Xunit;

namespace WebApiTests.Tests
{
    public class ListResultTests
    {
        [Fact]
        public void HasErrors_ReturnsFalse_WhenNoErrors()
        {
            var result = new ListResult<string>(new List<string> { "Data" });
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void HasErrors_ReturnsTrue_WhenErrorsPresent()
        {
            var errors = new List<Error> { new Error("Test Error", "ErrorCode") };
            var result = new ListResult<string>(new List<string> { "Data" }, errors);
            Assert.True(result.HasErrors());
        }

        [Fact]
        public void ImplicitOperator_Result_ReturnsFail_WhenDataIsNull()
        {
            var errors = new List<Error> { new Error("Test Error", "ErrorCode") };
            var listResult = new ListResult<string>(errors);
            Result result = listResult;


            Assert.False(result.IsSuccess());
            Assert.Equal(errors, result.Errors);
        }

        [Fact]
        public void ImplicitOperator_Result_ReturnsOk_WhenDataIsNotNull()
        {
            var data = new List<string> { "Data" };
            var listResult = new ListResult<string>(data);

            // Cria explicitamente um Result com listResult
            Result<ListResult<string>> result = Result.Ok(listResult);


            Assert.True(result.IsSuccess());

            Assert.Equal(data, result.Value()?.Data);
        }


        [Fact]
        public void ImplicitOperator_ListResult_ReturnsListResultWithErrors()
        {
            var errors = new List<Error> { new Error("Test Error", "ErrorCode") };
            var result = Result.Fail(errors);
            ListResult<string> listResult = result;
            Assert.True(listResult.HasErrors());
            Assert.Equal(errors, listResult.Errors);
        }
    }
}
