using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IProcessCcPayment
    {
        Task<ResponseModel> ProcessCcPayment(ProcessCcPaymentRequestModel request, string environment);
        Task<ResponseModel> SchedulePostData(string debtorAcct, DateTime postDate, decimal amount, string cardNumber, int numberOfPayments,
            string expMonth, string expYear, string environment);
        Task<ResponseModel> SchedulePostDataV2(SchedulePostDateRequest request, string environment);

    }
}
