//============================================================
// Student Number : S10222428, S10222497
// Student Name : Ho Yee Mei, Yang Yiling
// Module Group : T11 
//============================================================

using System;
using System.IO;
using System.Collections.Generic;

namespace Movie_Tix
{
    class Program
    {
        static void Main(string[] args)
        {
            // creating list for movie and cinema for creating object from list
            List<movie> movieList = new List<movie>();
            List<Cinema> cinemaList = new List<Cinema>();
            List<Screening> screeningList = new List<Screening>();
            //List<order> SortOrder = new List<order>();

            //add the different kind of genre in a list
            List<string> genrestring = new List<string>();

            //to add the orders in the list
            List<order> orderList = new List<order>();

            InitData(movieList,cinemaList);//create movie and cinema object and add to the list
            Initscreening(screeningList, movieList, cinemaList);//create screening object and add to list.
            CreateScreeningListForMovie(movieList, screeningList); // create individual screening list for each movie

            foreach (Screening s in screeningList)
            {
                s.seatsRemaining = s.Cinema.capacity; //to init seats remaining for screening data
            }

            while (true)
            {
                DisplayMenu(); 
                int choice = GetInt("Enter your choice: "); // get reader choice
                if (choice == 1)
                {
                    DisplayMovie(movieList); //1. List all movies
                    Console.WriteLine();
                }
                else if (choice == 2)
                {
                    ListmovieScreenings(movieList, screeningList); //2. List screening for selected movie
                    Console.WriteLine();
                }
                else if (choice == 3)
                {
                    AddSession(movieList, cinemaList, screeningList); //3. Add Session for movie
                    Console.WriteLine();
                }
                else if (choice == 4)
                {
                    DeleteScreening(screeningList, orderList); //4. Delete Screening session
                    Console.WriteLine();
                }
                else if (choice == 5)
                {
                    // order movie ticket
                    OrderMovieTicket(movieList, screeningList, orderList); // 5. order movie ticket for selected movie
                    Console.WriteLine();
                }
                else if (choice == 6)
                {
                    CancelOrder(orderList, movieList, screeningList); // 6. Cancel order if there is order created
                    Console.WriteLine();
                }
                else if (choice == 7)
                {
                    RecommendMovieByOrder(orderList, movieList); //list movie according to number of orders made
                    // to reset data accordingly
                    movieList = new List<movie>();
                    cinemaList = new List<Cinema>();
                    InitData(movieList, cinemaList);
                    //Console.WriteLine("pass");
                }
                else if (choice == 8)
                {
                    DisplayMovieAccordSeats(orderList, screeningList, movieList); //Display according to num of seats Available

                }
                else if (choice == 9)
                {
                    List<movie> movieWscreening = new List<movie>(); //
                    RecommendMoviebyGenre(movieList, genrestring,screeningList,orderList, movieWscreening);
                }
                else if (choice == 10)
                {
                    ListTop3(orderList, movieList);
                }
                else if (choice == 0)
                {
                    break;
                }

                else 
                { 
                    Console.WriteLine("Invalid Choice, Option not available.");
                    Console.WriteLine();
                }
                
            }
        }

        //display main menu
        static void DisplayMenu()
        {
            Console.WriteLine("----- Movie Ticketing System -----");
            Console.WriteLine("1. List all movies. \n2. List Movie Screenings \n3. Add Movie Screening Sessions \n" +
                "4. Delete Movie Screening Sessions \n5. Order Movie Ticket(s)\n6. Cancel Order of Ticket\n7. Recommend Movie by Order\n" +
                "8. Display Movie Screening According To Number of Seats Available\n9. Recommend Movie By Genre\n10. List Top 3 Movie Based on Tickets Sold\n0. Quit");
            Console.WriteLine("----------------------------------");
        }

