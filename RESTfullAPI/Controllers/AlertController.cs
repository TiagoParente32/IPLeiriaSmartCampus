using RESTfullAPI.Filters;
using RESTfullAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTfullAPI.Controllers
{
    public class AlertController : ApiController
    {
        string connectionString = Properties.Settings.Default.ConnectionString;
        //get alerts data
        [Route("api/alerts")]
        [BasicAuthentication]
        public IEnumerable<Alert> Get()
        {
            List<Alert> lista = new List<Alert>();
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Alerts", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Alert a = new Alert
                    {
                        Type = (string)reader["type"],
                        Value = int.Parse(reader["value"].ToString()),
                        Operator = (string)reader["operator"],
                        Between_value = int.Parse(reader["between_value"].ToString()),
                        Message = (string)reader["message"],
                        Enabled = (bool)reader["enabled"],
                        SensorID = int.Parse(reader["sensorID"].ToString()),
                        Timestamp = (DateTime)reader["timestamp"]
                    };
                    lista.Add(a);
                }
                reader.Close();
                sqlConnection.Close();
            }
            catch (Exception)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            return lista;
        }
    }
}
