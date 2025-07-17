using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected bool TryGetCurrentAccountId(out Guid id)
        {
            return Guid.TryParse(User?.FindFirst(ClaimTypes.Sid)?.Value, out id);
        }

        protected Guid GetCurrentAccountId()
        {
            if (TryGetCurrentAccountId(out Guid id))
            {
                return id;
            }

            throw new InvalidOperationException("Current user not found.");
        }
    }
}
