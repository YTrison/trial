using System;

namespace web_api_managemen_user.Class
{
    public class ResponseMessage
    {
       
        public bool status { set; get; }
        public string remark { set; get; }
        public object data { set; get; }
        public string message { get; set; }
       
    }

    public class ValidationAPIResult
    {
        public object data { set; get; }
        public string message { set; get; }
        public bool status { set; get; }
    }

    public class ValidationAPIResultPaging
    {
        public object data { set; get; }
        public string message { set; get; }
        public bool status { set; get; }

        public ValidationAPIResultPaging()
        {
            status = true;
            message = string.Empty;
            data = data;
        }
    }

    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }

    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        public PagedResponse(T data,int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;

        }
    }
}
