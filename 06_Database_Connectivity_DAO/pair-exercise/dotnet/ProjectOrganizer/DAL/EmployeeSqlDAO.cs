using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private string connectionString;
        private string sqlGetEmployee = "SELECT * FROM employee;";
        private string getEmployeeByName = "SELECT * FROM employee WHERE first_name = @first_name;";
        private string getEmpWithoutProj = "Select * FROM employee LEFTJOIN " +
            " project_employee ON employee.employee_id = project_employee.employee_id WHERE project_employee.project_id IS NULL;";
        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            IList<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand command = new SqlCommand(sqlGetEmployee, conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.JobTitle = Convert.ToString(reader["job_title"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.Gender = Convert.ToString(reader["gender"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);



                    employees.Add(employee);
                }

            }

            return employees;
        }

        /// <summary>
        /// Searches the system for an employee by first name or last name.
        /// </summary>
        /// <remarks>The search performed is a wildcard search.</remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>A list of employees that match the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {
            IList<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();


                SqlCommand command = new SqlCommand(getEmployeeByName, conn);

                command.Parameters.AddWithValue("@first_name", firstname);
                command.Parameters.AddWithValue("@last_name", lastname);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.JobTitle = Convert.ToString(reader["job_title"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.Gender = Convert.ToString(reader["gender"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                    employees.Add(employee);
                    
                }
                
            }
            return employees;
        }

               

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
             IList<Employee> EmployeesWithoutProjects = new List<Employee>();
             using (SqlConnection conn = new SqlConnection(connectionString))
             {
                conn.Open();

                SqlCommand command = new SqlCommand(getEmpWithoutProj, conn);
                               
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                    employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                    employee.JobTitle = Convert.ToString(reader["job_title"]);
                    employee.FirstName = Convert.ToString(reader["first_name"]);
                    employee.LastName = Convert.ToString(reader["last_name"]);
                    employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                    employee.Gender = Convert.ToString(reader["gender"]);
                    employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                    EmployeesWithoutProjects.Add(employee);

                }
             }
            return EmployeesWithoutProjects;

        }
        
    }
}
