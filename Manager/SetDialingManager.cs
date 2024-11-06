using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class SetDialingManager:ISetDialing
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;

        public SetDialingManager(ExistingDataDbContext context, ResponseModel response, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld,
            AdoDotNetConnection adoConnection)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _adoConnection = adoConnection;
        }

        public async Task<ResponseModel> SetDialing(SetDialingRequestModel request, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    var rowAdoTrans1 = _adoConnection.GetData("INSERT INTO ivr_recall_table SELECT GETDATE(),'"+ request.AreaCode + "','" + request.PhoneNumber + "'", environment);
                    var rowAdoTrans2 = _adoConnection.GetData("UPDATE apex_list_master " +
                                                             "SET call_date = getdate(), apex_status = 'CALLING' " +
                                                             "WHERE list_acct = " + request.ListAccount + " AND debtor_acct = '" + request.DebtorAccount + "'", environment);
                    await Task.CompletedTask;
                }
                else if (environment == "PO")
                {
                    var rowAdoTrans1 = _adoConnection.GetData("INSERT INTO ivr_recall_table SELECT GETDATE(),'" + request.AreaCode + "','" + request.PhoneNumber + "'", environment);
                    var rowAdoTrans2 = _adoConnection.GetData("UPDATE apex_list_master " +
                                                              "SET call_date = getdate(), apex_status = 'CALLING' " +
                                                              "WHERE list_acct = " + request.ListAccount + " AND debtor_acct = '" + request.DebtorAccount + "'", environment);
                    await Task.CompletedTask;
                }
                else
                {
                    var rowAdoTrans1 = _adoConnection.GetData("INSERT INTO ivr_recall_table SELECT GETDATE(),'" + request.AreaCode + "','" + request.PhoneNumber + "'", environment);
                    var rowAdoTrans2 = _adoConnection.GetData("UPDATE apex_list_master " +
                                                              "SET call_date = getdate(), apex_status = 'CALLING' " +
                                                              "WHERE list_acct = " + request.ListAccount + " AND debtor_acct = '" + request.DebtorAccount + "'", environment);
                    await Task.CompletedTask;
                }

            }
            catch (Exception e)
            {
                return _response.Response(false, false, e);
            }

            return _response.Response(true, true, "Successfully Set Dialing.");
        }
    }
}
