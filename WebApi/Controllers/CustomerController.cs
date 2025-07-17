using Business.Services;
using Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;

namespace WebApi.Controllers
{
    [Route("api/customer")]
    [Authorize]
    [ApiController]
    public class CustomerController : ApiControllerBase
    {
        [HttpPost]
        [RequiresPermission(Permission.CREATE_CUSTOMER)]
        public async Task<IActionResult> Save(
            [FromServices] ICustomerService service,
            [FromBody] SaveCustomer model,
            CancellationToken token = default)
        {
            var result = await service.SaveAsync(model.Id, model.Name, model.Email, token);

            if (result.Valid)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        [RequiresPermission(Permission.DELETE_CUSTOMER)]
        public async Task<IActionResult> Delete(
            [FromServices] ICustomerService service,
            [FromRoute] Guid id,
            CancellationToken token = default)
        {
            var result = await service.DeleteAsync(id, token);

            if (result.Valid)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
    }
}
