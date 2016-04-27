using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using MobileTasks.iOS.Models;

namespace MobileTasks.iOS.ViewSources
{
	public class MobileTaskViewSource : UITableViewSource
	{
		private List<MobileTask> tasks;
		private const string CellIdentifier = "TaskCell";
		private UITableViewController controller;

		public MobileTaskViewSource(UITableViewController controller, List<MobileTask> tasks)
		{
			this.controller = controller;
			this.tasks = tasks;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(CellIdentifier);
			var task = this.tasks[indexPath.Row];

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
			}

			cell.TextLabel.Text = task.Description;
			if (task.IsCompleted)
			{
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}
			else
			{
				cell.Accessory = UITableViewCellAccessory.None;
			}

			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return this.tasks.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var detail = controller.Storyboard.InstantiateViewController("DetailController") as DetailController;
			if (detail != null)
			{
				detail.SetTask(this.tasks[indexPath.Row]);
				controller.NavigationController.PushViewController(detail, true);
			}
		}
	}
}
