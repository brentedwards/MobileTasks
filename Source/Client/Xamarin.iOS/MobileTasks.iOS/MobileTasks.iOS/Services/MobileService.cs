using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.iOS.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;

namespace MobileTasks.iOS.Services
{
	public partial class MobileService
	{
		private const string TaskAction = "task";

		private static MobileService instance;
		public static MobileService Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MobileService();
				}

				return instance;
			}
		}

		private MobileServiceClient Client { get; set; }

		public MobileServiceUser User
		{
			get { return this.Client.CurrentUser; }
			set { this.Client.CurrentUser = value; }
		}

		public async Task LoginAsync(UIViewController controller, MobileServiceAuthenticationProvider provider)
		{
			await this.Client.LoginAsync(controller, provider, "commagenicmobiletasks");
		}

		public async Task<IEnumerable<MobileTask>> GetTasksAsync()
		{
			return await this.Client.InvokeApiAsync<IEnumerable<MobileTask>>(TaskAction, HttpMethod.Get, null);
		}

		public async Task<MobileTask> LoadTaskAsync(int id)
		{
			var parameters = new Dictionary<string, string>();
			parameters["id"] = id.ToString();

			return await this.Client.InvokeApiAsync<MobileTask>(TaskAction, HttpMethod.Get, parameters);
		}

		public async Task<MobileTask> UpsertTaskAsync(MobileTask task)
		{
			return await this.Client.InvokeApiAsync<MobileTask, MobileTask>("task", task);
		}

		public async Task DeleteTaskAsync(int id)
		{
			var parameters = new Dictionary<string, string>();
			parameters["id"] = id.ToString();

			await this.Client.InvokeApiAsync(TaskAction, HttpMethod.Delete, parameters);
		}

		public async Task LogoutAsync()
		{
			await this.Client.LogoutAsync();
		}

		public bool ResumeWithUrl(NSUrl url)
		{
			return this.Client.ResumeWithURL(url);
		}
	}
}
