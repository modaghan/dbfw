﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUtils
{
    public class JsonData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }
    }
}