using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// methods to make: CreateSpaceModel, ViewAllSpacesByVenueId,  convertwheelchairbooltostring, convertDateIntsToMonthsString
    /// </summary>
    public class SpaceSQLDAO
    {

        private string connectionString;

        private string searchBySpaceId = "SELECT id AS SpaceId, venue_id AS VenueId, name AS SpaceName, "
            + " is_accessible AS IsAccessible, daily_rate AS DailyRate, max_occupancy As MaxOccupancy, "
            + " open_from AS OpenFrom, open_to AS OpenTo FROM space WHERE id = @SpaceId;";

        private string returnSpacesAccessability = "SELECT is_accessible FROM space WHERE id = @SpaceId;";

        private string returnAllSpacesByVenueId = "SELECT id AS SpaceId FROM space WHERE venue_id = @VenueId;";



        public SpaceSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;
        }

        public Space CreateSpaceModel(int spaceId)
        {
            Space space = new Space();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(searchBySpaceId, conn);
                command.Parameters.AddWithValue("@SpaceId", spaceId);

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

                }

            }
            return space;

        }
        public string ConvertWheelChairBoolToString(Space thisSpace)
        {
            string isAccessible = "";
            Space space = new Space();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(returnSpacesAccessability, conn);
                command.Parameters.AddWithValue("@SpaceId", thisSpace.SpaceId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    space.WheelChairAccessibility = Convert.ToBoolean(reader["IsAccessible"]);
                    if (space.WheelChairAccessibility)
                    {
                        isAccessible = "Yes";
                    }
                    isAccessible = "No";

                }

            }
            return isAccessible;
        }

        
        public string ConvertOpenDateIntegerToMonth(Space space)
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
            if(space.OpenDate == null)
            {
                return "Open All Year";
            }

            return Months[space.OpenDate];

        }

        public string ConvertCloseDateIntegerToMonth(Space space)
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
            if (space.CloseDate == null)
            {
                return "Open All Year";
            }
            return Months[space.CloseDate];

        }

        public string CreateSpaceString(Space space)
        {
            CreateSpaceModel(space.SpaceId);
            ConvertCloseDateIntegerToMonth(space);
            ConvertOpenDateIntegerToMonth(space);
            ConvertWheelChairBoolToString(space);


            return ($"{space.SpaceId}) {space.Name} {space.OpenDate} {space.CloseDate} {space.DailyRate} {space.MaximumOccupancy}");

        }

        public List<string> DisplayAllSpacesByVenueId(string VenueId)
        {
            Space space = new Space();
            List<string> allSpacesByVenue = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(returnAllSpacesByVenueId, conn);
                command.Parameters.AddWithValue("@VenueId", VenueId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    space.SpaceId = Convert.ToInt32(reader["SpaceId"]);
                    allSpacesByVenue.Add(CreateSpaceString(space));
                }
            }
            return allSpacesByVenue;
        }

    }
}

