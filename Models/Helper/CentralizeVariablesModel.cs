using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Models.Helper
{
    public class CentralizeVariablesModel
    {
        public USAePayDefault USAePayDefault { get; set; } = new USAePayDefault();
    }
    public class USAePayDefault
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string Pin { get; set; }
    }

}
