using WEB_API.BL.Models;

namespace WEB_API.BL.API
{
    public interface IVaccineManagementBL
    {
        Task<List<VaccineDTO>> ListOfBabysUnvaccinatedVaccines(string babyId);
        Task<Dictionary<string, string>> ListOfBabysVaccines(string id);
        Task<List<VaccineDTO>> ShowUpcomingVaccines(string babyId);
    }
}