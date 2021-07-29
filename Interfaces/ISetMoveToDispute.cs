using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetMoveToDispute
    {
        Task<ResponseModel> SetMoveToDispute(string debtorAcct, decimal amountDisputed, string environment);
    }
}
