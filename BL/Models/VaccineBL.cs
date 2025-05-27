using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.Models;

namespace BL.Models
{
    public class VaccineBL
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int MinAgeMonths { get; set; }

        public int MaxAgeMonths { get; set; }

        public bool IsMandatory { get; set; }

    }
}
