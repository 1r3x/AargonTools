using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Interfaces
{
    public interface IAddNotes
    {
        Task<ResponseModel> CreateNotes(string debtorAccount, string notes);
    }
}
