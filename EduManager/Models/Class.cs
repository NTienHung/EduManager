using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Class
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public Guid? TeacherId { get; set; }

    public string? BranchId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Branch? Branch { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
