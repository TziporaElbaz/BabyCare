using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.Models;

namespace BL.Models
{
    public class AvailableAppointmentBL
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

    }
}

