using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.Windows.Models;
using System.Collections.ObjectModel;
using System.Net;
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

			try
			{
				var tasks = await this.MobileService.GetTasksAsync();

				foreach (var task in tasks)
				{
					this.Tasks.Add(task);
				}
				this.IsBusy = false;
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
				{
					await this.Logout();
				}
				else
				{
					this.IsBusy = false;
					await this.OnShowErrorAsync?.Invoke("Error loading tasks.");
					await this.Logout();
				}
			}
		}
	}
}
