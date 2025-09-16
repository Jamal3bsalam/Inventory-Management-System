namespace Inventory.Mostafa.Pl.Response.General
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Messsage { get; set; }
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public int? StatusCode { get; set; }

        public ApiResponse(bool success, int statusCode, string message, T data, object errors = null)
        {
            Success = success;
            StatusCode = statusCode;
            Messsage = message;
            Data = data;
            Errors = errors;
        }
    }
}
