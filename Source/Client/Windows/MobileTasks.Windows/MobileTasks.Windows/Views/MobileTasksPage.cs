using MobileTasks.Windows.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace MobileTasks.Windows.Views
{
	public class MobileTasksPage : Page
	{
		protected void WireUpViewModel(ViewModelBase viewModel)
		{
			viewModel.OnShowErrorAsync = this.ShowErrorAsync;
			viewModel.OnNavigate = this.Navigate;
		}

		private async Task ShowErrorAsync(string message)
		{
			var dialog = new MessageDialog(message);
			dialog.Commands.Add(new UICommand("Close"));
			await dialog.ShowAsync();
		}

		private void Navigate(string pageName)
		{
		}
	}
}
