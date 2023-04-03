using Prism.Commands;
using Prism.Mvvm;
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
    public class ConCentrationOverViewModel : BindableBase
    {
        private readonly MysqlDBCon DBCon;
        public ConCentrationOverViewModel()
        {
            DBCon = new MysqlDBCon();
            SelectedDate = DateTime.Now;
            AllAttentionRecords = new ObservableCollection<AttentionRecord>();
            CurAttentionRecord = new AttentionRecord();
            EditedLastTime = new DateTime();

            AllAttentionRecords = GetTodayAttentionRecord(SelectedDate);
            SearchRecordByDate = new DelegateCommand(SearchRecordByDateMethod);
            GotoDate = new DelegateCommand<object>(GotoDateMethod);
            DeleteRecord = new DelegateCommand<AttentionRecord>(DeleteRecordMethod);
            OpenEditCmd = new DelegateCommand<AttentionRecord>(OpenEditMethod);
            CloseEditCmd = new DelegateCommand(CloseEditMethod);
            EditLastTimeCmd = new DelegateCommand(EditLastTimeMethod);
        }

        public ObservableCollection<AttentionRecord> GetTodayAttentionRecord(DateTime dt)
        {
            DateTime dt0 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            ObservableCollection<AttentionRecord> res = new ObservableCollection<AttentionRecord>();
            string sql = "select * from attention_record left JOIN (select attentionID,attentionName from attention_project) attention " +
                "on attention.attentionID=attention_record.projectID where recordDate='"+ dt0 + "'";
            try
            {
                IDataReader dr = DBCon.sqlRead(sql);
                while (dr.Read())
                {
                    AttentionRecord ar = new AttentionRecord();
                    ar.ID = dr["recordID"].ToString();
                    ar.ProjectID = dr["projectID"].ToString() ;
                    ar.AttentionName = dr["attentionName"].ToString();
                    ar.Today = DateTime.Parse(dr["recordDate"].ToString());
                    ar.LastTime = DateTime.Parse(dr["lastTime"].ToString()) ;
                    res.Add(ar);
                }
                dr.Close();
            }
            catch(Exception e) { MessageBox.Show("查找失败。\n"+e.Message); }
            return res;
        }

        private ObservableCollection<AttentionRecord> records;

        public ObservableCollection<AttentionRecord> AllAttentionRecords
        {
            get { return records; }
            set { records = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SearchRecordByDate { get; set; }
        public void SearchRecordByDateMethod()
        {
            AllAttentionRecords = GetTodayAttentionRecord(SelectedDate);
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<object> GotoDate { get; set; }
        public void GotoDateMethod(object obj)
        {
            int day = Int16.Parse(obj.ToString());
            SelectedDate = SelectedDate.AddDays(day);
            SearchRecordByDateMethod();
        }

        public DelegateCommand<AttentionRecord> DeleteRecord { get; set; }
        public void DeleteRecordMethod(AttentionRecord ar)
        {
            MessageBoxResult vr = MessageBox.Show("确认删除？", "提示", MessageBoxButton.OKCancel);
            if (vr == MessageBoxResult.OK)
            {
                string sql = "delete from attention_record where recordID='" + ar.ID + "'";
                try
                {
                    int trow = DBCon.sqlExcute(sql);
                    if (trow > 0) { AllAttentionRecords.Remove(ar); MessageBox.Show("删除成功"); }
                }
                catch { MessageBox.Show("删除失败"); }
            }
        }

        private bool isTopDrawerOpen;

        public bool IsTopDrawerOpen
        {
            get { return isTopDrawerOpen; }
            set { isTopDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<AttentionRecord> OpenEditCmd { get; set; }
        public void OpenEditMethod(AttentionRecord obj)
        {
            IsTopDrawerOpen = true;
            CurAttentionRecord.ID = obj.ID;
            CurAttentionRecord.LastTime = obj.LastTime;
            EditedLastTime = obj.LastTime;
        }

        public DelegateCommand CloseEditCmd { get; set; }
        public void CloseEditMethod()
        {
            IsTopDrawerOpen = false;
        }

        public DelegateCommand EditLastTimeCmd { get; set; }
        public void EditLastTimeMethod()
        {
            var editspan = new TimeSpan(EditedLastTime.Hour, EditedLastTime.Minute, EditedLastTime.Second);
            var storespan = new TimeSpan(CurAttentionRecord.LastTime.Hour, CurAttentionRecord.LastTime.Minute, CurAttentionRecord.LastTime.Second);
            if(editspan > storespan)
            {
                MessageBox.Show("不能超过实际计时");
            }
            else
            {
                string sql = "update attention_record set lastTime='" + EditedLastTime + "' where recordID='" + CurAttentionRecord.ID + "'";
                try
                {
                    int row = DBCon.sqlExcute(sql);
                    if (row > 0)
                    {
                        MessageBox.Show("修改成功。");
                        IsTopDrawerOpen = false;
                        AllAttentionRecords = GetTodayAttentionRecord(SelectedDate);
                    }
                    
                }
                catch { MessageBox.Show("修改失败。"); }
            }
            

        }

        private DateTime editedLastTime;

        public DateTime EditedLastTime
        {
            get { return editedLastTime; }
            set { editedLastTime = value; RaisePropertyChanged(); }
        }

        private AttentionRecord curARecord;

        public AttentionRecord CurAttentionRecord
        {
            get { return curARecord; }
            set { curARecord = value; RaisePropertyChanged(); }
        }

    }
}
