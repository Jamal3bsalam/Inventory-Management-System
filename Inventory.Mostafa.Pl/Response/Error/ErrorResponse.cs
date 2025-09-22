namespace Inventory.Mostafa.Pl.Response.Error
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "The request is invalid or malformed",
                401 => "You are not authorized to access this resource. Please ensure you are logged in with valid credentials.",
                404 => "The requested resource could not be found",
                500 => "An unexpected error occurred on the server",
            };
            return message;
        }
    }
}
