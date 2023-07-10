using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IPreSchedulePaymentProcessing
    {
        Task<IList<LcgPaymentSchedule>> GetAllPreSchedulePaymentInfo(string environment);
        Task<LcgTablesViewModel> GetDetailsOfPreSchedulePaymentInfo(int paymentScheduleId, string environment);
        Task PostAll(string environment);
        Task OpenOrder(int orderId, string environment);
    }
}
