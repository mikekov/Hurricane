using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurricane.Extensions.Converter
{
    class VolumeToVectorImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double volume = (double)value;

            if (volume == 0)
                return Application.Current.Resources["VectorVolumeMute2"];
            else if (volume <= 0.33333)
                return Application.Current.Resources["VectorVolumeLow2"];
            else if (volume <= 0.66666)
                return Application.Current.Resources["VectorVolumeMedium2"];
            else
                return Application.Current.Resources["VectorVolumeLoud2"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
