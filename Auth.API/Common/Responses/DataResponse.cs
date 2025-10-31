namespace Auth.API.Common.Responses
{
    public class DataResponse<T>
    {
        public T? Data { get; set; } = default!;
    }
}
