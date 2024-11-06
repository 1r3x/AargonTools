using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager.GenericManager
{
    public class GatewaySelectionHelper
    {
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private readonly CurrentBackupTestEnvironmentDbContext _currentTestEnvironment;
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
                var indexCount = await
                    _context.LarryCcIndices.CountAsync(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A");//bug length 0-4
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
                gatewayName = await
                    _context.LarryCcIndices.Where(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefaultAsync();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;






            }
            else if (environment == "PO")
            {
                var indexCount = await
                    _contextProdOld.LarryCcIndices.CountAsync(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A");
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
                gatewayName = await
                    _contextProdOld.LarryCcIndices.Where(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefaultAsync();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;
            }

            else if (environment == "CBT")
            {
                var clientAccout = debtorAcct.Trim()[..4];

                var indexCount = await
                    _currentTestEnvironment.LarryCcIndices.CountAsync(x => x.ClientAcct == clientAccout && x.AcctStatus == "A");
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
                gatewayName = await
                    _currentTestEnvironment.LarryCcIndices.Where(x => x.ClientAcct == clientAccout && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefaultAsync();
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
                gatewayName = await
                    _contextTest.LarryCcIndices.Where(x => x.ClientAcct == debtorAcct.Substring(0, 4) && x.AcctStatus == "A").Select(s => s.Gateway).SingleOrDefaultAsync();
                return gatewayName == "UHSSECURE" ? "ELAVON" : gatewayName;
            }
        }

        //according to suffix employee no 
        public async Task<int> CcProcessEmployeeNumberAccordingToFlag(string debtorAcct, string environment)
        {
            var account = debtorAcct.Substring(0, 4);
            if (environment == "P")
            {
                if (await _context.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 1950;//National (no suffix): 1950
                }
                else if (await _context.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 71950;//Utilities (_D): 71950
                }
                else if (await _context.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 41950;//Hawaii (_H): 41950
                }
                else if (await _context.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 61950;//Local (_L): 61950
                }
                else if (await _context.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 31950;//TCR (_T): 31950
                }
                else if (await _context.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 51950;//Wamu (_W): 51950
                }
                else if (await _context.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 81950;//Purchased Debt (_P): 81950
                }
                else
                {
                    return 0;
                }
            }
            else if (environment == "PO")
            {
                if (await _contextProdOld.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 1950;//National (no suffix): 1950
                }
                else if (await _contextProdOld.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 71950;//Utilities (_D): 71950
                }
                else if (await _contextProdOld.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 41950;//Hawaii (_H): 41950
                }
                else if (await _contextProdOld.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 61950;//Local (_L): 61950
                }
                else if (await _contextProdOld.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 31950;//TCR (_T): 31950
                }
                else if (await _contextProdOld.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 51950;//Wamu (_W): 51950
                }
                else if (await _contextProdOld.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 81950;//Purchased Debt (_P): 81950
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (await _contextTest.ClientMasters.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 1950;//National (no suffix): 1950
                }
                else if (await _contextTest.ClientMasterDs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 71950;//Utilities (_D): 71950
                }
                else if (await _contextTest.ClientMasterHs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 41950;//Hawaii (_H): 41950
                }
                else if (await _contextTest.ClientMasterLs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 61950;//Local (_L): 61950
                }
                else if (await _contextTest.ClientMasterTs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 31950;//TCR (_T): 31950
                }
                else if (await _contextTest.ClientMasterWs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 51950;//Wamu (_W): 51950
                }
                else if (await _contextTest.ClientMasterPs.Where(x => x.ClientAcct == account).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    return 81950;//Purchased Debt (_P): 81950
                }
                else
                {
                    return 0;
                }
            }
        }



    }
}
