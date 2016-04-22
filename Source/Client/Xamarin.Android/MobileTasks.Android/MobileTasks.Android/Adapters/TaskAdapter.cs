using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MobileTasks.Android.Activities;
using MobileTasks.Android.Models;
using MobileTasks.Android.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

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

			taskCheckBox.LongClick += delegate
			{
				var menu = new PopupMenu(context, taskCheckBox);
				menu.Inflate(Resource.Menu.TaskMenu);
				menu.MenuItemClick += (sender, args) =>
				{
					switch (args.Item.ItemId)
					{
						case Resource.Id.edit:
							this.Edit(task);
							break;

						case Resource.Id.delete:
							this.Delete(task);
							break;
					}
				};
				menu.Show();
			};

			return view;
		}

		private void Edit(MobileTask task)
		{
			var json = JsonConvert.SerializeObject(task);

			var intent = new Intent(context, typeof(TaskDetailActivity));
			intent.PutExtra(Constants.Extras.Task, json);

			context.StartActivity(intent);
		}

		private void Delete(MobileTask task)
		{
			var builder = new AlertDialog.Builder(context)
				.SetTitle("Are you sure?")
				.SetMessage("Delete '" + task.Description + "'?")
				.SetPositiveButton(Resource.String.Yes, async (sender, args) =>
					{
						await MobileService.Instance.DeleteTaskAsync(task.Id);
						tasks.Remove(task);
						this.NotifyDataSetChanged();
					})
				.SetNegativeButton(Resource.String.No, (sender, args) => { /* Do nothing */ });

			builder.Create().Show();
		}
	}
}