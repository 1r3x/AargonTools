using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.ViewModel
{
    public class SetUpdateAddressRequestModel
    {
        [Required]
        [Column("DebtorAcct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
      
        [Required]
        [Column("Address1")]
        [StringLength(40)]
        public string Address1 { get; set; }


        [Required]
        [Column("Address2")]
        [StringLength(30)]
        public string Address2 { get; set; }

        [Required]
        [Column("City")]
        [StringLength(30)]
        public string City { get; set; }

        [Required]
        [Column("State")]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [Column("Zip")]
        [StringLength(10)]
        public string Zip { get; set; }


        [Required]
        [Column("ResidenceType")]
        [StringLength(15)]
        public string ResidenceType { get; set; }

        [Required]
        [Column("Source")]
        [StringLength(20)]
        public string Source { get; set; }

    }
}
