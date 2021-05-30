using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    //[Keyless]
    [Table("debtor_phone_info")]
    [Index(nameof(OtherAreaCode), nameof(OtherPhone), Name = "x_other_area_phone")]
    [Index(nameof(WorkAreaCode), nameof(WorkPhone), Name = "x_work_area_phone")]
    public partial class DebtorPhoneInfo
    {
        [Key]
        [Required]
        [Column("debtor_acct")]
        [StringLength(15)]
        public string DebtorAcct { get; set; }
        [Column("home_area_code")]
        [StringLength(3)]
        public string HomeAreaCode { get; set; }
        [Column("home_phone")]
        [StringLength(7)]
        public string HomePhone { get; set; }
        [Column("home_phone_ext")]
        [StringLength(10)]
        public string HomePhoneExt { get; set; }
        [Column("work_area_code")]
        [StringLength(3)]
        public string WorkAreaCode { get; set; }
        [Column("work_phone")]
        [StringLength(7)]
        public string WorkPhone { get; set; }
        [Column("work_phone_ext")]
        [StringLength(10)]
        public string WorkPhoneExt { get; set; }
        [Column("home_phone_ver")]
        [StringLength(1)]
        public string HomePhoneVer { get; set; }
        [Column("home_phone_ver_by")]
        public int? HomePhoneVerBy { get; set; }
        [Column("home_phone_ver_date", TypeName = "datetime")]
        public DateTime? HomePhoneVerDate { get; set; }
        [Column("home_phone_dont_call")]
        [StringLength(1)]
        public string HomePhoneDontCall { get; set; }
        [Column("work_phone_ver")]
        [StringLength(1)]
        public string WorkPhoneVer { get; set; }
        [Column("work_phone_ver_by")]
        public int? WorkPhoneVerBy { get; set; }
        [Column("work_phone_ver_date", TypeName = "datetime")]
        public DateTime? WorkPhoneVerDate { get; set; }
        [Column("work_phone_dont_call")]
        [StringLength(1)]
        public string WorkPhoneDontCall { get; set; }
        [Column("cell_area_code")]
        [StringLength(3)]
        public string CellAreaCode { get; set; }
        [Column("cell_phone")]
        [StringLength(7)]
        public string CellPhone { get; set; }
        [Column("cell_phone_ver")]
        [StringLength(1)]
        public string CellPhoneVer { get; set; }
        [Column("cell_phone_ver_by")]
        public int? CellPhoneVerBy { get; set; }
        [Column("cell_phone_ver_date", TypeName = "datetime")]
        public DateTime? CellPhoneVerDate { get; set; }
        [Column("cell_phone_dont_call")]
        [StringLength(1)]
        public string CellPhoneDontCall { get; set; }
        [Column("relative_area_code")]
        [StringLength(3)]
        public string RelativeAreaCode { get; set; }
        [Column("relative_phone")]
        [StringLength(7)]
        public string RelativePhone { get; set; }
        [Column("relative_phone_ver")]
        [StringLength(1)]
        public string RelativePhoneVer { get; set; }
        [Column("relative_phone_ver_by")]
        public int? RelativePhoneVerBy { get; set; }
        [Column("relative_phone_ver_date", TypeName = "datetime")]
        public DateTime? RelativePhoneVerDate { get; set; }
        [Column("relative_phone_dont_call")]
        [StringLength(1)]
        public string RelativePhoneDontCall { get; set; }
        [Column("other_area_code")]
        [StringLength(3)]
        public string OtherAreaCode { get; set; }
        [Column("other_phone")]
        [StringLength(7)]
        public string OtherPhone { get; set; }
        [Column("other_phone_ver")]
        [StringLength(1)]
        public string OtherPhoneVer { get; set; }
        [Column("other_phone_ver_by")]
        public int? OtherPhoneVerBy { get; set; }
        [Column("other_phone_ver_date", TypeName = "datetime")]
        public DateTime? OtherPhoneVerDate { get; set; }
        [Column("other_phone_dont_call")]
        [StringLength(1)]
        public string OtherPhoneDontCall { get; set; }
        [Column("home_schedule_date", TypeName = "datetime")]
        public DateTime? HomeScheduleDate { get; set; }
        [Column("work_schedule_date", TypeName = "datetime")]
        public DateTime? WorkScheduleDate { get; set; }
        [Column("cell_phone_text")]
        [StringLength(1)]
        public string CellPhoneText { get; set; }
        [Column("home_phone_pon_by")]
        public int? HomePhonePonBy { get; set; }
        [Column("home_phone_pon_date", TypeName = "datetime")]
        public DateTime? HomePhonePonDate { get; set; }
        [Column("work_phone_pon_by")]
        public int? WorkPhonePonBy { get; set; }
        [Column("work_phone_pon_date", TypeName = "datetime")]
        public DateTime? WorkPhonePonDate { get; set; }
        [Column("cell_phone_pon_by")]
        public int? CellPhonePonBy { get; set; }
        [Column("cell_phone_pon_date", TypeName = "datetime")]
        public DateTime? CellPhonePonDate { get; set; }
        [Column("relative_phone_pon_by")]
        public int? RelativePhonePonBy { get; set; }
        [Column("relative_phone_pon_date", TypeName = "datetime")]
        public DateTime? RelativePhonePonDate { get; set; }
        [Column("other_phone_pon_by")]
        public int? OtherPhonePonBy { get; set; }
        [Column("other_phone_pon_date", TypeName = "datetime")]
        public DateTime? OtherPhonePonDate { get; set; }
        [Column("client_cell_phone")]
        [StringLength(1)]
        public string ClientCellPhone { get; set; }
        [Column("cell_phone_is_debtors")]
        [StringLength(1)]
        public string CellPhoneIsDebtors { get; set; }
        [Column("auth_cell_dialer_calls")]
        [StringLength(1)]
        public string AuthCellDialerCalls { get; set; }
        [Column("home_phone_approved")]
        [StringLength(10)]
        public string HomePhoneApproved { get; set; }
        [Column("home_phone_approved_date", TypeName = "datetime")]
        public DateTime? HomePhoneApprovedDate { get; set; }
        [Column("home_phone_approved_by")]
        public int? HomePhoneApprovedBy { get; set; }
        [Column("cell_phone_approved")]
        [StringLength(10)]
        public string CellPhoneApproved { get; set; }
        [Column("cell_phone_approved_date", TypeName = "datetime")]
        public DateTime? CellPhoneApprovedDate { get; set; }
        [Column("cell_phone_approved_by")]
        public int? CellPhoneApprovedBy { get; set; }
        [Column("cell_phone_text_approved")]
        [StringLength(1)]
        public string CellPhoneTextApproved { get; set; }
        [Column("cell_phone_text_approved_date", TypeName = "datetime")]
        public DateTime? CellPhoneTextApprovedDate { get; set; }
        [Column("cell_phone_text_approved_by")]
        public int? CellPhoneTextApprovedBy { get; set; }
        [Column("third_party_first_name")]
        [StringLength(20)]
        public string ThirdPartyFirstName { get; set; }
        [Column("third_party_last_name")]
        [StringLength(30)]
        public string ThirdPartyLastName { get; set; }
        [Column("third_party_approved")]
        [StringLength(1)]
        public string ThirdPartyApproved { get; set; }
        [Column("third_party_approved_date", TypeName = "datetime")]
        public DateTime? ThirdPartyApprovedDate { get; set; }
        [Column("third_party_approved_by")]
        public int? ThirdPartyApprovedBy { get; set; }
        [Column("third_party2_first_name")]
        [StringLength(20)]
        public string ThirdParty2FirstName { get; set; }
        [Column("third_party2_last_name")]
        [StringLength(30)]
        public string ThirdParty2LastName { get; set; }
        [Column("third_party2_approved")]
        [StringLength(1)]
        public string ThirdParty2Approved { get; set; }
        [Column("third_party2_approved_date", TypeName = "datetime")]
        public DateTime? ThirdParty2ApprovedDate { get; set; }
        [Column("third_party2_approved_by")]
        public int? ThirdParty2ApprovedBy { get; set; }
        [Column("orig_home_area_code")]
        [StringLength(3)]
        public string OrigHomeAreaCode { get; set; }
        [Column("home_area_code_changed", TypeName = "datetime")]
        public DateTime? HomeAreaCodeChanged { get; set; }
        [Column("orig_home_phone")]
        [StringLength(7)]
        public string OrigHomePhone { get; set; }
        [Column("home_phone_changed", TypeName = "datetime")]
        public DateTime? HomePhoneChanged { get; set; }
        [Column("orig_work_area_code")]
        [StringLength(3)]
        public string OrigWorkAreaCode { get; set; }
        [Column("work_area_code_changed", TypeName = "datetime")]
        public DateTime? WorkAreaCodeChanged { get; set; }
        [Column("orig_work_phone")]
        [StringLength(7)]
        public string OrigWorkPhone { get; set; }
        [Column("work_phone_changed", TypeName = "datetime")]
        public DateTime? WorkPhoneChanged { get; set; }
        [Column("orig_cell_area_code")]
        [StringLength(3)]
        public string OrigCellAreaCode { get; set; }
        [Column("cell_area_code_changed", TypeName = "datetime")]
        public DateTime? CellAreaCodeChanged { get; set; }
        [Column("orig_cell_phone")]
        [StringLength(7)]
        public string OrigCellPhone { get; set; }
        [Column("cell_phone_changed", TypeName = "datetime")]
        public DateTime? CellPhoneChanged { get; set; }
    }
}
