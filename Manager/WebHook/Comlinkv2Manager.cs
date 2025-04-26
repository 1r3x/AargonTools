using AargonTools.Data.ADO;
using AargonTools.Interfaces.Email;
using AargonTools.Interfaces.WebHook;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.WebHook;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AargonTools.Manager.WebHook
{
    public class Comlinkv2Manager : IComlinkv2
    {
        private readonly AdoDotNetConnection _adoConnection;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public Comlinkv2Manager(AdoDotNetConnection adoConnection, IConfiguration configuration, IEmailService emailService)
        {
            _adoConnection = adoConnection;
            _configuration = configuration;
            _emailService = emailService;
        }


        private const string Z = ".5.1"; // Equivalent of the VB Z variable

        private string IsValidTextRequest(TextRequestModel request, string clientIp)
        {
            var validationErrors = new StringBuilder();

            // Validate IP address
            if (!IsValidIp(clientIp))
            {
                validationErrors.AppendLine($"INVALID IP {clientIp}");
            }

            // Only proceed with other validations if IP is valid
            if (validationErrors.Length == 0)
            {
                ValidateMsisdn(request.Msisdn, validationErrors);
                ValidateTo(request.To, validationErrors);
                ValidateMessageId(request.MessageId, validationErrors);
                ValidateType(request.Type, validationErrors);
                ValidateKeyword(request.Keyword, validationErrors);
                ValidateTimestamp(request.MessageTimestamp, validationErrors);
            }

            // Check if all validations passed
            bool allValidationsPassed = validationErrors.Length == 0;

            return allValidationsPassed ? string.Empty : validationErrors.ToString();
        }

        private void ValidateMsisdn(string msisdn, StringBuilder validationErrors)
        {
            if (string.IsNullOrEmpty(msisdn))
            {
                validationErrors.AppendLine("MSISDN MISSING");
            }
            else if (msisdn.Length < 10 || msisdn.Length > 11)
            {
                validationErrors.AppendLine("MSISDN LENGTH");
            }
            else if (!long.TryParse(msisdn, out _))
            {
                validationErrors.AppendLine("MSISDN NUMERIC");
            }
        }

        private void ValidateTo(string to, StringBuilder validationErrors)
        {
            if (string.IsNullOrEmpty(to))
            {
                validationErrors.AppendLine("TO MISSING");
            }
            else if (to.Length < 10 || to.Length > 11)
            {
                validationErrors.AppendLine("TO LENGTH");
            }
            else if (!long.TryParse(to, out _))
            {
                validationErrors.AppendLine("TO NUMERIC");
            }
        }

        private void ValidateMessageId(string messageId, StringBuilder validationErrors)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                validationErrors.AppendLine("MESSAGEID MISSING");
            }
        }

        private void ValidateType(string type, StringBuilder validationErrors)
        {
            if (string.IsNullOrEmpty(type))
            {
                validationErrors.AppendLine("TYPE MISSING");
            }
            else if (!type.Equals("text", StringComparison.OrdinalIgnoreCase) &&
                     !type.Equals("unicode", StringComparison.OrdinalIgnoreCase))
            {
                validationErrors.AppendLine("TYPE INVALID");
            }
        }

        private void ValidateKeyword(string keyword, StringBuilder validationErrors)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                validationErrors.AppendLine("KEYWORD MISSING");
            }
        }

        private void ValidateTimestamp(string timestamp, StringBuilder validationErrors)
        {
            if (string.IsNullOrEmpty(timestamp))
            {
                validationErrors.AppendLine("MESSAGE-TIMESTAMP MISSING");
            }
        }

        private bool IsValidIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            return ip.StartsWith("174.36.") ||
                   ip.StartsWith("174.37.") ||
                   ip.StartsWith("173.193.") ||
                   ip.StartsWith("119.81.") ||
                   ip.StartsWith("192.168") ||
                   ip.StartsWith("216.147.") ||
                   ip.StartsWith("169.63.");
        }

        //
        //-----------------------
        //

        public async Task<ProcessSmsResult> ProcessInboundSms(TextRequestModel request, string clientIp, string requestUrl)
        {
            // Validate request
            string invalidMsg = IsValidTextRequest(request, clientIp);
            if (!string.IsNullOrEmpty(invalidMsg))
            {
                Serilog.Log.Debug("Entering Comlinkv2 => Validation : {@invalidMsg}", invalidMsg);
                await _emailService.SendEmailAsync("akib@aargon.com", "Web", invalidMsg);
                return new ProcessSmsResult { Success = false, Message = invalidMsg };
            }

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                //using (var connection = new SqlConnection(_configuration.GetConnectionString("TestEnvironmentConnection")))
                {
                    await connection.OpenAsync();

                    // Log URL
                    await LogRequestUrlAsync(connection, requestUrl);

                    // Process phone numbers
                    string senderPhone = S(request.Msisdn.Substring(1, 10));
                    string tollFreeNumber = S(request.To.Substring(1, 10));

                    // Look up sender information
                    var (app, account) = await ResolveSenderInfoAsync(connection, senderPhone, tollFreeNumber);

                    // Store SMS in database
                    await StoreSmsMessageAsync(connection, request, app, account, senderPhone, tollFreeNumber);
                    Serilog.Log.Information("SMS processed successfully");
                    return new ProcessSmsResult { Success = true, Message = "SMS processed successfully" };
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Information("Error processing SMS :{@ex}",ex);
                return new ProcessSmsResult { Success = false, Message = "Internal server error" };
            }
        }

        private async Task LogRequestUrlAsync(SqlConnection connection, string url)
        {
            string sanitizedUrl = S(url);
            string sql = "INSERT INTO larry_sms_url (tran_date, url) VALUES (GETDATE(), @url)";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@url", sanitizedUrl);
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task<(string app, string account)> ResolveSenderInfoAsync(SqlConnection connection, string senderPhone, string tollFreeNumber)
        {
            // First try to get info from apex_sms_did
            string sql = @"
            SELECT company 
            FROM apex_sms_did 
            WHERE area_code = @areaCode 
            AND phone_num = @phoneNumber";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@areaCode", tollFreeNumber.Substring(0, 3));
                command.Parameters.AddWithValue("@phoneNumber", tollFreeNumber.Substring(3, 7));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string company = reader["company"].ToString();
                        return ($"SMSBLAST{company}", null);
                    }
                }
            }

            // Fall back to larry_sms_index
            sql = @"
            SELECT active_app, debtor_acct 
            FROM larry_sms_index 
            WHERE area_code = @areaCode 
            AND phone_num = @phoneNumber";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@areaCode", senderPhone.Substring(0, 3));
                command.Parameters.AddWithValue("@phoneNumber", senderPhone.Substring(3, 7));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        string app = reader["active_app"] is DBNull ? "" : reader["active_app"].ToString();
                        string account = reader["debtor_acct"] is DBNull ? "" : reader["debtor_acct"].ToString();
                        return (app, account);
                    }
                }
            }

            return ("", "");
        }

        private async Task StoreSmsMessageAsync(SqlConnection connection, TextRequestModel request, string app, string account,
                                             string senderPhone, string tollFreeNumber)
        {
            string sql = @"
            INSERT INTO larry_sms_master (
                direction, message_id, app, app_variable, debtor_acct,
                from_number, to_number, request_date, sms_date, sms,
                price, network, status, vendor, test_mode, completed
            ) VALUES (
                'I', @messageId, @app, '', @account,
                @senderPhone, @tollFreeNumber, GETDATE(), GETDATE(), @text,
                0, '', '', 'NEXMO', 'N', 'N'
            )";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@messageId", S(request.MessageId));
                command.Parameters.AddWithValue("@app", string.IsNullOrEmpty(app) ? DBNull.Value : (object)app);
                command.Parameters.AddWithValue("@account", string.IsNullOrEmpty(account) ? DBNull.Value : (object)account);
                command.Parameters.AddWithValue("@senderPhone", senderPhone);
                command.Parameters.AddWithValue("@tollFreeNumber", tollFreeNumber);
                command.Parameters.AddWithValue("@text", S(request.Text));

                await command.ExecuteNonQueryAsync();
            }
        }

        // Equivalent of the VB S() function with Z variable support
        private string S(string value)
        {
            if (value == null)
                return string.Empty;

            // Remove SQL comments
            while (value.Contains("--"))
            {
                value = value.Replace("--", "");
            }

            // Remove single quotes
            value = value.Replace("'", "");

            // Handle the Z variable case (from original VB code)
            if (value.StartsWith("192.168"))
            {
                value = value.Replace("192.168", $"192.168{Z}");
            }

            return value.Trim();
        }


    }


    // DTO for the result
    public class ProcessSmsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }


}


