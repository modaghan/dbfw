using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevToolPack
{
    public class CreateEventArgs : EventArgs
    {
        public bool Can { get; set; }
        public CreateEventArgs(bool can)
        {
            Can = can;
        }
    }
}
