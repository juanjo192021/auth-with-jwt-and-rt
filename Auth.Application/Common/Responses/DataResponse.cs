using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth.Application.Common.Responses
{
    public class DataResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; } = default!;
    }
}
