using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private string connectionString;

        private string sqlGetDepartment = "SELECT * FROM department;";

        private string sqlAddDepartment = "INSERT INTO department (name) " +
            " VALUES (@name);";

        private string sqlUpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @ID " +
            " VALUES (@name);";

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            IList<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand command = new SqlCommand(sqlGetDepartment, conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Department department = new Department();
                    department.Id = Convert.ToInt32(reader["department_id"]);
                    department.Name = Convert.ToString(reader["name"]);


                    departments.Add(department);
                }

            }

            return departments;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sqlAddDepartment, conn);

                command.Parameters.AddWithValue("@name", newDepartment.Name);
                
                count = command.ExecuteNonQuery();
            }
            return count;
        }
        
        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sqlUpdateDepartment, conn);
                command.Parameters.AddWithValue("@name", updatedDepartment.Name);
                
                return true;
            }
            
        }

    }
}
