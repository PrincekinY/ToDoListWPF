using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ToDoListWPF.Dao;
using ToDoListWPF.Models;
using ToDoListWPF.Views;

namespace ToDoListWPF.ViewModels
{
    public class AttentionViewModel:BindableBase
    {
        private readonly MysqlDBCon dbCon;
        private readonly string loginID;

        public AttentionViewModel()
        {
            dbCon = new MysqlDBCon();
            loginID = ConfigurationManager.AppSettings["loginAccount"];
            AttentionDT = new DispatcherTimer();
            CurAttentionRecord = new AttentionRecord();
            ShowCurLastTime = new TimeSpan();
            AllAttention = new ObservableCollection<AttentionProject>();
            AllAttention = GetAllAttentions();
            CanOpenConcentrationWindow = true; //

            IsRightDrawerOpen = false;
            CurrentAttentionProject = new AttentionProject();
            CurrentAttentionProject.ProjectType = 1;
            OpenAddCmd = new DelegateCommand(OpenAddMethod);
            OpenEditCmd = new DelegateCommand<AttentionProject>(OpenEditMethod);
            DeleteAttentionProjectCmd = new DelegateCommand<AttentionProject>(DeleteAttentionProjectMethod);
            AddAttentionProject = new DelegateCommand(AddAttentionProjectMethod);
            EditAttentionProject = new DelegateCommand(EditAttentionProjectMethod);
            RestartAttentionRecord = new DelegateCommand<AttentionProject>(RestartAttentionRecordMethod);
            EventBus.EventAggregatorInstance.GetEvent<DispatcherTimerWindowCloseEvent>().Subscribe(GetConcentrationWindowStatus);
        }

        private ObservableCollection<AttentionProject> allAttention;

        public ObservableCollection<AttentionProject> AllAttention
        {
            get { return allAttention; }
            set { allAttention = value; RaisePropertyChanged(); }
        }

        private string drawerTitle;

        public string DrawerTitle
        {
            get { return drawerTitle; }
            set { drawerTitle = value; RaisePropertyChanged(); }
        }


        private AttentionProject attentionProject;

