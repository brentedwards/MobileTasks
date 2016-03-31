using MobileTasks.Windows.Models;
using MobileTasks.Windows.Services;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MobileTasks.Windows.ViewModels
{
	public class ViewModelBase : ObservableObject
	{
		protected const string LastUsedProvider = "LastUsedProvider";

		protected MobileService MobileService
		{
			get { return MobileService.Instance; }
		}

		public Func<string, Task> OnShowErrorAsync { get; set; }
		
		public Action<string, object> OnNavigate { get; set; }

		public Action OnGoBack { get; set; }

		private bool isBusy;
		public bool IsBusy
		{
			get { return this.isBusy; }
			protected set
			{
				this.isBusy = value;
				this.OnPropertyChanged();
			}
		}

		public async Task Logout()
		{
			ApplicationData.Current.RoamingSettings.Values.Remove(LastUsedProvider);
			await this.MobileService.LogoutAsync();
			OnNavigate?.Invoke("Login", null);
		}

		public virtual Task OnLoaded()
		{
			return Task.FromResult(default(object));
		}
    }
}
