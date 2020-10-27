using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Capstone.Tests
{
    [TestClass]
    public class UnitTest1 : ParentTest
    {

        [TestMethod]
        public void DisplayAllSpacesShouldReturnSeven()
        {

            //Arrange
            int count = 0;

            SpaceSQLDAO spaceDao = new SpaceSQLDAO(connectionString);
            string sql = "SELECT name, id FROM space WHERE venue_id = 1;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);


                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {

                    count += 1;
                }

            }

            //Act
            List<string> spaceTest = spaceDao.DisplayAllSpacesByVenueId("1");
            //Assert
            Assert.AreEqual(count, spaceTest.Count);


        }
        [TestMethod]

        public void MonthTenReturnsOctober()
        {
            //Arrange
            string month = "10";
            SpaceSQLDAO dao = new SpaceSQLDAO(connectionString);
            //Act
            string result = dao.ConvertToMonth(month);
            string expected = "October";
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreateReservationTest()
        {
            int countBefore = 0;
            int countAfter = 0;



            //Arrange


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT count(*) FROM reservation;", conn);

                countBefore = (int)command.ExecuteScalar();
            }

            ReservationSQLDAO dao = new ReservationSQLDAO(connectionString);
            Reservation reservation = new Reservation();
            reservation.SpaceId = 1;
            reservation.StartDate = Convert.ToDateTime("2070-10-10");
            reservation.EndDate = Convert.ToDateTime("2070-10-15");
            reservation.NumberOfAttendees = 2;
            reservation.ReservedFor = "John Fulton";
            DateTime startDate = reservation.StartDate;
            DateTime endDate = reservation.EndDate;
            string reservedFor = reservation.ReservedFor;
            int numberOfAttendees = reservation.NumberOfAttendees;
            int spaceId = reservation.SpaceId;
            //Act
            dao.CreateReservation(startDate, endDate, numberOfAttendees, reservedFor, spaceId);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    "SELECT count(*) FROM reservation;", conn);

                countAfter = (int)command.ExecuteScalar();
            }

            //Assert
            Assert.AreEqual(countBefore + 1, countAfter);


        }
    }
}
