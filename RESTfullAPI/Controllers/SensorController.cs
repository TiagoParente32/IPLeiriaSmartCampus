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
using System.Web.Http.Controllers;

namespace RESTfullAPI.Controllers
{
    public class SensorController : ApiController
    {
        string connectionString = Properties.Settings.Default.ConnectionString;
        //get sensors
        [Route("api/sensors")]
        [BasicAuthentication]
        public IEnumerable<Sensor> Get()
        {
            List<Sensor> lista = new List<Sensor>();
            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Sensors", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sensor s = new Sensor
                    {
                        Id = int.Parse(reader["SensorID"].ToString()),
                        IsPersonal = bool.Parse(reader["isPersonal"].ToString()),
                        UserID = (reader["UserID"] == DBNull.Value) ? -1 : int.Parse(reader["UserID"].ToString()),
                        Floor = int.Parse(reader["Floor"].ToString())

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


        [Route("api/sensors")]
        [BasicAuthentication]
        //add personal  
        public HttpResponseMessage Post([FromBody]Sensor value)
        {

            SqlConnection sqlConnection = null;
            var authToken = Request.Headers.Authorization.Parameter;
            // decoding authToken we get decode value in 'Username:Password' format  
            var decodeauthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

            // spliting decodeauthToken using ':'   
            var arrUserNameandPassword = decodeauthToken.Split(':');

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sql = new SqlCommand("SELECT Id FROM Users WHERE username=@username", sqlConnection);
                sql.Parameters.AddWithValue("@username", arrUserNameandPassword[0]);
                int userID = (int)sql.ExecuteScalar();
                if(userID <= 0)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("No User Found With ID " + userID, Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO Sensors(isPersonal, UserID, Floor) VALUES (@isPersonal, @UserID, @Floor)", sqlConnection);
                cmd.Parameters.AddWithValue("@isPersonal", 1);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Floor", value.Floor);
               

                int result = cmd.ExecuteNonQuery();

                sqlConnection.Close();

                if (result > 0)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    response.Content = new StringContent("Added Personal Sensor for User with id " +userID, Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                    response.Content = new StringContent("Added Personal Sensor for User with id " + userID, Encoding.Unicode);
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
    }
}
