using System.Windows;
using System.Windows.Controls;

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
            

            ///重写最大最小化和关闭
            btnWinMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnWinMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnWinClose.Click += (s, e) => {
                this.Close();
            };

            ///拖动程序
            topNav.MouseMove += (s, e) =>
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();

            };
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
