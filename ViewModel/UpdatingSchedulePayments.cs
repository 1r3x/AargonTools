using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class UpdatingSchedulePayments
    {
        [Required]
        public int scheduleId { get; set; }
        [Required]
        public DateTime Updateddate { get; set; }
    }
}
