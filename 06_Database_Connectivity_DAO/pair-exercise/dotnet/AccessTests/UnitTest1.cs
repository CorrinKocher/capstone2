using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlTransaction transaction;
                conn.Open();
                transaction = conn.BeginTransaction();
                                
                SqlCommand command = new SqlCommand(
                    "SELECT count(*) FROM department;", conn);

                beforeCount = (int)command.ExecuteScalar();
                             
                            
                DepartmentSqlDAO dao = new DepartmentSqlDAO(connectionString);
                Department department = new Department();
                department.Name = "Johntown";
                              

                dao.CreateDepartment(department);
                //string sql_insert = "INSERT INTO department (name) VALUES (sales);";
                SqlCommand cmd = new SqlCommand("SELECT count(*) FROM department;", conn);
                afterCount = (int)cmd.ExecuteScalar();

                Assert.AreEqual(beforeCount + 1, afterCount );
            }
        }
    }
}
