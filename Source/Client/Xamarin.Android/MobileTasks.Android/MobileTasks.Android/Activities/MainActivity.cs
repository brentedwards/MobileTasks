using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using MobileTasks.Android.Services;
using MobileTasks.Android.Models;
using MobileTasks.Android.Adapters;

namespace MobileTasks.Android.Activities
{
	[Activity(Label = "Tasks")]
	public class MainActivity : ListActivity
	{
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var tasksListView = FindViewById<Button>(Resource.Id.tasks);

			try
			{
				var tasks = await MobileService.Instance.GetTasksAsync();

				ListAdapter = new TaskAdapter(this, tasks.ToList());
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					// TODO: Logout
				}
			}
		}
	}
}