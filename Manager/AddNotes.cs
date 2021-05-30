using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Models;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Manager
{
    public class AddNotes:IAddNotes
    {
        private static ExistingDataDbContext _context;

        public AddNotes(ExistingDataDbContext context)
        {
            _context = context;
        }

        async Task<NoteMaster> IAddNotes.CreateNotes(NoteMaster notesData)
        {
            try
            {
                
                await _context.NoteMasters.AddAsync(notesData);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return notesData;
        }


    }
}
