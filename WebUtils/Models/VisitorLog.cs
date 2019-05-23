using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUtils
{
    public class VisitorLog
    {
        public string http_user_agent { get; set; }
        public string remote_addr { get; set; }
        public string remote_host { get; set; }
        public string request_method { get; set; }
        public string server_name { get; set; }
        public string server_port { get; set; }
        public string server_software { get; set; }
        public string page_url { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}