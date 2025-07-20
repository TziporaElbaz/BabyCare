namespace WEB_API.BL.Models
{
    public class WorkerDTO
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
