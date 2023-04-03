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
using System.Windows.Threading;
using ToDoListWPF.Extensions;

namespace ToDoListWPF.ViewModels
{
    public class PagePreViewModel:BindableBase
    {
        private string filepath;

        public PagePreViewModel()
        {
            DT = new DispatcherTimer();
            //SelectFilePath = new DelegateCommand(SelectFilePathMethod);
            SelectFilePath = new DelegateCommand(SaveFile);
            StartConcentrateCmd = new DelegateCommand(StartConcentrateMethod);
            StopConcentrateCmd = new DelegateCommand(StopConcentrateMethod);

            InfoVoiceCmd = new DelegateCommand(InfoVoiceMethod);
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

        private DispatcherTimer dispatcherTimer;

        public DispatcherTimer DT
        {
            get { return dispatcherTimer; }
            set { dispatcherTimer = value; }
        }


        public DelegateCommand StartConcentrateCmd { get; set; }
        public void StartConcentrateMethod()
        {
            //DispatcherTimer dispatcherTimer = new DispatcherTimer();
            DT.Interval = new TimeSpan(0,0,1);
            DateTime starttime = DateTime.Now;
            StartTime = starttime;
            DT.Tick += timer_Tick;
            DT.Start();
        }

        public DelegateCommand StopConcentrateCmd { get; set; }
        public void StopConcentrateMethod()
        {
            DT.Stop();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            DateTime endtime = DateTime.Now;
            var lasttime = endtime - StartTime;
            ConcentrateTime = lasttime.ToString(@"hh\:mm\:ss");
        }

        private string concentime;

        public string ConcentrateTime
        {
            get { return concentime; }
            set { concentime = value; RaisePropertyChanged(); }
        }

        private DateTime start;

        public DateTime StartTime
        {
            get { return start; }
            set { start = value; RaisePropertyChanged(); }
        }

        public DelegateCommand InfoVoiceCmd { get; set; }
        public void InfoVoiceMethod()
        {
            BeepUp.Beep(700, 200);
        }
    }

	

}
