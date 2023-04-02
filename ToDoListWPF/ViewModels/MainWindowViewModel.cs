using Prism.Mvvm;

namespace ToDoListWPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Daily Life";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
