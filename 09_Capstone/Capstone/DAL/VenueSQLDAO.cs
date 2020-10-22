using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;

namespace Capstone.DAL
{
    /// <summary>
    /// Methods: DisplayAllVenues- do a join to category_venue, SelectVenue, ReturnAllAvailableDatesForAVenue, DispayVenueDetails
    /// </summary>
    public class VenueSQLDAO
    {
        private string connectionString;

        
        /// <summary>
        /// Creates a new sql based venue dao
        /// </summary>
        /// <param name="databaseConnectionString"></param>
        public VenueSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;            
        }

 

        public List<string> GetAllVenueNames()
        {
          
            List<Venue> venues = new List<Venue>();
            List<string> venueString = new List<string>();
                string venueIdAndName = "";
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT id, name FROM venue", conn);
                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    Venue venue = new Venue();
                    venue.VenueId = Convert.ToInt32(reader["id"]);
                    venue.VenueName = Convert.ToString(reader["name"]);

                    venues.Add(venue);
                }
                foreach (Venue item in venues)
                {

                    venueIdAndName = (($"({item.VenueId}) {item.VenueName}"));
                    venueString.Add(venueIdAndName);
                }
            }

            return venueString;          
        }


    }


}




