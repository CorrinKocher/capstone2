using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private string connectionString;
        private string sqlGetProjects = "SELECT * FROM project;";
        private string sqlAssignProject = "UPDATE project_employee SET employee_id = @employee_id WHERE project_id = @project_id;";
        private string sqlUnassignProject = "DELETE FROM project_employee WHERE employee_id = @employee_id AND project_id = @project_id;";
        private string sqlAddProject = "INSERT INTO project (name, from_date, to_date) VALUES(@name, @from_date, @to_date);";
        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            IList<Project> projects = new List<Project>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();

                SqlCommand command = new SqlCommand(sqlGetProjects, conn);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Project project = new Project();
                    project.ProjectId = Convert.ToInt32(reader["project_id"]);
                    project.Name = Convert.ToString(reader["name"]);
                    project.StartDate = Convert.ToDateTime(reader["from_date"]);
                    project.EndDate = Convert.ToDateTime(reader["to_date"]);

                    projects.Add(project);
                }
            }
            return projects;
        }
        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sqlAssignProject, conn);

                command.Parameters.AddWithValue("@project_id", projectId);
                command.Parameters.AddWithValue("@employee_id", employeeId);
                return true;
            }
        }
        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sqlUnassignProject, conn);

                command.Parameters.AddWithValue("@project_id", projectId);
                command.Parameters.AddWithValue("@employee_id", employeeId);
                return true;
            }
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            IList<Project> projects = new List<Project>();
            int count = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sqlAddProject, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Project project = new Project();
                    project.ProjectId = Convert.ToInt32(reader["project_id"]);
                    project.Name = Convert.ToString(reader["name"]);
                    project.StartDate = Convert.ToDateTime(reader["from_date"]);
                    project.EndDate = Convert.ToDateTime(reader["to_date"]);

                    projects.Add(project);

                    count = command.ExecuteNonQuery();
                }
            }
            return count;
        }

    }
}
