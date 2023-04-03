using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        public BookcaseViewModel()
        {
            dBCon = new MysqlDBCon();
            IsRightDrawerOpen = false;
            loginID = ConfigurationManager.AppSettings["loginAccount"];

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
        private readonly MysqlDBCon dBCon;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private readonly string loginID;

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
            CurrentBook.ID = obj.ID;
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
            string sql = "insert into booklist values('" + bid + "','" + bname + "','" + author + "','" + bstart + "','" + bend + "','"+bstatus_int+"','','"+DateTime.Now.Date.ToString()+"','"+loginID+"')";
            MysqlDBCon dBCon = new MysqlDBCon();
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0)
                {
                    BookSet.Add(new Books() { ID = bid, BookName = bname, Author = author, StartRead = bstart, EndRead = bend,Status=bstatus_str });
                    MessageBox.Show("添加成功.");
                }
            }
            catch { MessageBox.Show("添加失败."); }
        }

        public int StatusStrToInt(string obj)
        {
            if (obj == "想看") { return 1; }
            if (obj == "正在看") { return 2; }
            if(obj == "已看完") { return 3; }
            else { return 0; }
        }

        public IEnumerable<string> ReadingStatus => new[] { "想看", "正在看", "已看完" };


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
            ObservableCollection<Books> todoset = new ObservableCollection<Books>();
            //DateTime today = DateTime.Now;
            try
            {
                todoset = GetDateBooks();
            }
            catch { MessageBox.Show("查找失败啦！"); }
            return todoset;
        }

        public ObservableCollection<Books> GetDateBooks()
        {
            ObservableCollection<Books> bookset = new ObservableCollection<Books>();
            //var specific_date = dateTime.Date;
            string sql = "select * from booklist where operator='"+loginID+"'";
            MysqlDBCon dBCon = new MysqlDBCon();
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
                Books t = new Books() { ID = id, BookName = name, Author = author, StartRead = sday, EndRead = eday,Status=status ,BookImage=pic};
                bookset.Add(t);
            }
            dr.Close();
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
            MessageBoxResult vr = MessageBox.Show("确认删除？", "提示", MessageBoxButton.OKCancel);
            if (vr == MessageBoxResult.OK)
            {
                string tid = obj.ID;
                string sql = "delete from booklist where bookID='" + tid + "'";
                try
                {
                    int trow = dBCon.sqlExcute(sql);
                    if (trow > 0) { BookSet.Remove(obj); MessageBox.Show("删除成功"); }
                }
                catch { MessageBox.Show("删除失败"); }
            }
            
        }

        public DelegateCommand EditBookCmd { get; private set; }
        private void EditBookMethod()
        {
            string id = CurrentBook.ID;
            string name = CurrentBook.BookName;
            string author = CurrentBook.Author;
            var sdate = CurrentBook.StartRead;
            var edate = CurrentBook.EndRead;
            var tstatus = CurrentBook.Status;
            var status_int = StatusStrToInt(tstatus);
            //var todo = new Books() { BookID = tid, BookName = tname, BookDes = tdes, BookDay = tdate, BookStatus = tstatus };
            string sql = "update booklist set bookName='" + name + "',author='" + author + "',startread='" + sdate + "',endread='"+edate+"',status='" + status_int + "',changeTime='"+DateTime.Now.Date.ToString()+"' where bookID='" + id + "'";
            try
            {
                int trow = dBCon.sqlExcute(sql);
                if (trow > 0)
                {
                    var tindex = BookSet.IndexOf(BookSet.First(p => p.ID == id));
                    BookSet[tindex].BookName = name;
                    BookSet[tindex].Author = author;
                    BookSet[tindex].StartRead = sdate;
                    BookSet[tindex].EndRead = edate;
                    BookSet[tindex].Status = tstatus;
                    MessageBox.Show("修改成功。");
                    IsRightDrawerOpen = false;
                }
            }
            catch { MessageBox.Show("修改失败。"); }
        }

        public DelegateCommand<Books> UploadBookImageCmd { get; set; }
        public void UploadBookImageMethod(Books obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                obj.BookImage = openFileDialog.FileName;
                //判断文件是不是对的
                
                try
                {
                    string imagepath = obj.BookImage.Replace("\\","\\\\");
                    string sql = "update booklist set bookImage ='" + imagepath + "' where bookID='" + obj.ID + "'";
                    int brow = dBCon.sqlExcute(sql);
                    MessageBox.Show("上传成功。");
                }
                catch { MessageBox.Show("上传失败。"); }
            }
        }

    }
}
