using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class retrieves information on spaces from the DB
    /// </summary>
    public class SpaceSQLDAO
    {

        private string connectionString;

        private string topFive = "SELECT TOP(5) space.id AS spaceid, space.name as spacename, is_accessible, max_occupancy, daily_rate " +
            " FROM space WHERE space.id NOT IN (SELECT space.id FROM space JOIN reservation ON space.id = reservation.space_id " +
        " WHERE space.venue_id = @venueId AND start_date <= @startDate AND end_date >= @endDate) AND max_occupancy >= @max_occupancy AND venue_id = @venueId GROUP BY space.name, daily_rate, is_accessible, space.id, max_occupancy " +
            " ORDER BY daily_rate DESC;";

        private string searchByVenueId = "SELECT id AS SpaceId, venue_id AS VenueId, name AS SpaceName, "
            + " is_accessible AS IsAccessible, daily_rate AS DailyRate, max_occupancy As MaxOccupancy, "
            + " open_from AS OpenFrom, open_to AS OpenTo FROM space WHERE venue_id = @VenueId";

        private string returnSpacesAccessability = "SELECT is_accessible AS IsAccessible FROM space WHERE id = @SpaceId;";

        private string returnAllSpacesByVenueId = "SELECT id AS SpaceId FROM space WHERE venue_id = @VenueId;";



        public SpaceSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;
        }
        /// <summary>
        /// 
        ///This method retrieves space properties from the DB and creates a space object in .NET
        /// </summary>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        public List<string> DisplayAllSpacesByVenueId(string venueId)
        {
            List<string> allSpacesByVenue = new List<string>();
            String spaceString = "";
            string isAccessible = "";
            string openMonth = "";
            string closeMonth = "";
            Space space = new Space();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(searchByVenueId, conn);
                command.Parameters.AddWithValue("@VenueId", venueId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    space.SpaceId = Convert.ToInt32(reader["SpaceId"]);
                    space.VenueId = Convert.ToInt32(reader["VenueId"]);
                    space.Name = Convert.ToString(reader["SpaceName"]);
                    space.WheelChairAccessibility = Convert.ToBoolean(reader["IsAccessible"]);
                    space.DailyRate = Convert.ToDecimal(reader["DailyRate"]);
                    space.MaximumOccupancy = Convert.ToInt32(reader["MaxOccupancy"]);
                    space.OpenDate = Convert.ToString(reader["OpenFrom"]);
                    space.CloseDate = Convert.ToString(reader["OpenTo"]);
                    closeMonth = ConvertToMonth(space.CloseDate);
                    openMonth = ConvertToMonth(space.OpenDate);

                   if(space.WheelChairAccessibility)
                    {
                        isAccessible = "No";
                    }
                    isAccessible = "Yes";

                    spaceString = ($"{space.SpaceId}) {space.Name} {openMonth} {closeMonth} {space.DailyRate} {space.MaximumOccupancy} WheelChair Accessibility: {isAccessible}");
                    allSpacesByVenue.Add(spaceString);

                }



                return allSpacesByVenue;

            }
            



        }
    

        public string ConvertToMonth(string month)
        {
            Dictionary<string, string> Months = new Dictionary<string, string>()
            {
                {"1", "January" },
                {"2", "February" },
                {"3", "March" },
                {"4", "April" },
                {"5", "May" },
                {"6", "June" },
                {"7", "July" },
                {"8", "August" },
                {"9", "September" },
                {"10", "October" },
                {"11", "November" },
                {"12", "December" },

            };
            if (string.IsNullOrEmpty(month))
            {
                return "Open All Year";
            }

            return Months[month];

        }
                        

      
        /// <summary>
        /// displays the top5 available spaces for that venue based on client needs
        /// </summary>
        /// <param name="venueId"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="startDate"></param>
        /// <param name="numberOfAttendees"></param>
        /// <returns></returns>
        public List<string> TopFiveAvailable(int venueId, int numberOfDays, DateTime startDate, int numberOfAttendees)
        {
            List<Space> topSpaces = new List<Space>();
            List<string> topStrings = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(topFive, conn);
                command.Parameters.AddWithValue("@venueId", venueId);
                command.Parameters.AddWithValue("@max_occupancy",numberOfAttendees);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", startDate.AddDays(numberOfDays));
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    Space space = new Space();
                    space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
                    space.TotalCost = (decimal)numberOfDays * space.DailyRate;
                    space.Name = Convert.ToString(reader["spaceName"]);
                    space.SpaceId = Convert.ToInt32(reader["SpaceId"]);
                    space.MaximumOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                    space.WheelChairAccessibility = Convert.ToBoolean(reader["is_accessible"]);
                    topSpaces.Add(space);
                }
                foreach (Space item in topSpaces)
                {
                    string topSpaceString = "";
                    topSpaceString = ($"{item.SpaceId} {item.Name} {item.DailyRate.ToString("C")} {item.MaximumOccupancy} {item.WheelChairAccessibility} {item.TotalCost.ToString("C")}");
                    topStrings.Add(topSpaceString);
                }
            }
            return topStrings;
        }

    }
}

