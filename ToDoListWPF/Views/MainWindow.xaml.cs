using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDoListWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Frame todo_frame = new Frame() { Content = new ToDoView() };
        Frame fit_frame = new Frame() { Content = new FitnessView() };
        Frame book_frame = new Frame() { Content = new BookcaseView() };
        Frame pre_frame = new Frame() { Content = new PagePre() };
        public MainWindow()
        {
            InitializeComponent();


            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (_, __) => {
                SystemCommands.CloseWindow(this);
            }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (_, __) => { SystemCommands.MinimizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (_, __) => { SystemCommands.MaximizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (_, __) => { SystemCommands.RestoreWindow(this); }));
        }

        

        private void ShowToDoList(object sender, RoutedEventArgs e)
        {
            mainContent.Content = todo_frame;
        }

        private void ShowFitnessList(object sender, RoutedEventArgs e)
        {
            mainContent.Content = fit_frame;
        }

        private void ShowBookcaseList(object sender, RoutedEventArgs e)
        {
            mainContent.Content = book_frame;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Content = pre_frame;
        }
    }
}
