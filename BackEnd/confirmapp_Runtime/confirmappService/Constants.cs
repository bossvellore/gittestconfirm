using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace confirmappService
{
    public class Constants
    {
        public static string SigningKey = ConfigurationManager.AppSettings["SigningKey"];
        public static string AppURL = ConfigurationManager.AppSettings["ValidAudience"];
    }
}