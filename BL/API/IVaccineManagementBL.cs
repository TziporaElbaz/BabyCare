using WEB_API.DAL.Models;

namespace WEB_API.BL.API
{
    public interface IVaccineManagementBL
    {
        Task<List<Vaccine>> ListOfBabysUnvaccinatedVaccines(string babyId);
        Task<Dictionary<string, bool>> ListOfBabysVaccines(string id);
        Task<List<Vaccine>> ShowUpcomingVaccines(string babyId);
    }
}