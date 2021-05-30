using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Interfaces
{
    public interface IAddNotes
    {
        Task<NoteMaster> CreateNotes(NoteMaster notesData);
    }
}
