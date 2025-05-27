using WEB_API.Models;

namespace DAL.Services
{
    public interface IBabyVaccineManagementDAL
    {
        Task<BabyVaccine> CreateAsync(Baby baby, Vaccine vaccine);
        Task DeleteAsync(string babyId, string vaccine);
        Task<IEnumerable<BabyVaccine>> GetAllAsync();
        Task<List<Vaccine>> GetVaccinesAsync(string babyId);
        Task<BabyVaccine> UpdateAsync(BabyVaccine babyVaccine);
    }
}