using RESTfullAPI.Filters;
using RESTfullAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace RESTfullAPI.Controllers
{
    public class SensorDataController : ApiController
    {
        string connectionString = Properties.Settings.Default.ConnectionString;

        // GET: api/Sensors
        //get sensor data (ALL)

        [Route("api/sensors/data")]
        [BasicAuthentication]
        public IEnumerable<SensorData> Get()
        {
            List<SensorData> lista = new List<SensorData>();
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM SensorsData", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SensorData s = new SensorData
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        SensorID = int.Parse(reader["SensorID"].ToString()),
                        Temperature = float.Parse(reader["Temperature"].ToString()),
                        Humidity = float.Parse(reader["Humidity"].ToString()),
                        Battery = (int)reader["Battery"],
                        Timestamp = (DateTime)reader["Timestamp"],
                        isValid = (bool)reader["isValid"]
                    };
                    lista.Add(s);
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

        // POST: api/Sensor

        //add new sensor data
        [Route("api/sensors/data")]
        [BasicAuthentication]
        public HttpResponseMessage PostSensor([FromBody]SensorData value)
        {
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO SensorsData(SensorId, Temperature, Humidity, Battery, Timestamp, isValid) VALUES (@sensorID, @temperature, @humidity, @battery, @timestamp, @isValid)", sqlConnection);
                cmd.Parameters.AddWithValue("@sensorID", value.SensorID);
                cmd.Parameters.AddWithValue("@temperature", value.Temperature);
                cmd.Parameters.AddWithValue("@humidity", value.Humidity);
                cmd.Parameters.AddWithValue("@battery", value.Battery);
                cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                cmd.Parameters.AddWithValue("@isValid", 1);

                int result = cmd.ExecuteNonQuery();

                sqlConnection.Close();

                if (result > 0)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    response.Content = new StringContent("Added New Sensor Data", Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.Content = new StringContent("Could Not Add New Sensor Data", Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
            }
            catch (Exception)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No Sensor With ID="+value.SensorID, Encoding.Unicode);
                response.Headers.CacheControl = new CacheControlHeaderValue();
                return response;
            }
        }

        // Patch: api/sensors/{id}
        //validate / invalidate sensor data
        [Route("api/sensors/data/{id:int}")]
        [BasicAuthentication]
        public HttpResponseMessage Patch(int id)
        {
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE SensorsData SET isValid=~isValid WHERE Id=@id", sqlConnection);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();

                sqlConnection.Close();

                if (result > 0)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent("Validated/Invalidated Sensor Data for Data with ID=" + id, Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.Content = new StringContent("Could not Invalidate/Validate Sensor Data for Data with ID=" + id + " -----> Maybe try it with a valid Data ID", Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
            }
            catch (Exception e)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent(e.Message, Encoding.Unicode);
                response.Headers.CacheControl = new CacheControlHeaderValue();
                return response;
            }
        }

        //GET: api/Sensor/{floor}/date/date
        [Route("api/sensors/data/{floor:int}/{date1:datetime:regex(\\d{4}-\\d{2}-\\d{2})}/{date2:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [BasicAuthentication]
        public IHttpActionResult GetSensorDataPerFloorWithDateInterval(int floor, DateTime date1, DateTime date2)
        {
            

            SqlConnection conn = null;
            List<SensorData> lista = new List<SensorData>();
            try
            {
                
                conn = new SqlConnection(connectionString);
                conn.Open();
                //DateTime fromDate = Convert.ToDateTime(date1);
                //DateTime toDate = Convert.ToDateTime(date2);
                SqlCommand command = new SqlCommand("SELECT * FROM SensorsData d JOIN Sensors s ON d.SensorId=s.SensorId WHERE s.Floor=@floor AND d.Timestamp BETWEEN cast(@date1 as datetime) AND cast(@date2 as datetime)", conn);
                command.Parameters.AddWithValue("@floor", floor);
                command.Parameters.AddWithValue("@date1", date1);
                command.Parameters.AddWithValue("@date2", date2);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SensorData s = new SensorData
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        SensorID = int.Parse(reader["SensorID"].ToString()),
                        Temperature = float.Parse(reader["Temperature"].ToString()),
                        Humidity = float.Parse(reader["Humidity"].ToString()),
                        Battery = (int)reader["Battery"],
                        Timestamp = (DateTime)reader["Timestamp"],
                        isValid = (bool)reader["isValid"]
                    };
                    lista.Add(s);
                }
                reader.Close();
                conn.Close();

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return Ok(lista);
        }

        //GET: api/Sensor/{floor}
        [Route("api/sensors/data/{floor:int}")]
        [BasicAuthentication]
        public IHttpActionResult GetSensorPerFloor(int floor)
        {
            SqlConnection conn = null;
            List<SensorData> lista = new List<SensorData>();
            try
            {

                conn = new SqlConnection(connectionString);
                conn.Open();
                
                SqlCommand command = new SqlCommand("SELECT * FROM SensorsData d JOIN Sensors s ON d.SensorId=s.SensorId WHERE s.Floor=@floor", conn);
                command.Parameters.AddWithValue("@floor", floor);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SensorData s = new SensorData
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        SensorID = int.Parse(reader["SensorID"].ToString()),
                        Temperature = float.Parse(reader["Temperature"].ToString()),
                        Humidity = float.Parse(reader["Humidity"].ToString()),
                        Battery = (int)reader["Battery"],
                        Timestamp = (DateTime)reader["Timestamp"],
                        isValid = (bool)reader["isValid"]
                    };
                    lista.Add(s);
                }
                reader.Close();
                conn.Close();

            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return Ok(lista);

        }
    }
}
