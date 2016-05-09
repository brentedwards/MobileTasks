using MobileTasks.XForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MobileTasks.XForms.Views
{
	public partial class Login : ContentPage
	{
		public LoginViewModel ViewModel { get; private set; }

		public Login ()
		{
			InitializeComponent ();

			this.ViewModel = new LoginViewModel(this.Navigation);
			this.BindingContext = this.ViewModel;
		}
	}
}
