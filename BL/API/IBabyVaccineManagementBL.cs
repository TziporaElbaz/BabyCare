using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace WEB_API.BL.API
{
    public interface IBabyVaccineManagementBL
    {
        Task<BabyVaccine> CreateBabyVaccineAsync(string babyId, int vaccineId);
        Task DeleteBabyVaccineAsync(string babyId, string vaccine);
        Task<IEnumerable<BabyVaccine>> GetAllBabyVaccinesAsync();
        Task<List<VaccineDTO>> GetVaccinesByBabyIdAsync(string babyId);
        Task<BabyVaccine> UpdateBabyVaccineAsync(BabyVaccine babyVaccine);
    }
}