namespace MVCBanXeDap.Models
{
    public class ApiReponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public ApiReponse(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
}
