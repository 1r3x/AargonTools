using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetEmployeeTimeLogResponse:IExamplesProvider<GetEmployeeTimeLogResponse>
    {
        public bool Status { get; set; }
        public List<MultipleEmployeeTimeLogObj> Data { get; set; }
        public GetEmployeeTimeLogResponse GetExamples()
        {
            return new GetEmployeeTimeLogResponse()
            {
                Status = true,
                Data = new List<MultipleEmployeeTimeLogObj>()
                {
                    new MultipleEmployeeTimeLogObj()
                    {
                        empFullName = "DENNIS CALLANAN",
                        department = "FRONT END",
                        empID = 65,
                        stationName = "7STATION06",   
                        logTime ="2020-02-22T07:59:37.463",
                        reason = "START DAY",
                        numMinutes = 0,
                    },
                    new MultipleEmployeeTimeLogObj()
                    {
                        empFullName = "DENNIS CALLANAN",
                        department = "FRONT END",
                        empID = 65,
                        stationName = "7STATION06",
                        logTime ="2020-02-22T12:58:25.803",
                        reason = "END DAY",
                        numMinutes = 299,
                    },
                }
            };
        }

        public class MultipleEmployeeTimeLogObj
        {
            public string empFullName { get; set; }
            public string department { get; set; }
            public int empID { get; set; }
            public string stationName { get; set; }
            public string logTime { get; set; }
            public string reason { get; set; }
            public int numMinutes { get; set; }
        }
    }
}
