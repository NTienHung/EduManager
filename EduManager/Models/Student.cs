using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Student
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
