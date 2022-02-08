using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class GetClientInvoicePaymentsViewModel
    {
        public string debtor_acct { get; set; }
        public string supplied_acct { get; set; }
        public string date_of_service { get; set; }
        public string date_placed { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string client_amt { get; set; }
        public string agency_amt_decl { get; set; }
        public string fee_pct { get; set; }
        public string tran_date { get; set; }
        public string balance { get; set; }
        public string status_code { get; set; }
        public string payment_type { get; set; }
        public string total_payments_amt { get; set; }
        public string amount_due_agency { get; set; }
        public string amount_due_client { get; set; }
        public string cosigner_last_name { get; set; }
    }
}
