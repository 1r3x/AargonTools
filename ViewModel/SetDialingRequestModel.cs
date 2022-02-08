using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class SetDialingRequestModel
    {
        [StringLength(3)]
        public string AreaCode { get; set; }
        [StringLength(7)]
        public string PhoneNumber { get; set; }
        [StringLength(15)]
        public string DebtorAccount { get; set; }
        public int ListAccount { get; set; }
    }
}
