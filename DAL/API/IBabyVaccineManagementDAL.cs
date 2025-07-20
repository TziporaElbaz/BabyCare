
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public interface IBabyVaccineManagementDAL
    {
        Task<BabyVaccine> AddBabyVaccineAsync(Baby baby, Vaccine vaccine, DateOnly date);
        Task DeleteVaccineAsync(string babyId, string vaccine);
        Task<IEnumerable<BabyVaccine>> GetAllVaccinesAsync();
        Task<BabyVaccine?> GetBabyVaccineAsync(string babyId, int vaccineId);
        Task<List<Vaccine>> GetVaccinesAsync(string babyId);
        Task<BabyVaccine> UpdateVaccineAsync(BabyVaccine babyVaccine);
    }
}