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
        public bool keepRunning = true;
        public void Run()
        {

           
            while(keepRunning)
            {

                DisplayMainMenu();
                string menuSelection = Console.ReadLine();
                MainMenuSelection(menuSelection);
            }
                int venueIdRequested = 0;
                Console.WriteLine("Please enter the VenueId you would like more information on");
                venueIdRequested = int.Parse(Console.ReadLine());
                Console.WriteLine(venueDAO.DisplayVenueDetails(venueIdRequested));
                VenueMenu();
                string venueMenuSelection = Console.ReadLine();
                VenueMenuSelection(venueMenuSelection, venueIdRequested);
                Console.WriteLine();
                Console.ReadLine();
            
            //MenuAtBottomOfDisplayAllSpaces();

            //Space space = new Space();
            //int spaceIdRequested = 0;


            //DisplayMainMenu();
            //string menuSelection = Console.ReadLine();
            //MainMenuSelection(menuSelection);


            //Console.WriteLine("Please enter the VenueId you would like more information on");
            //venueIdRequested = int.Parse(Console.ReadLine());

            //Console.WriteLine(venueDAO.DisplayVenueDetails(venueIdRequested));

            //Console.WriteLine();
            //Console.WriteLine("please enter the spaceID you wouldlike to see");
            //spaceIdRequested = int.Parse(Console.ReadLine());
            //spaceDAO.CreateSpaceModel(spaceIdRequested);



        }

        public void DisplayMainMenu()
        {

            while(keepRunning)
            {

            Console.WriteLine("What would you like to do ?");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            }

        }

        public void MainMenuSelection(string menuSelection)
        {
            switch (menuSelection.ToUpper())
            {
                case "1":
                    foreach (string venue in venueDAO.GetAllVenueNames())
                    {
                        Console.WriteLine(venue);
                    }
                    break;
                case "Q":
                    Console.WriteLine("Thank you for using our service!");
                    keepRunning = false;
                    break;
            }
        }

        public void VenueMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) View Spaces");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("R) Return to Previous Screen");
        }

        public void MenuAtBottomOfDisplayAllSpaces()
        {

        }

        public void VenueMenuSelection(string venueMenuSelection, int venueIdRequested)
        {
            switch (venueMenuSelection.ToUpper())
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
                            break;
                        case "R":

                            break;
                    }

                    break;
                case "2":
                    CaseTwoVenueMenu(venueIdRequested);
                    break;
                case "R":
                    DisplayMainMenu();
                    break;

            }
        }

        public void BottomMenuSelection(string bottomMenuSelection, int venueIdRequested)
        {
            switch (bottomMenuSelection)
            {
                case "1":
                    CaseTwoVenueMenu(venueIdRequested);
                    break;
                case "R":

                    break;
            }
        }

        public void CaseTwoVenueMenu(int venueIdRequested)
        {
            Console.WriteLine("What Date would you like to start your reservation? (yyyy-mm-dd)");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("How many days would you like?");
            int numberOfDays = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Here's what is available:");
            if ((spaceDAO.TopFiveAvailable(venueIdRequested, numberOfDays, startDate)).Count > 0)
            {
                foreach (string item in spaceDAO.TopFiveAvailable(venueIdRequested, numberOfDays, startDate))
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("No spaces available, would you like to search again? Y/N");
                string tryAgain = Console.ReadLine();
                switch (tryAgain)
                {
                    case "Y":
                        VenueMenu();
                        break;
                    case "N":
                        DisplayMainMenu();
                        break;
                }
            }
        }











    }
}
