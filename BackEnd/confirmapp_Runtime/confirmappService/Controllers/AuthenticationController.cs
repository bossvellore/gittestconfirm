using confirmappService.ApiResults;
using confirmappService.DataObjects;
using confirmappService.Models;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace confirmappService.Controllers
{
    [MobileAppController]
    public class AuthenticationController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
           
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [Route("api/authentication/login"), HttpPost, AllowAnonymous]
        public IHttpActionResult Login([FromBody] JObject assertion)
        {
            var user = ValidateLogin((string)assertion.GetValue("userName"), (string)assertion.GetValue("password"));

            if (user!=null) // user-defined function, checks against a database
            {
                JwtSecurityToken token = AppServiceLoginHandler.CreateToken(new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, (string)assertion["userName"]) },
                    Constants.SigningKey,
                    Constants.AppURL,
                    Constants.AppURL,
                    TimeSpan.FromHours(24));
                return Ok(new LoginResult()
                {
                    IsAuthorized = true,
                    AuthenticationToken = token.RawData,
                    User = user
                });
            }
            else // user assertion was not valid
            {
                return Ok(new LoginResult()
                {
                    IsAuthorized = false
                });
            }
        }

        private User ValidateLogin(string email, string password)
        {
            var userTable = new confirmappContext().Set<User>();
            var query = from u in userTable
                        where u.Email == email
                        where u.Password == password
                        select u;

            // This will raise an exception if entity not found
            // Use SingleOrDefault instead
            var user = query.SingleOrDefault();
            return user;            
            
        }
    }
}