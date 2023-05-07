namespace UsersAPI.Models
{
    public class Response
    {
        public Response(string message, bool status)
        {
            Message = message;
            Status = status;
        }

        public Response(string message, bool status, object @object)
        {
            Message = message;
            Status = status;
            Object = @object;
        }

        public string Message { get; set; } = null!;
        public bool Status { get; set; }
        public object Object { get; set; } = null!;
    }
}
