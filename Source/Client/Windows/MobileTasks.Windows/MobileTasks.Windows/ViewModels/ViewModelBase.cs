using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.Windows.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MobileTasks.Windows.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		protected MobileService MobileService
		{
			get { return MobileService.Instance; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Func<string, Task> OnShowErrorAsync { get; set; }
		
		public Action<string> OnNavigate { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
