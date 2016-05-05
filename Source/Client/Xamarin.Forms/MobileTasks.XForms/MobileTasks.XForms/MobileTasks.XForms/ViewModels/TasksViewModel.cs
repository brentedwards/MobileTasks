using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.XForms.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileTasks.XForms.ViewModels
{
    public class TasksViewModel : ViewModelBase
    {
		public ObservableCollection<MobileTask> Tasks { get; private set; }

		public ICommand UpdateTaskCommand
		{
			get
			{
				return new Command<MobileTask>(async (task) => { await this.UpdateIsCompleted(task); });
			}
		}

		public TasksViewModel()
		{
			this.Tasks = new ObservableCollection<MobileTask>();

			MobileService.OnUserLoggedIn += async delegate
			{
				await this.LoadTasksAsync();
			};
		}

		public async Task LoadTasksAsync()
		{
			this.IsBusy = true;

			try
			{
				var tasks = await this.MobileService.GetTasksAsync();

				this.Tasks.Clear();
				foreach (var task in tasks)
				{
					this.Tasks.Add(task);
					task.PropertyChanged += this.OnTaskPropertyChanged;
				}
				this.IsBusy = false;
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
				{
					// TODO: Pop up Login Screen again
					//await this.Logout();
				}
			}
		}

		private async void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "IsCompleted")
			{
				var task = (MobileTask)sender;

				this.IsBusy = true;
				await this.MobileService.UpsertTaskAsync(task);
				this.IsBusy = false;
			}
		}

		private async Task UpdateIsCompleted(MobileTask task)
		{
			task.IsCompleted = !task.IsCompleted;

			this.IsBusy = true;
			await this.MobileService.UpsertTaskAsync(task);
			this.IsBusy = false;
		}
	}
}
