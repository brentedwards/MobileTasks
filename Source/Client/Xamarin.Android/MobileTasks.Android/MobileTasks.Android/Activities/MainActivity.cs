using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.Android;
using MobileTasks.Droid.Adapters;
using MobileTasks.Droid.Models;
using MobileTasks.Droid.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileTasks.Droid.Activities
{
	[Activity(Label = "Tasks")]
	public class MainActivity : AppCompatActivity
	{
		private ListView listView;
		private List<MobileTask> taskList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			this.listView = this.FindViewById<ListView>(global::Android.Resource.Id.List);

			this.listView.ItemClick += (sender, args) =>
			{
				var task = this.taskList[args.Position];
				this.Edit(task);
			};

			//var addTaskButton = this.FindViewById<ImageButton>(Resource.Id.addTask);
			//addTaskButton.Click += (sender, args) =>
			//{
			//	this.StartActivity(new Intent(this, typeof(TaskDetailActivity)));
			//};
		}

		protected override async void OnResume()
		{
			base.OnResume();

			await LoadTasks();
		}

		private async Task LoadTasks()
		{
			try
			{
				var tasks = await MobileService.Instance.GetTasksAsync();
				this.taskList = tasks.ToList();

				this.listView.Adapter = new TaskAdapter(this, this.taskList);
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					this.Finish();
				}
			}
		}

		private void Edit(MobileTask task)
		{
			var json = JsonConvert.SerializeObject(task);

			var intent = new Intent(this, typeof(TaskDetailActivity));
			intent.PutExtra(Constants.Extras.Task, json);

			this.StartActivity(intent);
		}
	}
}