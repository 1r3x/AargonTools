using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;

namespace AargonTools.Interfaces
{
    public interface ICardTokenizationDataHelper
    {
        Task<string> CreateCardInfo(LcgCardInfo cardObj, string environment);
        Task<string> CreatePaymentSchedule(LcgPaymentSchedule cardObj, string environment);
        Task<string> CreatePaymentScheduleHistory(LcgPaymentScheduleHistory cardObj, string environment);
        Task<string> InactivePaymentSchedule(int paymentScheduleId, string environment);
        Task<string> InactivePaymentSchedule2(string paymentScheduleId,int numberOfPayments, string environment);
    }
}
