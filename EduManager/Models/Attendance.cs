using System;
using System.Collections.Generic;

namespace EduManagerAPI.Models;

public partial class Attendance
{
    public string ClassId { get; set; } = null!;

    public string LessonId { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string? Comment { get; set; }

    public byte? Evaluation { get; set; }

    public byte? Attendance1 { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Lesson Lesson { get; set; } = null!;
}
