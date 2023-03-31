﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Models.Helper
{
    public class CentralizeVariablesModel
    {
        public USAePayDefault USAePayDefault { get; set; } = new USAePayDefault();
        public InstaMedOutlet InstaMedOutlet { get; set; } = new InstaMedOutlet();
        public InstaMedCredentials InstaMedCredentials { get; set; } = new InstaMedCredentials();
        public IClassProCredentials IClassProCredentials { get; set; } = new IClassProCredentials();
        public ElavonCredentials ElavonCredentials { get; set; } = new ElavonCredentials();

    }
    public class USAePayDefault
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string Pin { get; set; }
    }

    public class InstaMedOutlet
    {
        public string MerchantID { get; set; }
        public string StoreID { get; set; }
        public string TerminalID { get; set; }
    }


    public class InstaMedCredentials
    {
        public string BaseAddress { get; set; }
        public string APIkey { get; set; }
        public string APIsecret { get; set; }
    }

    public class IClassProCredentials
    {
        public string BaseAddress { get; set; }
        public string security_key { get; set; }
    }
    public class ElavonCredentials
    {
        public string BaseAddress { get; set; }
        public string ssl_merchant_id { get; set; }
        public string ssl_user_id { get; set; }
        public string ssl_pin { get; set; }
    }

}