using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AargonTools.Interfaces
{
    public interface ISetBlandResults
    {
        Task<ResponseModel> SetBlandResults(List<BlandResultsViewModel> interactResultModel, string environment);
    }
}
