using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoListWPF.Dao;
using ToDoListWPF.Models;

namespace ToDoListWPF.ViewModels
{
    public class FitnessViewModel:BindableBase
    {
        public FitnessViewModel()
        {
            //初始化
            CurrentFit = new Fitness();
            loginID = ConfigurationManager.AppSettings["loginAccount"];
            SelectMonth = DateTime.Now.Month;
            Allfit = new ObservableCollection<Fitness>();
            GetAllfit();

            OpenAddFitCmd = new DelegateCommand(OpenAddFitMethod);
            AddFitCmd = new DelegateCommand(AddFitMethod);
            DeleteFitCmd = new DelegateCommand<Fitness>(DeleteFitMethod);
            EditFitCmd = new DelegateCommand(EditFitMethod); 
            OpenEditFitCmd = new DelegateCommand<Fitness>(OpenEditFitMethod);
            FindSpecificDateFitCmd = new DelegateCommand(FindSpecificDateFitMethod);
        }

        public void GetAllfit()
        {
            DateTime dateTime = DateTime.Now;
            Allfit = GetFitByMonth(dateTime);
        }

        public ObservableCollection<Fitness> GetFitByMonth(DateTime dateTime)
        {
            //DateTime dateTime = DateTime.Now;
            ObservableCollection<Fitness> fitnesses = new ObservableCollection<Fitness>();
            var year = dateTime.Year;
            var month = dateTime.Date.Month;
            var next_month = dateTime.AddMonths(1).Month;
            var month_date = Convert.ToDateTime(year+"-"+month+"-01");
            var nextmonth_date = Convert.ToDateTime(year + "-" + next_month + "-01").AddDays(-1);
            string sql = "select * from fitnesslist where fitnessDay between '"+month_date+"' and '"+nextmonth_date+ "' ORDER BY fitnessDay";
            MysqlDBCon dBCon = new MysqlDBCon();
            IDataReader dr = dBCon.sqlRead(sql);
            while (dr.Read())
            {
                fitnesses.Add(new Fitness()
                {
                    ID = dr["fitnessID"].ToString(),
                    FitDay = DateTime.Parse(dr["fitnessDay"].ToString()).Date,
                    Weight = float.Parse(dr["weight"].ToString()),
                    Breakfast = dr["breakfast"].ToString(),
                    Lunch = dr["lunch"].ToString(),
                    Dinner = dr["dinner"].ToString(),
                    Sport = dr["sport"].ToString(),
                    Hipline = float.Parse(dr["hipline"].ToString()),
                    Waistline = float.Parse(dr["waistline"].ToString()),
                    Belly = float.Parse(dr["belly"].ToString()),
                    Bustline = float.Parse(dr["bustline"].ToString()),
                    Calfgirth = float.Parse(dr["calfgirth"].ToString()),
                    Thigh = float.Parse(dr["thigh"].ToString())
                });
            }
            return fitnesses;
        }

        

        private Fitness currentFit;
        private readonly IDialogService _dialogService;

        public Fitness CurrentFit
        {
            get { return currentFit; }
            set { currentFit = value;RaisePropertyChanged(); }
        }

        private readonly string loginID;
        private ObservableCollection<Fitness> allfit;

        public ObservableCollection<Fitness> Allfit
        {
            get { return allfit; }
            set { allfit = value; RaisePropertyChanged(); }
        }



        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        public DelegateCommand OpenAddFitCmd { get; private set; }
        private void OpenAddFitMethod()
        {
            IsRightDrawerOpen = true;
            DrawerTitle = "Add Fit";
            AddBtnVisibility = Visibility.Visible;
            EditBtnVisibility = Visibility.Collapsed;

            CurrentFit.FitDay = DateTime.Now;
        }

        public DelegateCommand AddFitCmd { get; private set; }
        private void AddFitMethod()
        {
            var currentfit_list = GetFitList();
            string sql_data = String.Join("','",currentfit_list);
            string sql = "insert into fitnesslist values('"+sql_data+"')";
            Console.WriteLine(sql);
            MysqlDBCon dBCon = new MysqlDBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0)
                {
                    CallMessageBox("Suceess to add.");
                    IsRightDrawerOpen = false;
                }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public void CallMessageBox(string message)
        {
            DialogParameters param = new DialogParameters();
            param.Add("MessageInfo", message);
            _dialogService.ShowDialog("NotificationDialog", param, arg => { });
        }

        public List<string> GetFitList()
        {
            List<string> list = new List<string>();
            list.Add(Guid.NewGuid().ToString());
            list.Add(CurrentFit.FitDay.Date.ToString());
            list.Add(CurrentFit.Weight.ToString());
            list.Add(CurrentFit.Breakfast);
            list.Add(CurrentFit.Lunch);
            list.Add(CurrentFit.Dinner);
            list.Add(CurrentFit.Sport);
            list.Add(CurrentFit.Hipline.ToString());
            list.Add(CurrentFit.Waistline.ToString());
            list.Add(CurrentFit.Belly.ToString());
            list.Add(CurrentFit.Bustline.ToString());
            list.Add(CurrentFit.Calfgirth.ToString());
            list.Add(CurrentFit.Thigh.ToString());
            return list;
        }

