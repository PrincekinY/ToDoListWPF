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

		private DateTime today;

		public DateTime Today
		{
			get { return today; }
			set { today = value; RaisePropertyChanged(); }
		}

		private DateTime lastTime;

		public DateTime LastTime
		{
			get { return lastTime; }
			set { lastTime = value; RaisePropertyChanged(); }
		}

		private string attentionName;

		public string AttentionName
		{
			get { return attentionName; }
			set { attentionName = value; RaisePropertyChanged(); }
		}



	}
}
