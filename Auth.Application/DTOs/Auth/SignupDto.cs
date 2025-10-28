using System.Collections;

namespace Auth.Application.DTOs.Auth
{
    public class SignupDto
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        //public IFormFile? FileImage { get; set; }

        public string DocumentType { get; set; } = null!;

        public string DocumentNumber { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        public string? Phone { get; set; }

        public string Mobile { get; set; } = null!;

        public string? Gender { get; set; }

        public string Address { get; set; } = null!;

        public int UserTypeId { get; set; }

        // Se supone que Mapster lo mapería automáticamente
        public List<int> Roles { get; set; } = [];
    }
}
