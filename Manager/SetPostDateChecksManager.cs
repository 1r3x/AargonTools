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
    public class SetPostDateChecksManager : ISetPostDateChecks
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;

        public SetPostDateChecksManager(ExistingDataDbContext context, ResponseModel response,
            TestEnvironmentDbContext contextTest, IAddNotes addNotes, AdoDotNetConnection adoConnection)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _adoConnection = adoConnection;
        }

        public async Task<ResponseModel> SetPostDateChecks(SetPostDateChecksRequestModel request,string environment)
        {
            var rawAdo = _adoConnection.GetData("DECLARE @return_value int EXEC " +
                                               "@return_value = [dbo].[sp_larry_check_postdate]" +
                                                "@Debtor_Acct = N'" + request.debtorAcct + "'," +
                                                "@Post_Date = N'" + request.postDate + "'," +
                                                "@Amount = " + request.amount + "," +
                                                "@Account_Number = N'" + request.accountNumber + "'," +
                                                "@Routing_Number = N'" + request.routingNumber + "'," +
                                                "@Total_PD = " + request.totalPd + "," +
                                                "@SIF = N'" + request.sif + "'" +
                                                "SELECT  'Return Value' = @return_value;", environment);
            await Task.CompletedTask;

            if (Convert.ToDecimal(rawAdo.Rows[0]["Return Value"]) == 0)
            {
                return _response.Response(true, true, "Successfully Set Post Date Checks");
            }
            else
            {
                return _response.Response(true, false, "Oops Something went wrong.");
            }


        }
    }
}
