using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Project.Web.DAL
{
    public class TimeCardSqlDAL:ITimeCardDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["TimeCardDB"].ConnectionString;
        private string SQL_GetAllRecord = "Select * from timecard WHERE timecard.user_name = @username";
        private string SQL_InsertTimeIn = "INSERT INTO timecard (user_name, start_datetime, project)VALUES(@username, @startdate, @project)";
        private string SQL_CheckClockOut = "Select * from timecard WHERE timecard.user_name = @username and timecard.end_datetime is null";
        private string SQL_ClockOut = "UPDATE timecard SET timecard.end_datetime=@enddate, timecard.notes = @notes WHERE timecard.user_name =@username AND timecard.end_datetime IS NULL";


        public List<TimeCardModel> GetAllRecords(string username)
        {
            List<TimeCardModel> output = new List<TimeCardModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetAllRecord, connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TimeCardModel r = new TimeCardModel();
                        r.UserName = Convert.ToString(reader["user_name"]);
                        r.Project = Convert.ToString(reader["project"]);
                        r.StartDate = Convert.ToDateTime(reader["start_datetime"]);
                        r.EndDate = Convert.ToString(reader["end_datetime"]);
                        r.Notes = Convert.ToString(reader["notes"]);

                        output.Add(r);
                    }
                }
            }catch(SqlException ex)
            {
                throw;
            }
            return output;
        }

      
        public bool SaveNewRecord(TimeCardModel r)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_InsertTimeIn, connection);
                    cmd.Parameters.AddWithValue("@username", r.UserName);
                    cmd.Parameters.AddWithValue("@startdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@project", r.Project);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }catch(SqlException ex)
            {
                throw;
            }
        }

        public bool ClockOut(TimeCardModel r)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_ClockOut, connection);
                    cmd.Parameters.AddWithValue("@username", r.UserName);
                    cmd.Parameters.AddWithValue("@enddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@notes", r.Notes);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }catch(SqlException ex)
            {
                throw;
            }
        }

        public bool CanClockOut(string username)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_CheckClockOut, connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }catch(SqlException ex)
            {
                throw;
            }

        }
    }
}