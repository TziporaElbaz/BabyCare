namespace WEB_API.DAL.Models
{
    public partial class BabyVaccine
    {
        public BabyVaccine() { }

        public BabyVaccine(Baby baby, Vaccine vaccine, DateOnly date)
        {
            Baby = baby;
            Vaccine = vaccine;
            BabyId = baby.Id;
            VaccineId = vaccine.Id;
            DateGiven = date;
        }

        public int Id { get; set; }

        public int BabyId { get; set; }

        public int VaccineId { get; set; }

        public DateOnly DateGiven { get; set; }

        public virtual Baby Baby { get; set; } = null!;

        public virtual Vaccine Vaccine { get; set; } = null!;
    }
}