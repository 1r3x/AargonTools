using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.EntityFrameworkCore;

namespace AargonTools.Manager
{




    public class SetHrmManager : ISetEmployeeTimeLogEntry
    {

        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static ResponseModel _response;
        private static GetTheCompanyFlag _companyFlag;
        private readonly AdoDotNetConnection _adoConnection;

        public SetHrmManager(ExistingDataDbContext context, ResponseModel response, GetTheCompanyFlag companyFlag,
            TestEnvironmentDbContext contextText, AdoDotNetConnection adoConnection, ProdOldDbContext contextProdOld)
        {
            _context = context;
            _contextTest = contextText;
            _response = response;
            _companyFlag = companyFlag;
            _adoConnection = adoConnection;
            _contextProdOld = contextProdOld;
        }
        public async Task<ResponseModel> SetEmployeeTimeLogEntry(int employeeId, string stationName, DateTime dateTime, string reasons, string environment)
        {
            try
            {
                if (environment == "P")
                {
                    if (reasons == "END DAY")
                    {
                        var startDayTime = await _context.EmployeeTimeLogs.SingleOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                             && x.Reason == "START DAY" && x.LogTime.Date == dateTime.Date);

                        var lastLoginDayTime = await _context.EmployeeTimeLogs.OrderByDescending(p => p.LogTime).FirstOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                            && x.Reason == "LOGIN" && x.LogTime.Date == dateTime.Date);

                        if (lastLoginDayTime == null)
                        {
                            var employeeTimeLog = new EmployeeTimeLog()
                            {
                                Employee = employeeId,
                                LogTime = dateTime,
                                StationName = stationName,
                                NumMinutes = Convert.ToInt32(dateTime.Subtract(startDayTime.LogTime).TotalMinutes),
                                Reason = reasons
                            };
                            await _context.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            var employeeTimeLog = new EmployeeTimeLog()
                            {
                                Employee = employeeId,
                                LogTime = dateTime,
                                StationName = stationName,
                                NumMinutes = Convert.ToInt32(dateTime.Subtract(lastLoginDayTime.LogTime).TotalMinutes),
                                Reason = reasons
                            };
                            await _context.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                            await _context.SaveChangesAsync();
                        }

                    }
                    else if (reasons == "LUNCH")
                    {
                        var startDayTime = await _context.EmployeeTimeLogs.SingleOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                            && x.Reason == "START DAY" && x.LogTime.Date == dateTime.Date);



                        var employeeTimeLog = new EmployeeTimeLog()
                        {
                            Employee = employeeId,
                            LogTime = dateTime,
                            StationName = stationName,
                            NumMinutes = Convert.ToInt32(dateTime.Subtract(startDayTime.LogTime).TotalMinutes),
                            Reason = reasons
                        };
                        await _context.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        var employeeTimeLog = new EmployeeTimeLog()
                        {
                            Employee = employeeId,
                            LogTime = dateTime,
                            StationName = stationName,
                            NumMinutes = 0,
                            Reason = reasons
                        };
                        await _context.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                        await _context.SaveChangesAsync();
                    }


                }
                else if (environment=="PO")
                {
                    if (reasons == "END DAY")
                    {
                        var startDayTime = await _contextProdOld.EmployeeTimeLogs.SingleOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                             && x.Reason == "START DAY" && x.LogTime.Date == dateTime.Date);

                        var lastLoginDayTime = await _contextProdOld.EmployeeTimeLogs.OrderByDescending(p => p.LogTime).FirstOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                            && x.Reason == "LOGIN" && x.LogTime.Date == dateTime.Date);

