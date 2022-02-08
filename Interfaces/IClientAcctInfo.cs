using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface IClientAcctInfo
    {
       
        public string ClientAcct { get; set; }
       
        public string AcctType { get; set; }
       
        public int? GroupNum { get; set; }
       
        public string PlacementType { get; set; }
       
        public string AcctStatus { get; set; }
      
        public string DirectsOnly { get; set; }
       
        public string NoLongerPlacing { get; set; }
       
        public decimal AmountPlaced { get; set; }
       
        public decimal AdjustmentsLife { get; set; }
       
        public decimal PaymentAmtLife { get; set; }
       
        public string RemitFullPmt { get; set; }
       
        public string RemissionInterval { get; set; }
       
        public DateTime? LastRemission { get; set; }
       
        public string DoNotMailInvoice { get; set; }
       
        public decimal InterestPct { get; set; }
        
        public decimal InsurancePct { get; set; }
       
        public decimal CommissionPct1 { get; set; }
       
        public decimal CommissionPct2 { get; set; }
       
        public decimal CollectionPct { get; set; }
       
        public decimal LegalPct { get; set; }
        
        public decimal PaymentArrangementPct { get; set; }
       
        public decimal SettlementPct { get; set; }
       
        public decimal? FixedRate1 { get; set; }
        
        public decimal? FixedRate2 { get; set; }
        
        public decimal? FixedRate3 { get; set; }
       
        public decimal? FixedRate4 { get; set; }
        
        public int? Employee { get; set; }
       
        public decimal? SalesmanCommission { get; set; }
        
        public decimal? SalesmanPpCommission { get; set; }
       
        public int? Employee2 { get; set; }
       
        public decimal? Salesman2Commission { get; set; }
       
        public decimal? Salesman2PpCommission { get; set; }
        
        public decimal? OperatingCostPerUnit { get; set; }
       
        public DateTime DatePlaced { get; set; }
       
        public string ReportToBureau { get; set; }
       
        public int? DaysBeforeReporting { get; set; }
       
        public int? TransferDay { get; set; }

        public int? RatingScore { get; set; }

        public string HipaaOnFile { get; set; }
       
        public string AllowBackdating { get; set; }
       
        public string FirstLetter { get; set; }
       
        public string ChargeFees { get; set; }
       
        public string Grade { get; set; }
       
        public int? ClientRep { get; set; }
       
        public int? AcctAge { get; set; }
       
        public string AuthCellCalls { get; set; }
       
        public string ChargeInterest { get; set; }
       
        public string ZeroCommission { get; set; }
       
        public DateTime? TcpaAuthDate { get; set; }
       
        public string ClientContract { get; set; }
       
        public DateTime? ClientContractDate { get; set; }
       
        public string ConsumerContract { get; set; }
       
        public string AuthTextMsgs { get; set; }
       
        public string AuthEmails { get; set; }
       
        public DateTime? AuthTextMsgsDate { get; set; }
        
        public DateTime? AuthEmailsDate { get; set; }
       
        public string AuthCollFees { get; set; }
       
        public string AuthAttyFees { get; set; }
       
        public decimal? CollFeeAmt { get; set; }
       
        public decimal? AttyFeePct { get; set; }
       
        public decimal? AttyFeeAmt { get; set; }
       
        public decimal? CcFeePct { get; set; }
       
        public string FileProbate { get; set; }
       
        public string FileProofOfClaim { get; set; }
       
        public string FileJudgments { get; set; }
       
        public string DontWork { get; set; }
       
        public DateTime? LastDirectPymtFile { get; set; }
       
        public DateTime? SifDateOfService { get; set; }
       
        public DateTime? SifDatePlaced { get; set; }
       
        public DateTime? SifStartDate { get; set; }
     
        public DateTime? SifEndDate { get; set; }
       
        public decimal? SettlementPctRestricted { get; set; }
       
        public int? SifDatePlacedNumDays { get; set; }
    }
}
