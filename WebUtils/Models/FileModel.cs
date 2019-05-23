using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUtils
{
    public class FileModel
    {
        public FileModel()
        {
            folder = "Others/";
        }
        public string file { get; set; }
        public string filename { get; set; }
        public string folder { get; set; }
    }
}