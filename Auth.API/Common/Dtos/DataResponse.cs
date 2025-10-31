using System.Text.Json.Serialization;

namespace Auth.API.Common.Dtos
{
    public class DataResponse<T>
    {
        //public int StatusCode { get; set; }
        //public string Message { get; set; } = null!;
        //public bool Success { get; set; }

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; } = default!;
    }
}
