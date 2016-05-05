using MobileTasks.XForms.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileTasks.XForms.Converters
{
	public class TaskImageSourceConverter : IValueConverter
	{
		public string CompletedSource { get; set; }
		public string IncompleteSource { get; set; }
		public string PastDueSource { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
					if (task.DateDue != null && DateTime.UtcNow > task.DateDue)
					{
						return this.PastDueSource;
					}
				}
			}

			return this.IncompleteSource;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
