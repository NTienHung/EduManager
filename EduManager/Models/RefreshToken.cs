using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class RefreshToken
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public string? Token { get; set; }

    public string? JwtId { get; set; }

    public bool? IsUsed { get; set; }

    public bool? IsRevoked { get; set; }

    public DateTime? AddedDate { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
