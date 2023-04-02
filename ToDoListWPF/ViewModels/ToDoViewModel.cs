using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoListWPF.Dao;
using ToDoListWPF.Models;

namespace ToDoListWPF.ViewModels
{
    public class ToDoViewModel:BindableBase
    {
        public ToDoViewModel()
        {
            //初始化
            //_dialogService = dialogService;
            IsRightDrawerOpen = false;
            SelectedDate = DateTime.Now.Date;
            loginID = ConfigurationManager.AppSettings["loginAccount"];
            dBCon = new MysqlDBCon();
            
            CurrentTodo = new Todos();

            TodoSet = GetTodayTodos();
            AddTodoCmd = new DelegateCommand(AddTodoMethod);
            DeleteTodoCmd = new DelegateCommand<Todos>(DeleteTodoMethod);
            EditTodoCmd = new DelegateCommand(EditTodoMethod);
            OpenAddTodoCmd = new DelegateCommand(OpenAddTodoMethod);
            OpenEditTodoCmd = new DelegateCommand<Todos>(OpenEditTodoMethod);
            FindSpecificDateTodoCmd = new DelegateCommand(FindSpecificDateTodoMethod);
            ChangeTodoCheckCmd = new DelegateCommand<Todos>(ChangeTodoCheckMethod);
            ChangeTodoUncheckCmd = new DelegateCommand<Todos>(ChangeTodoUncheckMethod);
            ShowEfficiencyCmd = new DelegateCommand(ShowEfficiencyMethod);
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


        private Todos currentTodo;

        public Todos CurrentTodo
        {
            get { return currentTodo; }
            set { currentTodo = value; RaisePropertyChanged(); }
        }

        private readonly string loginID;
        private readonly MysqlDBCon dBCon;
        private string todoDrawerTitle;

        public string TodoDrawerTitle
        {
            get { return todoDrawerTitle; }
            set { todoDrawerTitle = value; RaisePropertyChanged(); }
        }

        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }


        public DelegateCommand OpenAddTodoCmd { get; private set; }
        private void OpenAddTodoMethod()
        {
            IsRightDrawerOpen = true;
            TodoDrawerTitle = "Add Todo";
            AddBtnVisibility = Visibility.Visible;
            EditBtnVisibility = Visibility.Collapsed;

            CurrentTodo.TodoName = "";
            CurrentTodo.TodoDes = "";
            CurrentTodo.TodoDay = DateTime.Now.Date.AddDays(1);
        }

        public DelegateCommand<Todos> OpenEditTodoCmd { get; private set; }
        private void OpenEditTodoMethod(Todos obj)
        {
            IsRightDrawerOpen = true;
            TodoDrawerTitle = "Edit Todo";
            AddBtnVisibility= Visibility.Collapsed;
            EditBtnVisibility = Visibility.Visible;
            CurrentTodo.TodoName = obj.TodoName;
            CurrentTodo.TodoDes = obj.TodoDes;
            CurrentTodo.TodoStatus = obj.TodoStatus;
            CurrentTodo.TodoDay = obj.TodoDay;
            CurrentTodo.ID = obj.ID;
        }

