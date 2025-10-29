namespace Auth.Application.Enums
{
    public enum TokenStatus
    {
        Valid,
        Expired,
        InvalidFormat,
        InvalidSignature,
        Corrupt
    }
}
