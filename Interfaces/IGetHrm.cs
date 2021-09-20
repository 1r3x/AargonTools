using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface IGetHrm
    {
         Task<ResponseModel> GetEmployeeTimeLog(int employeeId, DateTime date, string environment);
    }
}
