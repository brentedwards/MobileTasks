using Android.App;
using Android.Views;
using Android.Widget;
using MobileTasks.Android;
using MobileTasks.Droid.Models;
using MobileTasks.Droid.Services;
using System;
using System.Collections.Generic;

namespace MobileTasks.Droid.Adapters
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

			var stateImage = view.FindViewById<ImageView>(Resource.Id.state);
			SetStateImage(task, stateImage);

			var description = view.FindViewById<TextView>(Resource.Id.description);
			description.Text = task.Description;

			var dateDue = view.FindViewById<TextView>(Resource.Id.dateDue);
			dateDue.Text = task.DateDue != null ? task.DateDue.Value.ToString("ddd, M/d/yy h:mm tt") : "No Due Date";

			stateImage.Click += async (sender, args) =>
			{
				task.IsCompleted = !task.IsCompleted;
				SetStateImage(task, stateImage);

				// TODO: Look busy
				await MobileService.Instance.UpsertTaskAsync(task);
			};

			return view;
		}

		private static void SetStateImage(MobileTask task, ImageView stateImage)
		{
			if (task.IsCompleted)
			{
				stateImage.SetImageResource(Resource.Drawable.IconCompleted);
			}
			else if (task.DateDue < DateTime.Now)
			{
				stateImage.SetImageResource(Resource.Drawable.IconPastDue);
			}
			else
			{
				stateImage.SetImageResource(Resource.Drawable.IconIncomplete);
			}
		}
	}
}