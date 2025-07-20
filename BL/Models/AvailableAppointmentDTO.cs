using WEB_API.DAL.Models;

namespace WEB_API.BL.Models
{
    public class AvailableAppointmentDTO
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
        public Worker Worker { get; internal set; }
    }
}

