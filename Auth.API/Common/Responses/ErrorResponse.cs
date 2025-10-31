namespace Auth.API.Common.Responses
{
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Errors { get; set; } = string.Empty;
        public string? TraceId { get; set; }
    }
}
