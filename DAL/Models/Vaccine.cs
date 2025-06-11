using System;
using System.Collections.Generic;

namespace   WEB_API.DAL.Models;
public partial class Vaccine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MinAgeMonths { get; set; }

    public int MaxAgeMonths { get; set; }

    public bool IsMandatory { get; set; }

    public virtual ICollection<BabyVaccine> BabyVaccines { get; set; } = new List<BabyVaccine>();
}
