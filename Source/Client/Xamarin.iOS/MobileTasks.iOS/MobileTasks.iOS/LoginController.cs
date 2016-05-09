using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.iOS.Services;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;

namespace MobileTasks.iOS
{
	partial class LoginController : UIViewController
	{
		public LoginController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Facebook.TouchUpInside += async (sender, args) =>
			{
				await this.AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
			};

			Microsoft.TouchUpInside += async (sender, args) =>
			{
				await this.AuthenticateAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
			};

			Twitter.TouchUpInside += async (sender, args) =>
			{
				await this.AuthenticateAsync(MobileServiceAuthenticationProvider.Twitter);
			};

			Google.TouchUpInside += async (sender, args) =>
			{
				await this.AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
			};
		}

		private async Task AuthenticateAsync(MobileServiceAuthenticationProvider provider)
		{
			try
			{
				await MobileService.Instance.LoginAsync(this, provider);

				var tasks = this.Storyboard.InstantiateViewController("TasksController") as TasksController;
				if (tasks != null)
				{
					this.NavigationController.PushViewController(tasks, true);
				}
			}
			catch (InvalidOperationException)
			{
				// TODO: Show an error;
				var blah = 0;
			}
		}
	}
}
