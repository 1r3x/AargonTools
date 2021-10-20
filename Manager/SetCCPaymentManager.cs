using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;

namespace AargonTools.Manager
{
    public class SetCCPaymentManager:ISetCCPayment
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;
        private readonly IUserService _userService;

        public SetCCPaymentManager(ExistingDataDbContext context, ResponseModel response,
            TestEnvironmentDbContext contextTest, IAddNotes addNotes, AdoDotNetConnection adoConnection, IUserService userService,
            ProdOldDbContext contextProdOld)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _adoConnection = adoConnection;
            _userService = userService;
            _contextProdOld = contextProdOld;
        }
        public async Task<ResponseModel> SetCCPayment(CcPaymnetRequestModel request,
            string environment)
        {

            List<String> companyContainsLIst = new List<String>();
            companyContainsLIst.Add("AARGON AGENCY");
            companyContainsLIst.Add("TOTAL CREDIT RECOVERY");


            List<String> approvalLIst = new List<String>();
            approvalLIst.Add("APPROVED");
            approvalLIst.Add("DECLINED");
            approvalLIst.Add("ERROR");


            List<String> sifStatusLIst = new List<String>();
            sifStatusLIst.Add("Y");
            sifStatusLIst.Add("N");

            if (request.paymentDate>DateTime.Now)
            {
                return _response.Response("Payment date won't be in future.");
            }
            else if (!companyContainsLIst.Contains(request.company))
            {
                return _response.Response("Please correct company name.");
            }
            else if (!approvalLIst.Contains(request.approvalStatus))
            {
                return _response.Response("Please correct approval status.");
            }
            else if (!request.refNo.Contains("USAEPAY2"))
            {
                return _response.Response("Please correct reference number.");
            }
            else if (!sifStatusLIst.Contains(request.sif))
            {
                return _response.Response("SIF must be just 'Y' or 'N' ");
            }
            try
            {
                if (environment == "P")
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = request.company,
                        UserId = request.userId,
                        UserName = request.userId + " "+"API",
                        ChargeTotal = request.chargeTotal,
                        Subtotal = request.chargeTotal,
                        PaymentDate = request.paymentDate,
                        ApprovalStatus = request.approvalStatus,
                        ApprovalCode = request.approvalCode,
                        OrderNumber = request.orderNumber,
                        RefNumber = request.refNo,
                        Sif = request.sif
                    };
                    await _context.CcPayments.AddAsync(ccPayment);
                    await _context.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = request.company,
                        UserId = request.userId,
                        UserName = request.userId + " " + "API",
                        ChargeTotal = request.chargeTotal,
                        Subtotal = request.chargeTotal,
                        PaymentDate = request.paymentDate,
                        ApprovalStatus = request.approvalStatus,
                        ApprovalCode = request.approvalCode,
                        OrderNumber = request.orderNumber,
                        RefNumber = request.refNo,
                        Sif = request.sif
                    };
                    await _contextProdOld.CcPayments.AddAsync(ccPayment);
                    await _contextProdOld.SaveChangesAsync();
                }
                else
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = request.company,
                        UserId = request.userId,
                        UserName = request.userId + " " + "API",
                        ChargeTotal = request.chargeTotal,
                        Subtotal = request.chargeTotal,
                        PaymentDate = request.paymentDate,
                        ApprovalStatus = request.approvalStatus,
                        ApprovalCode = request.approvalCode,
                        OrderNumber = request.orderNumber,
                        RefNumber = request.refNo,
                        Sif = request.sif
                    };
                    await _contextTest.CcPayments.AddAsync(ccPayment);
                    await _contextTest.SaveChangesAsync();
                }
                return _response.Response("Successfully set CC payment.");

            }
            catch (Exception e)
            {
                return _response.Response(e);
            }
        }
    }
}
