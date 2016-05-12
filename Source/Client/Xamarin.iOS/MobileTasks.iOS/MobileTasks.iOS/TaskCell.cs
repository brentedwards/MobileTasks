using MobileTasks.iOS.Models;
using MobileTasks.iOS.Services;
using System;
using UIKit;

namespace MobileTasks.iOS
{
	partial class TaskCell : UITableViewCell
	{
		private MobileTask task;

		public TaskCell(IntPtr handle) : base(handle)
		{
		}

		public TaskCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{
			this.Status.TouchUpInside += async (sender, args) =>
			{
				this.task.IsCompleted = !this.task.IsCompleted;
				this.SetStatusImage(this.task);

				// TODO: Look busy
				await MobileService.Instance.UpsertTaskAsync(this.task);
			};
		}

		public void UpdateTask(MobileTask task)
		{
			this.task = task;
			this.Description.Text = task.Description;
			this.DateDue.Text = task.DateDue != null ? task.DateDue.Value.ToString("ddd, M/d/yy h:mm tt") : "No Due Date";
			SetStatusImage(task);
		}

		private void SetStatusImage(MobileTask task)
		{
			if (task.IsCompleted)
			{
				this.Status.SetImage(new UIImage("IconCompleted"), UIControlState.Normal);
			}
			else if (task.DateDue < DateTime.Now)
			{
				this.Status.SetImage(new UIImage("IconPastDue"), UIControlState.Normal);
			}
			else
			{
				this.Status.SetImage(new UIImage("IconIncomplete"), UIControlState.Normal);
			}
		}
	}
}
