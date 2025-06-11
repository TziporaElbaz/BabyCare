using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyCare.BL.Models
{
    public class AppointmentBL
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        public int BabyId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        //public virtual Baby Baby { get; set; } = null!;

        //public virtual Worker Worker { get; set; } = null!;
    }
}
