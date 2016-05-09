using MobileTasks.Windows.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace MobileTasks.Windows.Views
{
	public class MobileTasksPage : Page
	{
		protected bool LogOutOnGoBack { get; set; }
		protected ViewModelBase ViewModel { get; set; }

		protected void WireUpViewModel(ViewModelBase viewModel)
		{
			this.ViewModel = viewModel;
			this.DataContext = viewModel;

			this.Loaded += async delegate
			{
				await viewModel.OnLoaded();
			};
			viewModel.OnShowErrorAsync = this.ShowErrorAsync;
			viewModel.OnNavigate = this.Navigate;
			viewModel.OnGoBack = this.GoBack;
		}

		private async Task ShowErrorAsync(string message)
		{
			var dialog = new MessageDialog(message);
			dialog.Commands.Add(new UICommand("Close"));
			await dialog.ShowAsync();
		}

		protected void GoBack()
		{
			this.Frame.GoBack();
		}

		protected void Navigate(string pageName, object parameter = null)
		{
			Type pageType = null;
			switch (pageName)
			{
				case "Tasks":
					pageType = typeof(Tasks);
					break;

				case "TaskDetail":
					pageType = typeof(TaskDetail);
					break;

				case "Login":
					while (this.Frame.CanGoBack)
					{
						this.Frame.GoBack();
					}
					return;
			}

			this.Frame.Navigate(pageType, parameter);
		}
	}
}
