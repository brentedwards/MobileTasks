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

		public TaskDetailViewModel()
		{
			this.Task = new MobileTask();
		}

		public async Task SaveTaskAsync()
		{
			this.IsBusy = true;
			this.Task = await this.MobileService.UpsertTask(this.Task);
			this.IsBusy = false;
			this.OnGoBack?.Invoke();
		}
	}
}
