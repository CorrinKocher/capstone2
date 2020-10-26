using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Capstone.DAL;
namespace Capstone
{
    public class UserInterface
    {
        //ALL Console.ReadLine and WriteLine in this class
        //NONE in any other class
        /// <summary>
        /// Methods to create:
        /// </summary>
        private VenueSQLDAO venueDAO;
        private SpaceSQLDAO spaceDAO;
        private ReservationSQLDAO reservationDAO;
        private string connectionString;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            this.venueDAO = new VenueSQLDAO(connectionString);
            this.spaceDAO = new SpaceSQLDAO(connectionString);
            this.reservationDAO = new ReservationSQLDAO(connectionString);
        }

        public void Run()
        {
            bool done = false;
            while (!done)
            {

                DisplayMainMenu();
                string menuSelection = Console.ReadLine();
                done = MainMenuSelection(menuSelection);


            }

        }

        public void DisplayMainMenu()
        {

            Console.WriteLine("What would you like to do ?");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");

        }

        public bool MainMenuSelection(string menuSelection)
        {
            switch (menuSelection)
            {
                case "1":
                    foreach (string venue in venueDAO.GetAllVenueNames())
                    {
                        Console.WriteLine(venue);
                    }
                    int venueIdRequested = 0;
                    Console.WriteLine("Please enter the VenueId you would like more information on");
                    venueIdRequested = int.Parse(Console.ReadLine());
                    Console.WriteLine(venueDAO.DisplayVenueDetails(venueIdRequested));
                    VenueMenu(venueIdRequested);

                    return false;
                case "Q":
                    Console.WriteLine("Thank you for using our service!");
                    return true;
            }
            Console.WriteLine("Please enter a valid selection");
            return false;
        }

        public void VenueMenu(int venueIdRequested)
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) View Spaces");
                Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("R) Return to Previous Screen");
                string venueMenuSelection = Console.ReadLine();
                done = VenueMenuSelection(venueMenuSelection, venueIdRequested);
               
            }


        }

                     

        public bool VenueMenuSelection(string venueMenuSelection, int venueIdRequested)
        {

            switch (venueMenuSelection)
            {
                case "1":
                    List<string> spacesList = spaceDAO.DisplayAllSpacesByVenueId(venueIdRequested.ToString());

                    foreach (string item in spacesList)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("What would you like to do next?");
                    Console.WriteLine(" 1) Reserve a Space");
                    Console.WriteLine(" R) Return to Previous Screen");
                    string bottomMenuSelection = Console.ReadLine();
                    switch (bottomMenuSelection)
                    {
                        case "1":
                            SearchForAReservation(venueIdRequested);
                            break;
                        case "R":
                            return true;

                    }

                    break;
                case "2":
                    SearchForAReservation(venueIdRequested);
                    return false;
                case "R":
                    return true;

            }
            return false;
        }

    
        public bool SearchForAReservation(int venueIdRequested)
        {
            bool isAvailable = true;
            bool done = false;
            while (!done)
            {
                string venueSpaceCost = "";
                Reservation reservation = new Reservation();
                int spaceId = 0;
                string johnFulton = "";
                int numberOfAttendees = 0;
                Console.WriteLine("What Date would you like to start your reservation? (yyyy-mm-dd)");

                DateTime startDate = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("How many days would you like?");
                int numberOfDays = int.Parse(Console.ReadLine());
                Console.WriteLine("How many people will be in attendance?");
                numberOfAttendees = int.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Here's what is available:");
                if ((spaceDAO.TopFiveAvailable(venueIdRequested, numberOfDays, startDate, numberOfAttendees)).Count > 0)
                {
                    foreach (string item in spaceDAO.TopFiveAvailable(venueIdRequested, numberOfDays, startDate, numberOfAttendees))
                    {

                        Console.WriteLine(item);
                    }
                    Console.WriteLine("What space would you like to reserve?");
                    spaceId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Who is this reservation for?");
                    johnFulton = Console.ReadLine();
                    DateTime endDate = startDate.AddDays(numberOfDays);
                    isAvailable = reservationDAO.CheckIfSpaceIsAvailable(startDate, endDate, numberOfAttendees, spaceId);
                    if (isAvailable == true)
                    {
                        reservation = reservationDAO.CreateReservation(startDate, endDate, numberOfAttendees, johnFulton, spaceId);
                        venueSpaceCost = reservationDAO.ReturnReservationVenueNameTotalCost(reservation, numberOfDays);
                        Console.WriteLine(reservationDAO.CreateReservationConfirmation(reservation, venueSpaceCost));
                        Console.WriteLine("Do you want to make another reservation? y/n");
                        if (Console.ReadLine() == "y")
                        {
                            return false;

                        }
                        else
                        {
                            done = !done;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No spaces available, would you like to search again? Y/N");
                        string tryAgain = Console.ReadLine();
                        switch (tryAgain)
                        {
                            case "Y":
                                return false;

                            case "N":
                                return true;

                        }
                    }
                    return false;

                }
                else
                {
                    Console.WriteLine("No spaces available, would you like to search again? Y/N");
                    string tryAgain = Console.ReadLine();
                    switch (tryAgain)
                    {
                        case "Y":
                            return false;

                        case "N":
                            return true;

                    }
                }
              
            }
            return false;


        }
    }
}   
        
        
        
        
        
        
        

