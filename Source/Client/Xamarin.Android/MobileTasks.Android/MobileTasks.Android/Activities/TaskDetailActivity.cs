
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Util;
using MobileTasks.Droid.Models;
using MobileTasks.Droid.Services;
using Newtonsoft.Json;
using System;

namespace MobileTasks.Droid.Activities
{
	[Activity(Label = "Detail")]
	public class TaskDetailActivity : AppCompatActivity, DatePickerDialog.IOnDateSetListener
	{
		private MobileTask task;

		private EditText description;
		private TextView date;
		private CheckBox completed;
		private CheckBox specifyDateDue;
		private Button changeDate;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.TaskDetail);

			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			this.SetSupportActionBar(toolbar);

			if (Intent.HasExtra(Constants.Extras.Task))
			{
				this.task = JsonConvert.DeserializeObject<MobileTask>(Intent.GetStringExtra(Constants.Extras.Task));
			}
			else
			{
				this.task = new MobileTask();
			}

			this.description = this.FindViewById<EditText>(Resource.Id.description);
			this.description.Text = this.task.Description;

			this.date = this.FindViewById<TextView>(Resource.Id.date);
			this.date.Text = this.task.DateDue != null ? this.task.DateDue.Value.ToString("ddd, M/d/yy h:mm tt") : null;

			this.changeDate = this.FindViewById<Button>(Resource.Id.changeDate);
			this.changeDate.Click += (sender, args) =>
			{
				var calendar = Calendar.GetInstance(Java.Util.TimeZone.Default);
				if (this.task.DateDue != null)
				{
					calendar.Time = new Date(this.task.DateDue.Value.Millisecond);
				}

				var picker = new DatePickerDialog(this, this, this.task.DateDue.Value.Year, this.task.DateDue.Value.Month - 1, this.task.DateDue.Value.Day);
				picker.Show();
			};

			this.specifyDateDue = this.FindViewById<CheckBox>(Resource.Id.specifyDateDue);
			this.specifyDateDue.CheckedChange += (sender, args) =>
			{
				if (args.IsChecked)
				{
					this.date.Visibility = ViewStates.Visible;
					this.changeDate.Visibility = ViewStates.Visible;
				}
				else
				{
					this.date.Visibility = ViewStates.Gone;
					this.changeDate.Visibility = ViewStates.Gone;
				}
			};
			this.specifyDateDue.Checked = this.task.DateDue != null;

			this.completed = this.FindViewById<CheckBox>(Resource.Id.completed);
			this.completed.Checked = this.task.IsCompleted;

			var saveButton = this.FindViewById<Button>(Resource.Id.save);
			saveButton.Click += async delegate
			{
				this.task.Description = description.Text;
				this.task.IsCompleted = completed.Checked;
				if (this.specifyDateDue.Checked && !string.IsNullOrWhiteSpace(date.Text))
				{
					DateTime parsedDate;
					if (DateTime.TryParse(date.Text, out parsedDate))
					{
						this.task.DateDue = parsedDate;
					}
					else
					{
						var builder = new Android.App.AlertDialog.Builder(this)
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

			var deleteButton = this.FindViewById<Button>(Resource.Id.delete);
			deleteButton.Click += delegate
			{
				var builder = new Android.App.AlertDialog.Builder(this)
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

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			this.task.DateDue = new DateTime(year, monthOfYear + 1, dayOfMonth);
			this.date.Text = this.task.DateDue.Value.ToString("ddd, M/d/yy h:mm tt");
		}
	}
}