        static void RecommendMoviebyGenre(List<movie> movieList, List<string> genrestring, List<Screening> screeningList, List<order> orderList, List<movie> movieWscreening)
        {
            foreach (movie m in movieList)
            {
                foreach (string g in m.genreList)
                {
                    if (genrestring.Contains(g))
                    {
                        //skip
                    }
                    else
                    {
                        genrestring.Add(g);
                    }
                }
            }
            Console.WriteLine("Genre Available: ");
            for (int i = 0; i < genrestring.Count; i++)
            {
                Console.WriteLine("{0,-15}{1}", i + 1, genrestring[i]);
            }
            int genre = GetInt("Which Genre do you prefer (choose 1)?");
            string genrename = genrestring[genre-1];
            Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4, -30}{5}", "S/No", "Title", "Duration", "Classification", "Genre", "Opening Date");

            for (int i = 0; i < genrestring.Count; i++)
            {
                movie m = movieList[i];
                if (m.genreList.Contains(genrename))
                {
                    List<string> genreList = m.GetGenreList();
                    string s = string.Join("/", genreList.ToArray());
                    Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4,-30}{5}", i + 1, m.title, m.duration, m.classification, s, m.openingDate.ToString("dd/MM/yyyy"));
                    movieWscreening.Add(m);

                }
            }

