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
<<<<<<< HEAD
               
=======
                int count = 0;

>>>>>>> cd069952c9972692c7038840d576d6657d92d5ca
                while (reader.Read())
                {
                    count += 1;
                }
<<<<<<< HEAD
            }
            //Act
            List<string>spaceTest = spaceDao.DisplayAllSpacesByVenueId("1");
            //Assert
            Assert.AreEqual(count, spaceTest.Count);
=======
                //Act
                spaceDao.DisplayAllSpacesByVenueId("1");
                //Assert
                Assert.AreEqual(count, spaceDao.DisplayAllSpacesByVenueId("1"));
            }
>>>>>>> cd069952c9972692c7038840d576d6657d92d5ca
        }
    }
}
