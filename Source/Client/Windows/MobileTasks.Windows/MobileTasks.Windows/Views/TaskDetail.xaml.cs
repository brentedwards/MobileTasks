using MobileTasks.Windows.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileTasks.Windows.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TaskDetail : MobileTasksPage
	{
		public TaskDetail()
		{
			this.InitializeComponent();
			this.ViewModel = new TaskDetailViewModel();

			this.WireUpViewModel(this.ViewModel);

			var navManager = SystemNavigationManager.GetForCurrentView();
			navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
			navManager.BackRequested += (s, e) =>
			{
				navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
				this.Frame.GoBack();
			};
		}
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter != null)
			{
				var id = int.Parse((string)e.Parameter);
				await ((TaskDetailViewModel)this.ViewModel).LoadTaskAsync(id);
			}
		}

		private async void Save_Click(object sender, RoutedEventArgs e)
		{
			await ((TaskDetailViewModel)this.ViewModel).SaveTaskAsync();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.GoBack();
		}

		private async void Delete_Click(object sender, RoutedEventArgs e)
		{
			await ((TaskDetailViewModel)this.ViewModel).DeleteTaskAsync();
		}
	}
}
