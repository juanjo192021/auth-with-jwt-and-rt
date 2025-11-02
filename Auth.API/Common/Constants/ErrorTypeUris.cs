namespace Auth.API.Common.Constants
{
    public static class ErrorTypeUris
    {
        public const string BadRequest = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        public const string Unauthorized = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2";
        public const string Forbidden = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4";
        public const string NotFound = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
        public const string Conflict = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10";
        public const string InternalServerError = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";
    }
}
