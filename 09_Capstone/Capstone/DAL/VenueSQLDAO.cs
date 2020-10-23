using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;

namespace Capstone.DAL
{
    /// <summary>
    /// Methods: DisplayAllVenues- do a join to category_venue, SelectVenue, ReturnAllAvailableDatesForAVenue, DisplayVenueDetails
    /// </summary>
    public class VenueSQLDAO
    {
        private string searchByVendorId = "SELECT venue.id AS VenueId, venue.name AS VenueName, city.name AS CityName, venue.description AS VenueDescription, city.state_abbreviation AS StateAbbreviation FROM venue JOIN city ON venue.city_id = city.id WHERE venue.id = @venueid;";
       // private string searchByVendorId = "SELECT * FROM venue JOIN category_venue "
       //     + " ON venue.id = category_venue.venue_id JOIN category ON category_venue.category_id "
       //    + " = category.id JOIN city ON venue.city_id = city.id WHERE venue.id = @venue.id;";
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT id, name FROM venue", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
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

        public string DisplayVenueDetails(int venueId) 
        {
            Venue venue = new Venue();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(searchByVendorId, conn);
                command.Parameters.AddWithValue("@venueid", venueId);

                SqlDataReader reader = command.ExecuteReader(); //System.Data.SqlClient.SqlException: 'Incorrect syntax near '.'.
                                                                //Must declare the scalar variable "@venue".'

                while (reader.Read())
                {
                    
                    venue.VenueId = Convert.ToInt32(reader["VenueId"]);
                    venue.VenueName = Convert.ToString(reader["VenueName"]);
                   // venue.Category = Convert.ToString(reader["category.name"]);
                    venue.City = Convert.ToString(reader["CityName"]);
                    venue.State = Convert.ToString(reader["StateAbbreviation"]);
                    venue.Description = Convert.ToString(reader["VenueDescription"]);

                    
                }
                return ($"{venue.VenueName} \n \n Location: {venue.City}, {venue.State}  \n \n {venue.Description}");
            }
                                   
        }


    }


}




