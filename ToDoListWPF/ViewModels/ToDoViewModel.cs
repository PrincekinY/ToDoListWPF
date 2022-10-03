using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ToDoViewModel(IDialogService dialogService)
        {
            //初始化
            _dialogService = dialogService;
            IsRightDrawerOpen = false;
            SelectedDate = DateTime.Now.Date;
            TodoSet = GetTodayTodos();
            CurrentTodo = new Todos();
            

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
            CurrentTodo.TodoID = obj.TodoID;
        }

        public DelegateCommand AddTodoCmd { get; private set; }
        private void AddTodoMethod()
        {
            string tid = Guid.NewGuid().ToString();
            string tname = CurrentTodo.TodoName;
            string tdes = CurrentTodo.TodoDes;
            var tdate = CurrentTodo.TodoDay.Date;
            int tstatus = 0;
            string sql = "insert into todolist values('"+tid+"','"+tname+"','"+tdes+"','"+ tstatus + "','"+tdate+"')";
            DBCon dBCon = new DBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { 
                    TodoSet.Add(new Todos() { TodoID=tid,TodoName=tname,TodoDes=tdes,TodoDay=tdate,TodoStatus=false});
                    CallMessageBox("Suceess to add."); }
            }
            catch { CallMessageBox("Fail to connect."); }
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
            string sql = "select * from todolist where todoDay='" + specific_date + "'";
            DBCon dBCon = new DBCon();
            IDataReader dr = dBCon.sqlRead(sql);
            while (dr.Read())
            {
                var id = dr["todoID"].ToString();
                var name = dr["todoName"].ToString();
                var des = dr["todoDes"].ToString();
                var day = DateTime.Parse(dr["todoDay"].ToString());
                var status = IntToBool(Int32.Parse(dr["todoStatus"].ToString()));
                Todos t = new Todos() { TodoID = id, TodoName = name, TodoDes = des, TodoDay = day, TodoStatus = status };
                todoset.Add(t);
            }
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
            string tid = obj.TodoID;
            string sql = "delete from todolist where todoID='" + tid + "'";
            DBCon dBCon=new DBCon();
            try {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { TodoSet.Remove(obj); CallMessageBox("Success to delete."); }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public DelegateCommand EditTodoCmd { get; private set; }
        private void EditTodoMethod()
        {
            string tid = CurrentTodo.TodoID;
            string tname = CurrentTodo.TodoName;
            string tdes = CurrentTodo.TodoDes;
            var tdate = CurrentTodo.TodoDay;
            var tstatus = CurrentTodo.TodoStatus;
            var status_int = BoolToInt(tstatus);
            //var todo = new Todos() { TodoID = tid, TodoName = tname, TodoDes = tdes, TodoDay = tdate, TodoStatus = tstatus };
            string sql = "update todolist set todoName='"+tname+"',todoDes='"+tdes+"',todoDay='"+tdate+"',todoStatus='"+ status_int + "' where todoID='" + tid + "'";
            DBCon dBCon = new DBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { 
                    var tindex = TodoSet.IndexOf(TodoSet.First(p=>p.TodoID==tid));
                    TodoSet[tindex].TodoName = tname;
                    TodoSet[tindex].TodoDes = tdes;
                    TodoSet[tindex].TodoDay = tdate;
                    TodoSet[tindex].TodoStatus = tstatus;
                    CallMessageBox("Success to edit."); }
            }
            catch { CallMessageBox("Fail to connect."); }
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
            var tid = obj.TodoID;
            string sql = "update todolist set todoStatus='1' where todoID='" + tid + "'";
            //Console.WriteLine(sql);
            DBCon dBCon = new DBCon();
            dBCon.sqlExcute(sql);
        }

        public DelegateCommand<Todos> ChangeTodoUncheckCmd { get; private set; }
        private void ChangeTodoUncheckMethod(Todos obj)
        {
            var tid = obj.TodoID;
            string sql = "update todolist set todoStatus='0' where todoID='" + tid + "'";
            //Console.WriteLine(sql);
            DBCon dBCon = new DBCon();
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
