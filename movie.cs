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
    class movie:IComparable<movie>
    {
        public string title { get; set; }
        public int duration { get; set; }
        public string classification { get; set; }
        public DateTime openingDate { get; set; }
        public int TicketsSold { get; set; }
        public List<Screening> IndscreeningList { get; set; } = new List<Screening>();
        public List<string> genreList { get; set; } = new List<string>();

        //constructor
        public movie() { }
        public movie(string t, int d, string c, DateTime od, List<string> gl)
        {
            title = t;
            duration = d;
            classification = c;
            openingDate = od;
            genreList = gl;
        }

        // other methods 
        public void AddGenre(string g)
        {
            genreList.Add(g);
        }
        public List<string> GetGenreList()
        {
            return genreList;
        }
        public void AddScreening(Screening s)
        {
            IndscreeningList.Add(s);
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public int CompareTo(movie m)
        {
            if (TicketsSold > m.TicketsSold)
                return -1;
            else if (TicketsSold == m.TicketsSold)
                return 0;
            else
                return 1;


        }
    }
}
