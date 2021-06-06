using System;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AargonTools.Data;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")] // api/todo
    [ApiController]
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

        [HttpGet]
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