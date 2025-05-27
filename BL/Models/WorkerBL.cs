using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class WorkerBL
    {
        public int Id { get; set; }

        public string WorkerId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public string WorkerType { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public int? Experience { get; set; }

    }
}
