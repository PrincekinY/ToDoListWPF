using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class BaseModel:BindableBase
    {
		private string id;

		public string ID
		{
			get { return id; }
			set { id = value; RaisePropertyChanged(); }
		}

		private DateTime changeTime;

		public DateTime ChangeTime
		{
			get { return changeTime; }
			set { changeTime = value; RaisePropertyChanged(); }
		}

		private string operatorName;

		public string OperatorName
		{
			get { return operatorName; }
			set { operatorName = value; RaisePropertyChanged(); }
		}

	}
}
