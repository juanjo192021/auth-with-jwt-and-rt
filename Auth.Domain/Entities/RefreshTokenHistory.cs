﻿using System;
using System.Collections.Generic;

namespace Auth.Domain.Entities;

public partial class RefreshTokenHistory
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool? Status { get; set; }

    public virtual User? User { get; set; }
}
