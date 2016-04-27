using Foundation;
using MobileTasks.iOS.Models;
using MobileTasks.iOS.Services;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using UIKit;

namespace MobileTasks.iOS
{
	partial class DetailController : UIViewController
	{
		private MobileTask task;

		public DetailController (IntPtr handle) : base (handle)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, args) =>
			{
				NavigationController.PopViewController(true);
			}), true);

			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Save, async (sender, args) =>
			{
				await this.Save();
			}), true);

			Busy.Hidden = true;
			HasDueDate.ValueChanged += (sender, args) =>
			{
				Date.Hidden = !HasDueDate.On;
			};

			if (task != null)
			{
				Description.Text = task.Description;
				Completed.On = task.IsCompleted;
				if (task.DateDue.HasValue)
				{
					HasDueDate.On = true;
					//Date.Date = task.DateDue.Value;
				}
			}
			else
			{
				task = new MobileTask();
				Completed.On = false;
				HasDueDate.On = false;
			}
		}

		public void SetTask(MobileTask task)
		{
			this.task = task;
		}

		private async Task Save()
		{
			task.Description = Description.Text;
			if (HasDueDate.On)
			{
				//task.DateDue = Date.Date;
			}
			else
			{
				task.DateDue = null;
			}
			task.IsCompleted = Completed.On;

			Busy.Hidden = false;
			await MobileService.Instance.UpsertTaskAsync(task);

			InvokeOnMainThread(() =>
			{
				Busy.Hidden = true;

				NavigationController.PopViewController(true);
			});
		}
	}
}
