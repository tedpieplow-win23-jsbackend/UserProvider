using System;
using System.Collections.Generic;

namespace UserProvider.Entities;

public partial class UserCourse
{
    public int UserCourseId { get; set; }

    public string ApplicationUserId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public virtual AspNetUser ApplicationUser { get; set; } = null!;
}
