using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountInformationController : ControllerBase
    {
        private readonly IGetAccountInformation _context;
        private readonly IGetInteractionsAcctData _contextGetInteractionsAcctData;

        public AccountInformationController(IGetAccountInformation context, IGetInteractionsAcctData contextGetInteractionsAcctData)
        {
            _context = context;
            _contextGetInteractionsAcctData = contextGetInteractionsAcctData;
        }




        /// <summary>
        ///  Returns the balance of a debtor account (Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Get the balance of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with any API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountBalance/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        /// 
        /// **GET Table/Fields Details**
        /// 
        /// [According to flag]
        /// client_master ,
        /// 
        /// client_master_d,
        /// 
        /// client_master_h,
        /// 
        /// client_master_l,
        /// 
        /// client_master_t,
        /// 
        /// client_master_w
        /// 
        /// Pull--> balance 
        /// </remarks>
        ///
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///

        [HttpGet("GetAccountBalance/{debtorAcct}")]
        [ProducesResponseType(typeof(GetAccountBalanceResponseModel), 200)]
        public async Task<IActionResult> GetAccountBalance(string debtorAcct)
        {
            Serilog.Log.Information("Entering GetAccountBalance => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                var item = await _context.GetAccountBalanceByDebtorAccount(debtorAcct, "P");

                Serilog.Log.Information("Successfully retrieved account balance for debtorAcct: {DebtorAcct}", debtorAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving account balance for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetAccountBalance => GET with debtorAcct: {DebtorAcct}", debtorAcct);

            }
        }


        /// <summary>
        ///  Returns account validity of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Check the validity of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountValidity/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [ProducesResponseType(typeof(GetAccountValidityResponseModel), 200)]
        [HttpGet("GetAccountValidity/{debtorAcct}")]
        public IActionResult GetAccountValidity(string debtorAcct)
        {

            Serilog.Log.Information("Entering GetAccountValidity => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            try
            {
                //P for prod.
                var item = _context.CheckAccountValidityByDebtorAccount(debtorAcct, "P");
                Serilog.Log.Information("Successfully checked account validity for debtorAcct: {DebtorAcct}", debtorAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error checking account validity for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetAccountValidity => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            }

        }

        /// <summary>
        ///  Returns account existences of a debtor account.(Prod.)
        /// </summary>
        ///
        /// <remarks>
        /// 
        /// **Details**:
        /// Get the existences of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountExistences/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// **GET Table/Fields Details**
        /// 
        /// [According to flag]
        /// 
        /// client_master ,
        /// 
        /// client_master_d,
        /// 
        /// client_master_h,
        /// 
        /// client_master_l,
        /// 
        /// client_master_t,
        /// 
        /// client_master_w
        /// 
        /// Pull--> If exist then return company flag  
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [HttpGet("GetAccountExistences/{debtorAcct}")]
        [ProducesResponseType(typeof(GetAccountExistenceResponseModel), 200)]
        public async Task<IActionResult> GetAccountExistences(string debtorAcct)
        {
            Serilog.Log.Information("Entering GetAccountExistences => GET with debtorAcct: {DebtorAcct}", debtorAcct);

            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                //P for prod.
                var item = await _context.CheckAccountExistenceByDebtorAccount(debtorAcct, "P");
                Serilog.Log.Information("Successfully checked account existence for debtorAcct: {DebtorAcct}", debtorAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error checking account existence for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetAccountExistences => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            }

        }


        /// <summary>
        ///  Returns recent credit card approval of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets any recent credit card approval of the debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetRecentApproval/1850-190058
        /// 
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        /// 
        ///  **GET Table/Fields Details**
        /// 
        /// cc_payment
        /// 
        /// Pull--> if have any payment within 5 min then return true by checking db fieled  payment_date and approval_status
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetRecentApprovalResponseModel), 200)]
        [HttpGet("GetRecentApproval/{debtorAcct}")]
        public async Task<IActionResult> GetRecentApproval(string debtorAcct)
        {

            Serilog.Log.Information("Entering GetRecentApproval => GET with debtorAcct: {DebtorAcct}", debtorAcct);

            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }


            try
            {
                //P for prod.
                var item = await _context.GetRecentApprovalByDebtorAccount(debtorAcct, "P");
                Serilog.Log.Information("Successfully retrieved recent approval for debtorAcct: {DebtorAcct}", debtorAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving recent approval for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetRecentApproval => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            }

        }


        /// <summary>
        ///  Returns  Multiples account and it's balance from a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets multiples account and it's balance of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/AccountInformation/GetMultiples/1850-190058
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        ///  **GET Table/Fields Details**
        /// 
        /// debtor_multiples
        /// 
        /// Pull--> debtor_acct,debtor_acct2
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        //[ProducesResponseType(typeof(GetMultiplesResponseModel), 200)]
        //[HttpGet("GetMultiples/{debtorAcct}")]
        //public async Task<IActionResult> GetMultiples(string debtorAcct)
        //{

        //    Serilog.Log.Information("Entering GetMultiples => GET with debtorAcct: {DebtorAcct}", debtorAcct);

        //    // Validate debtor account format
        //    var regex = new Regex(@"^\d{4}-\d{6}$");
        //    if (!regex.IsMatch(debtorAcct))
        //    {
        //        Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
        //        return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
        //    }

        //    try
        //    {
        //        //P for prod.
        //        var item = await _context.GetMultiples(debtorAcct, "P");
        //        Serilog.Log.Information("Successfully retrieved multiples for debtorAcct: {DebtorAcct}", debtorAcct);
        //        return Ok(item);
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Error(e, "Error retrieving multiples for debtorAcct: {DebtorAcct}", debtorAcct);
        //        throw;
        //    }
        //    finally
        //    {
        //        Serilog.Log.Information("Exiting GetMultiples => GET with debtorAcct: {DebtorAcct}", debtorAcct);
        //    }

        //}

        [ProducesResponseType(typeof(GetMultiplesResponseModel), 200)]
        [HttpGet("GetMultiples/{debtorAcct}")]
        public async Task<IActionResult> GetMultiples(string debtorAcct)
        {
            Serilog.Log.Information("Entering GetMultiples => GET with debtorAcct: {DebtorAcct}", debtorAcct);

            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            const int maxRetryCount = 3;
            int retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                try
                {
                    //P for prod.
                    var item = await _context.GetMultiples(debtorAcct, "P");
                    Serilog.Log.Information("Successfully retrieved multiples for debtorAcct: {DebtorAcct}", debtorAcct);
                    return Ok(item);
                }
                catch (SqlException ex) when (ex.Number == 1205) // Deadlock
                {
                    retryCount++;
                    Serilog.Log.Warning("Deadlock encountered, retrying... Attempt {RetryCount}", retryCount);
                    if (retryCount >= maxRetryCount)
                    {
                        Serilog.Log.Error(ex, "Max retry attempts reached for debtorAcct: {DebtorAcct}", debtorAcct);
                        return StatusCode(500, "A deadlock occurred. Please try again later.");
                    }
                }
                catch (Exception e)
                {
                    Serilog.Log.Error(e, "Error retrieving multiples for debtorAcct: {DebtorAcct}", debtorAcct);
                    throw;
                }
                finally
                {
                    Serilog.Log.Information("Exiting GetMultiples => GET with debtorAcct: {DebtorAcct}", debtorAcct);
                }
            }

            return StatusCode(500, "An unexpected error occurred.");
        }



        /// <summary>
        ///  Returns account Info of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets account's Info of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetAccountInfo/1850-190058
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        ///  **GET Table/Fields Details**
        /// 
        /// According to flag  : 
        /// debtor_acct_info,
        /// 
        /// debtor_acct_info_d,
        /// 
        /// debtor_acct_info_h,
        /// 
        /// debtor_acct_info_l,
        /// 
        /// debtor_acct_info_t,
        /// 
        /// debtor_acct_info_w 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetAccountInfoResponse), 200)]
        [HttpGet("GetAccountInfo/{debtorAcct}")]
        public async Task<IActionResult> GetAccountInfo(string debtorAcct)
        {
            Serilog.Log.Information("Entering GetAccountInfo => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                //P for prod.
                var item = await _context.GetAccountInfo(debtorAcct, "P");
                Serilog.Log.Information("Successfully retrieved account info for debtorAcct: {DebtorAcct}", debtorAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving account info for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetAccountInfo => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            }

        }

        /// <summary>
        ///  Returns the SIF of a debtor account.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets SIF of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/AccountInformation/GetSIF/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        ///  **GET Table/Fields Details**
        /// 
        /// debtor_multiples
        /// 
        /// Pull--> debtor_acct,debtor_acct2
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetSifResponse), 200)]
        [HttpGet("GetSIF/{debtorAcct}")]
        public async Task<IActionResult> GetSIF(string debtorAcct)
        {
            Serilog.Log.Information("Entering GetSIF => GET with debtorAcct: {DebtorAcct}", debtorAcct);

            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            try
            {
                //P for prod.
                var item = await _context.GetSIF(debtorAcct, "P");
                Serilog.Log.Information("Successfully retrieved SIF for debtorAcct: {DebtorAcct}", debtorAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving SIF for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetSIF => GET with debtorAcct: {DebtorAcct}", debtorAcct);
            }

        }



        /// <summary>
        ///  Returns Interactions Account Data.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the  interactions account data by passing JSON body like the request example. All parameters are not required
        /// but one
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetInteractionsAcctData
        /// and it's POST request for getting the data.
        ///
        /// **Details**:
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for phone [@"\d{10}"] ex. 2123037334,
        /// please don't forget about valid token.
        ///  **GET Table/Fields Details**
        ///  debtor_phone_info,
        ///  
        ///  check_detail,
        ///  
        ///  larry_cc_payments,
        ///  
        ///  debtor_pp_info,
        ///  
        ///  debtor_acct_info(flag),
        ///  
        ///  debtor_master(flag),
        ///  
        ///  client_master(flag),
        ///  
        ///  client_acct_info(flag)
        ///  
        /// 
        /// Pull--> 
        /// debtor_phone_info->home_area_code,home_phone,work_area_code,work_phone,cell_area_code,cell_phone,other_area_code,other_phone,
        /// relative_area_code,relative_phone,debtor_acct
        /// 
        /// check_detail->check_date
        /// 
        /// larry_cc_payments->date_process
        /// 
        /// debtor_pp_info->pp_date1
        /// 
        /// debtor_acct_info(flag)->debtor_acct,balance,email_address,acct_status
        /// 
        /// debtor_master(flag)->ssn,address1,address2,city,state_code,zip,birth_date,first_name,last_name,
        /// 
        /// client_master(flag)->client_name
        /// 
        /// client_acct_info(flag)->acct_type
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        //[ProducesResponseType(typeof(GetInteractionAcctDataExample), 200)]
        //[HttpPost("GetInteractionsAcctData")]
        //public async Task<IActionResult> GetInteractionsAcctData([FromBody] GetInteractionAcctDateRequestModel request)
        //{
        //    Serilog.Log.Information("GetInteractionsAcctData => GET");
        //    try
        //    {
        //        //P for prod.

        //        //original
        //        var item = await _contextGetInteractionsAcctData.GetInteractionsAcctData(request, "P");

        //        return Ok(item);
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Error(e.InnerException, e.Message);
        //        throw;
        //    }

        //}


        /// <summary>
        ///  Returns Interactions Account Data.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the  interactions account data by passing JSON body like the request example. All parameters are not required
        /// but one
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetInteractionsAcctData
        /// and it's POST request for getting the data.
        ///
        /// **Details**:
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for phone [@"\d{10}"] ex. 2123037334,
        /// please don't forget about valid token.
        ///  **GET Table/Fields Details**
        ///  debtor_phone_info,
        ///  
        ///  check_detail,
        ///  
        ///  larry_cc_payments,
        ///  
        ///  debtor_pp_info,
        ///  
        ///  debtor_acct_info(flag),
        ///  
        ///  debtor_master(flag),
        ///  
        ///  client_master(flag),
        ///  
        ///  client_acct_info(flag)
        ///  
        /// 
        /// Pull--> 
        /// debtor_phone_info->home_area_code,home_phone,work_area_code,work_phone,cell_area_code,cell_phone,other_area_code,other_phone,
        /// relative_area_code,relative_phone,debtor_acct
        /// 
        /// check_detail->check_date
        /// 
        /// larry_cc_payments->date_process
        /// 
        /// debtor_pp_info->pp_date1
        /// 
        /// debtor_acct_info(flag)->debtor_acct,balance,email_address,acct_status
        /// 
        /// debtor_master(flag)->ssn,address1,address2,city,state_code,zip,birth_date,first_name,last_name,
        /// 
        /// client_master(flag)->client_name
        /// 
        /// client_acct_info(flag)->acct_type
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetInteractionAcctDataExample), 200)]
        [HttpPost("GetInteractionsAcctData")]
        public async Task<IActionResult> GetInteractionsAcctData([FromBody] GetInteractionAcctDateRequestModel request)
        {


            Serilog.Log.Information("Entering GetInteractionsAcctData => POST with request: {@Request}", request);

            // Check if the client is still connected
            if (HttpContext.RequestAborted.IsCancellationRequested)
            {
                Serilog.Log.Warning("Client disconnected before processing");
                return StatusCode(StatusCodes.Status408RequestTimeout);
            }


            try
            {
                //P for prod.

                var item = await _contextGetInteractionsAcctData.GetInteractionsAcctDataSpeedRun(request, "P");

                Serilog.Log.Information("Successfully retrieved interactions account data for request: {@Request} ", request);

                return Ok(item);
            }

            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving interactions account data for request: {@Request}", request);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetInteractionsAcctData => POST with request: {@Request}", request);
            }

        }


        /// <summary>
        ///  Returns client invoice header.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client invoice header data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetClientInvoiceHeader/0001&amp;A
        /// and it's GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}"] ex. 0001, for company ex. A,
        /// Required a valid token.
        /// 
        ///  **GET Table/Fields Details**
        ///  
        /// client_acct_info(flag),client_master(flag)
        /// 
        /// Pull--> 
        /// client_acct(flag)->client_acct
        /// 
        /// client_master(flag)->orig_creditor,remit_full_pmt,address12,address22,city2,state_code2,zip2
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetClientInvoiceHeaderExample), 200)]
        [HttpGet("GetClientInvoiceHeader/{clientAcct}&{company}")]
        public async Task<IActionResult> GetClientInvoiceHeader(string clientAcct, string company)
        {
            Serilog.Log.Information("Entering GetClientInvoiceHeader => GET with clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
            try
            {
                //P for prod.
                var item = await _context.GetClientInvoiceHeader(clientAcct, company, "P");
                Serilog.Log.Information("Successfully retrieved client invoice header for clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving client invoice header for clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetClientInvoiceHeader => GET with clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
            }
        }


        /// <summary>
        ///  Returns Client Primary Contact.(prod)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client primary contact data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetClientPrimaryContact/0001&amp;A
        /// and it's GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}"] ex. 0001, for company ex. A,
        /// Required a valid token.
        /// 
        ///  **GET Table/Fields Details**
        ///  
        /// contact_master(flag)
        /// 
        /// Pull--> first_name,last_name
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetClientPrimaryContactExample), 200)]
        [HttpGet("GetClientPrimaryContact/{clientAcct}&{company}")]
        public async Task<IActionResult> GetClientPrimaryContact(string clientAcct, string company)
        {

            Serilog.Log.Information("Entering GetClientPrimaryContact => GET with clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);

            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(clientAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", clientAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            try
            {
                //P for prod.
                var item = await _context.GetClientPrimaryContact(clientAcct, company, "P");
                Serilog.Log.Information("Successfully retrieved client primary contact for clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving client primary contact for clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetClientPrimaryContact => GET with clientAcct: {ClientAcct}, company: {Company}", clientAcct, company);
            }

        }

        /// <summary>
        ///  Can Get Client Invoice Payments.(prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can Get Client Invoice Payments.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/AccountInformation/GetClientInvoicePayments
        /// (pass JSON body like the request example)
        ///  A valid token is required for setting the data.
        ///  **GET Table/Fields Details**
        /// 
        /// debtor_acct_info(flag),
        /// 
        /// debtor_master(flag),
        /// 
        /// debtor_payment_master
        /// 
        /// Pull--> 
        /// debtor_acct_info(flag)->debtor_acct,supplied_acct,date_of_service,date_placed,first_name,last_name,client_amt,agency_amt_decl,fee_pct,tran_date,cosigner_last_name
        /// 
        /// debtor_master(flag)->debtor_payment_master->balance,status_code,payment_type,agency_amt_decl,client_amt,remit_full_pmt,payment_type
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(GetClientInvoicePaymentsResponseExample), 200)]
        [HttpPost("GetClientInvoicePayments")]
        public async Task<IActionResult> GetClientInvoicePayments([FromBody] GetClientInvoiceRequestModel request)
        {
            Serilog.Log.Information("Entering GetClientInvoicePayments => POST with request: {@Request}", request);
            try
            {
                //P for prod.
                var item = await _context.GetClientInvoicePayments(request, "P");
                Serilog.Log.Information("Successfully retrieved client invoice payments for request: {@Request}", request);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving client invoice payments for request: {@Request}", request);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetClientInvoicePayments => POST with request: {@Request}", request);
            }
        }


        /// <summary>
        ///  Returns Next Payment Info.(prod)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client's new payment info by passing parameterized clientAcct like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/AccountInformation/GetNextPaymentInfo/0001-000001
        /// and it's a GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for company ex. A,
        /// Required a valid token.
        ///  **GET Table/Fields Details**
        /// 
        /// debtor_master_(flag),
        /// 
        /// debtor_acct_info_(flag),
        /// 
        /// client_master_(flag),
        /// 
        /// client_acct_info_(flag),
        /// 
        /// debtor_phone_info,
        /// 
        /// debtor_pp_info,
        /// 
        /// check_detail,
        /// 
        /// larry_cc_payments
        /// 
        /// 
        /// Pull--> 
        /// debtor_master_(flag) ->first_name,last_name,ssn,address1,city,state_code,zip,birth_date,
        /// 
        /// debtor_acct_info_(flag)->balance,payment_amt_life,date_of_service,last_payment_amt,begin_age_date
        /// 
        /// client_master_(flag)->client_name,client_desc,
        /// 
        /// client_acct_info_(flag)->charge_interest,report_to_bureau,
        /// 
        /// debtor_phone_info->home_area_code,home_phone,home_phone_ver,home_phone_dont_call,cell_area_code,cell_phone,cell_phone_ver,cell_phone_dont_call
        /// 
        /// debtor_pp_info->pp_amount1,pp_date1,
        /// 
        /// check_detail->check_amt,check_date
        /// 
        /// larry_cc_payments->total,date_process
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetNextPaymentInfoExample), 200)]
        [HttpGet("GetNextPaymentInfo/{clientAcct}")]
        public async Task<IActionResult> GetNextPaymentInfo(string clientAcct)
        {
            Serilog.Log.Information("Entering GetNextPaymentInfo => GET with clientAcct: {ClientAcct}", clientAcct);

            // Validate debtor account format
            var regex = new Regex(@"^\d{4}-\d{6}$");
            if (!regex.IsMatch(clientAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", clientAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            try
            {
                //P for prod.
                var item = await _context.GetNextPaymentInfo(clientAcct, "P");
                Serilog.Log.Information("Successfully retrieved next payment info for clientAcct: {ClientAcct}", clientAcct);
                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error retrieving next payment info for clientAcct: {ClientAcct}", clientAcct);
                throw;
            }
            finally
            {
                Serilog.Log.Information("Exiting GetNextPaymentInfo => GET with clientAcct: {ClientAcct}", clientAcct);
            }

        }



    }
}
