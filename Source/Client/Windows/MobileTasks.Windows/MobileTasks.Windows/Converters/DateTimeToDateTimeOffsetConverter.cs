using System;
using Windows.UI.Xaml.Data;

namespace MobileTasks.Windows.Converters
{
	public class DateTimeToDateTimeOffsetConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value != null && value is DateTime)
			{
				return new DateTimeOffset((DateTime)value);
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value != null && value is DateTimeOffset)
			{
				return ((DateTimeOffset)value).DateTime;
			}

			return null;
		}
	}
}
