namespace Auth.API.Contracts.Requests.UserType
{
    public class UpdateUserTypeRequest: CreateUserTypeRequest
    {
        public int Id { get; set; } = 0!;
    }
}
