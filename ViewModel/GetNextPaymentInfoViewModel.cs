using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class GetNextPaymentInfoViewModel
    {
        public string name_first_last { get; set; }
        public decimal balance { get; set; }
        public decimal amount_paid_life { get; set; }
        public DateTime date_of_service { get; set; }
        public decimal ssn9 { get; set; }
        public string street_number { get; set; }
        public string street_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string client_name { get; set; }
        public string client_description { get; set; }
        public string client_interst_bearingB { get; set; }
        public string client_credit_reportableB { get; set; }
        public string home_phone_number { get; set; }
        public string home_phone_verifiedB { get; set; }
        public string home_phone_ponB { get; set; }
        public string cell_phone_number { get; set; }
        public string cell_phone_verifiedB { get; set; }
        public string cell_phone_ponB { get; set; }
        public decimal last_payment_amount { get; set; }
        public DateTime last_payment_date { get; set; }
        public DateTime birth_date { get; set; }
        public decimal promise_amount { get; set; }
        public DateTime promise_date { get; set; }
        public decimal check_amt { get; set; }
        public DateTime check_date { get; set; }
        public decimal cc_amt { get; set; }
        public DateTime cc_date { get; set; }
    }
}
