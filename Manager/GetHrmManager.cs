using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{
    public class GetHrmManager : IGetHrm
    {
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextText;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private readonly AdoDotNetConnection _adoConnection;

        public GetHrmManager(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag,
            TestEnvironmentDbContext contextText, AdoDotNetConnection adoConnection)
        {
            _context = context;
            _contextText = contextText;
            _response = response;
            _companyFlag = companyFlag;
            _adoConnection = adoConnection;
        }

        public async Task<ResponseModel> GetEmployeeTimeLog(int employeeId, DateTime date, string environment)
        {
            if (environment == "P")
            {
                var employeeLog = await (from empTimeLog in _context.EmployeeTimeLogs
                                         join empInfo in _context.EmployeeInfos on empTimeLog.Employee equals empInfo.Employee
                                         where empTimeLog.Employee == employeeId && empTimeLog.LogTime.Date == date
                                         select new
                                         {
                                             EmpFullName = empInfo.FirstName + " " + empInfo.LastName, 
                                             empInfo.Department,
                                             EmpID = empTimeLog.Employee,
                                             empTimeLog.StationName,
                                             empTimeLog.LogTime,
                                             empTimeLog.Reason,
                                             empTimeLog.NumMinutes,
                                         }).ToListAsync();

                return _response.Response(employeeLog);
            }
            else
            {
                var employeeLog = await (from empTimeLog in _contextText.EmployeeTimeLogs
                                         join empInfo in _contextText.EmployeeInfos on empTimeLog.Employee equals empInfo.Employee
                                         where empTimeLog.Employee == employeeId && empTimeLog.LogTime.Date == date
                                         select new
                                         {
                                             EmpFullName = empInfo.FirstName + " " + empInfo.LastName,
                                             empInfo.Department,
                                             EmpID = empTimeLog.Employee,
                                             empTimeLog.StationName,
                                             empTimeLog.LogTime,
                                             empTimeLog.Reason,
                                             empTimeLog.NumMinutes,
                                         }).ToListAsync();

                return _response.Response(employeeLog);
            }

        }

    }
}
