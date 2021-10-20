using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;

namespace AargonTools.Interfaces
{
    public interface ISetInteractResults
    {
        Task<ResponseModel> SetInteractResults(InteractResult interactResultModel, string environment);
    }
}
