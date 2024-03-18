using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace AargonTools.Manager
{
    public class PreSchedulePaymentProcessingManager : IPreSchedulePaymentProcessing
    {
        private readonly TestEnvironmentDbContext _dbContext;
        private readonly ProdOldDbContext _dbContextProdOld;
        private readonly ExistingDataDbContext _dbContextForProd;
        private LcgTablesViewModel _preScheduleLcgTablesViewModel;
        private IList<LcgPaymentSchedule> _paymentSchedule;
        private decimal _tempAmount;

        public PreSchedulePaymentProcessingManager(TestEnvironmentDbContext dbContext, ProdOldDbContext dbContextProdOld,
            ExistingDataDbContext dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }



        public async Task<IList<LcgPaymentSchedule>> GetAllPreSchedulePaymentInfo(string environment)
        {
            //todo after quality check should active the code block

            DateTime startDate;
            DateTime endDate;
            if (DateAndTime.Now.DayOfWeek.ToString() == "Monday")
            {
                startDate = DateTime.Now.AddDays(-1);//for testing purpose should remove 
                endDate = DateTime.Now.AddDays(1000);//for testing purpose should remove 
            }
            else
            {
                startDate = DateTime.Now.AddDays(-1);//for testing purpose should remove
                endDate = DateTime.Now.AddDays(1000);//for testing purpose should remove
            }

            if (environment == "T")
            {
                return await (from schedule in _dbContext.LcgPaymentSchedules
                              join card in _dbContext.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate //&& card.AssociateDebtorAcct.Substring(0, 4) == "4514"//todo add 4984 for every environments
                              select schedule).ToListAsync();


                //return await _dbContext.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }

            else if (environment == "PO")
            {
                return await (from schedule in _dbContextProdOld.LcgPaymentSchedules
                              join card in _dbContextProdOld.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate //&& card.AssociateDebtorAcct.Substring(0, 4) == "4514"
                              select schedule).ToListAsync();

                //return await _dbContextProdOld.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }

            else if (environment == "P")
            {

                return await (from schedule in _dbContextForProd.LcgPaymentSchedules
                              join card in _dbContextForProd.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate //&& card.AssociateDebtorAcct.Substring(0, 4) == "4514"
                              select schedule).ToListAsync();

                //return await _dbContextForProd.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }

            else
            {
                return await (from schedule in _dbContext.LcgPaymentSchedules
                              join card in _dbContext.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate //&& card.AssociateDebtorAcct.Substring(0, 4) == "4514"
                              select schedule).ToListAsync();

                //return await _dbContext.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }

            //return await _dbContext.LcgPaymentSchedules.ToListAsync();

        }



        public async Task<LcgTablesViewModel> GetDetailsOfPreSchedulePaymentInfo(int paymentScheduleId, string environment)
        {

            if (environment == "T")
            {

                var viewModel = await (from paymentSchedule in _dbContext.LcgPaymentSchedules
                                       join cardInfo in _dbContext.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();

                var response = new LcgTablesViewModel()
                {
                    //cardInfo

                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule

                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history
                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };

                return response;

            }
            else if (environment == "PO")
            {

                var viewModel = await (from paymentSchedule in _dbContextProdOld.LcgPaymentSchedules
                                       join cardInfo in _dbContextProdOld.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();
                var response = new LcgTablesViewModel()
                {
                    //cardInfo
                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule
                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history

                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };
                return response;
            }






            else if (environment == "P")
            {


                var viewModel = await (from paymentSchedule in _dbContextForProd.LcgPaymentSchedules
                                       join cardInfo in _dbContextForProd.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();


                var response = new LcgTablesViewModel()
                {
                    //cardInfo
                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule
                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history
                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };

                return response;

            }
            else
            {
                var viewModel = await (from paymentSchedule in _dbContext.LcgPaymentSchedules
                                       join cardInfo in _dbContext.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();

                var response = new LcgTablesViewModel()
                {

                    //cardInfo

                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,

                    //paymentSchedule

                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,

                    //history

                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText

                };

                return response;

            }

        }



        public async Task PostAll(string environment)
        {
            _paymentSchedule = await GetAllPreSchedulePaymentInfo(environment);
            for (int i = 0; i < _paymentSchedule.Count; i++)
            {

                if (_paymentSchedule[i].IsActive == true)
                {
                    await OpenOrder(_paymentSchedule[i].Id, environment);
                }

            }
        }



        public async Task OpenOrder(int orderId, string environment)
        {
            

            _preScheduleLcgTablesViewModel =
                await GetDetailsOfPreSchedulePaymentInfo(orderId, environment);
            _tempAmount = _preScheduleLcgTablesViewModel.Amount;
            //await ProcessSaleTrans();
            

        }



    }
}
