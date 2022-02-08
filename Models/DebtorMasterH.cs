using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AargonTools.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("debtor_master_h")]
    [Index(nameof(FirstName), Name = "x_first_name")]
    [Index(nameof(LastName), Name = "x_last_name")]
    [Index(nameof(Ssn), Name = "x_ssn")]
    public partial class DebtorMasterH: IDebtorMaster
    {
        [Key]
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("ssn")]
        [StringLength(9)]
        public string Ssn { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(30)]
        public string LastName { get; set; }
        [Column("middle_name")]
        [StringLength(20)]
        public string MiddleName { get; set; }
        [Column("address1")]
        [StringLength(40)]
        public string Address1 { get; set; }
        [Column("address2")]
        [StringLength(30)]
        public string Address2 { get; set; }
        [Column("city")]
        [StringLength(30)]
        public string City { get; set; }
        [Column("state_code")]
        [StringLength(2)]
        public string StateCode { get; set; }
        [Column("zip")]
        [StringLength(10)]
        public string Zip { get; set; }
        [Column("birth_date", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column("marital_status")]
        [StringLength(10)]
        public string MaritalStatus { get; set; }
        [Column("sex")]
        [StringLength(1)]
        public string Sex { get; set; }
        [Column("residence_status")]
        [StringLength(15)]
        public string ResidenceStatus { get; set; }
        [Column("residence_ver_by")]
        public int? ResidenceVerBy { get; set; }
        [Column("residence_ver_date", TypeName = "datetime")]
        public DateTime? ResidenceVerDate { get; set; }
        [Column("address_change_date", TypeName = "datetime")]
        public DateTime? AddressChangeDate { get; set; }
        [Column("spanish")]
        [StringLength(1)]
        public string Spanish { get; set; }
        [Column("ssn_ver")]
        [StringLength(1)]
        public string SsnVer { get; set; }
        [Column("ssn_ver_by")]
        public int? SsnVerBy { get; set; }
        [Column("ssn_ver_date", TypeName = "datetime")]
        public DateTime? SsnVerDate { get; set; }
        [Column("orig_first_name")]
        [StringLength(20)]
        public string OrigFirstName { get; set; }
        [Column("first_name_changed", TypeName = "datetime")]
        public DateTime? FirstNameChanged { get; set; }
        [Column("orig_last_name")]
        [StringLength(30)]
        public string OrigLastName { get; set; }
        [Column("last_name_changed", TypeName = "datetime")]
        public DateTime? LastNameChanged { get; set; }
        [Column("orig_ssn")]
        [StringLength(9)]
        public string OrigSsn { get; set; }
        [Column("ssn_changed", TypeName = "datetime")]
        public DateTime? SsnChanged { get; set; }
        [Column("orig_dob", TypeName = "datetime")]
        public DateTime? OrigDob { get; set; }
        [Column("dob_changed", TypeName = "datetime")]
        public DateTime? DobChanged { get; set; }
        [Column("orig_address1")]
        [StringLength(40)]
        public string OrigAddress1 { get; set; }
        [Column("address1_changed", TypeName = "datetime")]
        public DateTime? Address1Changed { get; set; }
        [Column("orig_address2")]
        [StringLength(30)]
        public string OrigAddress2 { get; set; }
        [Column("address2_changed", TypeName = "datetime")]
        public DateTime? Address2Changed { get; set; }
        [Column("orig_city")]
        [StringLength(30)]
        public string OrigCity { get; set; }
        [Column("city_changed", TypeName = "datetime")]
        public DateTime? CityChanged { get; set; }
        [Column("orig_state")]
        [StringLength(2)]
        public string OrigState { get; set; }
        [Column("state_changed", TypeName = "datetime")]
        public DateTime? StateChanged { get; set; }
        [Column("orig_zip_code")]
        [StringLength(10)]
        public string OrigZipCode { get; set; }
        [Column("zip_code_changed", TypeName = "datetime")]
        public DateTime? ZipCodeChanged { get; set; }
    }
}
