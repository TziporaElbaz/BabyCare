using System;
using System.Collections.Generic;

namespace WEB_API.DAL.Models;
public partial class BabyVaccine
{
    public BabyVaccine() { }
    public BabyVaccine(Baby baby, Vaccine vaccine)
    {
        BabyId = baby.Id;
        Baby = baby;
        Vaccine = vaccine;
        VaccineId = vaccine.Id;
        DateGiven = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    }
    public int Id { get; set; }

    public int BabyId { get; set; }

    public int VaccineId { get; set; }

    public DateOnly DateGiven { get; set; }

    public virtual Baby Baby { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;
}
