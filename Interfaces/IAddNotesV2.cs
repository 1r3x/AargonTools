using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;

namespace AargonTools.Interfaces
{
    public interface IAddNotesV2
    {
        Task<ResponseModel> CreateNotes( AddNotesRequestModel request,string environment);
    }
}
