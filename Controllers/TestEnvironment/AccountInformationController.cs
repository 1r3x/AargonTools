using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Request;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers.TestEnvironment
{
    [Route("api/Test/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountInformationController : ControllerBase
    {
        private readonly IGetAccountInformation _context;

        public AccountInformationController(IGetAccountInformation context)
        {
            _context = context;
        }
        /// <summary>
        ///  Returns the balance of a debtor account (Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Get the balance of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with any API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountBalance/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
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
        /// Pull--> balance 
        /// </remarks>
        /// 
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///
        [ProducesResponseType(typeof(GetAccountBalanceResponseModel), 200)]
        [HttpGet("GetAccountBalance/{debtorAcct}")]
        public async Task<IActionResult> GetAccountBalance(string debtorAcct)
        {
            Serilog.Log.Information("Test GetAccountBalance => GET");
            try
            {
                //T for test.
                var item = await _context.GetAccountBalanceByDebtorAccount(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns account validity of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Check the validity of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountValidity/0001-000001
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
            Serilog.Log.Information("Test GetAccountValidity => GET");
            try
            {
                //T for test.
                var item = _context.CheckAccountValidityByDebtorAccount(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns account existences of a debtor account.(Test.)
        /// </summary>
        ///
        /// <remarks>
        /// 
        /// **Details**:
        /// Get the existences of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountExistences/0001-000001
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
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
        [ProducesResponseType(typeof(GetAccountExistenceResponseModel), 200)]
        [HttpGet("GetAccountExistences/{debtorAcct}")]
        public async Task<IActionResult> GetAccountExistences(string debtorAcct)
        {
            Serilog.Log.Information("Test GetAccountExistences => GET");
            try
            {
                //T for test.
                var item = await _context.CheckAccountExistenceByDebtorAccount(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns recent credit card approval of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets any recent credit card approval of the debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetRecentApproval/1850-190058
        /// 
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
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
            Serilog.Log.Information("Test GetRecentApproval => GET");
            try
            {
                //T for test.
                var item = await _context.GetRecentApprovalByDebtorAccount(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }


        /// <summary>
        ///  Returns  Multiples account and it's balance from a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets multiples account and it's balance of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the parameter with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetMultiples/1850-190058
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        ///  **GET Table/Fields Details**
        /// 
        /// debtor_multiples
        /// 
        /// Pull--> debtor_acct,debtor_acct2
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetMultiplesResponseModel), 200)]
        [HttpGet("GetMultiples/{debtorAcct}")]
        public async Task<IActionResult> GetMultiples(string debtorAcct)
        {
            Serilog.Log.Information("Test GetMultiples => GET");
            try
            {
                //T for test.
                var item = await _context.GetMultiples(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }
        /// <summary>
        ///  Returns account Info of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets account's Info of any debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetAccountInfo/1850-190058
        ///
        /// **Notes**
        /// The debtor account as a parameter is required and the format is [RegularExpression(@"\d{4}-\d{6}")]  ex. 0001-000001.
        /// 
        ///  **GET Table/Fields Details**
        /// 
        /// According to flag  :
        /// 
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
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        /// 
        [ProducesResponseType(typeof(GetAccountInfoResponse), 200)]
        [HttpGet("GetAccountInfo/{debtorAcct}")]
        public async Task<IActionResult> GetAccountInfo(string debtorAcct)
        {
            Serilog.Log.Information("Test GetAccountInfo => GET");
            try
            {
                //T for test.
                var item = await _context.GetAccountInfo(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns the SIF of a debtor account.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets SIF of a debtor account by passing the parametrize debtor account no. A valid token is required for getting the data.
        /// Pass the param with API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetSIF/0001-000001
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
        [ProducesResponseType(typeof(GetSifResponse), 200)]
        [HttpGet("GetSIF/{debtorAcct}")]
        public async Task<IActionResult> GetSIF(string debtorAcct)
        {
            Serilog.Log.Information("Test GetSIF => GET");
            try
            {
                //T for test.
                var item = await _context.GetSIF(debtorAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns Interactions Account Data.(Test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the  interactions account data by passing JSON body like the request example. All parameters are not required
        /// but one
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetInteractionsAcctData
        /// and it's POST request for getting the data.
        ///
        /// **Details**:
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for phone [@"\d{10}"] ex. 2123037334,
        /// please don't forget about valid token.
        /// 
        ///  **GET Table/Fields Details**
        ///  
        ///  debtor_phone_info,
        ///  
        /// check_detail,
        /// 
        /// larry_cc_payments,
        /// 
        /// debtor_pp_info,
        /// 
        /// debtor_acct_info(flag),
        /// 
        /// debtor_master(flag),
        /// 
        /// client_master(flag),
        /// 
        /// client_acct_info(flag)
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
        /// 
        /// debtor_master(flag)->ssn,address1,address2,city,state_code,zip,birth_date,first_name,last_name,
        /// 
        /// client_master(flag)->client_name
        /// 
        /// client_acct_info(flag)->acct_type
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 

        [ProducesResponseType(typeof(GetInteractionAcctDataExample), 200)]
        [HttpPost("GetInteractionsAcctData")]
        public async Task<IActionResult> GetInteractionsAcctData([FromBody] GetInteractionAcctDateRequestModel request)
        {
            Serilog.Log.Information("GetInteractionsAcctData => GET");
            try
            {
                //P for prod.
                var item = await _context.GetInteractionsAcctData(request, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns client invoice header.(Test)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client invoice header data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetClientInvoiceHeader/0001&amp;A
        /// and it's GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}"] ex. 0001, for company ex. A,
        /// Required a valid token.
        /// 
        ///  **GET Table/Fields Details**
        ///  
        /// client_acct_info(flag),
        /// client_master(flag)
        /// 
        /// Pull--> 
        /// 
        /// client_acct(flag)->client_acct
        /// 
        /// client_master(flag)->orig_creditor,remit_full_pmt,address12,address22,city2,state_code2,zip2
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetClientInvoiceHeaderExample), 200)]
        [HttpGet("GetClientInvoiceHeader/{clientAcct}&{company}")]
        public async Task<IActionResult> GetClientInvoiceHeader(string clientAcct, string company)
        {
            Serilog.Log.Information("GetClientInvoiceHeader => GET");
            try
            {
                //P for prod.
                var item = await _context.GetClientInvoiceHeader(clientAcct, company, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Returns Client Primary Contact.(Test)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client primary contact data by passing parameterized clientAcct &amp; company flag  like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/test/AccountInformation/GetClientPrimaryContact/0001&amp;A
        /// and it's GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}"] ex. 0001, for company ex. A,
        /// Required a valid token.
        /// 
        /// 
        ///  **GET Table/Fields Details**
        ///  
        /// contact_master(flag)
        /// 
        /// Pull--> first_name,last_name
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetClientPrimaryContactExample), 200)]
        [HttpGet("GetClientPrimaryContact/{clientAcct}&{company}")]
        public async Task<IActionResult> GetClientPrimaryContact(string clientAcct, string company)
        {
            Serilog.Log.Information("GetClientPrimaryContact => GET");
            try
            {
                //P for prod.
                var item = await _context.GetClientPrimaryContact(clientAcct, company, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        /// <summary>
        ///  Can Get Client Invoice Payments.(test.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can Get Client Invoice Payments.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/Test/AccountInformation/GetClientInvoicePayments
        /// (pass JSON body like the request example)
        ///  A valid token is required for setting the data.
        ///  
        /// 
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
        /// debtor_master(flag)->
        /// debtor_payment_master->balance,status_code,payment_type,agency_amt_decl,client_amt,remit_full_pmt,payment_type
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(GetClientInvoicePaymentsResponseExample), 200)]
        [HttpPost("GetClientInvoicePayments")]
        public async Task<IActionResult> GetClientInvoicePayments([FromBody] GetClientInvoiceRequestModel request)
        {
            Serilog.Log.Information("GetClientInvoicePayments  => GET");
            try
            {
                //P for prod.
                var item = await _context.GetClientInvoicePayments(request, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }



        /// <summary>
        ///  Returns Next Payment Info.(Test)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Gets the client's new payment info by passing parameterized clientAcct like the request example. All parameters are required
        /// for this endpoint . You can call with an API client like  https://g14.aargontools.com/api/Test/AccountInformation/GetNextPaymentInfo/0001-000001
        /// and it's a GET request for getting the data.
        /// 
        /// Regular expression for debtorAcct [@"\d{4}-\d{6}"] ex. 0001-000001, for company ex. A,
        /// Required a valid token.
        /// 
        /// 
        /// 
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
        /// 
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
        /// 
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        [ProducesResponseType(typeof(GetNextPaymentInfoExample), 200)]
        [HttpGet("GetNextPaymentInfo/{clientAcct}")]
        public async Task<IActionResult> GetNextPaymentInfo(string clientAcct)
        {
            Serilog.Log.Information("GetNextPaymentInfo => GET");
            try
            {
                //T for test.
                var item = await _context.GetNextPaymentInfo(clientAcct, "T");

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

    }
}
