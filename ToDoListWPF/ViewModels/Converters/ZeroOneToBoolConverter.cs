using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToDoListWPF.ViewModels.Converters
{
    public class ZeroOneToBoolConverter : IValueConverter
    {
        //model->view
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "0")
            {
                if (parameter.ToString() == "0") { return true; }
                else { return false; }
            }
            else
            {
                if (parameter.ToString() == "0") { return false; }
                else { return true; }
            }
        }

        //view->model
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                if (parameter.ToString() == "0") { return 0; }
                else { return 1; }
            }
            else
            {
                return 1;
            }
        }
    }
}
