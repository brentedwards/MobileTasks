using MobileTasks.Windows.Models;
using System;
using Windows.UI.Xaml.Data;

namespace MobileTasks.Windows.Converters
{
	public class TaskImageSourceConverter : IValueConverter
	{
		public string CompletedSource { get; set; }
		public string IncompleteSource { get; set; }
		public string PastDueSource { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var task = value as MobileTask;
			if (task != null)
			{
				if (task.IsCompleted)
				{
					return this.CompletedSource;
				}
				else
				{
					if (task.DateDue != null && DateTime.Now > task.DateDue)
					{
						return this.PastDueSource;
					}
				}
			}

			return this.IncompleteSource;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
