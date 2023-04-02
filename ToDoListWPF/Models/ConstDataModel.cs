using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class ConstDataModel
    {
        public ConstDataModel()
        {
        }

        public static readonly Dictionary<string, int> NewDictionary = new Dictionary<string, int>()
        {
            { "正向计时", 1 },
            { "逆向计时", 0 }
        };
    }
}
