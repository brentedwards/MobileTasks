using MobileTasks.XForms.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileTasks.XForms.ViewModels
{
	public class DetailViewModel : ViewModelBase
	{
		private INavigation navigation;

		public DetailViewModel(INavigation navigation)
		{
			this.Task = new MobileTask();
			this.DateDue = DateTime.Today;

			this.navigation = navigation;
		}

		private MobileTask task;
		public MobileTask Task
		{
			get { return this.task; }
			set
			{
				this.task = value;
				this.OnPropertyChanged();
			}
		}

		private bool specifyDueDate;
		public bool SpecifyDueDate
		{
			get { return this.specifyDueDate; }
			set
			{
				this.specifyDueDate = value;
				this.OnPropertyChanged();
			}
		}

		private DateTime dateDue;
		public DateTime DateDue
		{
			get { return this.dateDue; }
			set
			{
				this.dateDue = value;
				this.OnPropertyChanged();
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				return new Command(async () => { await this.SaveTaskAsync(); });
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				return new Command(async () => { await this.DeleteTaskAsync(); });
			}
		}

		public async Task SaveTaskAsync()
		{
			this.Task.DateDue = this.SpecifyDueDate ? this.DateDue : (DateTime?)null;

			this.IsBusy = true;
			await this.MobileService.UpsertTaskAsync(this.Task);
			this.IsBusy = false;

			await this.navigation.PopAsync();
		}

		public async Task DeleteTaskAsync()
		{
			this.IsBusy = true;
			await this.MobileService.DeleteTaskAsync(this.Task.Id);
			this.IsBusy = false;

			await this.navigation.PopAsync();
		}
	}
}
