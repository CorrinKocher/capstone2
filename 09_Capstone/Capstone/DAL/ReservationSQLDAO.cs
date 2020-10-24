using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// methods: bookReservation,
    /// </summary>
    public class ReservationSQLDAO
    {
        private string searchReservation = "SELECT space.name AS spaceName FROM space WHERE space.id NOT IN " +
        " (SELECT space.id FROM space JOIN reservation ON space.id = reservation.space_id " +
        " WHERE space.venue_id = @venueId AND start_date <= @startDate AND end_date >= @endDate) " +
        " AND space.venue_id = @venueId;";
       
        private string connectionString;
        public ReservationSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;
        }

        public List<string> SearchAvailableSpacesToReserve(int venueId, DateTime startDate, int numberOfDays)
        {
            List<string> openSpaces = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(searchReservation, conn);
                command.Parameters.AddWithValue("@venueId", venueId);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", startDate.AddDays(numberOfDays));

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string availableSpaces = Convert.ToString(reader["spaceName"]);
                    openSpaces.Add(availableSpaces);
                }
                
                return openSpaces;
            }
        }
        
    }
}

                    //Reservation reservation = new Reservation();
                    //reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
                    //reservation.EndDate = Convert.ToDateTime(reader["end_date"]);