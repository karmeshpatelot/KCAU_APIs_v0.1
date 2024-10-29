using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DAL.Entities
{
    public class T0040_CURRENCY_MASTER
    {
        public decimal Curr_ID { get; set; }
        public decimal Cmp_ID { get; set; }
        public string Curr_Name { get; set; }
        public decimal Curr_Rate { get; set; }
        public char Curr_Major { get; set; }
        public string Curr_Symbol { get; set; }
        public string Curr_Sub_Name { get; set; }
    }
}
