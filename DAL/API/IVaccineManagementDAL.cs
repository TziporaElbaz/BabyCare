
using WEB_API.Models;

namespace WEB_API.DAL.API
{
    public interface IVaccineManagementDAL
    {
        Task AddVaccineAsync(Vaccine vaccine);
        Task DeleteVaccineAsync(string name);
        Task<List<Vaccine>> GetAllVaccinesAsync();
        Task<Vaccine?> GetVaccineByIdAsync(string name);
        Task UpdateVaccineDetailsAsync(Vaccine updatedVaccine);
    }
}