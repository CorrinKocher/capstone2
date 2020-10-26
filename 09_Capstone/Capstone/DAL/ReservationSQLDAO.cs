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

        private string insertReservationIntoSQL = "INSERT INTO reservation(space_id, number_of_attendees, start_date, end_date, reserved_for) "
        + " OUTPUT inserted.reservation_id AS reservationId, inserted.space_id AS spaceId, "
        + " inserted.number_of_attendees AS numberOfAttendees, inserted.start_date AS startDate, "
        + " inserted.end_date AS endDate, inserted.reserved_for AS reservedForName "
        + " VALUES (@spaceId,@numberOfAttendees,@startDate, @endDate, @reservedForName);";
        private string returnReservationVenueNameAndSpaceName = "SELECT venue.name AS venueName, space.name AS spaceName, space.daily_rate AS dailyRate FROM venue "
        + " JOIN space ON venue.id = space.venue_id JOIN reservation ON space.id = reservation.space_id "
        + " WHERE reservation_id = @reservationId;";
        private string isSpaceAvailable = " SELECT space.name AS spaceName FROM space WHERE space.id IN " +
        " (SELECT space.id FROM space JOIN reservation ON space.id = reservation.space_id " +
        " WHERE space.id = @spaceId AND start_date <= @startDate AND end_date >= @endDate);";



        private string connectionString;
        public ReservationSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;
        }


        /// <summary>
        /// This method inserts a reservation into the system
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="numberOfAttendees"></param>
        /// <param name="reservedForName"></param>
        /// <param name="spaceId"></param>
        /// <returns></returns>

        public Reservation CreateReservation(DateTime startDate, DateTime endDate, int numberOfAttendees, string reservedForName, int spaceId)
        {
            Reservation reservation = new Reservation();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(insertReservationIntoSQL, conn);
                command.Parameters.AddWithValue("@spaceId", spaceId);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
                command.Parameters.AddWithValue("@numberOfAttendees", numberOfAttendees);
                command.Parameters.AddWithValue("@reservedForName", reservedForName);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reservation.ReservationId = Convert.ToInt32(reader["reservationId"]);
                    reservation.SpaceId = Convert.ToInt32(reader["spaceId"]);
                    reservation.StartDate = Convert.ToDateTime(reader["startDate"]);
                    reservation.EndDate = Convert.ToDateTime(reader["endDate"]);
                    reservation.ReservedFor = Convert.ToString(reader["reservedForName"]);
                    reservation.NumberOfAttendees = Convert.ToInt32(reader["numberOfAttendees"]);
                }
            }
            return reservation;
        }
        /// <summary>
        /// this method creates a reservation confirmation
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="venueSpaceTotalCost"></param>
        /// <returns></returns>
        public string CreateReservationConfirmation(Reservation reservation, string venueSpaceTotalCost)
        {

            string reservationString;
            string startDate = reservation.StartDate.ToString();
            string endDate = reservation.EndDate.ToString();
            string[] venueSpaceCost = venueSpaceTotalCost.Split("|");
            reservationString = ($"ConfirmationNumber: {reservation.ReservationId}\n Venue: {venueSpaceCost[0]} \n Space: {venueSpaceCost[1]} \n Reserved For: {reservation.ReservedFor} \n Attendees: {reservation.NumberOfAttendees} \n Arrival Date: {startDate} \n Depart Date: {endDate} \n Total Cost: {venueSpaceCost[2]} ");
            return reservationString;
        }
        /// <summary>
        /// this method calculates the total cost of a reservation, and obtains the venue/space names associated
        /// with a reservation. to be returned in the createReservationConfirmaion method
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public string ReturnReservationVenueNameTotalCost(Reservation reservation, int numberOfDays)
        {
            decimal totalCost = 0.00M;
            decimal dailyRate = 0.00M;
            string spaceName = "";
            string venueName = "";
            string venueSpacetotalCost = "";
            Reservation thisReservation = new Reservation();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(returnReservationVenueNameAndSpaceName, conn);
                command.Parameters.AddWithValue("@reservationId", reservation.ReservationId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    venueName = reservation.VenueName = Convert.ToString(reader["venueName"]);
                    spaceName = reservation.SpaceName = Convert.ToString(reader["spaceName"]);
                    dailyRate = reservation.TotalCost = Convert.ToDecimal(reader["dailyRate"]);
                    totalCost = (dailyRate * (decimal)numberOfDays);

                }
            }
            venueSpacetotalCost = ($"{venueName} | {spaceName} | {totalCost.ToString("C")}");
            return venueSpacetotalCost;
        }
        /// <summary>
        /// This method checks if the space requested by the user is available for reservation
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="numberOfAttendees"></param>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        public bool CheckIfSpaceIsAvailable(DateTime startDate, DateTime endDate, int numberOfAttendees, int spaceId)
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(isSpaceAvailable, conn);
                command.Parameters.AddWithValue("@spaceId", spaceId);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
                command.Parameters.AddWithValue("@numberOfAttendees", numberOfAttendees);

                return command.ExecuteScalar() == null;

                                 
              
            }
            
        }



    }
}
