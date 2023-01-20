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
    class order //: IComparable<order>
    {
        public int orderNo { get; set; }
        public DateTime orderDateTime { get; set; }
        public double amount { get; set; }
        public string status { get; set; }
        public List<ticket> ticketList { get; set; } = new List<ticket>();

        //constructor
        public order() { }
        public order(int on, DateTime odt)
        {
            orderNo = on;
            orderDateTime = odt;
        }

        // other method

        public void AddTicket(ticket t)
        {
            ticketList.Add(t);
        }

        public List<ticket> GetTicketList()
        {
            return ticketList;
        }
        public int GetCountList()
        {
            return ticketList.Count;
        }
        public override string ToString()
        {
            return base.ToString();
        }
        /*public int CompareTo(order o)
        {
            if (GetCountList()> o.GetCountList())
                return 1;
            else if (GetCountList() == o.GetCountList())
                return 0;
            else
                return -1;


        }
        */

    }
}
