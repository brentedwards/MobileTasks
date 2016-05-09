using MobileTasks.XForms.Models;
using MobileTasks.XForms.Services;

namespace MobileTasks.XForms.ViewModels
{
	public abstract class ViewModelBase : ObservableObject
	{
		protected MobileService MobileService
		{
			get { return MobileService.Instance; }
		}

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
	}
}
