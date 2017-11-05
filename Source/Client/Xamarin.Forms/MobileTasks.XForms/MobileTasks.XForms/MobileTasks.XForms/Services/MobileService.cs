﻿using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.XForms.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

#if IOS
using UIKit;
#endif // IOS

namespace MobileTasks.XForms.Services
{
	public partial class MobileService
	{
		private const string TaskAction = "task";

		public event EventHandler OnUserLoggedIn;

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
#if ANDROID
			await this.Client.LoginAsync(Xamarin.Forms.Forms.Context, provider, "commagenicmobiletasks");
#elif IOS
			// Launching from a modal window takes some finesse.
			// http://stackoverflow.com/questions/24136464/access-viewcontroller-in-dependencyservice-to-present-mfmailcomposeviewcontrolle
			var rootController = UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController.PresentedViewController;
			var navcontroller = rootController as UINavigationController;
			if (navcontroller != null)
			{
				rootController = navcontroller.VisibleViewController;
			}

			await this.Client.LoginAsync(rootController, provider, "commagenicmobiletasks");
#else
			await this.Client.LoginAsync(provider);
#endif

			this.OnUserLoggedIn?.Invoke(this, new EventArgs());
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

#if IOS
		public bool ResumeWithUrl(Foundation.NSUrl url)
		{
			return this.Client.ResumeWithURL(url);
		}
#endif
	}
}
