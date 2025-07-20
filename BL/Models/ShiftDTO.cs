namespace WEB_API.BL.Models
{
    public class ShiftDTO
    {
        public int Id { get; set; }

        public int Day { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string ShiftType { get; set; } = null!;

    }
}
