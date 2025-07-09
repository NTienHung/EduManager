using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Lesson
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? CourseId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Course? Course { get; set; }
}
