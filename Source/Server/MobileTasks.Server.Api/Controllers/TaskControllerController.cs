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
    public class TaskControllerController : ApiController
    {
        public string Sid
        {
            get
            {
                var claimsPrincipal = this.User as ClaimsPrincipal;
                return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }

        // GET api/TaskController
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

        public bool Post([FromBody]Task task)
        {
            var existed = false;
            var context = new MobileTasksEntities();

            var existingTask = context.Tasks.SingleOrDefault(_ => _.Id == task.Id);
            if (existingTask != null)
            {
                if (existingTask.Sid != this.Sid)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                existingTask.Description = task.Description = task.Description;
                existingTask.DateDue = task.DateDue;
                existingTask.DateCompleted = task.DateCompleted;
                existingTask.IsCompleted = task.IsCompleted;

                existed = true;
            }
            else
            {
                task.Sid = this.Sid;
                task.DateCreated = DateTime.UtcNow;

                context.Tasks.Add(task);
            }

            context.SaveChanges();
            return existed;
        }
    }
}
