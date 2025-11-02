namespace Auth.API.Contracts.Requests.UserType
{
    public class CreateUserTypeRequest
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } 
    }
}
