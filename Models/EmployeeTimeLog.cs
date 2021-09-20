using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("employee_time_log")]
    public partial class EmployeeTimeLog
    {
        
        [Column("employee")]
        public int Employee { get; set; }
        [Column("log_time", TypeName = "datetime")]
        [Key]
        public DateTime LogTime { get; set; }
        [Required]
        [Column("station_name")]
        [StringLength(30)]
        public string StationName { get; set; }
        [Column("num_minutes")]
        public int NumMinutes { get; set; }
        [Required]
        [Column("reason")]
        [StringLength(15)]
        public string Reason { get; set; }
    }
}
