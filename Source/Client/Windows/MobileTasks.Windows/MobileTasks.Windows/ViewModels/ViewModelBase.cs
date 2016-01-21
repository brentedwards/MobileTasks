using MobileTasks.Windows.Models;
using MobileTasks.Windows.Services;
using System;
using System.Threading.Tasks;

namespace MobileTasks.Windows.ViewModels
{
	public class ViewModelBase : ObservableObject
	{
		protected MobileService MobileService
		{
			get { return MobileService.Instance; }
		}

		public Func<string, Task> OnShowErrorAsync { get; set; }
		
		public Action<string> OnNavigate { get; set; }

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

		public virtual Task OnLoaded()
		{
			return Task.FromResult(default(object));
		}
    }
}
