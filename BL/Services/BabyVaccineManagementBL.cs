using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_API.BL.API;
using WEB_API.DAL.Models;
using WEB_API.DAL.Services;

namespace WEB_API.BL.Services
{
    public class BabyVaccineManagementBL : IBabyVaccineManagementBL
    {

        private readonly IBabyVaccineManagementDAL _babyVaccineManagementDAL;

        public BabyVaccineManagementBL(IBabyVaccineManagementDAL babyVaccineManagementDAL)
        {
            _babyVaccineManagementDAL = babyVaccineManagementDAL;
        }

        public async Task<BabyVaccine> CreateBabyVaccineAsync(string babyId, int vaccineId)
        {
           
            // כאן תוכל להוסיף לוגיקה עסקית נוספת אם יש צורך
            return await _babyVaccineManagementDAL.CreateAsync(babyId, vaccineId);
        }

        public async Task<List<Vaccine>> GetVaccinesByBabyIdAsync(string babyId)
        {
            return await _babyVaccineManagementDAL.GetVaccinesAsync(babyId);
        }

        public async Task<IEnumerable<BabyVaccine>> GetAllBabyVaccinesAsync()
        {
            return await _babyVaccineManagementDAL.GetAllAsync();
        }

        public async Task<BabyVaccine> UpdateBabyVaccineAsync(BabyVaccine babyVaccine)
        {
            return await _babyVaccineManagementDAL.UpdateAsync(babyVaccine);
        }

        public async Task DeleteBabyVaccineAsync(string babyId, string vaccine)
        {
            await _babyVaccineManagementDAL.DeleteAsync(babyId, vaccine);
        }
    }
}