        public AttentionProject CurrentAttentionProject
        {
            get { return attentionProject; }
            set { attentionProject = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<AttentionProject> GetAllAttentions()
        {
            ObservableCollection<AttentionProject> res = new ObservableCollection<AttentionProject>();
            string sql = "select * from attention_project where operator='" + loginID + "'";
            try
            {
                IDataReader dr = dbCon.sqlRead(sql);
                while (dr.Read())
                {
                    string id = dr["attentionID"].ToString();
                    string name = dr["attentionName"].ToString();
                    string des = dr["attentionDes"].ToString();
                    int type = Int16.Parse(dr["attentionType"].ToString());
                    DateTime inverseTime = DateTime.Parse(dr["inverseTime"].ToString());
                    res.Add(new AttentionProject() { ID = id, ProjectName = name, ProjectDes = des, ProjectType = type, InverseTime = inverseTime });
                }
                dr.Close();
            }
            catch {  }
            return res;
        }


        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand OpenAddCmd { get; set; }
        private void OpenAddMethod()
        {
            DrawerTitle = "添加专注项目";
            CurrentAttentionProject.ProjectName = "";
            CurrentAttentionProject.ProjectDes = "";
            CurrentAttentionProject.ProjectType = 1;
            IsRightDrawerOpen = true;
            AddBtnVisibility = Visibility.Visible;
            EditBtnVisibility = Visibility.Collapsed;

        }

        private Visibility addBtnVisibility;

        public Visibility AddBtnVisibility
        {
            get { return addBtnVisibility; }
            set { addBtnVisibility = value; RaisePropertyChanged(); }
        }

        private Visibility editBtnVisibilit;

        public Visibility EditBtnVisibility
        {
            get { return editBtnVisibilit; }
            set { editBtnVisibilit = value; RaisePropertyChanged(); }
        }



        public DelegateCommand<AttentionProject> OpenEditCmd { get; set; }
        private void OpenEditMethod(AttentionProject obj)
        {
            DrawerTitle = "修改专注项目";
            CurrentAttentionProject.ID = obj.ID;
            CurrentAttentionProject.ProjectName = obj.ProjectName;
            CurrentAttentionProject.ProjectDes = obj.ProjectDes;
            CurrentAttentionProject.ProjectType = obj.ProjectType;

            IsRightDrawerOpen = true;
            AddBtnVisibility = Visibility.Collapsed;
            EditBtnVisibility = Visibility.Visible;
        }

        public DelegateCommand<AttentionProject> DeleteAttentionProjectCmd { get; set; }
        private void DeleteAttentionProjectMethod(AttentionProject obj)
        {
            MessageBoxResult vr = MessageBox.Show("确认删除专注项目？（同时删除该项目对应的所有专注记录！）", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (vr == MessageBoxResult.OK) // 如果是确定，就执行下面代码，记得换上自己的代码喔
            {
                string sql = "delete from attention_project where attentionID='" + obj.ID + "'";
                try
                {
                    int tow = dbCon.sqlExcute(sql);
                    if (tow > 0)
                    {
                        AllAttention.Remove(obj);
                        MessageBox.Show("删除成功。");
                    }
                }
                catch { MessageBox.Show("删除失败。"); }
            }
            
        }

        public DelegateCommand AddAttentionProject { get; set; }
        public void AddAttentionProjectMethod()
        {
            string name = CurrentAttentionProject.ProjectName;
            string des = CurrentAttentionProject.ProjectDes;
            int type = CurrentAttentionProject.ProjectType;
            if (type == 1) { CurrentAttentionProject.InverseTime = new DateTime(2000,1,1,0,0,0) ; }
            else { 
                if (CurrentAttentionProject.InverseTime.Hour==0&& CurrentAttentionProject.InverseTime.Minute < 5)
                {
                    MessageBox.Show("倒计时时间小于5min，请重新设置。");
                }
            }
            string id = Guid.NewGuid().ToString();
            string sql = "insert into attention_project values('" + id + "','" + name + "','" + des + "','" + type + "','" + CurrentAttentionProject.InverseTime.ToString() + "','" + DateTime.Now.ToString() + "','" + loginID + "')";
            try
            {
                int row = dbCon.sqlExcute(sql);
                if (row > 0)
                {
                    IsRightDrawerOpen = false;
                    AllAttention.Add(new AttentionProject() { ID = id, ProjectName = name, ProjectDes = des, ProjectType = type, InverseTime = CurrentAttentionProject.InverseTime });
                    MessageBox.Show("添加成功");
                }
            }
            catch { MessageBox.Show("添加失败"); }

        }

        public DelegateCommand EditAttentionProject { get; set; }
        public void EditAttentionProjectMethod()
        {
            string name = CurrentAttentionProject.ProjectName;
            string des = CurrentAttentionProject.ProjectDes;
            int type = CurrentAttentionProject.ProjectType;
            if (type == 1) { CurrentAttentionProject.InverseTime = new DateTime(2000, 1, 1, 0, 0, 0); }
            else
            {
                if (CurrentAttentionProject.InverseTime.Hour == 0 && CurrentAttentionProject.InverseTime.Minute < 0)
                {
                    MessageBox.Show("倒计时时间小于5min，请重新设置。");
                }
            }
            string id = CurrentAttentionProject.ID;
            string sql = "update attention_project set attentionName='" + name + "',attentionDes='" + des + "',attentionType='" + type + "'," +
                "inverseTime='" + CurrentAttentionProject.InverseTime + "',changeTime='" + DateTime.Now + "' where attentionID='" + id + "'";
            try
            {
                int row = dbCon.sqlExcute(sql);
                if (row > 0)
                {
                    AllAttention.Remove(AllAttention.FirstOrDefault(p => p.ID == id));
                    AllAttention.Add(new AttentionProject() { ID = id, ProjectName = name, ProjectDes = des, ProjectType = type, InverseTime = CurrentAttentionProject.InverseTime });
                    IsRightDrawerOpen = false;
                    MessageBox.Show("修改成功");
                }
            }
            catch { MessageBox.Show("修改失败"); }

        }

        public DelegateCommand<AttentionProject> RestartAttentionRecord { get; set; }
        private void RestartAttentionRecordMethod(AttentionProject obj)
        {
            //目前的问题是，数据的显示不是实时的，需要每次点击才会实时刷新
            //正向计时
            //AttentionDT.Interval = new TimeSpan(0, 0, 1);
            //DateTime starttime = DateTime.Now;
            //ShowCurStartTime = starttime;
            //AttentionDT.Tick += timer_Tick;
            //AttentionDT.Start();
            //obj.CurRealTime += ShowCurLastTime;
            if (CanOpenConcentrationWindow)
            {
                if (obj.ProjectType == 1)
                {
                    AttentionDispatcherTimerView view = new AttentionDispatcherTimerView();
                    AttentionDispatcherTimerViewModel viewModel = new AttentionDispatcherTimerViewModel(obj);
                    view.DataContext = viewModel;
                    
                    view.Show();
                    CanOpenConcentrationWindow = false;
                }
                else
                {
                    InverseDispatcherTimerView view = new InverseDispatcherTimerView();
                    InverseDispatcherTimerViewModel viewModel = new InverseDispatcherTimerViewModel(obj);
                    view.DataContext = viewModel;
                    view.Show();
                    CanOpenConcentrationWindow = false;
                }
            }
            else
            {
                MessageBox.Show("已经打开了一个专注，结束后才能开启其他专注！");
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            DateTime endtime = DateTime.Now;
            var lasttime = endtime - ShowCurStartTime;
            ShowCurLastTime = lasttime;
        }

        public int JudgeConcentrateOperation(string id)
        {
            //判断是否当前存在其他的专注
            if (StartConcentrate)
            {
                if (CurAttentionRecord.ProjectID == id) {
                    //可以重新专注，需要存储或清空上一条专注，重新开启
                    return 2;
                }
                else { //别的项目正在占用，不可以开启
                    MessageBox.Show("当前正在专注计时，不能开启新的专注项目。"); return 0; }
            }
            else //如果没有其他的专注项目可以正常开启
            {
                return 1;
            }
        }

        private DispatcherTimer dispatcherTimer;

        public DispatcherTimer AttentionDT
        {
            get { return dispatcherTimer; }
            set { dispatcherTimer = value; }
        }


        private AttentionRecord curattentionrecord;

        public AttentionRecord CurAttentionRecord
        {
            get { return curattentionrecord; }
            set { curattentionrecord = value; RaisePropertyChanged(); }
        }

        private bool startConcentrate;

        public bool StartConcentrate
        {
            get { return startConcentrate; }
            set { startConcentrate = value; RaisePropertyChanged(); }
        }

        private TimeSpan showCurLastTime;

        public TimeSpan ShowCurLastTime
        {
            get { return showCurLastTime; }
            set { showCurLastTime = value; RaisePropertyChanged(); }
        }

        private DateTime showCurStartTime;

        public DateTime ShowCurStartTime
        {
            get { return showCurStartTime; }
            set { showCurStartTime = value; RaisePropertyChanged(); }
        }

        private bool canOpenConcentrationWindow;

        public bool CanOpenConcentrationWindow
        {
            get { return canOpenConcentrationWindow; }
            set { canOpenConcentrationWindow = value; RaisePropertyChanged(); }
        }

        public void GetConcentrationWindowStatus(bool obj)
        {
            CanOpenConcentrationWindow = true;
        }

    }
}
