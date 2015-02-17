using System;
using System.Globalization;
using System.Windows.Data;

namespace Hurricane.Extensions.Converter
{
    class FormatTimespan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var duration = (TimeSpan)value;
            var time = DoConvert(duration);

            if (parameter != null && (string)parameter == "Negative" && duration.Ticks > 0)
                return '-' + time;

            return time;
        }

        string DoConvert(TimeSpan duration)
        {
            if (duration == TimeSpan.Zero) return "00:00";

            if (duration.Hours > 0 || duration.Days > 0)
            {
                return string.Format("{0:00}:{1:mm}:{1:ss}", (int)duration.TotalHours, duration);
            }
            return duration.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
