using Business;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using WebApi.Authorization;

namespace WebApi.Controllers
{
    [Route("odata")]
    [Authorize]
    [ApiController]
    public class ODataController : Microsoft.AspNetCore.OData.Routing.Controllers.ODataController
    {
        [EnableQuery]
        [RequiresPermission(Commons.Models.Permission.VIEW_ACCOUNT)]
        [HttpGet("accounts")]
        public ActionResult<IEnumerable<Account>> GetAccounts(
            [FromServices] ApplicationDbContext database)
        {
            return Ok(database.Accounts
                .IgnoreAutoIncludes()
                .AsNoTrackingWithIdentityResolution());
        }

        [EnableQuery]
        [RequiresPermission(Commons.Models.Permission.VIEW_PROFILE)]
        [HttpGet("profiles")]
        public ActionResult<IEnumerable<Profile>> GetProfiles(
            [FromServices] ApplicationDbContext database)
        {
            return Ok(database.Profiles
                .IgnoreAutoIncludes()
                .AsNoTrackingWithIdentityResolution());
        }

        [EnableQuery]
        [RequiresPermission(Commons.Models.Permission.VIEW_CUSTOMER)]
        [HttpGet("customers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers(
            [FromServices] ApplicationDbContext database)
        {
            return Ok(database.Customers
                .IgnoreAutoIncludes()
                .AsNoTrackingWithIdentityResolution());
        }
    }
}
