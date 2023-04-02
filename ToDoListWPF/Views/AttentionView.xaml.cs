using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoListWPF.Views
{
    /// <summary>
    /// AttentionView.xaml 的交互逻辑
    /// </summary>
    public partial class AttentionView : Page
    {
        public AttentionView()
        {
            InitializeComponent();
        }

        private void PresetTimePicker_SelectedTimeChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.DateTime?> e)
        {
            var oldValue = e.OldValue.HasValue ? e.OldValue.Value.ToLongTimeString() : "NULL";
            var newValue = e.NewValue.HasValue ? e.NewValue.Value.ToLongTimeString() : "NULL";

            Debug.WriteLine($"PresentTimePicker's SelectedTime changed from {oldValue} to {newValue}");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(SelectedTimePicker.SelectedTime.ToString());
            MessageBox.Show(SelectedTimePicker.SelectedTime.GetType().ToString());
        }
    }
}
