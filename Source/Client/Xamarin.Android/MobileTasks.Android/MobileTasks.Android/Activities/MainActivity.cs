using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.Android.Adapters;
using MobileTasks.Android.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MobileTasks.Android.Activities
{
	[Activity(Label = "Tasks")]
	public class MainActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(savedInstanceState);
		}

		protected override async void OnResume()
		{
			base.OnResume();

			await LoadTasks();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
			var newTaskMenuItem = menu.FindItem(Resource.Id.newTask);
			newTaskMenuItem.SetIntent(new Intent(this, typeof(TaskDetailActivity)));

			return true;
		}

		private async Task LoadTasks()
		{
			try
			{
				var tasks = await MobileService.Instance.GetTasksAsync();
				var taskList = tasks.ToList();

				ListAdapter = new TaskAdapter(this, taskList);
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					this.Finish();
				}
			}
		}
	}
}