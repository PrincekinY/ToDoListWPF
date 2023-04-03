using Google.Protobuf.WellKnownTypes;
using ImTools;
using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Media;
using ToDoListWPF.Dao;
using ToDoListWPF.Extensions;
using ToDoListWPF.Models;
using ToDoListWPF.Views;

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

            //可视化
            TimePieSeriesCollection = new SeriesCollection();
            TimeBarSeriesCollection = new SeriesCollection();
            OnedayProjects = new List<string>();
            ProjectsXAxis = new Axis();
            PieChartVisibility = Visibility.Visible;
            BarChartVisibility = Visibility.Collapsed;
            ChangeVisibilityChart = new DelegateCommand<ConCentrationOverView>(ChangeVisibilityChartMethod);
            ShowPieCMD = new DelegateCommand(ShowPieMethod);    
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
            ShowPieMethod();
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

        private SeriesCollection timePieSeriesCollection;

        public SeriesCollection TimePieSeriesCollection
        {
            get { return timePieSeriesCollection; }
            set { timePieSeriesCollection = value; RaisePropertyChanged(); }
        }

        private SeriesCollection timebarSeriesCollection;

        public SeriesCollection TimeBarSeriesCollection
        {
            get { return timebarSeriesCollection; }
            set { timebarSeriesCollection = value; RaisePropertyChanged(); }
        }

        public SeriesCollection ProTimePieSeries2()
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            List<string> list_name = AllAttentionRecords.Select(t => t.AttentionName).Distinct().ToList();
            int totalsec = 0;
            List<int> partsec = new List<int>();
            for (var i=0;i<list_name.Count;i++)
            {
                partsec.Add(0);
                var res = AllAttentionRecords.Where(t => t.AttentionName == list_name[i]);
                foreach(var record in res)
                {
                    int secs = record.LastTime.Hour * 60 * 60 + record.LastTime.Minute * 60 + record.LastTime.Second;
                    partsec[i] += secs;
                    totalsec += secs;
                }
            }
            List<string> labels = new List<string>();
            for (var i = 0; i < list_name.Count; i++)
            {
                //TimeSpan ts = new TimeSpan(0, 0, partsec[i]);
                //labels.Add(ts.ToString(@"hh\:mm\:ss"));
                PieSeries pieSeries = new PieSeries();
                pieSeries.Title = list_name[i];
                pieSeries.Values = new ChartValues<int>();
                pieSeries.Values.Add(partsec[i]);
                //自定义数据标签
                pieSeries.LabelPoint = point => new TimeSpan(0, 0, (int)point.Y).ToString(@"hh\:mm\:ss");
                pieSeries.DataLabels = true;
                var c =new BeautifulColors().ReturnIntToColor(i);
                pieSeries.Fill = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
                seriesCollection.Add(pieSeries);
            }
            return seriesCollection;
        }

        public SeriesCollection ProTimePieSeries(List<string> list_name, List<int> partsec)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            for (var i = 0; i < list_name.Count; i++)
            {
                //TimeSpan ts = new TimeSpan(0, 0, partsec[i]);
                //labels.Add(ts.ToString(@"hh\:mm\:ss"));
                PieSeries pieSeries = new PieSeries();
                pieSeries.Title = list_name[i];
                pieSeries.Values = new ChartValues<int>();
                pieSeries.Values.Add(partsec[i]);
                //自定义数据标签
                pieSeries.LabelPoint = point => new TimeSpan(0, 0, (int)point.Y).ToString(@"hh\:mm\:ss");
                pieSeries.DataLabels = true;
                var c = new BeautifulColors().ReturnIntToColor(i);
                pieSeries.Fill = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
                seriesCollection.Add(pieSeries);
            }
            return seriesCollection;
        }

        public DelegateCommand ShowPieCMD { get; set; }
        public void ShowPieMethod()
        {
            List<string> list_name = AllAttentionRecords.Select(t => t.AttentionName).Distinct().ToList();
            List<int> partsec = new List<int>();
            for (var i = 0; i < list_name.Count; i++)
            {
                partsec.Add(0);
                var res = AllAttentionRecords.Where(t => t.AttentionName == list_name[i]);
                foreach (var record in res)
                {
                    int secs = record.LastTime.Hour * 60 * 60 + record.LastTime.Minute * 60 + record.LastTime.Second;
                    partsec[i] += secs;
                }
            }
            OnedayProjects.Clear();
            list_name.ForEach(i => OnedayProjects.Add(i)); ;

            TimePieSeriesCollection = ProTimePieSeries(list_name,partsec);
            TimeBarSeriesCollection = ProTimeBarSeries(list_name,partsec);
        }

        public SeriesCollection ProTimeBarSeries(List<string> list_name, List<int> partsec)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            //这样是用来画7天不一样的数据的
            //for (var i = 0; i < list_name.Count; i++)
            //{
            //StackedColumnSeries series = new StackedColumnSeries();
            //series.Title = list_name[i];
            //series.Values = new ChartValues<int>();
            //series.Values.Add(partsec[i]);
            //自定义数据标签
            //series.LabelPoint = point => new TimeSpan(0, 0, (int)point.Y).ToString(@"hh\:mm\:ss");
            //series.DataLabels = true;
            //var c = new BeautifulColors().ReturnIntToColor(i);
            //series.Fill = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
            //seriesCollection.Add(series);
            //}
            
            StackedColumnSeries series = new StackedColumnSeries();
            series.Title = "专注时间";
            series.Values = new ChartValues<int>();
            for (var i = 0; i < list_name.Count; i++)
            {
                series.Values.Add(partsec[i]);
            }
            series.LabelPoint = point => new TimeSpan(0, 0, (int)point.Y).ToString(@"hh\:mm\:ss");
            var c = new BeautifulColors().ReturnIntToColor(0);
            series.Fill = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
            seriesCollection.Add(series);
            return seriesCollection;
        }
        private List<string> onedayProjects;

        public List<string> OnedayProjects
        {
            get { return onedayProjects; }
            set { onedayProjects = value; RaisePropertyChanged(); }
        }

        private Axis projectsXAxis;

        public Axis ProjectsXAxis
        {
            get { return projectsXAxis; }
            set { projectsXAxis = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<ConCentrationOverView> ChangeVisibilityChart { get; set; }
        public void ChangeVisibilityChartMethod(ConCentrationOverView obj)
        {
            //int param = Int16.Parse(obj.ToString());
            //if(param == 0) { PieChartVisibility = Visibility.Visible;BarChartVisibility = Visibility.Collapsed; }
            //else { PieChartVisibility = Visibility.Collapsed; BarChartVisibility = Visibility.Visible; }
            if ((bool)obj.barradio.IsChecked) { PieChartVisibility = Visibility.Collapsed; 
                BarChartVisibility = Visibility.Visible;
                obj.piechart.IsSelected = true;
            }
            else { PieChartVisibility = Visibility.Visible; BarChartVisibility = Visibility.Collapsed; obj.barchart.IsSelected = true; }
        }

        private Visibility piechartVisibility;

        public Visibility PieChartVisibility
        {
            get { return piechartVisibility; }
            set { piechartVisibility = value; RaisePropertyChanged(); }
        }

        private Visibility barchartVisibility;

        public Visibility BarChartVisibility
        {
            get { return barchartVisibility; }
            set { barchartVisibility = value; RaisePropertyChanged(); }
        }

    }
}