            for (int i = 0; i < movieWscreening.Count; i++)
            {
                movie mw = movieWscreening[i];
                bool Avail = CheckingForScreening(movieList, screeningList, mw);
                if (Avail == true)
                {
                    Console.Write("There is screening available. Would you like to order tickets? [Yes/No]");
                    string resp = Console.ReadLine();
                    if (resp == "Yes")
                    {
                        OrderMovieTicket(movieList, screeningList, orderList);
                    }
                    else if (resp == "No")
                    {
                        Console.WriteLine("Remember to catch it!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Response. Enter [Yes/No]");
                    }
                    break;
                }
            }
        }
        static void InitData(List<movie> movieList, List<Cinema> cinemaList)
        {
            // for cinema
            using (StreamReader sr = new StreamReader("Cinema.csv"))
            {
                string s = sr.ReadLine(); // read the header
                while ((s=sr.ReadLine())!=null)
                {
                    string[] cindetails = s.Split(',');
                    string name = cindetails[0];
                    int hallno = Convert.ToInt32(cindetails[1]);
                    int cap = Convert.ToInt32(cindetails[2]);

                    Cinema c = new Cinema(name, hallno, cap);
                    cinemaList.Add(c);
                }    
            }

            //for movie
            using (StreamReader sr = new StreamReader("Movie.csv"))
            {
                string s = sr.ReadLine(); // read the header
                while ((s = sr.ReadLine()) != null)
                {
                    string[] movdetails = s.Split(',');
                    string title = movdetails[0];
                    int duration = Convert.ToInt32(movdetails[1]);
                    string classi = movdetails[3];
                    DateTime od = Convert.ToDateTime(movdetails[4]);

                    List<String> genreList = new List<string>();
                    string[] genreArray = movdetails[2].Split('/');
                    for (int j = 0; j < genreArray.Length; j++)
                        genreList.Add(genreArray[j]);


                    movie m = new movie(title,duration,classi, od, genreList);
                    movieList.Add(m);
                }
            }

            
        }

        static void Initscreening(List<Screening> screeningList, List<movie> movieList, List<Cinema> cinemaList)
        {
            int count = 1001;
            using (StreamReader sr = new StreamReader("Screening.csv"))
            {
                string s = sr.ReadLine();
                while ((s = sr.ReadLine()) != null)
                {
                    string[] screendetails = s.Split(',');
                    DateTime dt = Convert.ToDateTime(screendetails[0]);   //getting date time
                    string type = screendetails[1];  //getting screening type

                    // creating & searching cinema
                    int hallno = Convert.ToInt32(screendetails[3]);
                    Cinema c = SearchCinema(cinemaList, screendetails[2], hallno);

                    // creating and searching movie
                    string title = screendetails[4];
                    movie m = SearchMovie(movieList, title);
                    //count += 1;

                    Screening st = new Screening(count, dt, type, c, m);
                    screeningList.Add(st);
                    count += 1;
                }
            }
        }
        // DISPLAY CINEMA
        static void DisplayCinema(List<Cinema> cinemaList)   //- to display cinema
        {
            Console.WriteLine("{0,-15} {1,-15}{2,-15}{3,-15}","S/No", "Name", "Hall Number", "Capacity");
            for (int i = 0; i < cinemaList.Count; i++)
            {
                Cinema p = cinemaList[i];
                Console.WriteLine("{0,-15} {1,-15}{2,-15}{3,-15}",i+1, p.name, p.hallNo, p.capacity);
            }
        }
        static void DisplayMovie(List<movie> movieList)
        {
            Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4, -30}{5}","S/No", "Title", "Duration","Classification","Genre","Opening Date");
            for (int i = 0; i < movieList.Count; i++)
            {
                movie m = movieList[i];
                List<string> genreList = m.GetGenreList();
                string s = string.Join("/", genreList.ToArray());
                Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4,-30}{5}",i+1, m.title, m.duration,m.classification,s,m.openingDate.ToString("dd/MM/yyyy"));
            }
        }

        static void DisplayScreening(List<Screening> screeningList)
        {
            
            Console.WriteLine("{0,-10}{1,-30}{2,-20}{3, -30}{4}", "S/No", "Date/Time", "Screening Type", "Cinema - HallNo", "Movie Title");
            for (int i = 0; i < screeningList.Count; i++)
            {
                Screening m = screeningList[i];
                //string t = string.Join("")
                Console.WriteLine("{0,-10}{1,-30}{2,-20}{3, -30}{4}", m.screeningNo, m.screeningDateTime, m.screeningType, m.Cinema.name +" - "+ m.Cinema.hallNo, m.movie.title);
            }
        }

        //search for movie title
        static movie SearchMovie(List<movie> movieList, string giventitle)
        {
            movie m = null;
            
            foreach(movie t in movieList)
            {
                if (giventitle == t.title)
                {
                    m = t;
                }
            }
            return m;
        }
        // search cinema in cinemalist
        static Cinema SearchCinema(List<Cinema> cinemaList, string givenname, int hallno)
        {
            Cinema c = null;
            foreach (Cinema t in cinemaList)
            {
                if (givenname == t.name && hallno == t.hallNo)
                {
                    c = t;
                }
            }
            return c;
        }

        //search cinema in screening List
        static Screening SearchScreening(List<Screening> screeningList, int givensn)
        {
            Screening s = null;
            foreach (Screening t in screeningList)
            {
                if (t.screeningNo == givensn)
                {
                    s = t;
                }
            }
            return s;
        }
        static List<Screening> SearchScreeningrList(List<Screening> screeningList, string givenname, int hallno)
        {
            List<Screening> s = new List<Screening>();
            foreach (Screening t in screeningList)
            {
                if (givenname == t.Cinema.name && hallno == t.Cinema.hallNo)
                {
                    s.Add(t);
                }
            }
            return s;
        }



        //prompting user (4)
        static string ListmovieScreenings(List<movie> movieList, List<Screening> screeningList)
        {
            DisplayMovie(movieList);
            int moviechoiceNo;
            while (true)
            {
                moviechoiceNo = GetInt("Enter movie S/No: ");
                if (moviechoiceNo <= movieList.Count &&  moviechoiceNo > 0)
                {
                    break;
                }
                else { Console.WriteLine("Invalid Input. Choice Not Available."); }
            }
            string movietitle = movieList[moviechoiceNo - 1].title;
            movie t = SearchMovie(movieList, movietitle);
            bool availscreen = CheckingForScreening(movieList, screeningList, t);
            if (t != null)
            {
                if (availscreen == true)
                {
                    Console.WriteLine("{0,-10}{1,-25}{2,-20}{3,-20}{4,-20}", "S/No", "Date/Time", "Screening Type", "Location Name", "Hall No.");
                    for (int i = 0; i < screeningList.Count; i++)
                    {
                        Screening m = screeningList[i];
                        if (t.title == m.movie.title)
                        {
                            Console.WriteLine("{0,-10}{1,-25}{2,-20}{3,-20}{4,-20}", m.screeningNo, m.screeningDateTime, m.screeningType, m.Cinema.name, m.Cinema.hallNo);
                        }


                    }
                }
                else
                { 
                    Console.WriteLine("No Screening Available for the Movie.");
                }
                

            }
            else
            {
               Console.WriteLine("Movie Not Available");
            }
            return movietitle;
        }

        // add screening session(5)
        static void AddSession(List<movie> movieList, List<Cinema> cinemaList, List<Screening> screeningList)
        {
            DisplayMovie(movieList);
            int choice = GetInt("Enter Movie: "); // need to check if movie is available
            string title = movieList[choice - 1].title;

            string type;
            while (true)
            {
                Console.Write("Enter Screening Type [2D/3D]: ");
                type = Console.ReadLine();
                if (type == "2D" || type == "3D")
                {
                    break;
                }
            }
            DateTime dt = GetDateTime("Enter Screening Date Time [dd/mm/yyyy 8:00 AM/PM]: ");
            if (dt > DateTime.Now)
            {
                movie m = SearchMovie(movieList, title);

                // comparing the dates
                int value = DateTime.Compare(dt, m.openingDate);
                if (value > 0) // if it is after opening date.
                {
                    DisplayCinema(cinemaList);

                    int LocationNo = GetInt("Enter Location S/No: ");
                    string LocationName = cinemaList[LocationNo - 1].name;

                    int hallno = cinemaList[LocationNo - 1].hallNo;
                    Console.WriteLine(hallno);

                    Cinema c = SearchCinema(cinemaList, LocationName, hallno);
                    //Screening h = SearchScreening(screeningList, LocationName, hallno);
                    List<Screening> h = SearchScreeningrList(screeningList, LocationName, hallno);


                    int newscreeningNo = screeningList.Count + 1000 + 1;
                    // checking availablity of the hall
                    if (h.Count == 0)
                    {
                        Screening newscr = new Screening(newscreeningNo, dt, type, c, m);
                        screeningList.Add(newscr);
                        Console.WriteLine("New Screening added successfully!");
                    }
                    else
                    {
                        foreach (Screening S in h)
                        {
                            if (S.screeningDateTime.Date == dt.Date)
                            {
                                int duration = S.movie.duration + 30; // added 30 min of cleaning time
                                DateTime startmovie = S.screeningDateTime;
                                //Console.WriteLine(duration);
                                DateTime aftermovie = S.screeningDateTime.AddMinutes(duration);
                                //Console.WriteLine("{0},{1}", startmovie, aftermovie);

                                if ((dt >= startmovie) && (dt <= aftermovie))
                                {
                                    Console.WriteLine("Hall Not Available");
                                }
                                else
                                {
                                    Screening newscr = new Screening(newscreeningNo, dt, type, c, m);
                                    screeningList.Add(newscr);
                                    Console.WriteLine("New Screening added successfully!");
                                    //Console.WriteLine("Aftermovie");
                                }
                            }

                        }
                    }
                }
                else
                {
                    Console.WriteLine("Date is before Movie Opening Date.");
                }
            }
            else
            {
                Console.WriteLine("Date has already passed.");
            }
            
            
 
        }

        static void DeleteScreening(List<Screening> screeningList, List<order> orderList)
        {
            //You loop through orderlist and check if any of the orders are for that screening
            foreach (order o in orderList)
            {
                Screening HaveBooking = o.ticketList[0].screening; // to be able to check the screening for the ticket
                screeningList.Remove(HaveBooking);
            }

            DisplayScreening(screeningList); // showing user which does not have booking

            //and because orders dont have a direct link to screening, you have to go through the ticket, so order.ticketlist[0].screening to check the order's screening
            int screeningNo = GetInt("Enter screening No. to delete:"); // prompting user to enter the screening number
            bool statusremoval = false;
            for (int i = 0; i < screeningList.Count; i++)
            {
                if (screeningNo == screeningList[i].screeningNo)
                {
                    screeningList.Remove(screeningList[i]);
                    statusremoval = true;
                    Console.WriteLine("Screening {0} removed successfully", screeningNo);
                    break;
                }
                else
                {
                    statusremoval = false;
                }
            }
            if (statusremoval == false)
            {
                Console.WriteLine("Screening {0} not available.", screeningNo);
            }

            // to add back to the screening list
            foreach (order o in orderList)
            {
                Screening HaveBooking = o.ticketList[0].screening; 
                screeningList.Add(HaveBooking);
            }
        }

        static bool CheckingCapacity(int NumOfSeats, int seatsremaining)
        {
            bool pass;
            
            if (NumOfSeats > seatsremaining)
            {
                pass = false; // false = not enough seats
            }
            else
            {
                pass = true; // true = enough seats
            }
        
            return pass;
        }

        static int GetNoOfSeats(Screening choice, List<order> orderList, List<movie> movieList, List<Screening> screeningList)
        {
            int SeatsRemaining = choice.Cinema.capacity;
            //updating number of seats left
            if (orderList.Count >= 1)
            {
                for (int i = 0; i < orderList.Count; i++)
                {
                    order o = orderList[i];
                    if (choice.movie == o.ticketList[0].screening.movie && choice.screeningDateTime == o.ticketList[0].screening.screeningDateTime
                        && o.ticketList[0].screening == choice)
                    {
                        SeatsRemaining -= o.ticketList.Count;
                        //Console.WriteLine("Deducting...");
                        //Console.WriteLine("Num of ticket: " + o.ticketList.Count);
                    }
                }
            }
            return SeatsRemaining;

        }

        static void CreateScreeningListForMovie(List<movie> movieList, List<Screening> screeningList)
        {
            foreach (Screening t in screeningList)
            {
                foreach(movie m in movieList)
                {
                    if (t.movie.title == m.title)
                    {
                        m.AddScreening(t);
                    }
                }
            }
        }
        static bool CheckingForScreening(List<movie> movieList, List<Screening> screeningList, movie t)
        {
            bool availScreen;
            if (t.IndscreeningList.Count >= 1)
            {
                availScreen = true;
            }
            else { availScreen = false; }
            return availScreen;
        }
        static void OrderMovieTicket(List<movie> movieList, List<Screening> screeningList, List<order> orderList)
        {
            string title = ListmovieScreenings(movieList, screeningList); // list movie list & prompting user for movie
            movie moviechoice = null;
            foreach (movie m in movieList)
            {
                if (m.title == title)
                {
                    moviechoice = m;
                }
            }
            bool availscreen = CheckingForScreening(movieList, screeningList, moviechoice);

            while (availscreen == true)
            {
                int screeningchoice = GetInt("Enter S/No for Movie Screening: ") ; // prompting user for screening choice
                Screening choice = SearchScreening(screeningList, screeningchoice); // search screening

                if (choice != null)
                {
                    if (choice.movie.title != title) // checking whether the screeningNo that user enter is for that movie.
                    {
                        Console.WriteLine("Movie do not have that option. Please try again."); //display error message
                    }
                    else
                    {
                        Console.WriteLine("Choice of Screening Time: " + choice.screeningDateTime +" ("+ choice.screeningDateTime.DayOfWeek + ")"); // showing user the time they selected.
                        int NoOfSeats = GetInt("Enter Number of Tickets: "); // prompting user for num of seats needed.
                        int NoOfRemainingSeats = GetNoOfSeats(choice, orderList, movieList, screeningList); // getting the number seats remaining
                        //Console.WriteLine(NoOfRemainingSeats);
                        bool result = CheckingCapacity(NoOfSeats, NoOfRemainingSeats); // checking capacity for the seats.
                                                                                      //List<double> priceList = new List<double>();
                        if (result == false)
                        {
                            Console.WriteLine("Insufficient Seats Available"); //displaying error message - seats not enough
                        }
                        else
                        {
                            if (choice.movie.classification != "G") // to check age requirement for user.
                            {
                                double total = 0.00;
                                
                                while (true)
                                {
                                    Console.Write("Do you meet the age requirement of the movie(" + choice.movie.classification + ")?[Yes/No]");
                                    string answer = Console.ReadLine();
                                    if (answer == "Yes")
                                    {
                                        order o = CreatingOrder(orderList, NoOfSeats, choice); // asking for user choice
                                        foreach (ticket t in o.ticketList)
                                        {
                                            double price = t.CalculatePrice(choice);
                                            total += price;
                                            if (t is Adult)
                                            {
                                                Adult a = (Adult)t;
                                                if (a.PopcornOffer == true)
                                                {
                                                    total += 3;
                                                    //Console.WriteLine("3");
                                                }
                                            }
                                        }

                                        Console.WriteLine("Total Price: $" + total);
                                        MakingPayment(o, total);
                                        o.amount = total;
                                        break;
                                    }
                                    else if (answer == "No") // checking if it meets the classification requirement
                                    {
                                        Console.WriteLine("Sorry, Movie Not Available!");
                                        break;
                                    }
                                    else { Console.WriteLine("Invalid Input.Please enter [Yes/No]"); }
                                }
                                
                            }
                            else
                            {
                                double total = 0.00;
                                order o = CreatingOrder(orderList, NoOfSeats, choice);
                                foreach (ticket t in o.ticketList)
                                {
                                    double price = t.CalculatePrice(choice);
                                    total += price;

                                    if (t is Adult)
                                    {
                                        Adult a = (Adult)t;
                                        if (a.PopcornOffer == true)
                                        {
                                            total += 3;
                                            //Console.WriteLine("3");
                                        }
                                    }
                                }
                                Console.WriteLine("Total Price: " + total);
                                MakingPayment(o, total);
                                o.amount = total;
                            }

                        }
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Screening Option. Try Again.");
                }
            }
        }
        static void MakingPayment(order o, double amount)
        {
            while(true)
            {
                Console.Write("Enter (M) to make payment: ");
                string paymentInput = Console.ReadLine();
                if (paymentInput == "M")
                {
                    Console.WriteLine("Payment succesfully made!"); //showing status of payment 
                    o.amount = amount;
                    o.status = "Paid"; // changing the status of the payment
                    break;
                }
            }
            
        }
        static order CreatingOrder(List<order> orderList, int NoOfSeats, Screening choice)
        {
            order NewOrder = new order(orderList.Count + 1, DateTime.Now);
            NewOrder.status = "Unpaid";
            for (int i = 1; i <= NoOfSeats; i++)
            {
                Console.WriteLine("Ticket " + i);
                Console.Write("Are you Student/Senior Citizen?[Yes/No]");
                string respo = Console.ReadLine();
                if (respo =="Yes")
                {
                    Console.WriteLine("1. Student \n2. Senior Citizen");
                    int type = GetInt("Enter Type of Ticket (1/2):");
                    if (type == 1)
                    {
                        Console.Write("Enter Level of Study[Primary/Secondary/Tertiary]: ");
                        string l = Console.ReadLine();
                        if (l == "Primary" || l == "Secondary" || l == "Tertiary")
                        {
                            Student s = new Student(choice, l);
                            NewOrder.AddTicket(s);
                        }
                        else
                        {
                            Console.WriteLine("Level of Study not Available.");
                            i -= 1;
                        }

                    }
                    else if (type == 2)
                    {

                        int YOB = GetDateYear("Enter Year of Birth: ");
                        int age = DateTime.Now.Year - YOB;
                        if (age <= 55)
                        {
                            Console.WriteLine("Age is not allowed for Senior Citizen ticket.\nYou are only {0}y/o.\nSenior Citizen are aged 55 and above.", age);
                            i -= 1;
                        }
                        else
                        {
                            SeniorCitizen s = new SeniorCitizen(choice, YOB);
                            NewOrder.AddTicket(s);
                        }
                    }
                }
                else if (respo == "No") //by default it is adult ticket
                {
                    Console.Write("Would you like to purchase popcorn set at $3.00?(Yes/No)");
                    string response = Console.ReadLine();
                    if (response == "Yes")
                    {
                        Console.WriteLine("Popcorn added to cart!");
                        bool popcorn = true;
                        Adult a = new Adult(choice, popcorn);
                        NewOrder.AddTicket(a);

                    }
                    else if (response == "No")
                    {
                        bool popcorn = false;
                        Adult a = new Adult(choice, popcorn);
                        NewOrder.AddTicket(a);

                    }
                    else
                    {
                        Console.WriteLine("Invalid Option. Enter [Yes/No]");
                        i -= 1;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Option. Enter [Yes/No]");
                    i -= 1;
                }
            }
            orderList.Add(NewOrder);
            return NewOrder;
        }

        static void CancelOrder(List<order> orderList, List<movie> movieList, List<Screening> screeningList)
        {
            if (orderList.Count >= 1)
            {
                int orderNo;
                while (true)
                {
                    orderNo = GetInt("Enter Order Number:");
                    if (orderNo <= orderList.Count && orderNo > 0)
                    {
                        break;
                    }
                    else { Console.WriteLine("Invalid Order Number."); }
                }
                for (int i = 0; i < orderList.Count; i++)
                {
                    if (orderNo == orderList[i].orderNo)
                    {
                        order o = orderList[i];
                        DateTime movieTime = o.ticketList[0].screening.screeningDateTime;
                        int DiffInDays = (movieTime - DateTime.Now).Days;
                        Console.WriteLine("Diff in Days: " + DiffInDays);
                        if (DiffInDays >= 1)
                        {
                            int NoOfSeats = o.ticketList.Count;
                            //Console.WriteLine("No of seats" + NoOfSeats);

                            int remaining = GetNoOfSeats(o.ticketList[0].screening, orderList, movieList, screeningList);
                            //Console.WriteLine(remaining);
                            int NewNoofSeats = remaining + NoOfSeats;
                            //updating screening element
                            Screening s = o.ticketList[0].screening;
                            s.seatsRemaining = NewNoofSeats;


                            Console.WriteLine("Seats Remaining: " + NewNoofSeats);
                            o.status = "Cancelled";
                            Console.WriteLine("${0} would be refunded.", o.amount);
                            orderList.Remove(o);
                            Console.WriteLine("Order has been successfully cancelled.");
                        }
                        else
                        {
                            Console.WriteLine("Movie is already screened.");
                            Console.WriteLine("Unsuccessful Cancelation.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No Orders have been made.");
            }
            
        }
        //advanced feature
        static void RecommendMovieByOrder(List<order> orderList, List<movie> movieList)
        {
            if (orderList.Count >= 1)
            {
                Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4, -30}{5,-30}{6}", "S/No", "Title", "Duration", "Classification", "Genre", "Opening Date","Tickets Sold");
                for (int i = 0; i < orderList.Count; i++)
                {
                    order o = orderList[i];
                    movie m;
                    for (int j = 0; j < movieList.Count; j++)
                    {
                        m = movieList[j];
                        if (m == o.ticketList[0].screening.movie)
                        {
                            m.TicketsSold += o.ticketList.Count;
                            //Console.WriteLine(m.TicketsSold);
                        }
                    }
                    movieList.Sort();
                }
                for (int j = 0; j < movieList.Count; j++)
                {
                    movie m = movieList[j];

                    List<string> genreList = m.GetGenreList();
                    string s = string.Join("/", genreList.ToArray());
                    Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4,-30}{5,-30}{6,-10}", j + 1, m.title, m.duration, m.classification, s, m.openingDate.ToString("dd/MM/yyyy"), m.TicketsSold);

                    m.TicketsSold = 0;
                }
            }
            
            
        }

        static void ListTop3(List<order> orderList, List<movie> movieList)
        {
            if (orderList.Count >= 1)
            {
                Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4, -30}{5,-30}{6}", "S/No", "Title", "Duration", "Classification", "Genre", "Opening Date", "Tickets Sold");
                for (int i = 0; i < orderList.Count; i++)
                {
                    order o = orderList[i];
                    movie m;
                    for (int j = 0; j < movieList.Count; j++)
                    {
                        m = movieList[j];
                        if (m == o.ticketList[0].screening.movie)
                        {
                            m.TicketsSold += o.ticketList.Count;
                            //Console.WriteLine(m.TicketsSold);
                        }
                    }
                    movieList.Sort();
                }
                for (int j = 0; j < movieList.Count; j++)
                {
                    if (j == 3)
                    {
                        break;
                    }
                    movie m = movieList[j];

                    List<string> genreList = m.GetGenreList();
                    string s = string.Join("/", genreList.ToArray());
                    Console.WriteLine("{0,-10} {1,-30}{2,-15}{3,-20}{4,-30}{5,-30}{6,-10}", j + 1, m.title, m.duration, m.classification, s, m.openingDate.ToString("dd/MM/yyyy"), m.TicketsSold);

                    m.TicketsSold = 0;
                }
            }


        }
        // according to number of seats available
        static void DisplayMovieAccordSeats(List<order> orderList, List<Screening> screeningList,List<movie> movieList)
        {
            if (orderList.Count >= 1)
            {
                for (int i = 0; i < orderList.Count; i++)
                {
                    order o = orderList[i];
                    Screening s = o.ticketList[0].screening;
                    s.seatsRemaining = GetNoOfSeats(s, orderList, movieList, screeningList);
                }

            }
            screeningList.Sort();
            DisplayMovie(movieList);
            int moviechoiceNo;
            while (true)
            {
                moviechoiceNo = GetInt("Enter movie S/No: ");
                if (moviechoiceNo <= movieList.Count && moviechoiceNo > 0)
                {
                    break;
                }
                else { Console.WriteLine("Invalid Input. Choice Not Available."); }
            }
            string movietitle = movieList[moviechoiceNo - 1].title;
            movie t = SearchMovie(movieList, movietitle);
            bool availscreen = CheckingForScreening(movieList, screeningList, t);
            if (t != null)
            {
                if (availscreen == true)
                {
                    Console.WriteLine("{0,-10}{1,-25}{2,-20}{3,-20}{4,-20}{5}", "S/No", "Date/Time", "Screening Type", "Location Name", "Hall No.","Seats Remaining");
                    for (int i = 0; i < screeningList.Count; i++)
                    {
                        Screening m = screeningList[i];
                        if (t.title == m.movie.title)
                        {
                            Console.WriteLine("{0,-10}{1,-25}{2,-20}{3,-20}{4,-20}{5}", m.screeningNo, m.screeningDateTime, m.screeningType, m.Cinema.name, m.Cinema.hallNo, m.seatsRemaining);
                        }


                    }
                }
                else
                {
                    Console.WriteLine("No Screening Available for the Movie.");
                }


            }
            else
            {
                Console.WriteLine("Movie Not Available");
            }
        }
        static int GetDateYear(string prompt)
        {
            int n;
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    n = Convert.ToInt32(Console.ReadLine());
                    if (n>1900)
                    {
                        break;
                    }    
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Invalid year entered, Please try again.");
                }
            }
            return n;
        }
        static int GetInt(string prompt)
        {
            int n;
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    n = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input, Please enter an integer.");
                }
            }
            return n;
        }

        static DateTime GetDateTime(string prompt)
        {
            DateTime n;
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    n = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input, Please enter a valid DateTime in format given.");
                }
            }
            return n;
        }
    }
}
   

