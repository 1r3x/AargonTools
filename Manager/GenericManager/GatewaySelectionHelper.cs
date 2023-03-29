using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;

namespace AargonTools.Manager.GenericManager
{
    public class GatewaySelectionHelper
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static CurrentBackupTestEnvironmentDbContext _currentTestEnvironment;
        public GatewaySelectionHelper(ExistingDataDbContext context, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld,
            CurrentBackupTestEnvironmentDbContext currentTestEnvironment)
        {
            _context = context;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _currentTestEnvironment = currentTestEnvironment;
        }


        public async Task<string> UniversalCcProcessGatewaySelectionHelper(string debtorAcct, string environment)
        {

            if (environment == "P")
            {
                var indexCount =
                    _context.LarryCcIndices.Count(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A");//bug length 0-4
                var gatewayName = "";
                if (indexCount == 0)
                {
                    return gatewayName;
                }


                if (debtorAcct[..4] == "4252")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4279")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4756")
                    gatewayName = "TMCBONHAMELAVON";
                gatewayName =
                    _context.LarryCcIndices.Where(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefault();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;






            }
            else if (environment == "PO")
            {
                var indexCount =
                    _contextProdOld.LarryCcIndices.Count(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A");
                var gatewayName = "";
                if (indexCount == 0)
                {
                    return gatewayName;
                }


                if (debtorAcct[..4] == "4252")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4279")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4756")
                    gatewayName = "TMCBONHAMELAVON";
                gatewayName =
                    _contextProdOld.LarryCcIndices.Where(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefault();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;
            }

            else if (environment == "CBT")
            {
                var clientAccout = debtorAcct.Trim()[..4];

                var indexCount =
                    _currentTestEnvironment.LarryCcIndices.Count(x => x.ClientAcct == clientAccout && x.AcctStatus == "A");
                var gatewayName = "";
                if (indexCount == 0)
                {
                    return gatewayName;
                }


                if (debtorAcct[..4] == "4252")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4279")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4756")
                    gatewayName = "TMCBONHAMELAVON";
                gatewayName =
                    _currentTestEnvironment.LarryCcIndices.Where(x => x.ClientAcct == clientAccout && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefault();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;
            }
            else
            {
                var fast4DigitFilter = debtorAcct.Substring(0, 5);

                var indexCount =
                    _contextTest.LarryCcIndices.Count(x => x.ClientAcct == fast4DigitFilter && x.AcctStatus == "A");
                var gatewayName = "";
                if (indexCount == 0)
                {
                    return gatewayName;
                }


                if (debtorAcct[..4] == "4252")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4279")
                    gatewayName = "GVALLEYH";
                if (debtorAcct[..4] == "4756")
                    gatewayName = "TMCBONHAMELAVON";
                gatewayName =
                    _contextTest.LarryCcIndices.Where(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefault();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;
            }
        }




    }
}
