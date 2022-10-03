using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class Fitness:BindableBase
    {
		private string fitID;

		public string FitID
		{
			get { return fitID; }
			set { fitID = value; RaisePropertyChanged(); }
		}

		private DateTime fitDay;

		public DateTime FitDay
		{
			get { return fitDay; }
			set { fitDay = value; RaisePropertyChanged(); }
		}

		private float weight;

		public float Weight
		{
			get { return weight; }
			set { weight = value; RaisePropertyChanged(); }
		}

		private string breakfast;

		public string Breakfast
		{
			get { return breakfast; }
			set { breakfast = value; RaisePropertyChanged(); }
		}
        private string lunch;

        public string Lunch
        {
            get { return lunch; }
            set { lunch = value; RaisePropertyChanged(); }
        }

        private string dinner;

        public string Dinner
        {
            get { return dinner; }
            set { dinner = value; RaisePropertyChanged(); }
        }
        private string sport;

        public string Sport
        {
            get { return sport; }
            set { sport = value; RaisePropertyChanged(); }
        }
        private float hipline;

        public float Hipline
        {
            get { return hipline; }
            set { hipline = value; RaisePropertyChanged(); }
        }
        private float waistline;

        public float Waistline
        {
            get { return waistline; }
            set { waistline = value; RaisePropertyChanged(); }
        }
        private float belly;

        public float Belly
        {
            get { return belly; }
            set { belly = value; RaisePropertyChanged(); }
        }
        private float bustline;

        public float Bustline
        {
            get { return bustline; }
            set { bustline = value; RaisePropertyChanged(); }
        }
        private float calfgirth;

        public float Calfgirth
        {
            get { return calfgirth; }
            set { calfgirth = value; RaisePropertyChanged(); }
        }
        private float thigh;

        public float Thigh
        {
            get { return thigh; }
            set { thigh = value; RaisePropertyChanged(); }
        }
    }
}
