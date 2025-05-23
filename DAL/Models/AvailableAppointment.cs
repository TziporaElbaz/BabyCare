﻿using System;
using System.Collections.Generic;
using Project.Models;

namespace Project.DAL.Models;

public partial class AvailableAppointment
{
    public int Id { get; set; }

    public int WorkerId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Worker Worker { get; set; } = null!;
}
