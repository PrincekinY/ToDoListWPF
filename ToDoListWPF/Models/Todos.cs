using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class Todos:BindableBase
    {
		private string todoID;

		public string TodoID
		{
			get { return todoID; }
			set { todoID = value; RaisePropertyChanged(); }
		}

		private string todoname;

		public string TodoName
		{
			get { return todoname; }
			set { todoname = value; RaisePropertyChanged(); }
		}

		private string tododes;

		public string TodoDes
		{
			get { return tododes; }
			set { tododes = value; RaisePropertyChanged(); }
		}

		private DateTime tododay;

		public DateTime TodoDay
		{
			get { return tododay; }
			set { tododay = value; RaisePropertyChanged(); }
		}

		private bool todostatus;

		public bool TodoStatus
		{
			get { return todostatus; }
			set { todostatus = value; RaisePropertyChanged(); }
		}




	}
}
