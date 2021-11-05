using System;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;

namespace AargonTools.Interfaces
{
    public interface ISetEmployeeTimeLogEntry
    {
        Task<ResponseModel> SetEmployeeTimeLogEntry(int employeeId,string stationName, DateTime dateTime, string reasons, string environment);
    }
}