        public List<string> GetFitListExceptID()
        {
            List<string> list = new List<string>();
            list.Add(CurrentFit.FitDay.Date.ToString());
            list.Add(CurrentFit.Weight.ToString());
            list.Add(CurrentFit.Breakfast);
            list.Add(CurrentFit.Lunch);
            list.Add(CurrentFit.Dinner);
            list.Add(CurrentFit.Sport);
            list.Add(CurrentFit.Hipline.ToString());
            list.Add(CurrentFit.Waistline.ToString());
            list.Add(CurrentFit.Belly.ToString());
            list.Add(CurrentFit.Bustline.ToString());
            list.Add(CurrentFit.Calfgirth.ToString());
            list.Add(CurrentFit.Thigh.ToString());
            return list;
        }

        private Visibility addBtnVisibility;

        public Visibility AddBtnVisibility
        {
            get { return addBtnVisibility; }
            set { addBtnVisibility = value; RaisePropertyChanged(); }
        }

        private Visibility editBtnVisibility;

        public Visibility EditBtnVisibility
        {
            get { return editBtnVisibility; }
            set { editBtnVisibility = value; RaisePropertyChanged(); }
        }

        private string drawerTitle;

        public string DrawerTitle
        {
            get { return drawerTitle; }
            set { drawerTitle = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<Fitness> DeleteFitCmd { get; private set; }
        private void DeleteFitMethod(Fitness obj)
        {
            string tid = obj.ID;
            string sql = "delete from todolist where todoID='" + tid + "'";
            MysqlDBCon dBCon = new MysqlDBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { Allfit.Remove(obj); CallMessageBox("Success to delete."); }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public DelegateCommand EditFitCmd { get; private set; }
        private void EditFitMethod()
        {
            var list = GetFitListExceptID();
            string fid = CurrentFit.ID;
            string sql_data = string.Join("','", list);
            //var todo = new Todos() { TodoID = tid, TodoName = tname, TodoDes = tdes, TodoDay = tdate, TodoStatus = tstatus };
            //string sql = "update todolist set(fitnessDay,weight,breakfast,lunch,dinner,sport,hipline,waistline,belly,bustline,calfgirth,thigh) values('"+sql_data+"') where fitnessID='" + fid + "'";
            string sql = String.Format("update fitnesslist set fitnessDay='{0}',weight='{1}',breakfast='{2}',lunch='{3}',dinner='{4}',sport='{5}',hipline='{6}',waistline='{7}',belly='{8}',bustline='{9}',calfgirth='{10}',thigh='{11}' where fitnessID='{12}'",
                list[0],list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9], list[10], list[11], fid);
            MysqlDBCon dBCon = new MysqlDBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0)
                {
                    var tindex = Allfit.IndexOf(Allfit.First(p => p.ID == fid));
                    Allfit[tindex].FitDay = CurrentFit.FitDay;
                    Allfit[tindex].Weight = CurrentFit.Weight;
                    Allfit[tindex].Breakfast = CurrentFit.Breakfast;
                    Allfit[tindex].Lunch = CurrentFit.Lunch;
                    Allfit[tindex].Dinner = CurrentFit.Dinner;
                    Allfit[tindex].Sport = CurrentFit.Sport;
                    Allfit[tindex].Hipline = CurrentFit.Hipline;
                    Allfit[tindex].Waistline = CurrentFit.Waistline;
                    Allfit[tindex].Belly = CurrentFit.Belly;
                    Allfit[tindex].Bustline = CurrentFit.Bustline;
                    Allfit[tindex].Calfgirth = CurrentFit.Calfgirth;
                    Allfit[tindex].Thigh = CurrentFit.Thigh;
                    CallMessageBox("Success to edit.");
                    IsRightDrawerOpen = false;
                }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public DelegateCommand<Fitness> OpenEditFitCmd { get; private set; }
        private void OpenEditFitMethod(Fitness obj)
        {
            IsRightDrawerOpen = true;
            DrawerTitle = "Edit Fit";
            AddBtnVisibility = Visibility.Collapsed;
            EditBtnVisibility = Visibility.Visible;
            CurrentFit.ID = obj.ID;
            CurrentFit.FitDay = obj.FitDay;
            CurrentFit.Weight = obj.Weight;
            CurrentFit.Breakfast = obj.Breakfast;
            CurrentFit.Lunch = obj.Lunch;
            CurrentFit.Dinner = obj.Dinner;
            CurrentFit.Sport = obj.Sport;
            CurrentFit.Hipline = obj.Hipline;
            CurrentFit.Waistline = obj.Waistline;
            CurrentFit.Belly = obj.Belly;
            CurrentFit.Bustline = obj.Bustline;
            CurrentFit.Calfgirth = obj.Calfgirth;
            CurrentFit.Thigh = obj.Thigh;
        }

        public DelegateCommand FindSpecificDateFitCmd { get; private set; }
        private void FindSpecificDateFitMethod()
        {
            int month = SelectMonth;
            try
            {
                DateTime dateTime = DateTime.Now;
                var year = dateTime.Year;
                var month_date = DateTime.Parse(year + "-" + month + "-01");
                Allfit = GetFitByMonth(month_date);
            }
            catch { CallMessageBox("查找失败啦！"); }
        }

        public IEnumerable<int> Months => new[] { 1,2,3,4,5,6,7,8,9, 10, 11, 12 };

        private int selectMonth;

        public int SelectMonth
        {
            get { return selectMonth; }
            set { selectMonth = value; RaisePropertyChanged(); }
        }

    }
}
