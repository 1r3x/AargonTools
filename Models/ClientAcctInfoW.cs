using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AargonTools.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace AargonTools.Models
{
    [Keyless]
    [Table("client_acct_info_w")]
    public partial class ClientAcctInfoW: IClientAcctInfo
    {
        [Required]
        [Column("client_acct")]
        [StringLength(4)]
        public string ClientAcct { get; set; }
        [Required]
        [Column("acct_type")]
        [StringLength(20)]
        public string AcctType { get; set; }
        [Column("group_num")]
        public int? GroupNum { get; set; }
        [Column("placement_type")]
        [StringLength(20)]
        public string PlacementType { get; set; }
        [Required]
        [Column("acct_status")]
        [StringLength(1)]
        public string AcctStatus { get; set; }
        [Required]
        [Column("directs_only")]
        [StringLength(1)]
        public string DirectsOnly { get; set; }
        [Required]
        [Column("no_longer_placing")]
        [StringLength(1)]
        public string NoLongerPlacing { get; set; }
        [Column("amount_placed", TypeName = "money")]
        public decimal AmountPlaced { get; set; }
        [Column("adjustments_life", TypeName = "money")]
        public decimal AdjustmentsLife { get; set; }
        [Column("payment_amt_life", TypeName = "money")]
        public decimal PaymentAmtLife { get; set; }
        [Required]
        [Column("remit_full_pmt")]
        [StringLength(1)]
        public string RemitFullPmt { get; set; }
        [Required]
        [Column("remission_interval")]
        [StringLength(1)]
        public string RemissionInterval { get; set; }
        [Column("last_remission", TypeName = "datetime")]
        public DateTime? LastRemission { get; set; }
        [Required]
        [Column("do_not_mail_invoice")]
        [StringLength(1)]
        public string DoNotMailInvoice { get; set; }
        [Column("interest_pct", TypeName = "money")]
        public decimal InterestPct { get; set; }
        [Column("insurance_pct", TypeName = "money")]
        public decimal InsurancePct { get; set; }
        [Column("commission_pct1", TypeName = "money")]
        public decimal CommissionPct1 { get; set; }
        [Column("commission_pct2", TypeName = "money")]
        public decimal CommissionPct2 { get; set; }
        [Column("collection_pct", TypeName = "money")]
        public decimal CollectionPct { get; set; }
        [Column("legal_pct", TypeName = "money")]
        public decimal LegalPct { get; set; }
        [Column("payment_arrangement_pct", TypeName = "money")]
        public decimal PaymentArrangementPct { get; set; }
        [Column("settlement_pct", TypeName = "money")]
        public decimal SettlementPct { get; set; }
        [Column("fixed_rate1", TypeName = "money")]
        public decimal? FixedRate1 { get; set; }
        [Column("fixed_rate2", TypeName = "money")]
        public decimal? FixedRate2 { get; set; }
        [Column("fixed_rate3", TypeName = "money")]
        public decimal? FixedRate3 { get; set; }
        [Column("fixed_rate4", TypeName = "money")]
        public decimal? FixedRate4 { get; set; }
        [Column("employee")]
        public int? Employee { get; set; }
        [Column("salesman_commission", TypeName = "money")]
        public decimal? SalesmanCommission { get; set; }
        [Column("salesman_pp_commission", TypeName = "money")]
        public decimal? SalesmanPpCommission { get; set; }
        [Column("employee2")]
        public int? Employee2 { get; set; }
        [Column("salesman2_commission", TypeName = "money")]
        public decimal? Salesman2Commission { get; set; }
        [Column("salesman2_pp_commission", TypeName = "money")]
        public decimal? Salesman2PpCommission { get; set; }
        [Column("operating_cost_per_unit", TypeName = "money")]
        public decimal? OperatingCostPerUnit { get; set; }
        [Column("date_placed", TypeName = "datetime")]
        public DateTime DatePlaced { get; set; }
        [Required]
        [Column("report_to_bureau")]
        [StringLength(1)]
        public string ReportToBureau { get; set; }
        [Column("days_before_reporting")]
        public int? DaysBeforeReporting { get; set; }
        [Column("transfer_day")]
        public int? TransferDay { get; set; }
        [Column("rating_score")]
        public int? RatingScore { get; set; }
        [Required]
        [Column("hipaa_on_file")]
        [StringLength(1)]
        public string HipaaOnFile { get; set; }
        [Required]
        [Column("allow_backdating")]
        [StringLength(1)]
        public string AllowBackdating { get; set; }
        [Column("first_letter")]
        [StringLength(20)]
        public string FirstLetter { get; set; }
        [Required]
        [Column("charge_fees")]
        [StringLength(1)]
        public string ChargeFees { get; set; }
        [Column("grade")]
        [StringLength(2)]
        public string Grade { get; set; }
        [Column("client_rep")]
        public int? ClientRep { get; set; }
        [Column("acct_age")]
        public int? AcctAge { get; set; }
        [Column("auth_cell_calls")]
        [StringLength(1)]
        public string AuthCellCalls { get; set; }
        [Column("charge_interest")]
        [StringLength(1)]
        public string ChargeInterest { get; set; }
        [Column("zero_commission")]
        [StringLength(1)]
        public string ZeroCommission { get; set; }
        [Column("tcpa_auth_date", TypeName = "datetime")]
        public DateTime? TcpaAuthDate { get; set; }
        [Column("client_contract")]
        [StringLength(1)]
        public string ClientContract { get; set; }
        [Column("client_contract_date", TypeName = "datetime")]
        public DateTime? ClientContractDate { get; set; }
        [Column("consumer_contract")]
        [StringLength(1)]
        public string ConsumerContract { get; set; }
        [Column("auth_text_msgs")]
        [StringLength(1)]
        public string AuthTextMsgs { get; set; }
        [Column("auth_emails")]
        [StringLength(1)]
        public string AuthEmails { get; set; }
        [Column("auth_text_msgs_date", TypeName = "datetime")]
        public DateTime? AuthTextMsgsDate { get; set; }
        [Column("auth_emails_date", TypeName = "datetime")]
        public DateTime? AuthEmailsDate { get; set; }
        [Column("auth_coll_fees")]
        [StringLength(1)]
        public string AuthCollFees { get; set; }
        [Column("auth_atty_fees")]
        [StringLength(1)]
        public string AuthAttyFees { get; set; }
        [Column("coll_fee_amt", TypeName = "money")]
        public decimal? CollFeeAmt { get; set; }
        [Column("atty_fee_pct", TypeName = "money")]
        public decimal? AttyFeePct { get; set; }
        [Column("atty_fee_amt", TypeName = "money")]
        public decimal? AttyFeeAmt { get; set; }
        [Column("cc_fee_pct", TypeName = "money")]
        public decimal? CcFeePct { get; set; }
        [Column("file_probate")]
        [StringLength(1)]
        public string FileProbate { get; set; }
        [Column("file_proof_of_claim")]
        [StringLength(1)]
        public string FileProofOfClaim { get; set; }
        [Column("file_judgments")]
        [StringLength(1)]
        public string FileJudgments { get; set; }
        [Column("dont_work")]
        [StringLength(1)]
        public string DontWork { get; set; }
        [Column("last_direct_pymt_file", TypeName = "datetime")]
        public DateTime? LastDirectPymtFile { get; set; }
        [Column("sif_date_of_service", TypeName = "datetime")]
        public DateTime? SifDateOfService { get; set; }
        [Column("sif_date_placed", TypeName = "datetime")]
        public DateTime? SifDatePlaced { get; set; }
        [Column("sif_start_date", TypeName = "datetime")]
        public DateTime? SifStartDate { get; set; }
        [Column("sif_end_date", TypeName = "datetime")]
        public DateTime? SifEndDate { get; set; }
        [Column("settlement_pct_restricted", TypeName = "money")]
        public decimal? SettlementPctRestricted { get; set; }
        [Column("sif_date_placed_num_days")]
        public int? SifDatePlacedNumDays { get; set; }
    }
}
