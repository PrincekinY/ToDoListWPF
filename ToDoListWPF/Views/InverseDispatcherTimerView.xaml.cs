using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ToDoListWPF.Models;

namespace ToDoListWPF.Views
{
    /// <summary>
    /// AttentionDispatcherTimer.xaml 的交互逻辑
    /// </summary>
    public partial class InverseDispatcherTimerView : Window
    {
        public InverseDispatcherTimerView()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (_, __) => {
                EventBus.EventAggregatorInstance.GetEvent<DispatcherTimerWindowCloseEvent>().Publish(true);
                SystemCommands.CloseWindow(this);
            }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (_, __) => { SystemCommands.MinimizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (_, __) => { SystemCommands.MaximizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (_, __) => { SystemCommands.RestoreWindow(this); }));
        }

        private void FixTop_BtnClick(object sender, RoutedEventArgs e)
        {
            SubWindowConI.Topmost = true;
        }

        private void CancelFixTop_BtnClick(object sender, RoutedEventArgs e)
        {
            SubWindowConI.Topmost = false;
        }
    }
}
