using MobileTasks.XForms.Models;
using MobileTasks.XForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MobileTasks.XForms.Views
{
	public partial class Detail : ContentPage
	{
		public DetailViewModel ViewModel { get; private set; }

		public Detail (MobileTask task)
		{
			InitializeComponent ();

			this.ViewModel = new DetailViewModel(this.Navigation);
			this.ViewModel.Task = task;
			this.BindingContext = this.ViewModel;
		}
	}
}
