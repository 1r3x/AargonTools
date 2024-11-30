﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using AargonTools.Manager.GenericManager;
using System.Text.RegularExpressions;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SetAccountsDetailsController : ControllerBase
    {
        private readonly IAddBadNumbers _contextBadNumbers;
        private readonly ISetMoveAccount _contextSetMoveAccount;
        private readonly IAddNotes _contextAddNotes;
        private readonly ISetDoNotCall _setDoNotCall;
        private readonly ISetNumber _setNumber;
        private readonly ISetMoveToHouse _setMoveToHouse;
        private readonly ISetMoveToDispute _setMoveToDispute;
        private readonly ISetPostDateChecks _setPostDateChecks;
        private readonly ISetMoveToQueue _setMoveToQueue;
        private readonly ISetInteractResults _setInteractionResults;
        private readonly IAddNotesV2 _contextAddNotesV2;
        private readonly ISetDialing _contextSetDialing;
        private readonly ISetUpdateAddress _contextSetUpdateAddress;
        private readonly ISetBlandResults _setBlandsResults;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public SetAccountsDetailsController(IAddBadNumbers contextBadNumbers, ISetMoveAccount contextSetMoveAccount, IAddNotes contextAddNotes
        , ISetDoNotCall setDoNotCall, ISetNumber setNumber, ISetMoveToHouse setMoveToHouse, ISetMoveToDispute setMoveToDispute, ISetPostDateChecks setPostDateChecks
        , ISetMoveToQueue setMoveToQueue, ISetInteractResults setInteractionResults, IAddNotesV2 contextAddNotesV2, ISetDialing contextSetDialing,
        ISetUpdateAddress contextSetUpdateAddress, ISetBlandResults setBlandsResults, IConfiguration configuration, IUserService userService)
        {
            _contextBadNumbers = contextBadNumbers;
            _contextSetMoveAccount = contextSetMoveAccount;
            _contextAddNotes = contextAddNotes;
            _setDoNotCall = setDoNotCall;
            _setNumber = setNumber;
            _setMoveToHouse = setMoveToHouse;
            _setMoveToDispute = setMoveToDispute;
            _setPostDateChecks = setPostDateChecks;
            _setMoveToQueue = setMoveToQueue;
            _setInteractionResults = setInteractionResults;
            _contextAddNotesV2 = contextAddNotesV2;
            _contextSetDialing = contextSetDialing;
            _contextSetUpdateAddress = contextSetUpdateAddress;
            _setBlandsResults = setBlandsResults;
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        ///  Set a bad number.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can add a bad number , and the bad number will remove from debtor account and add a notes about the action.A valid token is required for setting the data
        ///Pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetBadNumbers/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// 
        ///**GET Table/Fields Details**
        ///
        /// debtor_bad_numbers,
        /// 
        /// debtor_phone_info
        /// 
        /// Insert:
        /// 
        /// 
        /// debtor_bad_numbers-->debtor_acct,home_area_code,home_phone,time_attempted,reason
        /// 
        /// Update:
        /// 
        /// debtor_phone_info-->home_phone,work_phone,cell_phone,other_phone
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(AddBadNumberResponse), 200)]
        [HttpPost("SetBadNumbers/{debtorAcct}&{phoneNo}")]
        public async Task<IActionResult> SetBadNumbers(string debtorAcct, string phoneNo)
        {
            Serilog.Log.Information("SetBadNumbers => POST request received for debtorAcct: {DebtorAcct}, phoneNo: {PhoneNo}", debtorAcct, phoneNo);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            // Validate phone number format
            var phoneNoRegex = new Regex(@"^\d{10}$");
            if (!phoneNoRegex.IsMatch(phoneNo))
            {
                Serilog.Log.Warning("Invalid phone number format for phoneNo: {PhoneNo}", phoneNo);
                return BadRequest("Invalid phone number format. It must be a 10-digit number.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextBadNumbers.AddBadNumbers(debtorAcct, phoneNo, "P");
                    Serilog.Log.Information("Bad number added successfully for debtorAcct: {DebtorAcct}, phoneNo: {PhoneNo}", debtorAcct, phoneNo);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while adding bad number for debtorAcct: {DebtorAcct}, phoneNo: {PhoneNo}", debtorAcct, phoneNo);
                throw;
            }

            Serilog.Log.Error("Something went wrong for debtorAcct: {DebtorAcct}, phoneNo: {PhoneNo}", debtorAcct, phoneNo);
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        /// <summary>
        ///  Can move debtor account to a queue.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can move debtor account to a queue if the queue is available and the account is active
        /// then add a notes about the action and make a log to a specific log table.A valid token is required for setting the data
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveAccount/0001-000001&amp;70
        /// (pass both parameter separated by '&amp;')
        /// 
        ///**GET Table/Fields Details**
        ///
        /// debtor_acct_info(flag),
        /// 
        /// note_master,
        /// 
        /// MoveAccountApiLogs
        /// 
        /// Input:
        /// 
        /// note_master-->note_date ,debtor_acct,employee,activity_code,note_text
        /// 
        /// MoveAccountApiLogs->DebtorAcct,FromQueue,ToQueue,MoveDate,Requestor
        /// 
        /// Update:
        /// 
        /// debtor_acct_info(flag)->employee
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveAccountResponse), 200)]
        [HttpPost("SetMoveAccount/{debtorAcct}&{toQueue}")]
        public async Task<IActionResult> SetMoveAccount(string debtorAcct, int toQueue)
        {
            Serilog.Log.Information("SetMoveAccount => POST request received for debtorAcct: {DebtorAcct}, toQueue: {ToQueue}", debtorAcct, toQueue);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            // Validate phone number format
            if (toQueue < 0)
            {
                Serilog.Log.Warning("Invalid queue format for: {toQueue}", toQueue);
                return BadRequest("Invalid queue format. It must be a non-negative integer.");
            }

            try
            {
                var data = await _contextSetMoveAccount.SetMoveAccount(debtorAcct, toQueue, "P");
                Serilog.Log.Information("Account moved successfully for debtorAcct: {DebtorAcct}, toQueue: {ToQueue}", debtorAcct, toQueue);
                return Ok(data);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while moving account for debtorAcct: {DebtorAcct}, toQueue: {ToQueue}", debtorAcct, toQueue);
                throw;
            }

        }

        /// <summary>
        ///  This endpoint can add a note.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// an add a note to a specific debtor account.   A valid token is required for getting the data
        ///  Pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetNotes/0001-000001&amp;This is the notes
        /// (pass both parameter separated by '&amp;')
        /// 
        ///**GET Table/Fields Details**
        ///
        /// note_master
        /// 
        /// Input--> debtor_acct,note_date,activity_code,employee,note_text 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetAddNotesResponse), 200)]
        [HttpPost("SetNotes/{debtorAcct}&{notes}")]
        public async Task<IActionResult> SetNotes(string debtorAcct, string notes)
        {
            Serilog.Log.Information("SetNotes => POST request received for debtorAcct: {DebtorAcct}, notes: {Notes}", debtorAcct, notes);
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotes.CreateNotes(debtorAcct, notes, "P");
                    Serilog.Log.Information("Notes added successfully for debtorAcct: {DebtorAcct}", debtorAcct);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while adding notes for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        /// <summary>
        ///  Can set a phone number's status [don't call].(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can set a phone number's status [don't call]. A valid token is required for setting the data
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetDoNotCall/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// 
        ///**GET Table/Fields Details**
        ///
        /// debtor_phone_info,
        /// 
        /// note_master
        /// 
        /// Input:
        /// 
        /// note_master--> debtor_acct,note_date,activity_code,employee,note_text 
        /// 
        /// Update: 
        /// 
        /// debtor_phone_info-->cell_phone_dont_call
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetDoNotCallResponse), 200)]
        [HttpPost("SetDoNotCall/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetDoNotCall(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("SetDoNotCall => POST request received with debtorAcct: {DebtorAcct}, cellPhoneNo: {CellPhoneNo}", debtorAcct, cellPhoneNo);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            // Validate phone number format
            var phoneNoRegex = new Regex(@"^\d{10}$");
            if (!phoneNoRegex.IsMatch(cellPhoneNo))
            {
                Serilog.Log.Warning("Invalid phone number format for phoneNo: {PhoneNo}", cellPhoneNo);
                return BadRequest("Invalid phone number format. It must be a 10-digit number.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setDoNotCall.SetDoNotCallManager(debtorAcct, cellPhoneNo, "P");
                    Serilog.Log.Information("SetDoNotCallManager executed successfully.Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetDoNotCall request for debtorAcct: {DebtorAcct}, cellPhoneNo: {CellPhoneNo}", debtorAcct, cellPhoneNo);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  Can set a phone number for a debtor.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can set a phone number for a debtor.A valid token is required for setting the data
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetNumber/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// 
        ///**GET Table/Fields Details**
        ///
        /// debtor_phone_info(flag),
        /// 
        /// new_phone_numbers,
        /// 
        /// note_master
        /// 
        /// Pull-->
        /// *  debtor_phone_info(flag)
        /// 
        /// Input: 
        /// 
        /// new_phone_numbers->debtor_acct,area_code,phone_num,entered_by,date_acquired,date_used
        /// 
        /// note_master--> debtor_acct,note_date,activity_code,employee,note_text 
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetNumberResponses), 200)]
        [HttpPost("SetNumber/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetNumber(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("SetNumber => POST request received with debtorAcct: {DebtorAcct}, cellPhoneNo: {CellPhoneNo}", debtorAcct, cellPhoneNo);
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            // Validate phone number format
            var phoneNoRegex = new Regex(@"^\d{10}$");
            if (!phoneNoRegex.IsMatch(cellPhoneNo))
            {
                Serilog.Log.Warning("Invalid phone number format for phoneNo: {PhoneNo}", cellPhoneNo);
                return BadRequest("Invalid phone number format. It must be a 10-digit number.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setNumber.SetNumber(debtorAcct, cellPhoneNo, "P");
                    Serilog.Log.Information("SetNumber executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetNumber request for debtorAcct: {DebtorAcct}, cellPhoneNo: {CellPhoneNo}", debtorAcct, cellPhoneNo);
                throw;

            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  Can set move to house.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can set move to house and make a movement log for a debtor account.
        /// A valid token is required for setting the data
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveToHouse/0001-000001
        /// 
        ///**GET Table/Fields Details**

        /// api_move_logs,
        /// 
        /// debtor_phone_info(flag),
        /// 
        /// detor_master(flag),
        /// 
        /// api_move_settings
        /// 
        /// Pull: 
        /// 
        /// api_move_settings-->target_employee 
        /// 
        /// Input:
        /// 
        /// api_move_logs-->debtor_acc,move_setup_id,previous_employee,new_employee,move_date
        /// 
        /// Update:
        /// 
        /// debtor_phone_info(flag) --> employee
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToHouseResponses), 200)]
        [HttpPost("SetMoveToHouse/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToHouse(string debtorAcct)
        {
            Serilog.Log.Information("SetMoveToHouse => POST request received with debtorAcct: {DebtorAcct}", debtorAcct);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }


            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToHouse.SetMoveToHouse(debtorAcct, "P");
                    Serilog.Log.Information("SetMoveToHouse executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetMoveToHouse request for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  This endpoint can set move to dispute.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to dispute and make a movement log for a debtor account.
        /// A valid token is required for setting the data
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveToDispute/0001-000001
        ///
        ///**GET Table/Fields Details**
        ///
        /// debtor_phone_info(flag),
        /// 
        /// new_phone_numbers,
        /// 
        /// note_master
        /// 
        /// Pull--> *  debtor_phone_info(flag)
        /// 
        /// Input: 
        /// 
        /// new_phone_numbers->debtor_acct,area_code,phone_num,entered_by,date_acquired,date_used
        /// 
        /// note_master--> debtor_acct,note_date,activity_code,employee,note_text 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToDisputeResponses), 200)]
        [HttpPost("SetMoveToDispute/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToDispute(string debtorAcct)
        {
            Serilog.Log.Information("SetMoveToDispute => POST request received with debtorAcct: {DebtorAcct}", debtorAcct);
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }


            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToDispute.SetMoveToDispute(debtorAcct, "P");
                    Serilog.Log.Information("SetMoveToDispute executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetMoveToDispute request for debtorAcct: {DebtorAcct}", debtorAcct);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  Can set post date checks and take necessary actions.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can set post date checks and take necessary actions.
        /// A valid token is required for setting the data
        ///You can pass parameters with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetPostDateChecks
        /// (pass JSON body like the request example)
        /// **Notes**
        /// All the  parameter is required for the endpoint .
        /// 
        /// 
        ///**GET Table/Fields Details**
        ///
        /// routing_numbers,
        /// 
        /// larry_automated_payments,
        /// 
        /// debtor_acct_info(flag),
        /// 
        /// check_detail,
        /// 
        /// debtor_phone_info,
        /// 
        /// debtor_master(flag),
        /// 
        /// batch_checks
        /// 
        /// Pull and Push --> sp_larry_check_postdate 
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetPostDateChecksResponse), 200)]
        [HttpPost("SetPostDateChecks")]
        public async Task<IActionResult> SetPostDateChecks([FromBody] SetPostDateChecksRequestModel requestModel)
        {
           
            Serilog.Log.Information("SetPostDateChecks => POST request received with requestModel: {@RequestModel}", requestModel);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(requestModel.debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", requestModel.debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setPostDateChecks.SetPostDateChecks(requestModel, "P");
                    Serilog.Log.Information("SetPostDateChecks executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetPostDateChecks request with requestModel: {@RequestModel}", requestModel);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }




        /// <summary>
        ///  This endpoint can set move to queue.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can set move to house and make a movement log for a debtor account.
        /// A valid token is required for setting the data.
        ///Pass required parameters with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveToQueue/0001-000001&amp;'TYPE'
        /// (Pass all the  parameter separated by '&amp;')
        /// 
        ///**GET Table/Fields Details**
        ///
        /// debtor_phone_info(flag),
        /// 
        /// new_phone_numbers,
        /// 
        /// note_master
        /// 
        /// Pull-->
        /// 
        /// *  debtor_phone_info(flag)
        /// 
        /// Input: 
        /// 
        /// new_phone_numbers->debtor_acct,area_code,phone_num,entered_by,date_acquired,date_used
        /// 
        /// note_master--> debtor_acct,note_date,activity_code,employee,note_text 
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveToQueueResponse), 200)]
        [HttpPost("SetMoveToQueue/{debtorAcct}&{type}")]
        public async Task<IActionResult> SetMoveToQueue(string debtorAcct, string type)
        {
            Serilog.Log.Information("SetMoveToQueue => POST request received with debtorAcct: {DebtorAcct}, type: {Type}", debtorAcct, type);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}",debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToQueue.SetMoveToQueue(debtorAcct, type, "P");
                    Serilog.Log.Information("SetMoveToQueue executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetMoveToQueue request for debtorAcct: {DebtorAcct}, type: {Type}", debtorAcct, type);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  Can set Interaction Results.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can set Interaction Results.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetInteractionResults
        /// (pass JSON body like the request example)
        ///  A valid token is required for setting the data.
        ///  
        ///**GET Table/Fields Details**
        ///
        /// interact_results
        /// 
        /// Insert--> ANI,call_result,debtor_acct,end_time,last_dialogue,opening_intent,payment_amt,start_time,term_reason,transfer_reason
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(SetInteractionResultsResponse), 200)]
        [HttpPost]
        [Route("SetInteractionResults")]
        public async Task<IActionResult> SetInteractionResults([FromBody] InteractResult interactResultModel)
        {
            Serilog.Log.Information("SetInteractionResults => POST request received with interactResultModel: {@InteractResultModel}", interactResultModel);
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(interactResultModel.DebtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", interactResultModel.DebtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setInteractionResults.SetInteractResults(interactResultModel, "P");
                    Serilog.Log.Information("SetInteractionResults executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetInteractionResults request with interactResultModel: {@InteractResultModel}", interactResultModel);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  Can add a note (JSON body request).(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can add a note to a specific debtor account.
        /// A valid token is required for setting the data.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetNotesV2
        /// (pass JSON body like the request example)
        /// 
        ///**GET Table/Fields Details**
        /// 
        /// note_master
        /// 
        /// Input--> debtor_acct,note_date,activity_code,employee,note_text 
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [HttpPost("SetNotesV2")]
        public async Task<IActionResult> SetNotesV2([FromBody] AddNotesRequestModel request)
        {
            Serilog.Log.Information("SetNotesV2 => POST request received with request: {@Request}", request);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(request.DebtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", request.DebtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotesV2.CreateNotes(request, "P");
                    Serilog.Log.Information("SetNotesV2 executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetNotesV2 request with request: {@Request}", request);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        /// <summary>
        ///  Can set a dialing (JSON body request).(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can set dialing.
        /// A valid token is required for setting the data.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetDialing
        /// (pass JSON body like the request example)
        /// 
        ///**GET Table/Fields Details**
        /// 
        /// ivr_recall_table,
        /// apex_list_master
        /// 
        /// insert: ivr_recall_table,
        /// 
        /// update: apex_list_master
        /// 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [HttpPost("SetDialing")]
        [ProducesResponseType(typeof(SetDialingResponseModelExample
        ), 200)]
        public async Task<IActionResult> SetDialing([FromBody] SetDialingRequestModel request)
        {
            Serilog.Log.Information("SetDialing => POST request received with request: {@Request}", request);
            
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(request.DebtorAccount))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", request.DebtorAccount);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextSetDialing.SetDialing(request, "P");
                    Serilog.Log.Information("SetDialing executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetDialing request with request: {@Request}", request);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        /// <summary>
        ///  Can Update Address (JSON body request).(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Can Update Address.
        /// A valid token is required for setting the data.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetUpdateAddress
        /// (pass JSON body like the request example)
        /// 
        ///**GET Table/Fields Details**
        /// 
        /// debtor_master(flag),
        /// 
        /// debtor_acct_info(flag),
        /// 
        /// note_master
        /// 
        /// Update: 
        /// 
        /// debtor_acct_info(flag)->mail_return 
        /// 
        /// debtor_master(flag)->address1,address2,city,state_code,zip,residence_status,address_change_date
        /// 
        /// Insert:
        /// 
        /// note_master->debtor_acct,note_date,activity_code,employee,note_text 
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [HttpPost("SetUpdateAddress")]
        [ProducesResponseType(typeof(SetUpdateAddressResponseModel
        ), 200)]
        public async Task<IActionResult> SetUpdateAddress([FromBody] SetUpdateAddressRequestModel request)
        {
            Serilog.Log.Information("SetUpdateAddress => POST request received with request: {@Request}", request);
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(request.DebtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", request.DebtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextSetUpdateAddress.SetUpdateAddress(request, "P");
                    Serilog.Log.Information("SetUpdateAddress executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetUpdateAddress request with request: {@Request}", request);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        /// <summary>
        ///  This endpoint can insert Bland results bypassing the IP filter (JSON body request).(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint, you can insert Bland call results while bypassing the IP filter. 
        /// And please don't forget about a valid token.
        /// You can pass the parameter with API client like https://g14.aargontools.com/no-ip-filter/SetBlandResults
        /// (pass JSON body like the request example)
        /// 
        ///**GET Table/Fields Details**
        ///
        /// Insert:
        /// 
        /// note_master->AI_call_results,note_text  
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///

        [AllowAnonymous]
        [HttpPost("/no-ip-filter/SetBlandResults")]

        public async Task<IActionResult> SetBlandResults([FromBody] List<BlandResultsViewModel> request)
        {
            Serilog.Log.Information("SetBlandResults => POST request received from IP: {ClientIpAddress}", _userService.GetClientIpAddress());

            try
            {
                if (ModelState.IsValid)
                {
                    //
                    // Extract the token from the request
                    var token = request[0].variables.token;

                    // Perform your token validation logic here
                    var tokenHandler = new JwtSecurityTokenHandler();
                    //get the kay from centralize json 
                    var key = _configuration["JwtConfig:Secret"];

                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = false,
                        ClockSkew = TimeSpan.Zero
                    };



                    try
                    {
                        tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                        Serilog.Log.Information("Token validation successful.");
                    }
                    catch (Exception e)
                    {
                        Serilog.Log.Warning(e, "Token validation failed.");
                        return Unauthorized("Invalid token");
                    }



                    //
                    var data = await _setBlandsResults.SetBlandResults(request, "P");
                    Serilog.Log.Information("SetBlandResults executed successfully. Returning data: {@data}", data);
                    return Ok(data);


                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetBlandResults request.");
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        /// <summary>
        ///  This endpoint can insert Bland results (JSON body request).(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint, you can insert Bland call results. 
        /// And please don't forget about a valid token.
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetBlandResults
        /// (pass JSON body like the request example)
        /// 
        ///**GET Table/Fields Details**
        ///
        /// Insert:
        /// 
        /// note_master->AI_call_results,note_text  
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [HttpPost("SetBlandResults")]

        public async Task<IActionResult> SetBlandResultsV2([FromBody] List<BlandResultsViewModel> request)
        {
            Serilog.Log.Information("SetBlandResults => POST request received with request: {@Request}", request);
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setBlandsResults.SetBlandResults(request, "P");
                    Serilog.Log.Information("SetBlandResults executed successfully. Returning data: {@Data}", data);
                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing SetBlandResults request with request: {@Request}", request);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


    }
}
