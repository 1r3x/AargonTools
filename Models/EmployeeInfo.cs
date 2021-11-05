using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("employee_info")]
    [Index(nameof(LastName), nameof(FirstName), Name = "x_name")]
    public partial class EmployeeInfo
    {
        [Column("employee")]
        public int Employee { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(20)]
        public string LastName { get; set; }
        [Column("position")]
        [StringLength(20)]
        public string Position { get; set; }
        [Column("password")]
        [StringLength(10)]
        public string Password { get; set; }
        [Required]
        [Column("employee_type")]
        [StringLength(1)]
        public string EmployeeType { get; set; }
        [Column("department")]
        [StringLength(15)]
        public string Department { get; set; }
        [Required]
        [Column("company")]
        [StringLength(1)]
        public string Company { get; set; }
        [Required]
        [Column("administrator")]
        [StringLength(1)]
        public string Administrator { get; set; }
        [Required]
        [Column("acct_status")]
        [StringLength(1)]
        public string AcctStatus { get; set; }
        [Required]
        [Column("callable")]
        [StringLength(1)]
        public string Callable { get; set; }
        [Required]
        [Column("mailable")]
        [StringLength(1)]
        public string Mailable { get; set; }
        [Required]
        [Column("payable")]
        [StringLength(1)]
        public string Payable { get; set; }
        [Required]
        [Column("reportable")]
        [StringLength(1)]
        public string Reportable { get; set; }
        [Column("commission_pct", TypeName = "money")]
        public decimal? CommissionPct { get; set; }
        [Column("commission_start")]
        public int? CommissionStart { get; set; }
        [Column("salary", TypeName = "money")]
        public decimal? Salary { get; set; }
        [Required]
        [Column("commission_or")]
        [StringLength(1)]
        public string CommissionOr { get; set; }
        [Required]
        [Column("spoken_language")]
        [StringLength(10)]
        public string SpokenLanguage { get; set; }
        [Column("address1")]
        [StringLength(30)]
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
        [Column("home_phone")]
        [StringLength(10)]
        public string HomePhone { get; set; }
        [Column("ssn")]
        [StringLength(9)]
        public string Ssn { get; set; }
        [Column("birth_date", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column("hire_date", TypeName = "datetime")]
        public DateTime? HireDate { get; set; }
        [Column("termination_date", TypeName = "datetime")]
        public DateTime? TerminationDate { get; set; }
        [Column("emergency_contact")]
        [StringLength(100)]
        public string EmergencyContact { get; set; }
        [Column("comment")]
        [StringLength(100)]
        public string Comment { get; set; }
    }
}
