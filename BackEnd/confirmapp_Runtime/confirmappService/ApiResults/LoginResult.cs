using confirmappService.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace confirmappService.ApiResults
{
    public class LoginResult
    {
        public bool IsAuthorized { get; set; }
        public string AuthenticationToken { get; set; }
        public User User { get; set; }
    }
}