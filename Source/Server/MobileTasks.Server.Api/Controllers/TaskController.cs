using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Security.Claims;
using MobileTasks.Server.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;

namespace MobileTasks.Server.Api.Controllers
{
    [Authorize]
    [MobileAppController]
    public class TaskController : ApiController
    {
        public string Sid
        {
            get
            {
                var claimsPrincipal = this.User as ClaimsPrincipal;
                return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

        // GET api/Task
        public IEnumerable<Task> Get()
        {
            var context = new MobileTasksEntities();

            return context.Tasks.Where(_ => _.Sid == this.Sid);
        }

        public Task Get(int id)
        {
            var context = new MobileTasksEntities();

            var task = context.Tasks.SingleOrDefault(_ => _.Id == id);
            if (task != null && task.Sid != this.Sid)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return task;
        }

        public Task Post([FromBody]Task task)
        {
            var context = new MobileTasksEntities();

            var resultTask = context.Tasks.SingleOrDefault(_ => _.Id == task.Id);
            if (resultTask != null)
            {
                if (resultTask.Sid != this.Sid)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                resultTask.Description = task.Description = task.Description;
                resultTask.DateDue = task.DateDue;
                resultTask.DateCompleted = task.DateCompleted;
                resultTask.IsCompleted = task.IsCompleted;
            }
            else
            {
				resultTask = task;

				resultTask.Sid = this.Sid;
				resultTask.DateCreated = DateTime.UtcNow;

                context.Tasks.Add(resultTask);
            }

            context.SaveChanges();
            return resultTask;
        }
    }
}
