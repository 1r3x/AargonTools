using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;

namespace AargonTools.Manager
{
    public class ResponseModel:IResponseModel
    {
        public ResponseModel()
        {
            //Message = "Oops an error has occurred while getting the data!";
        }
        public bool Status { get; set; }
        //public string Message { get; set; }

        public object Data { get; set; }

        //public ResponseModel Response(bool isSuccess, string msg)
        //{
        //    return new ResponseModel { Status = isSuccess, Message = msg, Data = null };
        //}

        public ResponseModel Response(object data)
        {
            return new() { Status = true, Data = data };
        }
    }

    public interface IResponseModel
    {
        //ResponseModel Response(bool isSuccess, string msg);
        ResponseModel Response(object data);
    }
}
