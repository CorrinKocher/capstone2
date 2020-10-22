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
            this.spaceDAO = new SpaceSQLDAO(connectionString);
        }
        
        public void Run()
        {
            Console.WriteLine(venueDAO.GetAllVenueNames().ToString());
            Console.WriteLine("Reached the User Interface.");
            Console.ReadLine();
        }
        
       

    }
}
