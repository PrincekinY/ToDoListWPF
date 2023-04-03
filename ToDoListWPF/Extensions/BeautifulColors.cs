using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Extensions
{
    public class BeautifulColors
    {
        public BeautifulColors()
        {
        }


        public Color ReturnIntToColor(int obj)
        {
            Color color;
            switch (obj){
                case 1: color = ColorTranslator.FromHtml("#ffd3b6"); break;
                case 2: color = ColorTranslator.FromHtml("#dcedc1"); break;
                case 3: color = ColorTranslator.FromHtml("#a8e6cf"); break;
                case 4: color = ColorTranslator.FromHtml("#dbe2ef"); break;
                case 5: color = ColorTranslator.FromHtml("#ffe2e2"); break;
                case 6: color = ColorTranslator.FromHtml("#e0f9b5"); break;
                case 7: color = ColorTranslator.FromHtml("#fefdca"); break;
                case 8: color = ColorTranslator.FromHtml("#cbf1f5"); break;
                case 9: color = ColorTranslator.FromHtml("#e3fdfd"); break;
                case 10: color = ColorTranslator.FromHtml("#ffd3b6"); break;
                case 12: color = ColorTranslator.FromHtml("#ffaaa5"); break;
                case 13: color = ColorTranslator.FromHtml("#dcedc1"); break;
                case 11: color = ColorTranslator.FromHtml("#c3bef0"); break;
                case 14: color = ColorTranslator.FromHtml("#cadefc"); break;
                case 15: color = ColorTranslator.FromHtml("#defcf9"); break;
                case 16: color = ColorTranslator.FromHtml("#a8d8ea"); break;
                default: color = ColorTranslator.FromHtml("#ffaaa5"); break;
            }
            return color;
        }
    }
}
