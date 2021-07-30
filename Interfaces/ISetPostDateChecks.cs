using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetPostDateChecks
    {
        Task<ResponseModel> SetPostDateChecks(string debtorAcct, DateTime postDate,decimal amount,string accountNumber,
            string routingNumber,int totalPd,char sif,string environment);
    }
}
