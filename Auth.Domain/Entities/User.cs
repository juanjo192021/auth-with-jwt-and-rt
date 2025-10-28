using System;
using System.Collections.Generic;

namespace Auth.Domain.Entities;

public partial class User
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

    public bool IsConfirmed { get; set; }

    public bool IsActive { get; set; }

    public DateTime RegistrationDate { get; set; }

    public int UserTypeId { get; set; }

    public virtual ICollection<RefreshTokenHistory> RefreshTokenHistories { get; set; } = new List<RefreshTokenHistory>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual UserType UserType { get; set; } = null!;
}
