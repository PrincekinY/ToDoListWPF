using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoListWPF.Dao;
using ToDoListWPF.Models;

namespace ToDoListWPF.ViewModels
{
    public class BookcaseViewModel : BindableBase
    {
        public BookcaseViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            IsRightDrawerOpen = false;

            BookSet = GetTodayBooks();
            CurrentBook = new Books();


            AddBookCmd = new DelegateCommand(AddBookMethod);
            DeleteBookCmd = new DelegateCommand<Books>(DeleteBookMethod);
            EditBookCmd = new DelegateCommand(EditBookMethod);
            OpenAddBookCmd = new DelegateCommand(OpenAddBookMethod);
            OpenEditBookCmd = new DelegateCommand<Books>(OpenEditBookMethod);
            UploadBookImageCmd = new DelegateCommand<Books>(UploadBookImageMethod);
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


        private Books currentBook;

        public Books CurrentBook
        {
            get { return currentBook; }
            set { currentBook = value; RaisePropertyChanged(); }
        }


        private string todoDrawerTitle;

        public string BookDrawerTitle
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


        public DelegateCommand OpenAddBookCmd { get; private set; }
        private void OpenAddBookMethod()
        {
            IsRightDrawerOpen = true;
            BookDrawerTitle = "Add Book";
            AddBtnVisibility = Visibility.Visible;
            EditBtnVisibility = Visibility.Collapsed;

            CurrentBook.BookName = "";
            CurrentBook.Author = "";
            CurrentBook.Status = "";
            CurrentBook.StartRead = DateTime.Now;
            CurrentBook.EndRead = DateTime.Now;
        }

        public DelegateCommand<Books> OpenEditBookCmd { get; private set; }
        private void OpenEditBookMethod(Books obj)
        {
            IsRightDrawerOpen = true;
            BookDrawerTitle = "Edit Book";
            AddBtnVisibility = Visibility.Collapsed;
            EditBtnVisibility = Visibility.Visible;
            CurrentBook.BookName = obj.BookName;
            CurrentBook.Author = obj.Author;
            CurrentBook.Status = obj.Status;
            CurrentBook.StartRead = obj.StartRead;
            CurrentBook.EndRead = obj.EndRead;
        }

        public DelegateCommand AddBookCmd { get; private set; }
        private void AddBookMethod()
        {
            string bid = Guid.NewGuid().ToString();
            string bname = CurrentBook.BookName;
            string author = CurrentBook.Author;
            var bstart = CurrentBook.StartRead.Date;
            var bend = CurrentBook.EndRead.Date;
            string bstatus_str = CurrentBook.Status;
            int bstatus_int = StatusStrToInt(bstatus_str);
            string sql = "insert into booklist values('" + bid + "','" + bname + "','" + author + "','" + bstart + "','" + bend + "','"+bstatus_int+"')";
            DBCon dBCon = new DBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0)
                {
                    BookSet.Add(new Books() { BookID = bid, BookName = bname, Author = author, StartRead = bstart, EndRead = bend,Status=bstatus_str });
                    CallMessageBox("Suceess to add.");
                }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public int StatusStrToInt(string obj)
        {
            if (obj == "想看") { return 1; }
            if (obj == "正在看") { return 2; }
            if(obj == "已看完") { return 3; }
            else { return 0; }
        }

        public IEnumerable<string> ReadingStatus => new[] { "想看", "正在看", "已看完" };

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


        private ObservableCollection<Books> todoset;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Books> BookSet
        {
            get { return todoset; }
            set { todoset = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<Books> GetTodayBooks()
        {
            ObservableCollection<Books> todoset;
            //DateTime today = DateTime.Now;
            todoset = GetDateBooks();
            return todoset;
        }

        public ObservableCollection<Books> GetDateBooks()
        {
            ObservableCollection<Books> bookset = new ObservableCollection<Books>();
            //var specific_date = dateTime.Date;
            string sql = "select * from booklist";
            DBCon dBCon = new DBCon();
            IDataReader dr = dBCon.sqlRead(sql);
            while (dr.Read())
            {
                var id = dr["bookID"].ToString();
                var name = dr["bookName"].ToString();
                var author = dr["author"].ToString();
                var sday = DateTime.Parse(dr["startread"].ToString());
                var eday = DateTime.Parse(dr["endread"].ToString());
                var status = IntToBool(Int32.Parse(dr["status"].ToString()));
                var pic = dr["bookImage"].ToString();
                Books t = new Books() { BookID = id, BookName = name, Author = author, StartRead = sday, EndRead = eday,Status=status ,BookImage=pic};
                bookset.Add(t);
            }
            return bookset;
        }

        public string IntToBool(int i)
        {
            if (i == 1) { return "想看"; }
            if (i == 2) { return "正在看"; }
            if (i == 3) { return "已看完"; }
            else { return ""; }
        }


        public DelegateCommand<Books> DeleteBookCmd { get; private set; }
        private void DeleteBookMethod(Books obj)
        {
            string tid = obj.BookID;
            string sql = "delete from booklist where bookID='" + tid + "'";
            DBCon dBCon = new DBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0) { BookSet.Remove(obj); CallMessageBox("Success to delete."); }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public DelegateCommand EditBookCmd { get; private set; }
        private void EditBookMethod()
        {
            string id = CurrentBook.BookID;
            string name = CurrentBook.BookName;
            string author = CurrentBook.Author;
            var sdate = CurrentBook.StartRead;
            var edate = CurrentBook.EndRead;
            var tstatus = CurrentBook.Status;
            var status_int = StatusStrToInt(tstatus);
            //var todo = new Books() { BookID = tid, BookName = tname, BookDes = tdes, BookDay = tdate, BookStatus = tstatus };
            string sql = "update booklist set bookName='" + name + "',author='" + author + "',startread='" + sdate + "',endread='"+edate+"',todoStatus='" + status_int + "' where bookID='" + id + "'";
            DBCon dBCon = new DBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0)
                {
                    var tindex = BookSet.IndexOf(BookSet.First(p => p.BookID == id));
                    BookSet[tindex].BookName = name;
                    BookSet[tindex].Author = author;
                    BookSet[tindex].StartRead = sdate;
                    BookSet[tindex].EndRead = edate;
                    BookSet[tindex].Status = tstatus;
                    CallMessageBox("Success to edit.");
                }
            }
            catch { CallMessageBox("Fail to connect."); }
        }

        public DelegateCommand<Books> UploadBookImageCmd { get; set; }
        public void UploadBookImageMethod(Books obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            obj.BookImage = openFileDialog.FileName;
            //判断文件是不是对的
            DBCon dBCon = new DBCon();
            try
            {
                string sql = "update booklist set bookImage = '" + obj.BookImage + "' where bookID='" + obj.BookID + "'";
                int brow = dBCon.sqlExcute(sql);
            }
            catch { CallMessageBox("Fail to save db."); }
        }

    }
}
