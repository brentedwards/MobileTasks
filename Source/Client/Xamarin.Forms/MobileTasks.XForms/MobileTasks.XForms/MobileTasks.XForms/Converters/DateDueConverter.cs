using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileTasks.XForms.Converters
{
	public class DateDueConverter : IValueConverter
	{
		public string NoDateDueText { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && value is DateTime)
			{
				return ((DateTime)value).ToString("ddd, M/d/yy h:mm tt");
			}

			return this.NoDateDueText;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
