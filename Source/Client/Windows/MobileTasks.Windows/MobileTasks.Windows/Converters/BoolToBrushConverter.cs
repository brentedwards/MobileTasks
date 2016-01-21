using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MobileTasks.Windows.Converters
{
	public class BoolToBrushConverter : IValueConverter
	{
		public Brush TrueBrush { get; set; }
		public Brush FalseBrush { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool)
			{
				return (bool)value ? TrueBrush : FalseBrush;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
