using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class GetClientInvoiceRequestModel
    {
        [RegularExpression(@"\d{4}",
            ErrorMessage = "Please correct the format of client account")]
        public string ClientAccount { get; set; }
        [Required]
        [StringLength(1)]
        //[RegularExpression(@"/[ADHLTW]/",
        //    ErrorMessage = "Please only provide the company alphabet(Uppercase)")]
        public string Company { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
