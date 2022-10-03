using Prism.Ioc;
using System.Windows;
using ToDoListWPF.Views;
using total.ViewModels.Dialogs;
using total.Views.Dialogs;

namespace ToDoListWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<OperationMessage, OperationMessageViewModel>("NotificationDialog");
        }
    }
}
