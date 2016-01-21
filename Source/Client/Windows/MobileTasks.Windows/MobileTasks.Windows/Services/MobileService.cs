using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.Windows.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileTasks.Windows.Services
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

		public async Task LoginAsync(MobileServiceAuthenticationProvider provider)
		{
			await this.Client.LoginAsync(provider);
		}

		public async Task<IEnumerable<MobileTask>> GetTasks()
		{
			return await this.Client.InvokeApiAsync<IEnumerable<MobileTask>>(TaskAction, HttpMethod.Get, null);
		}

		public async Task<MobileTask> UpsertTask(MobileTask task)
		{
			return await this.Client.InvokeApiAsync<MobileTask, MobileTask>("task", task);
		}

		public async Task LogoutAsync()
		{
			await this.Client.LogoutAsync();
		}
	}
}
