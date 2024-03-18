using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class GetInteractionAcctDataViewModel
    {
        public string debtorAcct { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string stateCode { get; set; }
        public string zip { get; set; }
        public string ssn { get; set; }
        public string birthDate { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string clientName { get; set; }
        public string debtType { get; set; }
        public double balance { get; set; }
        public string emailAddress { get; set; }
        public string homePhoneNumber { get; set; }
        public string cellPhoneNumber { get; set; }
        public string workPhoneNumber { get; set; }
        public string relativePhoneNumber { get; set; }
        public string otherPhoneNumer { get; set; }
        public string accountStatus { get; set; }
        public DateTime? promiseDate { get; set; }
        public string date_of_Service { get; set; }
        public string employee { get; set; }
        public string companyFlag { get; set; }
    }
}
