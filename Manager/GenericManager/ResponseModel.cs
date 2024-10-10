namespace AargonTools.Manager.GenericManager
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Data = "Oops something went wrong. Please try again";
        }

        public bool Status { get; set; }
        public object Data { get; set; }

        public ResponseModel Response(bool status, bool transStatus, object data)
        {
            return new ResponseWithTransaction
            {
                Status = status,
                TransactionStatus = transStatus,
                Data = data
            };
        }

        public ResponseModel Response(bool status, bool transStatus, object data, object dbData)
        {
            return new ResponseForDbInsertQa
            {
                Status = status,
                TransactionStatus = transStatus,
                Data = data,
                DbQaTransaction = dbData
            };
        }

        public ResponseModel Response(object data)
        {
            return new ResponseModel
            {
                Status = true,
                Data = data
            };
        }

        public ResponseModel Response(bool status, object data)
        {
            return new ResponseModel
            {
                Status = status,
                Data = data
            };
        }
    }

    public class ResponseWithTransaction : ResponseModel
    {
        public bool TransactionStatus { get; set; }
    }

    public class ResponseForDbInsertQa : ResponseWithTransaction
    {
        public object DbQaTransaction { get; set; }
    }

    public interface IResponseModel
    {
        ResponseModel Response(object data);
    }
}
