using System;

namespace AargonTools.Interfaces
{
    public interface IDebtorAcctInfo
    {
         string DebtorAcct { get; set; }
         string AcctType { get; set; }
         string SuppliedAcct { get; set; }
         string AcctStatus { get; set; }
         string Corporate { get; set; }
         string Legal { get; set; }
         string StatusCode { get; set; }
         string ActivityCode { get; set; }
         int? Disposition { get; set; }
         DateTime? ScheduleDate { get; set; }
         DateTime? IvrScheduleDate { get; set; }
         decimal AmountPlaced { get; set; }
         decimal AdjustmentsLife { get; set; }
         decimal InterestAmtLife { get; set; }
         decimal PaymentAmtLife { get; set; }
         decimal AdminFeesDue { get; set; }
         decimal AdminFeesPaid { get; set; }
         decimal AdminFeesBalance { get; set; }
         decimal CostsDue { get; set; }
         decimal CostsPaid { get; set; }
         decimal CostsBalance { get; set; }
         decimal AttorneyFeesDue { get; set; }
         decimal AttorneyFeesPaid { get; set; }
         decimal AttorneyFeesBalance { get; set; }
         decimal DamagesDue { get; set; }
         decimal DamagesPaid { get; set; }
         decimal DamagesBalance { get; set; }
         decimal ReturnCheckFeesDue { get; set; }
         decimal ReturnCheckFeesPaid { get; set; }
         decimal ReturnCheckFeesBalance { get; set; }
         decimal TotalFeesBalance { get; set; }
         decimal Balance { get; set; }
         int BrokenPromises { get; set; }
         int Placement { get; set; }
         string MailReturn { get; set; }
         string MediaOnFile { get; set; }
         string AcctDesc { get; set; }
         int? Employee { get; set; }
         int? EnteredBy { get; set; }
         DateTime? DateOfService { get; set; }
         DateTime? DatePlacedPrecollect { get; set; }
         DateTime DatePlaced { get; set; }
         DateTime? DateInactivated { get; set; }
         DateTime? DateLastAccessed { get; set; }
         DateTime BeginAgeDate { get; set; }
         decimal? LastPaymentAmt { get; set; }
         string BillAs { get; set; }
         string AccountAlert { get; set; }
         string NsfCheckOnFile { get; set; }
         string WroteNsfCheck { get; set; }
         string BankAcctClosed { get; set; }
         DateTime? FeeEntryDate { get; set; }
         string SuppliedAcct2 { get; set; }
         string OrigLenderName { get; set; }
         string CosignerFirstName { get; set; }
         string CosignerLastName { get; set; }
         string AgencyNum { get; set; }
         string SuppliedAcct3 { get; set; }
         string SuppliedAcct4 { get; set; }
         string ClientRating { get; set; }
         int? AgencyRating { get; set; }
         int? AgencyAttorneyCode { get; set; }
         string InsuranceType { get; set; }
         string OutOfStatute { get; set; }
         DateTime? CostsEntryDate { get; set; }
         string EmailAddress { get; set; }
         string RestrictPromo { get; set; }
         decimal? LiquidEdge { get; set; }
         string ServiceAddrSame { get; set; }
         DateTime? PromoStart { get; set; }
         decimal? CollectionFeesDue { get; set; }
         decimal? CollectionFeesPaid { get; set; }
         decimal? CollectionFeesBalance { get; set; }
         DateTime? CfpbComplaint { get; set; }
         DateTime? EmailApprovedDate { get; set; }
         int? EmailApprovedBy { get; set; }
         string EmailApproved { get; set; }
         string DoNotChargeInterest { get; set; }
         DateTime? MailReturnSetDate { get; set; }
         decimal? OrigAmountPlaced { get; set; }

         DateTime? DischargeDate { get; set; }
         decimal? TotalCharges { get; set; }
         string FinClass { get; set; }
         string EmailOptIn { get; set; }
         DateTime? EmailOptInDate { get; set; }

         string EmailOptOut { get; set; }
         DateTime? EmailOptOutDate { get; set; }
    }
}
