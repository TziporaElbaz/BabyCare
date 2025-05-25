using System;
using System.Collections.Generic;

namespace WEB_API.Models;

public partial class Baby
{
    public int Id { get; set; }

    public string BabyId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public bool Gender { get; set; }

    public double Weight { get; set; }

    public double Height { get; set; }

    public double HeadCircumference { get; set; }

    public double BirthWeight { get; set; }

    public bool IsInGrowthCurve { get; set; }

    public string? MotherName { get; set; }

    public string? FatherName { get; set; }

    public string ParentPhone { get; set; } = null!;

    public string? ParentEmail { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<BabyVaccine> BabyVaccines { get; set; } = new List<BabyVaccine>();
}
