using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class ViewingSchedulePaymentsRequestModel
    {
        [Required]
        public DateTime date { get; set; }
       
    }
}
