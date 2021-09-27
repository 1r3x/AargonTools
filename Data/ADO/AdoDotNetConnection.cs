using System;
using System.Data;
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
            catch (Exception)
            {
                // ignored
            }

            return objResult;

        }
    }
}
