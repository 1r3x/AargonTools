using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AargonTools.Data;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Models;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")] // api/todo
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IGetAccountInformation _context2;
        private readonly AdoDotNetConnection _adoConnection;

        public TodoController(ApiDbContext context,AdoDotNetConnection adoConnection, IGetAccountInformation context2)
        {
            _context = context;
            _adoConnection = adoConnection;
            _context2 = context2;
        }
        /// <summary>
        ///  This GetItems Endpoint returns all the items from the Prod Db test table.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check data flow from prod db. You don't required any authentication
        /// for this endpoint .  
        /// </remarks>
        /// <response code="200">Successfully get the data.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<TestApiData>),200)]
        public  IActionResult GetItems()
        {
            Serilog.Log.Information("  Todo => GET");
            try
            {
                //LINQ
                //var items = await _context.TestApiData.ToListAsync();
                //return Ok(items);

                //vs

                //ADO.NET
                var rowAdo = _adoConnection.GetData("SELECT * FROM TestApiData");
                var listOfItems = new List<TestApiData>();
                for (var i = 0; i < rowAdo.Rows.Count; i++)
                {
                    var itemData = new TestApiData
                    {
                        Id = Convert.ToInt32(rowAdo.Rows[i]["Id"]),
                        Description = rowAdo.Rows[i]["Description"].ToString(),
                        Title = rowAdo.Rows[i]["Title"].ToString(),
                        Done = Convert.ToBoolean(rowAdo.Rows[i]["Done"])
                    };
                    listOfItems.Add(itemData);
                }
                return Ok(listOfItems);

            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(TestApiData data)
        {
            Serilog.Log.Information("  Todo => POST");
            if (ModelState.IsValid)
            {
                await _context.TestApiData.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { data.Id }, data);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            Serilog.Log.Information("  Todo => POST");
            try
            {
                var item = await _context.TestApiData.FirstOrDefaultAsync(x => x.Id == id);

                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e.InnerException, e.Message);
                throw;
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, TestApiData item)
        {
            if (id != item.Id)
                return BadRequest();

            var existItem = await _context.TestApiData.FirstOrDefaultAsync(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            existItem.Title = item.Title;
            existItem.Description = item.Description;
            existItem.Done = item.Done;

            // Implement the changes on the database level
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existItem = await _context.TestApiData.FirstOrDefaultAsync(x => x.Id == id);

            if (existItem == null)
                return NotFound();

            _context.TestApiData.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(existItem);
        }

    }
}