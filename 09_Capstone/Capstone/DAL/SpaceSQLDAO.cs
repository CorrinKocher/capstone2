﻿using System;
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

        private string topFive = "SELECT TOP(5) space.id AS spaceid, space.name as spacename, is_accessible, max_occupancy, daily_rate " +
            " FROM space WHERE space.id NOT IN (SELECT space.id FROM space JOIN reservation ON space.id = reservation.space_id " +
        " WHERE space.venue_id = @venueId AND start_date <= @startDate AND end_date >= @endDate) AND venue_id = @venueId GROUP BY space.name, daily_rate, is_accessible, space.id, max_occupancy " +
            " ORDER BY daily_rate DESC;";

        private string searchBySpaceId = "SELECT id AS SpaceId, venue_id AS VenueId, name AS SpaceName, "
            + " is_accessible AS IsAccessible, daily_rate AS DailyRate, max_occupancy As MaxOccupancy, "
            + " open_from AS OpenFrom, open_to AS OpenTo FROM space WHERE id = @SpaceId;";

        private string returnSpacesAccessability = "SELECT is_accessible AS IsAccessible FROM space WHERE id = @SpaceId;";

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
            if(string.IsNullOrEmpty(month))
            {
                return "Open All Year";
            }

            return Months[month];

        }

       

        

        public string CreateSpaceString(Space space)
        {
           
            Space newSpace = CreateSpaceModel(space.SpaceId);
            string closeMonth = ConvertToMonth(newSpace.CloseDate);
            string openMonth = ConvertToMonth(newSpace.OpenDate);
            string accessibility = ConvertWheelChairBoolToString(newSpace); // do we need later?


            return ($"{newSpace.SpaceId}) {newSpace.Name} {openMonth} {closeMonth} {newSpace.DailyRate} {newSpace.MaximumOccupancy}");

        }

        public List<string> DisplayAllSpacesByVenueId(string VenueId)
        {
           
            List<string> allSpacesByVenue = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(returnAllSpacesByVenueId, conn);
                command.Parameters.AddWithValue("@VenueId", VenueId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Space space = new Space();
                    space.SpaceId = Convert.ToInt32(reader["SpaceId"]);
                    allSpacesByVenue.Add(CreateSpaceString(space));
                }
            }
            return allSpacesByVenue;
        }

        public List<string> TopFiveAvailable(int venueId, int numberOfDays, DateTime startDate)
        {
            List<Space> topSpaces = new List<Space>();
            List<string> topStrings = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(topFive, conn);
                command.Parameters.AddWithValue("@venueId", venueId);
                
                command.Parameters.AddWithValue("startDate", startDate);
                command.Parameters.AddWithValue("@endDate", startDate.AddDays(numberOfDays));
                Space space = new Space();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
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
            }return topStrings;
        }

    }
}

