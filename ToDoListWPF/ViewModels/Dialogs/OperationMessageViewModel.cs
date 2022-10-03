using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace total.ViewModels.Dialogs
{
    public class OperationMessageViewModel : BindableBase, IDialogAware
    {

        private string message;

        public OperationMessageViewModel()
        {
            Title = "MessageInfo";
            CloseDialogCmd = new DelegateCommand(CloseDialogMethod);
        }

        public DelegateCommand CloseDialogCmd { get ; set; }

        private void CloseDialogMethod()
        {
            
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            
        }


        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;

        }


        /// <summary>
        /// 关闭对话框时，触发的内容（自定义保存）
        /// </summary>
        public void OnDialogClosed()
        {
            
        }

        /// <summary>
        /// 打开之前，允许接收的参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            var param = parameters.GetValue<string>("MessageInfo");
            Message = param;
        }
    }
}
