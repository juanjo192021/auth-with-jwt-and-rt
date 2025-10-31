using System.Text.Json.Serialization;

namespace Auth.API.Common.Dtos
{
    public class PaginationRequest
    {
        public string? Page { get; set; } = "1";
        public string? PageSize { get; set; } = "10";
        public string? Search { get; set; }
    }
}
