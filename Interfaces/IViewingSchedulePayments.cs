using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface IViewingSchedulePayments
    {
        ResponseModel GetSchedulePayments(ViewingSchedulePaymentsRequestModel date, string environment);
        ResponseModel ViewPaymentHistory(ViewingSchedulePaymentsRequestModel date, string environment);
        ResponseModel UpdatingSchedulePayment(UpdatingSchedulePayments updatingSchedulePaymentsRequest, string environment);
        ResponseModel DeleteSchedulePayment(DeleteSchedulePayments deleteSchedulePaymentsRequest, string environment);
    }
}
