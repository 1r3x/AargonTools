using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Controllers.TestEnvironment
{
    [Route("api/Test/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SetAccountsDetailsController : ControllerBase
    {
        private readonly IAddBadNumbers _contextBadNumbers;
        private readonly ISetMoveAccount _contextSetMoveAccount;
        private readonly IAddNotes _contextAddNotes;
        private readonly ISetDoNotCall _setDoNotCall;

        public SetAccountsDetailsController(IAddBadNumbers contextBadNumbers, ISetMoveAccount contextSetMoveAccount, IAddNotes contextAddNotes
        ,ISetDoNotCall setDoNotCall)
        {
            _contextBadNumbers = contextBadNumbers;
            _contextSetMoveAccount = contextSetMoveAccount;
            _contextAddNotes = contextAddNotes;
            _setDoNotCall = setDoNotCall;
        }

        [HttpPost("SetBadNumbers/{debtorAcct}&{phoneNo}")]
        public async Task<IActionResult> SetBadNumbers(string debtorAcct, string phoneNo)
        {
            Serilog.Log.Information("Test SetBadNumbers => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextBadNumbers.AddBadNumbers(debtorAcct, phoneNo,"T");

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

        [HttpPost("SetMoveAccount/{debtorAcct}&{toQueue}")]
        public async Task<IActionResult> SetMoveAccount(string debtorAcct, int toQueue)
        {
            Serilog.Log.Information("Test SetMoveAccount => POST");
            try
            {
                var data = await _contextSetMoveAccount.SetMoveAccount(debtorAcct, toQueue,"T");
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
            Serilog.Log.Information("Test SetNotes => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _contextAddNotes.CreateNotes(debtorAcct, notes,"T");

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
                    var data = await _setDoNotCall.SetDoNotCallManager(debtorAcct, cellPhoneNo, "T");

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
