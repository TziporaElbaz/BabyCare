using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int WorkerId { get; set; }

    public int BabyId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Baby Baby { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
