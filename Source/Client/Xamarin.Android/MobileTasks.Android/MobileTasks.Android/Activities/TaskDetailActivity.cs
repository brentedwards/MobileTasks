
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileTasks.Android.Models;
using MobileTasks.Android.Services;
using Newtonsoft.Json;
using System;

namespace MobileTasks.Android.Activities
{
	[Activity(Label = "Detail")]
	public class TaskDetailActivity : Activity
	{
		private MobileTask task;

		private EditText description;
		private EditText date;
		private CheckBox completed;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.TaskDetail);

			if (Intent.HasExtra(Constants.Extras.Task))
			{
				this.task = JsonConvert.DeserializeObject<MobileTask>(Intent.GetStringExtra(Constants.Extras.Task));
			}
			else
			{
				this.task = new MobileTask();
			}

			description = FindViewById<EditText>(Resource.Id.description);
			description.Text = this.task.Description;

			date = FindViewById<EditText>(Resource.Id.date);
			date.Text = this.task.DateDue != null ? this.task.DateDue.Value.ToString("d") : null;

			completed = FindViewById<CheckBox>(Resource.Id.completed);
			completed.Checked = this.task.IsCompleted;

			var saveButton = FindViewById<Button>(Resource.Id.save);
			saveButton.Click += async delegate
			{
				this.task.Description = description.Text;
				this.task.IsCompleted = completed.Checked;
				if (!string.IsNullOrWhiteSpace(date.Text))
				{
					DateTime parsedDate;
					if (DateTime.TryParse(date.Text, out parsedDate))
					{
						this.task.DateDue = parsedDate;
					}
					else
					{
						var builder = new AlertDialog.Builder(this)
							.SetTitle("Parse Error")
							.SetMessage("Please enter a valid date.")
							.SetPositiveButton(Resource.String.Ok, (sender, args) => { /* Do nothing */ });

						builder.Create().Show();
						return;
					}
				}
				else
				{
					this.task.DateDue = null;
				}
				await MobileService.Instance.UpsertTaskAsync(this.task);
				this.Finish();
			};

			var deleteButton = FindViewById<Button>(Resource.Id.delete);
			deleteButton.Click += delegate
			{
				var builder = new AlertDialog.Builder(this)
				.SetTitle("Are you sure?")
				.SetMessage("Delete '" + task.Description + "'?")
				.SetPositiveButton(Resource.String.Yes, async (sender, args) =>
				{
					await MobileService.Instance.DeleteTaskAsync(task.Id);
					this.Finish();
				})
				.SetNegativeButton(Resource.String.No, (sender, args) => { /* Do nothing */ });

				builder.Create().Show();
			};
		}
	}
}