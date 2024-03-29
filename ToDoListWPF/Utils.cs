﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBACAPSPP
{
    public class Utils
    {
        //Utils.LogWrite
        //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
        private static readonly ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        public Utils()
        {
        }

        public static void LogWrite(Exception ex)
        {
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }
            var now = DateTime.Now;
            var logpath = @"Log\" + now.Year + "" + now.Month + "" + now.Day + ".log";
            var log = "\r\n----------------------" + DateTime.Now + " --------------------------\r\n"
                      + ex.Message
                      + "\r\n"
                      + ex.InnerException
                      + "\r\n"
                      + ex.StackTrace
                      + "\r\n----------------------footer--------------------------\r\n";
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                LogWriteLock.EnterWriteLock();
                File.AppendAllText(logpath, log);
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }
    }
}
