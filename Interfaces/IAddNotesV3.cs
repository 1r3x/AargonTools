using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IAddNotesV3
    {
        Task<ResponseModel> CreateNotes(NoteMaster request, string environment);
    }
}
