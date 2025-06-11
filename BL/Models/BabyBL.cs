
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.BL.API;

namespace WEB_API.BL.Models
{
    public class BabyBL
    {
      

        public int Id { get; set; }

        public string BabyId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public string? MotherName { get; set; }

        public string? FatherName { get; set; }

        public string ParentPhone { get; set; } = null!;

        public string? ParentEmail { get; set; }

        public string Address { get; set; } = null!;
        public double Weight { get; set; }

        public double Height { get; set; }
        public bool Gender { get; set; }


      


    }
}
