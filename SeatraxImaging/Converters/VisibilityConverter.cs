using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SeatraxImaging.Converters {
    class VisibilityConverter : IValueConverter {
        public Visibility OnTrue { get; set; }
        public Visibility OnFalse { get; set; }

        public VisibilityConverter() {
            OnFalse = Visibility.Collapsed;
            OnTrue = Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, string language) {
            var v = (bool)value;
            //Debug.WriteLine("SaveButtonVisibleConverter " + v);
            return v ? OnTrue : OnFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            if(value is Visibility == false) {
                return DependencyProperty.UnsetValue;
            }

            if((Visibility)value == OnTrue) {
                return true;
            } else {
                return false;
            }
        }
    }
}
