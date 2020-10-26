using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            string sql = "SELECT name FROM space WHERE venue_id = 1;";
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
            List<string>spaceTest = spaceDao.DisplayAllSpacesByVenueId("1");
            //Assert
            Assert.AreEqual(count, spaceTest.Count);


        }
               

    }
}
