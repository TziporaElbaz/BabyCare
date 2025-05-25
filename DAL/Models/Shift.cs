using System;
using System.Collections.Generic;

namespace WEB_API.Models;

public partial class Shift
{
    public int Id { get; set; }

    public int Day { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string ShiftType { get; set; } = null!;

    public virtual ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
}
