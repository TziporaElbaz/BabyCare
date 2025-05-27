using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.Models;

namespace BL.Models
{
    public class ShiftBL
    {
        public int Id { get; set; }

        public int Day { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string ShiftType { get; set; } = null!;

    }
}
