using System;
using Windows.UI.Xaml.Data;

namespace MobileTasks.Windows.Converters
{
	public class DateDueConverter : IValueConverter
	{
		public string NoDateDueText { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value != null && value is DateTime)
			{
				return ((DateTime)value).ToString("ddd, M/d/yy h:mm tt");
			}

			return this.NoDateDueText;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
