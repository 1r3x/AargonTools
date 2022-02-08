using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class GetClientInvoiceHeaderViewModel
    {
        public string client_acct { get; set; }
        public string orig_creditor { get; set; }
        public string remit_full_pmt { get; set; }
        public string address12 { get; set; }
        public string address22 { get; set; }
        public string city2 { get; set; }
        public string state_code2 { get; set; }
        public string zip2 { get; set; }
    }
}
