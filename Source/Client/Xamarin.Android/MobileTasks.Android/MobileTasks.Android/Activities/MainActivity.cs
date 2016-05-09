using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
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
	public class MainActivity : ListActivity
	{
		private List<MobileTask> taskList;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			this.ListView.ItemClick += (sender, args) =>
			{
				var task = this.taskList[args.Position];
				this.Edit(task);
			};
		}

		protected override async void OnResume()
		{
			base.OnResume();

			await LoadTasks();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			this.MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
			var newTaskMenuItem = menu.FindItem(Resource.Id.newTask);
			newTaskMenuItem.SetIntent(new Intent(this, typeof(TaskDetailActivity)));

			return true;
		}

		private async Task LoadTasks()
		{
			try
			{
				var tasks = await MobileService.Instance.GetTasksAsync();
				this.taskList = tasks.ToList();

				this.ListAdapter = new TaskAdapter(this, this.taskList);
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