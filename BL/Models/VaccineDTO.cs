namespace WEB_API.BL.Models
{
    public class VaccineDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int MinAgeMonths { get; set; }

        public int MaxAgeMonths { get; set; }

        public bool IsMandatory { get; set; }

    }
}
