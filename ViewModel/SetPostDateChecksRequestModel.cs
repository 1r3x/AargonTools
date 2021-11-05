using System;

namespace AargonTools.ViewModel
{
    public class SetPostDateChecksRequestModel
    {
        public string debtorAcct { get; set; }
        public DateTime postDate { get; set; }
        public decimal amount { get; set; }
        public string accountNumber { get; set; }
        public string routingNumber { get; set; }
        public int totalPd { get; set; }
        public char sif { get; set; }
    }
}
