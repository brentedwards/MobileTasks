using MobileTasks.XForms.Models;
using MobileTasks.XForms.Services;
using MobileTasks.XForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MobileTasks.XForms.Views
{
	public partial class Tasks : ContentPage
	{
		public TasksViewModel ViewModel { get; private set; }

		public Tasks ()
		{
			InitializeComponent ();

			this.ViewModel = new TasksViewModel();
			this.BindingContext = this.ViewModel;

			TaskList.ItemTapped += async (sender, args) =>
			{
				var task = (MobileTask)args.Item;
				var detail = new Detail(new MobileTask
				{
					Id = task.Id,
					Description = task.Description,
					DateDue = task.DateDue,
					IsCompleted = task.IsCompleted
				});
				detail.ViewModel.IsExistingTask = true;
				await Navigation.PushAsync(detail);
			};

			Add.Clicked += async (sender, args) =>
			{
				await Navigation.PushAsync(new Detail(new MobileTask()));
			};

			ShowLoginDialog();
		}

		public async Task ShowLoginDialog()
		{
			await Navigation.PushModalAsync(new Login());
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (MobileService.Instance.User != null)
			{
				await this.ViewModel.LoadTasksAsync();
			}
		}
	}
}
