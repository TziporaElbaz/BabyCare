using System;
using System.Collections.Generic;

namespace WEB_API.Models;

public partial class BabyVaccine
{
    public int Id { get; set; }

    public int BabyId { get; set; }

    public int VaccineId { get; set; }

    public DateOnly DateGiven { get; set; }

    public virtual Baby Baby { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;
}
