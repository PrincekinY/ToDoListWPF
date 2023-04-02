using Prism.Mvvm;
using System;
using ToDoListWPF.Models;

namespace ToDoListWPF.ViewModels
{
    public class AttentionDispatcherTimerViewModel : BindableBase
    {
        private string _title = "Concentration";
        private AttentionProject thisProject;

        public AttentionProject ThisProject
        {
            get { return thisProject; }
            set { thisProject = value; RaisePropertyChanged(); }
        }


        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public AttentionDispatcherTimerViewModel(AttentionProject project)
        {
            ThisProject = project;
            Console.WriteLine(ThisProject.ID);
        }
    }
}
