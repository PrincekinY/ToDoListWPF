using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class AttentionProject:BaseModel
    {
		private string projectName;

		public string ProjectName
		{
			get { return projectName; }
			set { projectName = value; RaisePropertyChanged(); }
		}

		private int projectType; //专注类型，倒计时，正向是1

		public int ProjectType
		{
			get { return projectType; }
			set { projectType = value; RaisePropertyChanged(); }
		}

		private DateTime inverseTime; //如果是倒计时，倒计时持续的时间

		public DateTime InverseTime
		{
			get { return inverseTime; }
			set { inverseTime = value; RaisePropertyChanged(); }
		}

		private DateTime curRealTime;

		public DateTime CurRealTime
		{
			get { return curRealTime; }
			set { curRealTime = value; RaisePropertyChanged(); }
		}


		private string projectDes;

		public string ProjectDes
		{
			get { return projectDes; }
			set { projectDes = value; RaisePropertyChanged(); }
		}

	}

	public class AttentionRecord : BaseModel
	{
		private string projectID;

		public string ProjectID
		{
			get { return projectID; }
			set { projectID = value; RaisePropertyChanged(); }
		}

		private DateTime startTime;

		public DateTime StartTime
		{
			get { return startTime; }
			set { startTime = value; RaisePropertyChanged(); }
		}

		private DateTime endTime;

		public DateTime EndTime
		{
			get { return endTime; }
			set { endTime = value; RaisePropertyChanged(); }
		}

		private DateTime lastTime;

		public DateTime LastTime
		{
			get { return lastTime; }
			set { lastTime = value; RaisePropertyChanged(); }
		}


	}
}
