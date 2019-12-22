using RESTfullAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace RESTfullAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        string connectionString = Properties.Settings.Default.ConnectionString;
        
        [Route("api/register")]
        [OverrideAuthorization]
        public HttpResponseMessage Post([FromBody] User user)
        {
            SqlConnection sqlConnection = null;

            try
            {
                byte[] hash;
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using (SHA512 shaM = new SHA512Managed())
                {
                    var data = Encoding.UTF8.GetBytes(user.Password);
                    hash = shaM.ComputeHash(data);
                }

                string hashToCompare = BitConverter.ToString(hash);
                string trimmedHash = hashToCompare.Replace("-", "").ToLower();

                SqlCommand cmd = new SqlCommand("INSERT INTO Users(username, password) VALUES (@username, @password)", sqlConnection);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", trimmedHash);
                int result = cmd.ExecuteNonQuery();

                sqlConnection.Close();

                if (result > 0)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    response.Content = new StringContent("Created user with username " +user.Username, Encoding.Unicode);
                    response.Headers.CacheControl = new CacheControlHeaderValue();
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent("Could not create user with username " + user.Username, Encoding.Unicode);
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
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Error Creating User (username already in user OR wrong body parameters!", Encoding.Unicode);
                response.Headers.CacheControl = new CacheControlHeaderValue();
                return response;
            }
        }
    }
}
