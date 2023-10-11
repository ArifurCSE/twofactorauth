namespace TwoFAAPI.Data
{
    public class CommonResponse
    {
        public CommonResponse()
        {

        }
        public CommonResponse(dynamic result)
        {
            if (result != null)
            {
                Result = result;
                IsSucceed = true;
                HttpStatusCode = 200;
                Message = "Data Found!";
            }
            else
            {
                Result = result;
                IsFailed = true;
                HttpStatusCode = 400;
                Message = "Data Not Found!";
            }

        }
        public CommonResponse(dynamic result, string message)
        {
            if (result != null)
            {
                Result = result;
                IsSucceed = true;
                HttpStatusCode = 200;
                Message = message;
            }
            else
            {
                Result = result;
                IsFailed = true;
                HttpStatusCode = 400;
                Message = "Request Failed";
            }

        }

        public CommonResponse(string message)
        {
            IsFailed = true;
            HttpStatusCode = 400;
            Message = message;
        }

        public bool IsSucceed { get; set; }
        public bool IsFailed { get; set; }
        public bool IsDuplicated { get; set; }
        public dynamic Result { get; set; }
        public string Message { get; set; }
        public int HttpStatusCode { get; set; }

    }
}
