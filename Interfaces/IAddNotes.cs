using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface IAddNotes
    {
        Task<ResponseModel> CreateNotes(string debtorAccount, string notes, string environment);
    }
}
