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

		public DetailController(IntPtr handle) : base(handle)
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

			//Busy.Hidden = true;
			HasDateDue.ValueChanged += (sender, args) =>
			{
				DateDue.Hidden = !HasDateDue.On;
			};

			if (task != null)
			{
				Description.Text = task.Description;
				Completed.On = task.IsCompleted;
				if (task.DateDue.HasValue)
				{
					HasDateDue.On = true;
					DateDue.Date = (NSDate)task.DateDue.Value;
				}
				else
				{
					HasDateDue.On = false;
					DateDue.Hidden = true;
				}
			}
			else
			{
				task = new MobileTask();
				Completed.On = false;
				HasDateDue.On = false;
				DateDue.Hidden = true;
			}
		}

		public void SetTask(MobileTask task)
		{
			this.task = task;
		}

		private async Task Save()
		{
			task.Description = Description.Text;
			if (HasDateDue.On)
			{
				task.DateDue = (DateTime)DateDue.Date;
			}
			else
			{
				task.DateDue = null;
			}
			task.IsCompleted = Completed.On;

			//Busy.Hidden = false;
			await MobileService.Instance.UpsertTaskAsync(task);

			InvokeOnMainThread(() =>
			{
				//Busy.Hidden = true;

				NavigationController.PopViewController(true);
			});
		}
	}
}
