using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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

        public SetAccountsDetailsController(IAddBadNumbers contextBadNumbers, ISetMoveAccount contextSetMoveAccount, IAddNotes contextAddNotes
        ,ISetDoNotCall setDoNotCall, ISetNumber setNumber, ISetMoveToHouse setMoveToHouse)
        {
            _contextBadNumbers = contextBadNumbers;
            _contextSetMoveAccount = contextSetMoveAccount;
            _contextAddNotes = contextAddNotes;
            _setDoNotCall = setDoNotCall;
            _setNumber = setNumber;
            _setMoveToHouse = setMoveToHouse;
        }

        /// <summary>
        ///  This endpoint can set a bad number.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can add a bad number , and the bad number will remove from debtor account and add a notes about the action.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetBadNumbers/0001-000001&amp;7025052773
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        /// 
        [ProducesResponseType(typeof(AddBadNumberResponse), 200)]
        [HttpPost("SetBadNumbers/{debtorAcct}&{phoneNo}")]
        public async Task<IActionResult> SetBadNumbers(string debtorAcct, string phoneNo)
        {
            Serilog.Log.Information("  SetBadNumbers => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextBadNumbers.AddBadNumbers(debtorAcct, phoneNo,"P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        /// <summary>
        ///  This endpoint can move debtor account to a queue.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// By using this endpoint you can move debtor account to a queue if the queue is available and the account is active
        /// then add a notes about the action and make a log to a specific log table.
        /// And please don't forget about a valid token.
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetMoveAccount/0001-000001&amp;70
        /// (pass both parameter separated by '&amp;')
        /// </remarks>
        /// <response code="200">Successful Request.</response>
        /// <response code="401">Invalid Token/Token Not Available</response>
        ///
        [ProducesResponseType(typeof(SetMoveAccountResponse), 200)]
        [HttpPost("SetMoveAccount/{debtorAcct}&{toQueue}")]
        public async Task<IActionResult> SetMoveAccount(string debtorAcct, int toQueue)
        {
            Serilog.Log.Information("  SetMoveAccount => POST");
            try
            {
                var data = await _contextSetMoveAccount.SetMoveAccount(debtorAcct, toQueue,"P");
                return Ok(data);
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }

        }


        [HttpPost("SetNotes/{debtorAcct}&{notes}")]
        public async Task<IActionResult> SetNotes(string debtorAcct, string notes)
        {
            Serilog.Log.Information("  SetNotes => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotes.CreateNotes(debtorAcct, notes,"P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        [HttpPost("SetDoNotCall/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetDoNotCall(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("  SetDoNotCall => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setDoNotCall.SetDoNotCallManager(debtorAcct, cellPhoneNo, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        [HttpPost("SetNumber/{debtorAcct}&{cellPhoneNo}")]
        public async Task<IActionResult> SetNumber(string debtorAcct, string cellPhoneNo)
        {
            Serilog.Log.Information("  SetNumber => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setNumber.SetNumber(debtorAcct, cellPhoneNo, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }



        [HttpPost("SetMoveToHouse/{debtorAcct}")]
        public async Task<IActionResult> SetMoveToHouse(string debtorAcct)
        {
            Serilog.Log.Information("  SetMoveToHouse => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setMoveToHouse.SetMoveToHouse(debtorAcct, "P");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }





    }
}
