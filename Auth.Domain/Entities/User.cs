using System;
using System.Collections.Generic;

namespace Auth.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<RefreshTokenHistory> RefreshTokenHistories { get; set; } = new List<RefreshTokenHistory>();
}
