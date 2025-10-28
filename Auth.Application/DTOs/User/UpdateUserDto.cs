using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

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

        public bool Status { get; set; }

        public int UserTypeId { get; set; }
    }
}
