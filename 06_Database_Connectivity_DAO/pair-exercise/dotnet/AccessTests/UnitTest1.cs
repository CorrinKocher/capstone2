using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace AccessTests
{
    [TestClass]
    public class AccessTests

    {
        private string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=EmployeeDB;Integrated Security=True";

        private TransactionScope transaction;

        [TestMethod]
        public void AddDepartmentTest()
        {
            int beforeCount = 0;
            int afterCount = 0;
            transaction = new TransactionScope();
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction;
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    SqlCommand command = new SqlCommand(
                        "SELECT count(*) FROM department;", conn);

                    beforeCount = (int)command.ExecuteScalar();
                }

                DepartmentSqlDAO dao = new DepartmentSqlDAO(connectionString);
                Department department = new Department();
                department.Name = "Johntown";


                dao.CreateDepartment(department);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT count(*) FROM department;", conn);
                    afterCount = (int)cmd.ExecuteScalar();

                }
                Assert.AreEqual(beforeCount + 1, afterCount);
            }
            catch
            {
                transaction.Dispose();
            }
        }

        [TestMethod]
        [DataRow("Fredrick", "Keppard", 1)]
        [DataRow("Flo", "Henderson", 1)]
        public void SearchShouldReturnRightNumber(string firstname, string lastname, int expectedNumberOfEmployees)
        {



            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(connectionString);

            // Act
            IList<Employee> employees = dao.Search(firstname, lastname);

            // Assert
            Assert.AreEqual(expectedNumberOfEmployees, employees.Count);
        }

        [TestMethod]
        public void GetAllEmployeesShouldReturnAllEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(connectionString);

            // Act
            IList<Employee> employees = dao.GetAllEmployees();

            // Assert
            Assert.AreEqual(12, employees.Count);
        }

    }

}
