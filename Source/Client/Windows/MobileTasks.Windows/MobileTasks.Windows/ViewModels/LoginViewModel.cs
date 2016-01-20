using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;

namespace MobileTasks.Windows.ViewModels
{
	public class LoginViewModel : ViewModelBase
    {
        public async Task AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            try
            {
				await this.MobileService.LoginAsync(provider);

				this.OnNavigate?.Invoke("Tasks");
            }
            catch (InvalidOperationException)
            {
				await this.OnShowErrorAsync?.Invoke("Authentication failed. Please try again.");
            }
        }
    }
}
