using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using System.Linq;
using Windows.Storage;

namespace MobileTasks.Windows.ViewModels
{
	public class LoginViewModel : ViewModelBase
    {
		private const string Tasks = "Tasks";

		public override Task OnLoaded()
		{
			this.CheckPreviousAuthentication();

			return base.OnLoaded();
		}

		public void CheckPreviousAuthentication()
		{
			PasswordCredential credential = null;

			var settings = ApplicationData.Current.RoamingSettings;

			var lastUsedProvider = settings.Values[LastUsedProvider] as string;
			if (lastUsedProvider != null)
			{
				try
				{
					var passwordVault = new PasswordVault();

					// Try to get an existing credential from the vault.
					credential = passwordVault.FindAllByResource(lastUsedProvider).FirstOrDefault();
				}
				catch (Exception)
				{
					// When there is no matching resource an error occurs, which we ignore.
				}
			}

			if (credential != null)
			{
				this.MobileService.User = new MobileServiceUser(credential.UserName);
				credential.RetrievePassword();
				this.MobileService.User.MobileServiceAuthenticationToken = credential.Password;

				this.OnNavigate?.Invoke(Tasks, null);
			}
		}

		public async Task AuthenticateAsync(MobileServiceAuthenticationProvider provider)
		{
			var passwordVault = new PasswordVault();
			PasswordCredential credential = null;

			var settings = ApplicationData.Current.RoamingSettings;

			try
			{
				await this.MobileService.LoginAsync(provider);
				credential = new PasswordCredential(provider.ToString(), this.MobileService.User.UserId, this.MobileService.User.MobileServiceAuthenticationToken);
				passwordVault.Add(credential);

				settings.Values[LastUsedProvider] = provider.ToString();

				this.OnNavigate?.Invoke(Tasks, null);
			}
			catch (InvalidOperationException)
			{
				await this.OnShowErrorAsync?.Invoke("Authentication failed. Please try again.");
			}
        }
    }
}
