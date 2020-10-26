using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;

namespace Capstone.DAL
{
    /// <summary>
    /// Methods: This class retrieves, inserts venues from SQL
    /// </summary>
    public class VenueSQLDAO
    {
        private string searchByVendorId = "SELECT venue.id AS VenueId, " +
            " venue.name AS VenueName, city.name AS CityName, venue.description AS VenueDescription, " +
            " city.state_abbreviation AS StateAbbreviation FROM venue JOIN city ON venue.city_id = city.id " +
            " WHERE venue.id = @venueid;";
       
        private string connectionString;
        private string categoryName = "SELECT category.name AS CategoryName FROM category " +
            " JOIN category_venue ON category.id = category_venue.category_id WHERE venue_id = @venueid;";

        /// <summary>
        /// Creates a new sql based venue dao
        /// </summary>
        /// <param name="databaseConnectionString"></param>
        public VenueSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;
        }


        /// <summary>
        /// This method retrieves a list of all the venues in the DB
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// This method displays the venue details, including category information retrieved in 
        /// "retrieve category for venue
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public string DisplayVenueDetails(int venueId)
        {
            Venue venue = new Venue();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(searchByVendorId, conn);
                command.Parameters.AddWithValue("@venueid", venueId);

                SqlDataReader reader = command.ExecuteReader(); 
                                                                

                while (reader.Read())
                {

                    venue.VenueId = Convert.ToInt32(reader["VenueId"]);
                    venue.VenueName = Convert.ToString(reader["VenueName"]);
                    venue.CityName = Convert.ToString(reader["CityName"]);
                    venue.State = Convert.ToString(reader["StateAbbreviation"]);
                    venue.Description = Convert.ToString(reader["VenueDescription"]);

                }
                return ($"\n\n{venue.VenueName}\nLocation: {venue.CityName}, {venue.State}\nCategories:{RetreiveCategoryForVenue(venueId)} \n\n{venue.Description}");
            }

        }
        /// <summary>
        /// This method gets the venues categories from theDB
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        public string RetreiveCategoryForVenue(int venueId)
        {
            Venue venue = new Venue();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(categoryName, conn);
                command.Parameters.AddWithValue("@venueid", venueId);
                string allCategories = "";
                List<string> categories = new List<string>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    venue.Category = Convert.ToString(reader["CategoryName"]);
                    categories.Add(venue.Category);
                }
                foreach (string item in categories)
                {
                    allCategories += item + " ";
                }
                return allCategories;
                
            }
                
               
        }

    }


}




