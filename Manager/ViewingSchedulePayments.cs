using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Manager
{
    public class ViewingSchedulePayments : IViewingSchedulePayments
    {
        private readonly TestEnvironmentDbContext _dbContext;
        private readonly ProdOldDbContext _dbContextProdOld;
        private readonly ExistingDataDbContext _dbContextForProd;
        private static ResponseModel _response;

        public ViewingSchedulePayments(TestEnvironmentDbContext dbContext, ProdOldDbContext dbContextProdOld,
            ExistingDataDbContext dbContextForProd, ResponseModel response)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
            _response = response;
        }
        public ResponseModel GetSchedulePayments(ViewingSchedulePaymentsRequestModel date, string environment)
        {
            if (environment == "P")
            {
                var sceduledPayments = from paymentSchedule in _dbContextForProd.LcgPaymentSchedules
                                       where paymentSchedule.EffectiveDate.Date == date.date && paymentSchedule.IsActive == true
                                       join cardInfo in _dbContextForProd.LcgCardInfos
                                    on paymentSchedule.CardInfoId equals cardInfo.Id into rightGroup
                                       from right in rightGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           paymentSchedule.Id,
                                           paymentSchedule.EffectiveDate,
                                           paymentSchedule.NumberOfPayments,
                                           paymentSchedule.PatientAccount,
                                           right.AssociateDebtorAcct,
                                           right.CardHolderName,
                                           right.ExpirationMonth,
                                           right.ExpirationYear
                                       };
                return _response.Response(true, sceduledPayments);
            }
            else if (environment == "PO")
            {
                var sceduledPayments = from paymentSchedule in _dbContextProdOld.LcgPaymentSchedules
                                       where paymentSchedule.EffectiveDate.Date == date.date && paymentSchedule.IsActive == true
                                       join cardInfo in _dbContextProdOld.LcgCardInfos
                                    on paymentSchedule.CardInfoId equals cardInfo.Id into rightGroup
                                       from right in rightGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           paymentSchedule.Id,
                                           paymentSchedule.EffectiveDate,
                                           paymentSchedule.NumberOfPayments,
                                           paymentSchedule.PatientAccount,
                                           right.AssociateDebtorAcct,
                                           right.CardHolderName,
                                           right.ExpirationMonth,
                                           right.ExpirationYear
                                       };
                return _response.Response(true, sceduledPayments);
            }
            else if (environment == "T")
            {
                var sceduledPayments = from paymentSchedule in _dbContext.LcgPaymentSchedules
                                       where paymentSchedule.EffectiveDate.Date == date.date && paymentSchedule.IsActive == true
                                       join cardInfo in _dbContext.LcgCardInfos
                                    on paymentSchedule.CardInfoId equals cardInfo.Id into rightGroup
                                       from right in rightGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           paymentSchedule.Id,
                                           paymentSchedule.EffectiveDate,
                                           paymentSchedule.NumberOfPayments,
                                           paymentSchedule.PatientAccount,
                                           right.AssociateDebtorAcct,
                                           right.CardHolderName,
                                           right.ExpirationMonth,
                                           right.ExpirationYear
                                       };
                return _response.Response(true, sceduledPayments);
            }
            else
            {
                var sceduledPayments = from paymentSchedule in _dbContext.LcgPaymentSchedules
                                       where paymentSchedule.EffectiveDate.Date == date.date && paymentSchedule.IsActive == true
                                       join cardInfo in _dbContext.LcgCardInfos
                                    on paymentSchedule.CardInfoId equals cardInfo.Id into rightGroup
                                       from right in rightGroup.DefaultIfEmpty()
                                       select new
                                       {
                                           paymentSchedule.Id,
                                           paymentSchedule.EffectiveDate,
                                           paymentSchedule.NumberOfPayments,
                                           paymentSchedule.PatientAccount,
                                           right.AssociateDebtorAcct,
                                           right.CardHolderName,
                                           right.ExpirationMonth,
                                           right.ExpirationYear
                                       };
                return _response.Response(true, sceduledPayments);
            }

        }

        public ResponseModel UpdatingSchedulePayment(UpdatingSchedulePayments updatingSchedulePaymentsRequest, string environment)
        {
            if (environment == "P")
            {
                _dbContextForProd.LcgPaymentSchedules
                    .Where(p => p.Id == updatingSchedulePaymentsRequest.scheduleId)
                    .ToList()
                    .ForEach(x => x.EffectiveDate = updatingSchedulePaymentsRequest.Updateddate);
                _dbContextForProd.SaveChanges();
                return _response.Response(true, "Data changed to " + updatingSchedulePaymentsRequest.Updateddate);
            }
            else if (environment == "PO")
            {
                _dbContextProdOld.LcgPaymentSchedules
                    .Where(p => p.Id == updatingSchedulePaymentsRequest.scheduleId)
                    .ToList()
                    .ForEach(x => x.EffectiveDate = updatingSchedulePaymentsRequest.Updateddate);
                _dbContextProdOld.SaveChanges();
                return _response.Response(true, "Data changed to " + updatingSchedulePaymentsRequest.Updateddate);
            }
            else if (environment == "T")
            {
                _dbContext.LcgPaymentSchedules
                    .Where(p => p.Id == updatingSchedulePaymentsRequest.scheduleId)
                    .ToList()
                    .ForEach(x => x.EffectiveDate = updatingSchedulePaymentsRequest.Updateddate);
                _dbContext.SaveChanges();
                return _response.Response(true, "Data changed to " + updatingSchedulePaymentsRequest.Updateddate);
            }
            else
            {
                _dbContext.LcgPaymentSchedules
                   .Where(p => p.Id == updatingSchedulePaymentsRequest.scheduleId)
                   .ToList()
                   .ForEach(x => x.EffectiveDate = updatingSchedulePaymentsRequest.Updateddate);
                _dbContext.SaveChanges();
                return _response.Response(true, "Data changed to " + updatingSchedulePaymentsRequest.Updateddate);
            }
        }

        public ResponseModel DeleteSchedulePayment(DeleteSchedulePayments deleteSchedulePaymentsRequest, string environment)
        {
            if (environment == "P")
            {
                _dbContextForProd.LcgPaymentSchedules
                    .Where(p => p.Id == deleteSchedulePaymentsRequest.scheduleId)
                    .ToList()
                    .ForEach(x => x.IsActive = false);
                _dbContextForProd.SaveChanges();
                return _response.Response(true, "Inactivated ");
            }
            else if (environment == "PO")
            {
                _dbContextProdOld.LcgPaymentSchedules
                    .Where(p => p.Id == deleteSchedulePaymentsRequest.scheduleId)
                    .ToList()
                    .ForEach(x => x.IsActive = false);
                _dbContextProdOld.SaveChanges();
                return _response.Response(true, "Inactivated ");
            }
            else if (environment == "T")
            {
                _dbContext.LcgPaymentSchedules
                   .Where(p => p.Id == deleteSchedulePaymentsRequest.scheduleId)
                   .ToList()
                   .ForEach(x => x.IsActive = false);
                _dbContext.SaveChanges();
                return _response.Response(true, "Inactivated ");
            }
            else
            {
                _dbContext.LcgPaymentSchedules
                  .Where(p => p.Id == deleteSchedulePaymentsRequest.scheduleId)
                  .ToList()
                  .ForEach(x => x.IsActive = false);
                _dbContext.SaveChanges();
                return _response.Response(true, "Inactivated ");
            }

        }

        public ResponseModel ViewPaymentHistory(ViewingSchedulePaymentsRequestModel date, string environment)
        {
            if (environment == "P")
            {
                var sceduledPaymentsHistory = from paymnetHistory in _dbContextForProd.LcgPaymentScheduleHistories
                                       where paymnetHistory.TimeLog.Value.Date == date.date
                                      
                                       select new
                                       {
                                           paymnetHistory.PaymentScheduleId,
                                           paymnetHistory.ResponseCode,
                                           paymnetHistory.ResponseMessage,
                                           paymnetHistory.TimeLog,
                                           paymnetHistory.TransactionId,
                                           paymnetHistory.AuthorizationNumber,
                                           paymnetHistory.AuthorizationText
                                          
                                       };
                return _response.Response(true, sceduledPaymentsHistory);
            }
            else if (environment == "PO")
            {
                var sceduledPaymentsHistory = from paymnetHistory in _dbContextProdOld.LcgPaymentScheduleHistories
                                              where paymnetHistory.TimeLog.Value.Date == date.date

                                              select new
                                              {
                                                  paymnetHistory.PaymentScheduleId,
                                                  paymnetHistory.ResponseCode,
                                                  paymnetHistory.ResponseMessage,
                                                  paymnetHistory.TimeLog,
                                                  paymnetHistory.TransactionId,
                                                  paymnetHistory.AuthorizationNumber,
                                                  paymnetHistory.AuthorizationText

                                              };
                return _response.Response(true, sceduledPaymentsHistory);
            }
            else if (environment == "T")
            {
                var sceduledPaymentsHistory = from paymnetHistory in _dbContext.LcgPaymentScheduleHistories
                                              where paymnetHistory.TimeLog.Value.Date == date.date

                                              select new
                                              {
                                                  paymnetHistory.PaymentScheduleId,
                                                  paymnetHistory.ResponseCode,
                                                  paymnetHistory.ResponseMessage,
                                                  paymnetHistory.TimeLog,
                                                  paymnetHistory.TransactionId,
                                                  paymnetHistory.AuthorizationNumber,
                                                  paymnetHistory.AuthorizationText

                                              };
                return _response.Response(true, sceduledPaymentsHistory);
            }
            else
            {
                var sceduledPaymentsHistory = from paymnetHistory in _dbContext.LcgPaymentScheduleHistories
                                              where paymnetHistory.TimeLog.Value.Date == date.date

                                              select new
                                              {
                                                  paymnetHistory.PaymentScheduleId,
                                                  paymnetHistory.ResponseCode,
                                                  paymnetHistory.ResponseMessage,
                                                  paymnetHistory.TimeLog,
                                                  paymnetHistory.TransactionId,
                                                  paymnetHistory.AuthorizationNumber,
                                                  paymnetHistory.AuthorizationText

                                              };
                return _response.Response(true, sceduledPaymentsHistory);
            }
        }
    }

}
