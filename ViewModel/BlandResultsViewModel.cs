using System.Collections.Generic;
using System;

namespace AargonTools.ViewModel
{
    public class BlandResultsViewModel
    {
        public string call_id { get; set; }
        public double call_length { get; set; }
        public string batch_id { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public bool completed { get; set; }
        public DateTime created_at { get; set; }
        public bool inbound { get; set; }
        public string queue_status { get; set; }
        public string endpoint_url { get; set; }
        public int max_duration { get; set; }
        public string error_message { get; set; }
        public Variables variables { get; set; }
        public string answered_by { get; set; }
        public bool record { get; set; }
        public string recording_url { get; set; }
        public string c_id { get; set; }
        public List<object> metadata { get; set; }
        public string summary { get; set; }
        public double price { get; set; }
        public DateTime started_at { get; set; }
        public bool local_dialing { get; set; }
        public string call_ended_by { get; set; }
        public List<PathwayLog> pathway_logs { get; set; }
        public string analysis_schema { get; set; }
        public string analysis { get; set; }
        public string transferred_to { get; set; }
        public string concatenated_transcript { get; set; }
        public List<Transcript> transcripts { get; set; }
        public string status { get; set; }
        public string corrected_duration { get; set; }
        public DateTime end_at { get; set; }
        public string disposition_tag { get; set; }
    }

    public class Variables
    {
        public string now { get; set; }
        public string now_utc { get; set; }
        public string short_from { get; set; }
        public string short_to { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string call_id { get; set; }
        public string phone_number { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string language { get; set; }
        public DateTime timestamp { get; set; }
        public string timezone { get; set; }
        public string callID { get; set; }
        public int BlandStatusCode { get; set; }
        public string token { get; set; }
        public string firstName1 { get; set; }
        public string lastName1 { get; set; }
        public bool status { get; set; }
        public string ssn1 { get; set; }
        public string accountStatus1 { get; set; }
        public string address11 { get; set; }
        public string debtorAccount1 { get; set; }
        public double Balance { get; set; }
        public string ClientName { get; set; }
        public string flag { get; set; }
        public string ssn_user { get; set; }
        public string amount99 { get; set; }
        public bool status99 { get; set; }
        public double accountBalance99 { get; set; }
        public double sifDiscount99 { get; set; }
        public double sifPayNow { get; set; }
        public int sifPct { get; set; }
        public string sif1 { get; set; }
        public string hsa { get; set; }
        public string ccNumber { get; set; }
        public string expiriedDate { get; set; }
        public string cvv { get; set; }
        public string CardholderFullName { get; set; }
        public string transactionId { get; set; }
        public string responseMessage { get; set; }
        public string transactionStatus { get; set; }
        public string authorizationNumber { get; set; }
        public string toQueue1 { get; set; }
    }

    public class PathwayLog
    {
        public string role { get; set; }
        public string text { get; set; }
        public string decision { get; set; }
        public DateTime created_at { get; set; }
        public string pathway_info { get; set; }
        public string chosen_node_id { get; set; }
    }

    public class Transcript
    {
        public int id { get; set; }
        public string user { get; set; }
        public string text { get; set; }
        public DateTime created_at { get; set; }
    }


}
