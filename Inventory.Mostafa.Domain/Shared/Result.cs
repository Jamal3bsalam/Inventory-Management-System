namespace Inventory.Mostafa.Domain.Shared
{
    public class Result<T>
    {
        public bool? IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public Result(bool isSucess, string message, T? data = default)
        {
            IsSuccess = isSucess;
            Message = message;
            Data = data;
        }

        public static Result<T> Success(T data, string message = "Operation completed successfully.")
        {
            return new Result<T>(true, message, data);
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>(false, message);
        }
    }
}
