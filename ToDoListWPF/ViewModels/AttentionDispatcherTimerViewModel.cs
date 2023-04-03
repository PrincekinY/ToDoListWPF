using Mysqlx.Crud;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;
using ToDoListWPF.Dao;
using ToDoListWPF.Models;

namespace ToDoListWPF.ViewModels
{
    public class AttentionDispatcherTimerViewModel : BindableBase
    {
        private string _title = "Concentration";
        private AttentionProject thisProject;
        private readonly string loginID;

        public AttentionProject ThisProject
        {
            get { return thisProject; }
            set { thisProject = value; RaisePropertyChanged(); }
        }


        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public AttentionDispatcherTimerViewModel(AttentionProject project)
        {
            loginID = ConfigurationManager.AppSettings["loginAccount"];
            ThisProject = project;
            Console.WriteLine(ThisProject.ID);

            DT = new DispatcherTimer();
            StopPeriod = new TimeSpan();
            OnPeriod = new TimeSpan();
            CanRestart = true;
            CanStop = false;

            //初始化显示LastTime
            var startlast = ThisProject.InverseTime - new DateTime(ThisProject.InverseTime.Year, ThisProject.InverseTime.Month, ThisProject.InverseTime.Day, 0, 0, 0);
            LastTime = startlast.ToString(@"hh\:mm\:ss");

            RestartConcentration = new DelegateCommand(RestartConcentrationMethod);
            StopConcentrateCmd = new DelegateCommand(StopConcentrateMethod);
            SaveConcentration = new DelegateCommand(SaveConcentrationMethod);
        }

        private string lastTime;

        public string LastTime
        {
            get { return lastTime; }
            set { lastTime = value; RaisePropertyChanged(); }
        }

        public DelegateCommand RestartConcentration { get; set; }
        public void RestartConcentrationMethod()
        {
            DT.Interval = new TimeSpan(0,0, 1);
            DateTime starttime = DateTime.Now;
            StartTime = starttime;
            DT.Tick += timer_Tick;
            DT.Start();
            CanRestart = false;
            CanStop = true;
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            DateTime endtime = DateTime.Now;
            var lasttime = endtime - StartTime;
            OnPeriod = lasttime;
            LastTime = (lasttime+StopPeriod).ToString(@"hh\:mm\:ss");
        }

        private DispatcherTimer dispatcherTimer;

        public DispatcherTimer DT
        {
            get { return dispatcherTimer; }
            set { dispatcherTimer = value; }
        }

        private DateTime dateTime;

        public DateTime StartTime
        {
            get { return dateTime; }
            set { dateTime = value; RaisePropertyChanged(); }
        }

        public DelegateCommand StopConcentrateCmd { get; set; }
        public void StopConcentrateMethod()
        {
            DT.Stop();
            StopPeriod += OnPeriod;
            CanStop = false;
            CanRestart = true;
        }

        private TimeSpan stopPeriod;

        public TimeSpan StopPeriod
        {
            get { return stopPeriod; }
            set { stopPeriod = value; RaisePropertyChanged(); }
        }

        private TimeSpan onPeriod;

        public TimeSpan OnPeriod
        {
            get { return onPeriod; }
            set { onPeriod = value; RaisePropertyChanged(); }
        }

        private bool canrestart;

        public bool CanRestart
        {
            get { return canrestart; }
            set { canrestart = value; RaisePropertyChanged(); }
        }

        private bool canStop;

        public bool CanStop
        {
            get { return canStop; }
            set { canStop = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SaveConcentration { get; set; }
        public void SaveConcentrationMethod()
        {
            MessageBoxResult vr = MessageBox.Show("确认存储专注记录？", "提示", MessageBoxButton.OKCancel);
            if (vr == MessageBoxResult.OK) // 如果是确定，就执行下面代码，记得换上自己的代码喔
            {
                //处理时间
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,0,0,0);
                var res = LastTime.Split(":");
                string id = Guid.NewGuid().ToString();
                var lasttime = new DateTime(dt.Year, dt.Month, dt.Day, Int16.Parse(res[0]), Int16.Parse(res[1]), Int16.Parse(res[2]));
                string sql = "insert into attention_record values('"+id+"','"+dt+"','"+ lasttime + "','"+DateTime.Now+"','"+loginID+"','"+ThisProject.ID+"')";
                MysqlDBCon mysqlDBCon = new MysqlDBCon();
                try
                {
                    int row = mysqlDBCon.sqlExcute(sql);
                    if (row > 0)
                    {
                        MessageBox.Show("保存成功！");
                        //重置
                        StopConcentrateMethod();
                        StopPeriod = new TimeSpan();
                        OnPeriod = new TimeSpan();
                        LastTime = "00:00:00";
                    }
                }
                catch { MessageBox.Show("保存失败！"); }
            }

        }

    }
}
