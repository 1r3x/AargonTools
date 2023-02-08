using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;

namespace AargonTools.Manager
{
    public class AddCcPaymentV2 : IAddCcPaymentV2
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static CurrentBackupTestEnvironmentDbContext _contextCurrentBackupTest;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;
        private readonly IUserService _userService;

        public AddCcPaymentV2(ExistingDataDbContext context, ResponseModel response,
            TestEnvironmentDbContext contextTest, IAddNotes addNotes, AdoDotNetConnection adoConnection, IUserService userService,
            ProdOldDbContext contextProdOld, CurrentBackupTestEnvironmentDbContext contextCurrentBackupTest)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _adoConnection = adoConnection;
            _userService = userService;
            _contextProdOld = contextProdOld;
            _contextCurrentBackupTest = contextCurrentBackupTest;
        }
        public async Task<ResponseModel> AddCcPayment(CcPayment request, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.DebtorAcct,
                        Company = request.Company,
                        UserId = request.UserId,
                        UserName = request.UserId + " " + "API",
                        ChargeTotal = request.ChargeTotal,
                        Subtotal = request.Subtotal,
                        PaymentDate = request.PaymentDate,
                        ApprovalStatus = request.ApprovalStatus,
                        ApprovalCode = request.ApprovalCode,
                        OrderNumber = request.OrderNumber,
                        RefNumber = request.RefNumber,
                        Sif = request.Sif
                    };
                    await _context.CcPayments.AddAsync(ccPayment);
                    await _context.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.DebtorAcct,
                        Company = request.Company,
                        UserId = request.UserId,
                        UserName = request.UserId + " " + "API",
                        ChargeTotal = request.ChargeTotal,
                        Subtotal = request.Subtotal,
                        PaymentDate = request.PaymentDate,
                        ApprovalStatus = request.ApprovalStatus,
                        ApprovalCode = request.ApprovalCode,
                        OrderNumber = request.OrderNumber,
                        RefNumber = request.RefNumber,
                        Sif = request.Sif
                    };
                    await _contextProdOld.CcPayments.AddAsync(ccPayment);
                    await _contextProdOld.SaveChangesAsync();
                }
                else if (environment == "CBT")
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.DebtorAcct,
                        Company = request.Company,
                        UserId = request.UserId,
                        UserName = request.UserId + " " + "API",
                        ChargeTotal = request.ChargeTotal,
                        Subtotal = request.Subtotal,
                        PaymentDate = request.PaymentDate,
                        ApprovalStatus = request.ApprovalStatus,
                        ApprovalCode = request.ApprovalCode,
                        OrderNumber = request.OrderNumber,
                        RefNumber = request.RefNumber,
                        Sif = request.Sif
                    };
                    await _contextCurrentBackupTest.CcPayments.AddAsync(ccPayment);
                    await _contextCurrentBackupTest.SaveChangesAsync();
                }
                else
                {
                    var datetimeNow = DateTime.Now;
                    var ccPayment = new CcPayment()
                    {
                        DebtorAcct = request.DebtorAcct,
                        Company = request.Company,
                        UserId = request.UserId,
                        UserName = request.UserId + " " + "API",
                        ChargeTotal = request.ChargeTotal,
                        Subtotal = request.Subtotal,
                        PaymentDate = request.PaymentDate,
                        ApprovalStatus = request.ApprovalStatus,
                        ApprovalCode = request.ApprovalCode,
                        OrderNumber = request.OrderNumber,
                        RefNumber = request.RefNumber,
                        Sif = request.Sif
                    };
                    await _contextTest.CcPayments.AddAsync(ccPayment);
                    await _contextTest.SaveChangesAsync();
                }

                return _response.Response(true, true, "Successfully set CC payment.");

            }
            catch (Exception e)
            {
                return _response.Response(true, false, e);
            }
        }
    }
}
