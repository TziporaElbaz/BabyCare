using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class WorkerShift
{
    public int Id { get; set; }

    public int WorkerId { get; set; }

    public int ShiftId { get; set; }

    public virtual Shift Shift { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
    public WorkerShift( Shift shift, Worker worker)
    {   

    }
}
