namespace AargonTools.Manager.GenericManager
{
    public class ResponseModel:IResponseModel
    {
        public ResponseModel()
        {
            Data = "Oops something went wrong. Please try again";
        }
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }

        public object Data { get; set; }

       

        public ResponseModel Response(object data)
        {
            return new() { Status = true, Data = data };
        }


        public ResponseModel Response(bool status,object data)
        {
            return new() { Status = status, Data = data };
        }

        public ResponseModel Response(bool status,bool transStatus, object data)
        {
            return new() { Status = status,TransactionStatus = transStatus, Data = data };
        }


    }

    public interface IResponseModel
    {
        //ResponseModel Response(bool isSuccess, string msg);
        ResponseModel Response(object data);
    }
}
