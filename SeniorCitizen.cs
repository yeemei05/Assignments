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
    class SeniorCitizen : ticket
    {
        public int yearOfBirth { get; set; }

        // constructor
        public SeniorCitizen():base() { }
        public SeniorCitizen(Screening s, int y) : base(s)
        {
            yearOfBirth = y;
        }

        public override double CalculatePrice(Screening s)
        {
            double p = 0.00;
            bool isWeekend = (int)s.screeningDateTime.DayOfWeek >= 5;

            if (s.screeningDateTime < s.movie.openingDate.AddDays(7))
            {
                if (screening.screeningType == "2D")
                {
                    if (isWeekend == true)
                    {
                        p = 12.50;
                    }
                    else
                    {
                        p = 8.50;

                    }
                }
                else
                {
                    if (isWeekend == true)
                    {
                        p = 14.00;
                    }
                    else
                    {
                        p = 11.00;
                    }
                }
            }
            else
            {
                if (screening.screeningType == "2D")
                {
                    if (isWeekend == true)
                    {
                        p = 12.50;
                    }
                    else
                    {
                        p = 5.00;
                    }
                }
                else
                {
                    if (isWeekend == true)
                        p = 14.00;
                    else
                        p = 6.00;

                }
            }
            
            return p;
        }
        public override string ToString()
        {
            return base.ToString()
                + "Year Of Birth: " + yearOfBirth;
        }
    }
}
