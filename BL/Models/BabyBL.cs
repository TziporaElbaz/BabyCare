using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyCare.BL.Models
{
    public class BabyBL
    {
        public string BabyId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public string? MotherName { get; set; }

        public string? FatherName { get; set; }

        public string ParentPhone { get; set; } = null!;

        public string? ParentEmail { get; set; }

        public string Address { get; set; } = null!;

    }
}
