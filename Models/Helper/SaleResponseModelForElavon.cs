using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AargonTools.Models.Helper
{
    public class SaleResponseModelForElavon
    {
        [XmlElement("txn")]
        public txn txn { get; set; }

    }

    public class txn
    {
        public string ssl_issuer_response { get; set; }
        public string ssl_last_name { get; set; }
        public string ssl_company { get; set; }
        public string ssl_phone { get; set; }
        public string ssl_card_number { get; set; }
        public string ssl_departure_date { get; set; }
        public string ssl_oar_data { get; set; }
        public string ssl_result { get; set; }
        public string ssl_txn_id { get; set; }
        public string ssl_avs_response { get; set; }
        public string ssl_approval_code { get; set; }
        public string ssl_email { get; set; }
        public string ssl_amount { get; set; }
        public string ssl_avs_zip { get; set; }
        public string ssl_txn_time { get; set; }
        public string ssl_description { get; set; }
        public string ssl_exp_date { get; set; }
        public string ssl_card_short_description { get; set; }
        public string ssl_completion_date { get; set; }
        public string ssl_address2 { get; set; }
        public string ssl_get_token { get; set; }
        public string ssl_customer_code { get; set; }
        public string ssl_country { get; set; }
        public string ssl_card_type { get; set; }
        public string ssl_transaction_type { get; set; }
        public string ssl_salestax { get; set; }
        public string ssl_avs_address { get; set; }
        public string ssl_account_balance { get; set; }
        public string ssl_ps2000_data { get; set; }
        public string ssl_state { get; set; }
        public string ssl_ship_to_zip { get; set; }
        public string ssl_city { get; set; }
        public string ssl_result_message { get; set; }
        public string ssl_first_name { get; set; }
        public string ssl_invoice_number { get; set; }
        public string ssl_ship_to_address1 { get; set; }
        public string ssl_cvv2_response { get; set; }
        public string ssl_partner_app_id { get; set; }

    }
}
