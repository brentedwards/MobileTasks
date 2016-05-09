using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileTasks.XForms.ViewModels
{
	public class LoginViewModel : ViewModelBase
    {
		private INavigation navigation;

		public LoginViewModel(INavigation navigation)
		{
			this.navigation = navigation;
		}

		public ICommand FacebookCommand
		{
			get
			{
				return new Command(async _ =>
				{
					await AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
				});
			}
		}

		public ICommand MicrosoftCommand
		{
			get
			{
				return new Command(async _ =>
				{
					await AuthenticateAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
				});
			}
		}

		public ICommand TwitterCommand
		{
			get
			{
				return new Command(async _ =>
				{
					await AuthenticateAsync(MobileServiceAuthenticationProvider.Twitter);
				});
			}
		}

		public ICommand GoogleCommand
		{
			get
			{
				return new Command(async _ =>
				{
					await AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
				});
			}
		}

		public async Task AuthenticateAsync(MobileServiceAuthenticationProvider provider)
		{
			try
			{
				await this.MobileService.LoginAsync(provider);

				await this.navigation.PopModalAsync();
			}
			catch (InvalidOperationException)
			{
				// TODO: Show error
				//await this.OnShowErrorAsync?.Invoke("Authentication failed. Please try again.");
			}
		}
	}
}
