using MobileTasks.Windows.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MobileTasks.Windows.ViewModels
{
	public class TasksViewModel : ViewModelBase
	{
		public ObservableCollection<MobileTask> Tasks { get; private set; }

		public TasksViewModel()
		{
			this.Tasks = new ObservableCollection<MobileTask>();
		}

		public override async Task OnLoaded()
		{
			this.IsBusy = true;
			var tasks = await this.MobileService.GetTasks();

			foreach (var task in tasks)
			{
				this.Tasks.Add(task);
			}
			this.IsBusy = false;
		}
	}
}
