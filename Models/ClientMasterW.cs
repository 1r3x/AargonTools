using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("client_master_w")]
    public partial class ClientMasterW
    {
        [Required]
        [Column("client_acct")]
        [StringLength(4)]
        public string ClientAcct { get; set; }
        [Required]
        [Column("tax_id")]
        [StringLength(12)]
        public string TaxId { get; set; }
        [Required]
        [Column("client_name")]
        [StringLength(40)]
        public string ClientName { get; set; }
        [Required]
        [Column("phonetic_name")]
        [StringLength(40)]
        public string PhoneticName { get; set; }
        [Required]
        [Column("address1")]
        [StringLength(30)]
        public string Address1 { get; set; }
        [Column("address2")]
        [StringLength(30)]
        public string Address2 { get; set; }
        [Required]
        [Column("city")]
        [StringLength(30)]
        public string City { get; set; }
        [Required]
        [Column("state_code")]
        [StringLength(2)]
        public string StateCode { get; set; }
        [Required]
        [Column("zip")]
        [StringLength(10)]
        public string Zip { get; set; }
        [Column("address12")]
        [StringLength(30)]
        public string Address12 { get; set; }
        [Column("address22")]
        [StringLength(30)]
        public string Address22 { get; set; }
        [Column("city2")]
        [StringLength(30)]
        public string City2 { get; set; }
        [Column("state_code2")]
        [StringLength(2)]
        public string StateCode2 { get; set; }
        [Column("zip2")]
        [StringLength(10)]
        public string Zip2 { get; set; }
        [Column("phone_area_code")]
        [StringLength(3)]
        public string PhoneAreaCode { get; set; }
        [Column("phone_num")]
        [StringLength(7)]
        public string PhoneNum { get; set; }
        [Column("phone_area_code2")]
        [StringLength(3)]
        public string PhoneAreaCode2 { get; set; }
        [Column("phone_num2")]
        [StringLength(7)]
        public string PhoneNum2 { get; set; }
        [Column("phone_ext2")]
        [StringLength(10)]
        public string PhoneExt2 { get; set; }
        [Column("fax_area_code")]
        [StringLength(3)]
        public string FaxAreaCode { get; set; }
        [Column("fax_num")]
        [StringLength(7)]
        public string FaxNum { get; set; }
        [Column("email_address")]
        [StringLength(40)]
        public string EmailAddress { get; set; }
        [Required]
        [Column("info_verified")]
        [StringLength(1)]
        public string InfoVerified { get; set; }
        [Column("info_verified_by")]
        public int? InfoVerifiedBy { get; set; }
        [Column("info_verified_date", TypeName = "datetime")]
        public DateTime? InfoVerifiedDate { get; set; }
        [Column("client_desc")]
        [StringLength(255)]
        public string ClientDesc { get; set; }
        [Column("orig_creditor")]
        [StringLength(40)]
        public string OrigCreditor { get; set; }
    }
}
