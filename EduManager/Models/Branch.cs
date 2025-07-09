using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Branch
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
