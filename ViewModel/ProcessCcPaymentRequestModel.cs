using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class ProcessCcPaymentRequestModel
    {
        public string debtorAcc { get; set; }
        public string ccNumber { get; set; }
        public string expiredDate { get; set; }
        public string cvv { get; set; }
        public int numberOfPayments { get; set; }
        public decimal amount { get; set; }
    }
}
