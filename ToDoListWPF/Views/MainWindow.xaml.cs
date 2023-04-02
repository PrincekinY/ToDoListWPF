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
        Frame atten_frame = new Frame() { Content = new AttentionView() };
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
            ChangeStyle();
            Style style = (Style)this.FindResource("SelectButton");
            ToDoBtn.Style = style;
        }

        private void ShowFitnessList(object sender, RoutedEventArgs e)
        {
            mainContent.Content = fit_frame;
            ChangeStyle();
            Style style = (Style)this.FindResource("SelectButton");
            FitBtn.Style = style;
        }

        private void ShowBookcaseList(object sender, RoutedEventArgs e)
        {
            mainContent.Content = book_frame;
            ChangeStyle();
            Style style = (Style)this.FindResource("SelectButton");
            BookBtn.Style = style;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Content = pre_frame;
            ChangeStyle();
        }

        private void Attention_Click(object sender, RoutedEventArgs e)
        {
            mainContent.Content = atten_frame;
            ChangeStyle();
            Style style = (Style)this.FindResource("SelectButton");
            AttentionBtn.Style = style;
        }

        public void ChangeStyle()
        {
            Style style = (Style)this.FindResource("MaterialDesignFlatLightButton");
            ToDoBtn.Style = style;
            FitBtn.Style = style;
            BookBtn.Style = style;
            AttentionBtn.Style = style;
        }
    }
}
