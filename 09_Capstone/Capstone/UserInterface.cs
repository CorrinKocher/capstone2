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
        /// <summary>
        /// this starts and stops the console from displaying all other menus
        /// </summary>
        public void Run()
        {
            bool done = false;
            while (!done)
            {

                DisplayMainMenu();
                string menuSelection = Console.ReadLine().ToUpper();
                done = MainMenuSelection(menuSelection);


            }

        }
        /// <summary>
        /// this method just displays the Main Menu options
        /// </summary>
        public void DisplayMainMenu()
        {

            Console.WriteLine("What would you like to do ?");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");

        }
        /// <summary>
        /// This method allows you to continue to navigate the main
        /// menu and venue menu as long as you dont select quit
        /// </summary>
        /// <param name="menuSelection"></param>
        /// <returns></returns>
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
                    if (venueIdRequested <= 15)
                    {

                        Console.WriteLine(venueDAO.DisplayVenueDetails(venueIdRequested));
                        VenueMenu(venueIdRequested);

                        return false;
                    }
                    Console.WriteLine("Please enter a valid selection");
                    return false;
                case "Q":
                    Console.WriteLine("Thank you for using our service!");
                    return true;
            }
            Console.WriteLine("Please enter a valid selection");
            return false;
        }
        /// <summary>
        /// this method displays the venue menu options and creates a variable to insert into
        /// the venue menue selection method
        /// </summary>
        /// <param name="venueIdRequested"></param>
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
                string venueMenuSelection = Console.ReadLine().ToUpper();
                done = VenueMenuSelection(venueMenuSelection, venueIdRequested);

            }


        }


        /// <summary>
        /// this method takes in the selection from the user from VenueMenu
        /// and allows the user to continue to navigate the venue options until they select
        /// R to return the previous screen. This method calls the method search for a reservation
        /// (below) so that a reservation can be made by the user.
        /// </summary>
        /// <param name="venueMenuSelection"></param>
        /// <param name="venueIdRequested"></param>
        /// <returns></returns>
        public bool VenueMenuSelection(string venueMenuSelection, int venueIdRequested)
        {

            switch (venueMenuSelection)
            {
                case "1":
                    //Space space = new Space()
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
                    string bottomMenuSelection = Console.ReadLine().ToUpper();
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

        /// <summary>
        /// this method retrieves data from the user needed to create a reservation and
        /// allows the user to continue to make reservations
        /// </summary>
        /// <param name="venueIdRequested"></param>
        /// <returns></returns>
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
                        string tryAgain = Console.ReadLine().ToUpper();
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
                    string tryAgain = Console.ReadLine().ToUpper();
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








