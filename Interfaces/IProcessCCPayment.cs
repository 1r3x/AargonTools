using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface IProcessCcPayment
    {
        Task<ResponseModel> ProcessCcPayment(string debtorAcc, string ccNumber, string expiredDate, string cvv, int numberOfPayments, decimal amount, string environment);
        Task<ResponseModel> SchedulePostData(string debtorAcct, DateTime postDate, decimal amount, string cardNumber, int numberOfPayments,
            string expMonth, string expYear, string environment);


    }
}
