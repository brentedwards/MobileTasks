using CoreAnimation;
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

			this.SetBackgroundGradient();
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

		private void SetBackgroundGradient()
		{
			var startColor = UIColor.FromRGB(0xb4, 0xec, 0x51); //"#B4EC51"
			var endColor = UIColor.FromRGB(0x42, 0x93, 0xc21); //"#429321"
 
			var gradientLayer = new CAGradientLayer();
			gradientLayer.Colors = new CoreGraphics.CGColor[] { startColor.CGColor, endColor.CGColor };
			gradientLayer.Locations = new NSNumber[] { 0.0, 1.0 };
 
 
			gradientLayer.Frame = this.View.Bounds;
			this.View.Layer.InsertSublayer(gradientLayer, 0);
		}

	}
}
