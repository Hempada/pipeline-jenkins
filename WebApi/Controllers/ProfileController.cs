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
    public class ProfileController : ApiControllerBase
    {
        [HttpPost]
        [RequiresPermission(Permission.CREATE_PROFILE)]
        public async Task<IActionResult> Save(
            [FromServices] IProfileService service,
            [FromBody] SaveProfile model,
            CancellationToken token = default)
        {
            var result = await service.SaveAsync(model.Id, model.Name, model.Permissions, token);

            if (result.Valid)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        [RequiresPermission(Permission.DELETE_PROFILE)]
        public async Task<IActionResult> Delete(
            [FromServices] IProfileService service,
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
