using AargonTools.Data.ADO;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.SqlTypes;
using System.Threading.Tasks;

namespace AargonTools.Manager.GenericManager
{
    public class Sp_larry_cc_postdateV2
    {
        private readonly AdoDotNetConnection _adoConnection;
        public Sp_larry_cc_postdateV2(AdoDotNetConnection adoConnection)
        {
            _adoConnection = adoConnection;
        }

        public async Task PostDateCCProcess(string debtorAccount, DateTime postDate, double amount, string cardNoOrRef, string cvv,
            string expMonth, string expYear, int totalPd, string environment)
        {
            try
            {
                var exicutionResult = await _adoConnection.GetDataAsync(
                        "EXEC sp_larry_cc_postdateV2 " +
                        "@Debtor_Acct ='" + debtorAccount + "'," +
                        "@Post_Date ='" + postDate + "'," +
                        "@Amount ='" + amount + "'," +
                        "@Card_Num ='" + cardNoOrRef + "'," +
                        "@Card_CVV ='" + cvv + "'," +
                        "@Exp_Month ='" + expMonth + "'," +
                        "@Exp_Year ='" + expYear + "'," +
                        "@Total_PD ='" + totalPd + "';", environment);
                Serilog.Log.Information("sp_larry_cc_postdateV2 exicuted successfully.");
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("sp_larry_cc_postdateV2 exicuted with error : ", ex);
            }

        }


    }
}
