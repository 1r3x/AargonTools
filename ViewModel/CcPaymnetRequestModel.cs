using System;

namespace AargonTools.ViewModel
{
    public class CcPaymnetRequestModel
    {
        public string debtorAcc { get; set; }
        public string company { get; set; }
        public string userId { get; set; }
        public decimal chargeTotal { get; set; }
        public DateTime paymentDate { get; set; }
        public string approvalStatus { get; set; }
        public string approvalCode { get; set; }
        public string orderNumber { get; set; }
        public string refNo { get; set; }
        public string sif { get; set; }

    }
}
