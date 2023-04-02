using Microsoft.VisualBasic.ApplicationServices;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDoListWPF.Dao;
using ToDoListWPF.Views;

namespace ToDoListWPF.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            LoginCMD = new DelegateCommand<LoginView>(LoginMethod);
        }

        public DelegateCommand<LoginView> LoginCMD { get;private set; }
        private void LoginMethod(LoginView loginView)
        {
            string id = loginView.accountID.Text;
            string pwd = loginView.password.Text;
            string findsql = "select userPwd,userRole from userinfo where userID='"+id+"'";
            MysqlDBCon mysqlDBCon = new MysqlDBCon();
            try
            {
                IDataReader dr = mysqlDBCon.sqlRead(findsql);
                while (dr.Read())
                {
                    string pwd_exist = dr["userPwd"].ToString();
                    if (pwd_exist == pwd)
                    {
                        string role_exist = dr["userRole"].ToString();
                        //打开MainWindows
                        EditLoginInfo(id, role_exist);
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        loginView.Close();
                    }
                    else
                    {
                        MessageBox.Show("账号和密码不匹配，请检查输入。");
                    }
                }
                dr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("网络异常。\n"+e.Message);
            }
        }

        private void EditLoginInfo(string userID, string userrolw)
        {
            //修改配置文件中键值为key的项的值
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["loginAccount"].Value = userID;
            config.AppSettings.Settings["loginRole"].Value = userrolw;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
