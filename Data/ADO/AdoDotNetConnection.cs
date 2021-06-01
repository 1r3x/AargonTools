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


        public DataTable GetData(string cmdText)
        {
            var objResult = new DataTable();
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var myCon = new SqlConnection(connectionString);
                using var myCommand = new SqlCommand(cmdText, myCon);
                var da = new SqlDataAdapter(myCommand);
                da.Fill(objResult);
            }
            catch (Exception)
            {
                // ignored
            }

            return objResult;

        }
    }
}
