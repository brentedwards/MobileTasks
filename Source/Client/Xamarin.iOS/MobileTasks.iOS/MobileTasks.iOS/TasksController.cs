using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.iOS.Services;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;
using System.Linq;
using MobileTasks.iOS.ViewSources;

namespace MobileTasks.iOS
{
	partial class TasksController : UITableViewController
	{
		public TasksController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			TableView.RegisterNibForCellReuse(UINib.FromName("TaskCell", null), MobileTaskViewSource.CellIdentifier);

			NavigationItem.SetLeftBarButtonItem(null, true);

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender, args) =>
			{
				var detail = this.Storyboard.InstantiateViewController("DetailController") as DetailController;
				if (detail != null)
				{
					this.NavigationController.PushViewController(detail, true);
				}
			}), true);
		}

		public override async void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			await this.LoadTasks();
		}

		private async Task LoadTasks()
		{
			try
			{
				var tasks = await MobileService.Instance.GetTasksAsync();
				
				InvokeOnMainThread(() =>
				{
					TableView.Source = new MobileTaskViewSource(this, tasks.ToList());
					TableView.ReloadData();
				});
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					this.NavigationController.PopViewController(true);
				}
			}
		}
	}
}
