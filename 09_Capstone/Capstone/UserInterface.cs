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
        private string connectionString;
        


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            this.venueDAO = new VenueSQLDAO(connectionString);
            //this.spaceDAO = new SpaceSQLDAO(connectionString);
        }
        public void Run()
        {
           
            DisplayMainMenu();
            string menuSelection = Console.ReadLine();
            MainMenuSelection(menuSelection);
            int venueIdRequested = 0;
            Console.WriteLine("Please enter the VenueId you would like more information on");
            venueIdRequested = int.Parse(Console.ReadLine());
            Console.WriteLine(venueDAO.DisplayVenueDetails(venueIdRequested));           
            VenueMenu();
            string venueMenuSelection = Console.ReadLine();
            
        }
        
        public void DisplayMainMenu()
        {

            Console.WriteLine("What would you like to do ?");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            
        }

        public void MainMenuSelection(string menuSelection)
        {
            switch (menuSelection)
            {
                case "1":
                    foreach (string venue in venueDAO.GetAllVenueNames())
                    {
                        Console.WriteLine(venue);
                    }
                    break;
                case "Q":
                    Console.WriteLine("Thank you for using our service!");
                    break;
            }
        }
        public void VenueMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine(  "1) View Spaces");
            Console.WriteLine(  "2) Search for Reservation");
            Console.WriteLine(  "R) Return to Previous Screen");
        }













    }
}
