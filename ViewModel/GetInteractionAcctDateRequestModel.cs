using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class GetInteractionAcctDateRequestModel
    {
        [RegularExpression(@"\d{4}-\d{6}",
            ErrorMessage = "Please correct the format of the debtor account")]
        public string debtorAcct { get; set; }
        [RegularExpression(@"\d{10}",
            ErrorMessage = "Please correct the format of the home phone number")]
        public string phone { get; set; }
        [RegularExpression(@"\d{9}",
            ErrorMessage = "Please correct the format of the ssn")]
        public string ssn { get; set; }
    }
}
