namespace DevilFruits.BLL.Responses
{
    public class ActionResponse<T>
    {
        public bool WasSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ActionResponse<T> Success(T data, string message = "")
            => new() { WasSuccess = true, Data = data, Message = message };
        public static ActionResponse<T> Fail(string message)
             => new() { WasSuccess = false, Message = message };

        public static ActionResponse<T> Fail(string message, T partialResult)
            => new() { WasSuccess = false, Message = message, Data = partialResult };
    }
}
