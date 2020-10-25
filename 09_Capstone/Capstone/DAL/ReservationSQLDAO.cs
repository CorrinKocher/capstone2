﻿using System;
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
        + " VALUES ('@spaceId','@numberOfAttendees','@startDate', '@endDate', '@reservedForName');";
        private string returnReservationVenueNameAndSpaceName = "SELECT venue.name AS venueName, space.name AS spaceName FROM venue "
        + " JOIN space ON venue.id = space.id JOIN reservation ON space.id = reservation.space_id "
        + " WHERE reservation_id = @reservationId;";
        



        private string connectionString;
        public ReservationSQLDAO(string databaseConnectionString)
        {
            this.connectionString = databaseConnectionString;
        }

        //public List<string> SearchAvailableSpacesToReserve(int venueId, DateTime startDate, int numberOfDays)
        //{
        //    List<string> openSpaces = new List<string>();
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand command = new SqlCommand(searchReservation, conn);
        //        command.Parameters.AddWithValue("@venueId", venueId);
        //        command.Parameters.AddWithValue("@startDate", startDate);
        //        command.Parameters.AddWithValue("@endDate", startDate.AddDays(numberOfDays));

        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            string availableSpaces = Convert.ToString(reader["spaceName"]);
        //            openSpaces.Add(availableSpaces);
        //        }

        //        return openSpaces;
        //    }
        //}

        public Reservation CreateReservation(DateTime startDate, DateTime endDate, int numberOfAttendees, string reservedForName, string spaceId)
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
                    reservation.SpaceId = Convert.ToInt32(reader[spaceId]);
                    reservation.StartDate = Convert.ToDateTime(reader["startDate"]);
                    reservation.EndDate = Convert.ToDateTime(reader["endDate"]);
                    reservation.ReservedFor = Convert.ToString(reader["reservedForName"]);
                    reservation.NumberOfAttendees = Convert.ToInt32(reader["numberOfAttendees"]);
                }
            }
            return reservation;
        }
        public string CreateReservationConfirmation(Reservation reservation, string venueName, string SpaceName)
        {
            string reservationString;
            //string reservationId = reservation.ReservationId.ToString();
            string startDate = reservation.StartDate.ToString();
            string endDate = reservation.EndDate.ToString();
            //string numberOfAteendees = reservation.NumberOfAttendees.ToString();
            //string reservedFor = reservation.ReservedFor;
            //need to possible create total cost string here
            reservationString = ($"ConfirmationNumber: {reservation.ReservationId}\n Venue: {venueName} \n Space: {SpaceName} \n Reserved For: {reservation.ReservedFor} \n Attendees: {reservation.NumberOfAttendees} \n Arrival Date: {startDate} \n Depart Date: {endDate} \n Total Cost : NEED TO MERGE FOR TOTALCOST" );
            return reservationString;
        }
        public string ReturnReservationVenueName(Reservation reservation)
        {
            string venueName = "";
            Reservation thisReservation = new Reservation();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(returnReservationVenueNameAndSpaceName, conn);
                command.Parameters.AddWithValue("@reservationId", reservation.ReservationId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reservation.VenueName = Convert.ToString(reader["venueName"]);
                    venueName = Convert.ToString(reader["venueName"]);
                }
            }
            return venueName;
        }
        public string ReturnReservationSpaceName(Reservation reservation)
        {
            string spaceName = "";
            Reservation thisReservation = new Reservation();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(returnReservationVenueNameAndSpaceName, conn);
                command.Parameters.AddWithValue("@reservationId", reservation.ReservationId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reservation.SpaceName = Convert.ToString(reader["spaceName"]);
                    spaceName = Convert.ToString(reader["spaceName"]);
                }
            }
            return spaceName;
        }

    }
}
