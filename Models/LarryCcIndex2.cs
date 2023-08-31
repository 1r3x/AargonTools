using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("larry_cc_index2")]
    public partial class LarryCcIndex2
    {
        [Column("client_acct")]
        [StringLength(4)]
        public string ClientAcct { get; set; }
        [Column("client_id")]
        [StringLength(50)]
        public string ClientId { get; set; }
        [Column("client_user")]
        [StringLength(50)]
        public string ClientUser { get; set; }
        [Column("client_key")]
        [StringLength(50)]
        public string ClientKey { get; set; }
        [Column("client_pass")]
        [StringLength(100)]
        public string ClientPass { get; set; }
        [Column("gateway")]
        [StringLength(50)]
        public string Gateway { get; set; }
        [Column("acct_status")]
        [StringLength(1)]
        public string AcctStatus { get; set; }
        [Column("company")]
        [StringLength(1)]
        public string Company { get; set; }
    }
}
