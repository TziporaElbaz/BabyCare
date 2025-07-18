﻿
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public interface IBabyVaccineManagementDAL
    {
        Task<BabyVaccine> CreateAsync(string babyId, int vaccineId);
        Task DeleteAsync(string babyId, string vaccine);
        Task<IEnumerable<BabyVaccine>> GetAllAsync();
        Task<List<Vaccine>> GetVaccinesAsync(string babyId);
        Task<BabyVaccine> UpdateAsync(BabyVaccine babyVaccine);
    }
}