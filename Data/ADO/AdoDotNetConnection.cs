using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AargonTools.Data.ADO
{
    public class AdoDotNetConnection
    {
        private readonly IConfiguration _configuration;
        public AdoDotNetConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public DataTable GetData(string cmdText, string environment)
        {
            var objResult = new DataTable();
            try
            {
                if (environment == "P")
                {
                    var connectionString = _configuration.GetConnectionString("DefaultConnection");
                    using var myCon = new SqlConnection(connectionString);
                    using var myCommand = new SqlCommand(cmdText, myCon);
                    var da = new SqlDataAdapter(myCommand);
                    da.Fill(objResult);
                }
                else if (environment=="PO")
                {
                    var connectionString = _configuration.GetConnectionString("ProdOldConnection");
                    using var myCon = new SqlConnection(connectionString);
                    using var myCommand = new SqlCommand(cmdText, myCon);
                    var da = new SqlDataAdapter(myCommand);
                    da.Fill(objResult);
                }
                else
                {
                    var connectionString = _configuration.GetConnectionString("TestEnvironmentConnection");
                    using var myCon = new SqlConnection(connectionString);
                    using var myCommand = new SqlCommand(cmdText, myCon);
                    var da = new SqlDataAdapter(myCommand);
                    da.Fill(objResult);
                }
            }
            catch (Exception e)
            {
                var errorForDebugging=e;
            }

            return objResult;

        }


        public async Task<DataTable> GetDataAsync(string cmdText, string environment)
        {
            var objResult = new DataTable();
            try
            {
                string connectionString;
                if (environment == "P")
                {
                    connectionString = _configuration.GetConnectionString("DefaultConnection");
                }
                else if (environment == "PO")
                {
                    connectionString = _configuration.GetConnectionString("ProdOldConnection");
                }
                else
                {
                    connectionString = _configuration.GetConnectionString("TestEnvironmentConnection");
                }

                using var myCon = new SqlConnection(connectionString);
                using var myCommand = new SqlCommand(cmdText, myCon);
                var da = new SqlDataAdapter(myCommand);

                await myCon.OpenAsync();
                await Task.Run(() => da.Fill(objResult));
            }
            catch (Exception e)
            {
                var errorForDebugging = e;
            }

            return objResult;
        }

    }
}
