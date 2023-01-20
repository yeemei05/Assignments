//============================================================
// Student Number : S10222428, S10222497
// Student Name : Ho Yee Mei, Yang Yiling
// Module Group : T11 
//============================================================
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Tix
{
    class Cinema
    {
        public string name { get; set; }
        public int hallNo { get; set; }
        public int capacity { get; set; }
        
        //constructor
        public Cinema() { }
        public Cinema(String n, int hn, int c)
        {
            name = n;
            hallNo = hn;
            capacity = c;
        }

        //methods
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
