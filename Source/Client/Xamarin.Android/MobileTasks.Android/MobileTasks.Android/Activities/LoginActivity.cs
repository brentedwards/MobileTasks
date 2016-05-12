using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileTasks.Droid.Services;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace MobileTasks.Droid.Activities
{
	[Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
	public class LoginActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Login);

			var facebook = FindViewById<ImageButton>(Resource.Id.facebook);
			facebook.Click += async delegate { await AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook); };

			var microsoft = FindViewById<ImageButton>(Resource.Id.microsoft);
			microsoft.Click += async delegate { await AuthenticateAsync(MobileServiceAuthenticationProvider.MicrosoftAccount); };

			var twitter = FindViewById<ImageButton>(Resource.Id.twitter);
			twitter.Click += async delegate { await AuthenticateAsync(MobileServiceAuthenticationProvider.Twitter); };

			var google = FindViewById<ImageButton>(Resource.Id.google);
			google.Click += async delegate { await AuthenticateAsync(MobileServiceAuthenticationProvider.Google); };
		}

		private async Task AuthenticateAsync(MobileServiceAuthenticationProvider provider)
		{
			try
			{
				await MobileService.Instance.LoginAsync(this, provider);

				var intent = new Intent(this, typeof(MainActivity));
				StartActivity(intent);
			}
			catch (InvalidOperationException)
			{
				// TODO: Show an error;
				var blah = 0;
			}
		}
	}
}