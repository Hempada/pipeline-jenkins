using Business.Services;
using Commons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        [HttpPost]
        [RequiresPermission(Permission.CREATE_ACCOUNT)]
        public async Task<IActionResult> Save(
            [FromServices] IAccountService service,
            [FromBody] SaveAccount model,
            CancellationToken token = default)
        {
            var result = await service.SaveAsync(model.Id, model.Name, model.Username, model.Email, model.ProfileId, token);

            if (result.Valid)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        [RequiresPermission(Permission.DELETE_ACCOUNT)]
        public async Task<IActionResult> Delete(
            [FromServices] IAccountService service,
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
