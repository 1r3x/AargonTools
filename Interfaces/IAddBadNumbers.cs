using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Models;

namespace AargonTools.Interfaces
{
    public interface IAddBadNumbers
    {
        Task<DebtorPhoneInfo> AddBadNumbers(string accountNo,String phoneNo);
    }
}
