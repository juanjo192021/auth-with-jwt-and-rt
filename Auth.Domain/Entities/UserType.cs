using System;
using System.Collections.Generic;

namespace Auth.Domain.Entities;

public partial class UserType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