        public DelegateCommand AddTodoCmd { get; private set; }
        private void AddTodoMethod()
        {
            string tid = Guid.NewGuid().ToString();
            string tname = CurrentTodo.TodoName;
            string tdes = CurrentTodo.TodoDes;
            var tdate = CurrentTodo.TodoDay.Date;
            int tstatus = 0;
            string sql = "insert into todolist values('"+tid+"','"+tname+"','"+tdes+"','"+ tstatus + "','"+tdate+"','"+DateTime.Now.Date+"','"+loginID+"')";
            
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0 && tdate==SelectedDate.Date) { 
                    TodoSet.Add(new Todos() { ID=tid,TodoName=tname,TodoDes=tdes,TodoDay=tdate,TodoStatus=false});
                    MessageBox.Show("添加成功。"); }
                IsRightDrawerOpen = false;
            }
            catch { MessageBox.Show("添加失败。"); }
        }

        public void CallMessageBox(string message)
        {
            DialogParameters param = new DialogParameters();
            param.Add("MessageInfo", message);
            _dialogService.ShowDialog("NotificationDialog", param, arg => { });
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<Todos> todoset;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Todos> TodoSet
        {
            get { return todoset; }
            set { todoset = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<Todos> GetTodayTodos()
        {
            ObservableCollection<Todos> todoset;
            DateTime today = DateTime.Now;
            todoset = GetDateTodos(today);
            return todoset;
        }

        public ObservableCollection<Todos> GetDateTodos(DateTime dateTime)
        {
            ObservableCollection<Todos> todoset = new ObservableCollection<Todos>();
            var specific_date = dateTime.Date;
            string sql = "select * from todolist where todoDate='" + specific_date + "' and operator='"+loginID+"'";

            IDataReader dr = dBCon.sqlRead(sql);
            while (dr.Read())
            {
                var id = dr["todoID"].ToString();
                var name = dr["todoName"].ToString();
                var des = dr["todoDes"].ToString();
                var status = IntToBool(Int32.Parse(dr["todoStatus"].ToString()));
                Todos t = new Todos() { ID = id, TodoName = name, TodoDes = des, TodoDay=specific_date, TodoStatus = status };
                todoset.Add(t);
            }
            dr.Close();
            return todoset;
        }

        public bool IntToBool(int i)
        {
            if(i == 0) { return false; }
            else { return true; }
        }

        public int BoolToInt(bool i)
        {
            if (i) { return 1; }
            else { return 0; }
        }

        public DelegateCommand<Todos> DeleteTodoCmd { get;private set; }
        private void DeleteTodoMethod(Todos obj)
        {
            string tid = obj.ID;
            string sql = "delete from todolist where todoID='" + tid + "'";

            try {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { TodoSet.Remove(obj); MessageBox.Show("删除成功。"); }
            }
            catch { MessageBox.Show("删除失败。"); }
        }

        public DelegateCommand EditTodoCmd { get; private set; }
        private void EditTodoMethod()
        {
            string tid = CurrentTodo.ID;
            string tname = CurrentTodo.TodoName;
            string tdes = CurrentTodo.TodoDes;
            var tdate = CurrentTodo.TodoDay.Date;
            var tstatus = CurrentTodo.TodoStatus;
            var status_int = BoolToInt(tstatus);
            //var todo = new Todos() { TodoID = tid, TodoName = tname, TodoDes = tdes, TodoDay = tdate, TodoStatus = tstatus };
            string sql = "update todolist set todoName='"+tname+"',todoDes='"+tdes+"',todoDate='"+tdate+"',todoStatus='"+ status_int + "',changeTime='"+DateTime.Now.Date+"' where todoID='" + tid + "'";

            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { 
                    var tindex = TodoSet.IndexOf(TodoSet.First(p=>p.ID==tid));
                    TodoSet[tindex].TodoName = tname;
                    TodoSet[tindex].TodoDes = tdes;
                    TodoSet[tindex].TodoDay = tdate;
                    TodoSet[tindex].TodoStatus = tstatus;
                    MessageBox.Show("修改成功。"); }
                IsRightDrawerOpen = false;
            }
            catch { MessageBox.Show("修改失败。"); }
        }

        public DelegateCommand FindSpecificDateTodoCmd { get; private set; }
        private void FindSpecificDateTodoMethod()
        {
            var date = SelectedDate;
            TodoSet = GetDateTodos(date);
        }

        public DelegateCommand<Todos> ChangeTodoCheckCmd { get; private set; }
        private void ChangeTodoCheckMethod(Todos obj)
        {
            var tid = obj.ID;
            string sql = "update todolist set todoStatus='1' where todoID='" + tid + "'";
            //Console.WriteLine(sql);

            dBCon.sqlExcute(sql);
        }

        public DelegateCommand<Todos> ChangeTodoUncheckCmd { get; private set; }
        private void ChangeTodoUncheckMethod(Todos obj)
        {
            var tid = obj.ID;
            string sql = "update todolist set todoStatus='0' where todoID='" + tid + "'";
            //Console.WriteLine(sql);

            dBCon.sqlExcute(sql);
        }

        private string efficiency;

        public string Efficiency
        {
            get { return efficiency; }
            set { efficiency = value; RaisePropertyChanged(); }
        }

        public DelegateCommand ShowEfficiencyCmd { get; private set; }
        private void ShowEfficiencyMethod()
        {
            int all_todo = TodoSet.Count;
            int true_todo = TodoSet.Where(p=>p.TodoStatus==true).Count();
            Efficiency = true_todo.ToString() + " / " + all_todo.ToString(); 
        }


    }
}
