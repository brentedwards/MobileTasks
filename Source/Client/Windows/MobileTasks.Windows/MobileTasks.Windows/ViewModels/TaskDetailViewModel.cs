using MobileTasks.Windows.Models;
using System;
using System.Threading.Tasks;

namespace MobileTasks.Windows.ViewModels
{
	public class TaskDetailViewModel : ViewModelBase
	{
		private MobileTask task;
		public MobileTask Task
		{
			get { return this.task; }
			set
			{
				this.task = value;
				if (value != null && value.DateDue != null)
				{
					this.SpecifyDateDue = true;
					this.DateDue = value.DateDue.Value;
				}

				this.OnPropertyChanged();
			}
		}

		private bool isEdit;
		public bool IsEdit
		{
			get { return this.isEdit; }
			set
			{
				this.isEdit = value;
				this.OnPropertyChanged();
			}
		}

		private bool specifyDateDue;
		public bool SpecifyDateDue
		{
			get { return this.specifyDateDue; }
			set
			{
				this.specifyDateDue = value;
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

		public TaskDetailViewModel()
		{
			this.Task = new MobileTask();
			this.DateDue = DateTime.Today;
		}

		public async Task LoadTaskAsync(int id)
		{
			this.IsBusy = true;
			this.Task = await this.MobileService.LoadTaskAsync(id);
			this.IsEdit = true;
			this.IsBusy = false;
		}

		public async Task SaveTaskAsync()
		{
			this.Task.DateDue = this.SpecifyDateDue ? this.DateDue : (DateTime?)null;

			this.IsBusy = true;
			this.Task = await this.MobileService.UpsertTaskAsync(this.Task);
			this.IsBusy = false;
			this.OnGoBack?.Invoke();
		}

		public async Task DeleteTaskAsync()
		{
			this.IsBusy = true;
			await this.MobileService.DeleteTaskAsync(this.Task.Id);
			this.IsBusy = false;
			this.OnGoBack?.Invoke();
		}
	}
}
