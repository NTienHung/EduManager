using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Teacher
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? CustomerId { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual AspNetUser? Customer { get; set; }
}
