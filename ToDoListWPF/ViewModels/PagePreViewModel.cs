using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
//using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace ToDoListWPF.ViewModels
{
    public class PagePreViewModel:BindableBase
    {
        private string filepath;

        public PagePreViewModel()
        {
            //SelectFilePath = new DelegateCommand(SelectFilePathMethod);
            SelectFilePath = new DelegateCommand(SaveFile);
        }

        public DelegateCommand SelectFilePath { get; set; }
        public void SelectFilePathMethod_OpenFolder()
        {
            //浏览文件的位置，打开的是文件夹
            //FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.ShowDialog();
            //FilePath = fbd.SelectedPath;
        }

        public void SelectFilePathMethod()
        {
            //打开文件的位置
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            //ofd.Filter = "Excel|*.xls;*.xlsx";
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; ;
            //FilePath = ofd.FileName;
        }

        public void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            //sfd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            FilePath = sfd.FileName;
        }

        public string FilePath
        {
            get { return filepath; }
            set { filepath = value; RaisePropertyChanged(); }
        }
    }

	

}
