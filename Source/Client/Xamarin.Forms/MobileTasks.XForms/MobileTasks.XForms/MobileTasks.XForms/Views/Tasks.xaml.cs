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
				await Navigation.PushAsync(new Detail(task));
			};

			ShowLoginDialog();
		}

		public async Task ShowLoginDialog()
		{
			await Navigation.PushModalAsync(new Login());
		}
	}
}