                        if (lastLoginDayTime == null)
                        {
                            var employeeTimeLog = new EmployeeTimeLog()
                            {
                                Employee = employeeId,
                                LogTime = dateTime,
                                StationName = stationName,
                                NumMinutes = Convert.ToInt32(dateTime.Subtract(startDayTime.LogTime).TotalMinutes),
                                Reason = reasons
                            };
                            await _contextProdOld.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                            await _contextProdOld.SaveChangesAsync();
                        }
                        else
                        {
                            var employeeTimeLog = new EmployeeTimeLog()
                            {
                                Employee = employeeId,
                                LogTime = dateTime,
                                StationName = stationName,
                                NumMinutes = Convert.ToInt32(dateTime.Subtract(lastLoginDayTime.LogTime).TotalMinutes),
                                Reason = reasons
                            };
                            await _contextProdOld.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                            await _contextProdOld.SaveChangesAsync();
                        }

                    }
                    else if (reasons == "LUNCH")
                    {
                        var startDayTime = await _contextProdOld.EmployeeTimeLogs.SingleOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                            && x.Reason == "START DAY" && x.LogTime.Date == dateTime.Date);



                        var employeeTimeLog = new EmployeeTimeLog()
                        {
                            Employee = employeeId,
                            LogTime = dateTime,
                            StationName = stationName,
                            NumMinutes = Convert.ToInt32(dateTime.Subtract(startDayTime.LogTime).TotalMinutes),
                            Reason = reasons
                        };
                        await _contextProdOld.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                        await _contextProdOld.SaveChangesAsync();

                    }
                    else
                    {
                        var employeeTimeLog = new EmployeeTimeLog()
                        {
                            Employee = employeeId,
                            LogTime = dateTime,
                            StationName = stationName,
                            NumMinutes = 0,
                            Reason = reasons
                        };
                        await _contextProdOld.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                        await _contextProdOld.SaveChangesAsync();
                    }
                }
                else
                {
                    if (reasons == "END DAY")
                    {
                        var startDayTime = await _contextTest.EmployeeTimeLogs.SingleOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                             && x.Reason == "START DAY" && x.LogTime.Date == dateTime.Date);

                        var lastLoginDayTime = await _contextTest.EmployeeTimeLogs.OrderByDescending(p=>p.LogTime).FirstOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                            && x.Reason == "LOGIN" && x.LogTime.Date == dateTime.Date);

                        if (lastLoginDayTime == null)
                        {
                            var employeeTimeLog = new EmployeeTimeLog()
                            {
                                Employee = employeeId,
                                LogTime = dateTime,
                                StationName = stationName,
                                NumMinutes = Convert.ToInt32(dateTime.Subtract(startDayTime.LogTime).TotalMinutes),
                                Reason = reasons
                            };
                            await _contextTest.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                            await _contextTest.SaveChangesAsync();
                        }
                        else
                        {
                            var employeeTimeLog = new EmployeeTimeLog()
                            {
                                Employee = employeeId,
                                LogTime = dateTime,
                                StationName = stationName,
                                NumMinutes = Convert.ToInt32(dateTime.Subtract(lastLoginDayTime.LogTime).TotalMinutes),
                                Reason = reasons
                            };
                            await _contextTest.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                            await _contextTest.SaveChangesAsync();
                        }

                    }
                    else if (reasons == "LUNCH")
                    {
                        var startDayTime = await _contextTest.EmployeeTimeLogs.SingleOrDefaultAsync(x => x.Employee == employeeId && x.StationName == stationName
                            && x.Reason == "START DAY" && x.LogTime.Date == dateTime.Date);


                       
                        var employeeTimeLog = new EmployeeTimeLog()
                        {
                            Employee = employeeId,
                            LogTime = dateTime,
                            StationName = stationName,
                            NumMinutes = Convert.ToInt32(dateTime.Subtract(startDayTime.LogTime).TotalMinutes),
                            Reason = reasons
                        };
                        await _contextTest.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                        await _contextTest.SaveChangesAsync();

                    }
                    else
                    {
                        var employeeTimeLog = new EmployeeTimeLog()
                        {
                            Employee = employeeId,
                            LogTime = dateTime,
                            StationName = stationName,
                            NumMinutes = 0,
                            Reason = reasons
                        };
                        await _contextTest.EmployeeTimeLogs.AddAsync(employeeTimeLog);
                        await _contextTest.SaveChangesAsync();
                    }


                }


                return _response.Response("Time Log Saved Successfully.");
            }
            catch (Exception e)
            {
                return _response.Response(e);
            }
        }
    }
}
