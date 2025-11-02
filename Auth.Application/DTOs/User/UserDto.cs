namespace Auth.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string DocumentType { get; set; } = null!;

        public string DocumentNumber { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public string? Phone { get; set; }

        public string Mobile { get; set; } = null!;

        public string? Gender { get; set; }

        public string Address { get; set; } = null!;

        public bool Confirmed { get; set; }

        public bool Status { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string UserType { get; set; } = null!;

        public List<string> Roles { get; set; } = new List<string>();

    }
}
