using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            foreach (string venue in venueDAO.GetAllVenueNames())
            {
                Console.WriteLine(venue);
            }
            int venueIdRequested = 0;
            Console.WriteLine("Please enter the VenueId you would like more information on");
            venueIdRequested = int.Parse(Console.ReadLine());
            venueDAO.DisplayVenueDetails(venueIdRequested);
            Console.WriteLine(venueDAO.DisplayVenueDetails(venueIdRequested));
        }






















        //public void WriteAllVenues()
        //{
        //    foreach (string venue in venueDAO.GetAllVenueNames())
        //    {
        //        Console.WriteLine(venue);
        //    }
        //}

         



}
}
