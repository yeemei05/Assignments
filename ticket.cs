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
    abstract class ticket
    {
        public Screening screening { get; set; }

        // constructor 
        public ticket() { }
        public ticket(Screening s)
        {
            screening = s;
        }

        // other methods

        public abstract double CalculatePrice(Screening s);
        public override string ToString()
        {
            return base.ToString();
        }
    }

}
