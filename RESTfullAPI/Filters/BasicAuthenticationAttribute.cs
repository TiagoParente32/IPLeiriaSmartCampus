using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RESTfullAPI.Filters
{

    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            
            if (actionContext.Request.Headers.Authorization != null)
            {
                byte[] hash;
                var authToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(authToken));

                // spliting decodeauthToken using ':'   
                var arrUserNameandPassword = decodeauthToken.Split(':');

                using (SHA512 shaM = new SHA512Managed())
                {
                    var data = Encoding.UTF8.GetBytes(arrUserNameandPassword[1]);
                    hash = shaM.ComputeHash(data);
                }


                // at 0th postion of array we get username and at 1st we get password  
                if (IsAuthorized(arrUserNameandPassword[0], hash))
                {
                    // setting current principle  
                    Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request
                 .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public static bool IsAuthorized(string Username, byte[] hash)
        {
            string connectionString = Properties.Settings.Default.ConnectionString;
            bool isValid = false;
            SqlConnection sqlConnection = null;

            
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                string hashToCompare = BitConverter.ToString(hash);
                string trimmedHash = hashToCompare.Replace("-","").ToLower();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE username=@username AND password=@passwordHash", sqlConnection);
                cmd.Parameters.AddWithValue("@username", Username);
                cmd.Parameters.AddWithValue("@passwordHash", trimmedHash);
                int result = (int) cmd.ExecuteScalar();

                if(result > 0)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }

                sqlConnection.Close();

            }
            catch (Exception)
            {
                isValid = false;
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
            return isValid;
        }
    }




}