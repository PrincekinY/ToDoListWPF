﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Extensions
{
    public class BeepUp
    {
        /// <param name="iFrequency">声音频率（从37Hz到32767Hz）。在windows95中忽略</param>  
        /// <param name="iDuration">声音的持续时间，以毫秒为单位。</param>  
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;  
        public static extern bool Beep(int frequency, int duration);
    }
}
