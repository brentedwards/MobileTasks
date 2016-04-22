using MobileTasks.Windows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public TaskDetailViewModel()
		{
			this.Task = new MobileTask();
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
