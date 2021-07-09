﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Table("api_move_settings")]
    public partial class ApiMoveSetting
    {
        [Key]
        [Column("move_setup_id")]
        public int MoveSetupId { get; set; }
        [Required]
        [Column("company")]
        [StringLength(5)]
        public string Company { get; set; }
        [Column("from_employee")]
        public int FromEmployee { get; set; }
        [Column("to_employee")]
        public int ToEmployee { get; set; }
        [Column("target_employee")]
        public int TargetEmployee { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
