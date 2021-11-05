using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface IAddBadNumbers
    {
        Task<ResponseModel> AddBadNumbers(string accountNo,string phoneNo,string environment);
    }
}
