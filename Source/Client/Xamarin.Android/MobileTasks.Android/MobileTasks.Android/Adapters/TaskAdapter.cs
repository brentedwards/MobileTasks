using Android.App;
using Android.Views;
using Android.Widget;
using MobileTasks.Android.Models;
using MobileTasks.Android.Services;
using System.Collections.Generic;

namespace MobileTasks.Android.Adapters
{
	public class TaskAdapter : BaseAdapter<MobileTask>
	{
		private List<MobileTask> tasks;
		private Activity context;

		public TaskAdapter(Activity context, List<MobileTask> tasks)
			: base()
		{
			this.context = context;
			this.tasks = tasks;
		}

		public override MobileTask this[int position]
		{
			get { return tasks[position]; }
		}

		public override int Count
		{
			get { return tasks.Count; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var task = tasks[position];
			var view = convertView;
			if (view == null)
			{
				view = context.LayoutInflater.Inflate(Resource.Layout.TaskView, null);
			}

			var taskCheckBox = view.FindViewById<CheckBox>(Resource.Id.task);
			taskCheckBox.Text = task.Description;
			taskCheckBox.Checked = task.IsCompleted;
			taskCheckBox.CheckedChange += async (sender, args) =>
			{
				task.IsCompleted = args.IsChecked;

				// TODO: Look busy
				await MobileService.Instance.UpsertTaskAsync(task);
			};

			return view;
		}
	}
}