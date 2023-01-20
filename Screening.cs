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
    class Screening : IComparable<Screening>
    {
        public int screeningNo { get; set; }
        public DateTime screeningDateTime { get; set; }
        public string screeningType { get; set; }
        public int seatsRemaining { get; set; }
        public Cinema Cinema { get; set; }
        public movie movie { get; set; }

        public Screening() { }
        public Screening(int sn, DateTime sdt, string st, Cinema c, movie m)
        {
            screeningNo = sn;
            screeningDateTime = sdt;
            screeningType = st;
            Cinema = c;
            movie = m;
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public int CompareTo(Screening s)
        {
            if (seatsRemaining > s.seatsRemaining)
                return -1;
            else if (seatsRemaining == s.seatsRemaining)
                return 0;
            else
                return 1;


        }
    }
}
