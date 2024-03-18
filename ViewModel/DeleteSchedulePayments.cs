using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class DeleteSchedulePayments
    {
        [Required]
        public int scheduleId { get; set; }

    }
}
