using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth.Application.Common.Responses
{
    public class DataDto<T>
    {
        public T? Data { get; set; } = default!;
    }
}
