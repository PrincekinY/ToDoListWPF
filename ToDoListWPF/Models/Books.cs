using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class Books:BaseModel
    {
		private string bookid;


		private string bookname;

		public string BookName
		{
			get { return bookname; }
			set { bookname = value; RaisePropertyChanged(); }
		}

		private string author;

		public string Author
		{
			get { return author; }
			set { author = value; RaisePropertyChanged(); }
		}

		private DateTime startread;

		public DateTime StartRead
		{
			get { return startread; }
			set { startread = value; RaisePropertyChanged(); }
		}

		private DateTime endread;

		public DateTime EndRead
		{
			get { return endread; }
			set { endread = value; RaisePropertyChanged(); }
		}

		private string status;

		public string Status
		{
			get { return status; }
			set { status = value; RaisePropertyChanged(); }
		}

		private string bookimage;

		public string BookImage
		{
			get { return bookimage; }
			set { bookimage = value; RaisePropertyChanged(); }
		}


	}
}
