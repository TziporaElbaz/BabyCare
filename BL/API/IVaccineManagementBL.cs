using WEB_API.Models;

namespace BL.API
{
    public interface IVaccineManagementBL
    {
        List<Vaccine> ListOfBabysUnvaccinatedVaccines(string babyId);
        Dictionary<Vaccine, bool> ListOfBabysVaccines(string id);
        List<Vaccine> ShowUpcomingVaccines(string babyId);
    }
}