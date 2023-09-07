using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class CardTokenizationDataHelper : ICardTokenizationDataHelper
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static CurrentBackupTestEnvironmentDbContext _contextCurrentBackupTest;

        public CardTokenizationDataHelper(ExistingDataDbContext context, TestEnvironmentDbContext contextTest,
            ProdOldDbContext contextProdOld, CurrentBackupTestEnvironmentDbContext contextCurrentBackupTest)
        {
            _context = context;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _contextCurrentBackupTest = contextCurrentBackupTest;
        }

        public async Task<string> CreateCardInfo(LcgCardInfo cardObj, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    await _contextTest.LcgCardInfos.AddAsync(cardObj);
                    await _contextTest.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    await _contextProdOld.LcgCardInfos.AddAsync(cardObj);
                    await _contextProdOld.SaveChangesAsync();
                }
                else if (environment == "P")
                {
                    await _context.LcgCardInfos.AddAsync(cardObj);
                    await _context.SaveChangesAsync();
                }
                else if (environment == "CBT")
                {
                    await _contextCurrentBackupTest.LcgCardInfos.AddAsync(cardObj);
                    await _contextCurrentBackupTest.SaveChangesAsync();
                }
                else
                {
                    await _contextTest.LcgCardInfos.AddAsync(cardObj);
                    await _contextTest.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "Successful";
        }

        public async Task<string> CreatePaymentSchedule(LcgPaymentSchedule cardObj, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    await _contextTest.LcgPaymentSchedules.AddAsync(cardObj);
                    await _contextTest.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    await _contextProdOld.LcgPaymentSchedules.AddAsync(cardObj);
                    await _contextProdOld.SaveChangesAsync();
                }
                else if (environment == "P")
                {
                    await _context.LcgPaymentSchedules.AddAsync(cardObj);
                    await _context.SaveChangesAsync();
                }
                else if (environment == "CBT")
                {
                    await _contextCurrentBackupTest.LcgPaymentSchedules.AddAsync(cardObj);
                    await _contextCurrentBackupTest.SaveChangesAsync();
                }
                else
                {
                    await _contextTest.LcgPaymentSchedules.AddAsync(cardObj);
                    await _contextTest.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "Successful";
        }

        public async Task<string> CreatePaymentScheduleHistory(LcgPaymentScheduleHistory cardObj, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    await _contextTest.LcgPaymentScheduleHistories.AddAsync(cardObj);
                    await _contextTest.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    await _contextProdOld.LcgPaymentScheduleHistories.AddAsync(cardObj);
                    await _contextProdOld.SaveChangesAsync();
                }
                else if (environment == "P")
                {
                    await _context.LcgPaymentScheduleHistories.AddAsync(cardObj);
                    await _context.SaveChangesAsync();
                }
                else if (environment == "CBT")
                {
                    await _contextCurrentBackupTest.LcgPaymentScheduleHistories.AddAsync(cardObj);
                    await _contextCurrentBackupTest.SaveChangesAsync();
                }
                else
                {
                    await _contextTest.LcgPaymentScheduleHistories.AddAsync(cardObj);
                    await _contextTest.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "Successful";
        }

        public async Task<string> InactivePaymentSchedule(int paymentScheduleId, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var paymentScheduleUpdate =
                        _context.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _context.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var paymentScheduleUpdate =
                        _contextProdOld.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _contextProdOld.SaveChangesAsync();
                }
                else if (environment == "CBT")
                {
                    var paymentScheduleUpdate =
                        _contextCurrentBackupTest.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _contextCurrentBackupTest.SaveChangesAsync();
                }
                else
                {
                    var paymentScheduleUpdate =
                        _contextTest.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _contextTest.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return "Successful";


        }
    }
}
