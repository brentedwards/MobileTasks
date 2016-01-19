using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Security.Claims;
using MobileTasks.Server.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace MobileTasks.Server.Api.Controllers
{
    [Authorize]
    [MobileAppController]
    public class TaskControllerController : ApiController
    {
        // GET api/TaskController
        public IEnumerable<Task> Get()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;

            var context = new MobileTasksEntities();

            return context.Tasks.Where(_ => _.Sid == sid);
        }
    }
}
