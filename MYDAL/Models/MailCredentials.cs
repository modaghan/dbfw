using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MailCredentials
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string DefaultAddress { get; set; }
    }
}
