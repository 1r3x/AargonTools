using System;

namespace AargonTools.ViewModel
{
    public class SchedulePostDateRequest
    {
        public string debtorAcct { get; set; }
        public DateTime postDate { get; set; }
        public decimal amount { get; set; }
        public string cardNumber { get; set; }
        public int numberOfPayments { get; set; }
        public string expMonth { get; set; }
        public string expYear { get; set; }
    }
}
