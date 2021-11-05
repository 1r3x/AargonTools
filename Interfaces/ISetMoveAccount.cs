using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetMoveAccount
    {
        //If Account Exists Then
        //Get Company Flag
        //    If Account Is Active Then
        //If Target Queue Exists Then
        //    Insert Note("ONLINE MOVE ACCT (FromQueue -> ToQueue)")
        //Move Account
        //Log the Move in a table


        Task<ResponseModel> SetMoveAccount(string debtorAcct,int toQueue,string environment);
    }
}
