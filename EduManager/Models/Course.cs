using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Course
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
