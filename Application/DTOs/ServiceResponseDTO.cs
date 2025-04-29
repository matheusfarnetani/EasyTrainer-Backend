namespace Application.DTOs
{
    public class ServiceResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int? StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ServiceResponseDTO<T> CreateSuccess(T data, string message = "Success")
        {
            return new ServiceResponseDTO<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResponseDTO<T> CreateFailure(string message, List<string>? errors = null, int? statusCode = null)
        {
            return new ServiceResponseDTO<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }
}