﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFinal.Models
{
    public class Account
    {
       public string Name { get; set; }
        public string Password { get; set; }

        public string location { get; set; }
        
        public string email { get; set; }

        public string full_name { get; set; }

        public int phone_NR { get; set; }

        public string tagname { get; set; }
        public int idtag { get; set; }

     
    }
}
