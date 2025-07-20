using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.Models;


namespace WEB_API.DAL.Services
{
    public class BabyVaccineManagementDAL : IBabyVaccineManagementDAL
    {
        private readonly myDatabase _context;

        public BabyVaccineManagementDAL(myDatabase context)
        {
            _context = context;
        }

        public async Task<BabyVaccine> AddBabyVaccineAsync(Baby baby, Vaccine vaccine, DateOnly date)
        {
            var babyVaccine = new BabyVaccine(baby, vaccine, date);
            _context.Set<BabyVaccine>().Add(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;
        }

        //שליפת כל החיסונים של תינוק מסוים
        public async Task<List<Vaccine>> GetVaccinesAsync(string babyId)
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby)
                                 .Include(bv => bv.Vaccine)
                                 .Where(bv => bv.Baby.BabyId == babyId)
                                 .Select(bv => bv.Vaccine)
                                 .ToListAsync();
        }

        public async Task<BabyVaccine?> GetBabyVaccineAsync(string babyId, int vaccineId)
        {
            return await _context.Set<BabyVaccine>()
                                             .Include(bv => bv.Baby)
                                             .Include(bv => bv.Vaccine)
                                             .FirstOrDefaultAsync(bv => bv.Baby.BabyId == babyId && bv.VaccineId == vaccineId);
        }

        //שליפת כל החיסונים של התינוקות
        public async Task<IEnumerable<BabyVaccine>> GetAllVaccinesAsync()
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby)
                                 .Include(bv => bv.Vaccine)
                                 .ToListAsync();
        }

        //עדכון פרטי תינוק
        public async Task<BabyVaccine> UpdateVaccineAsync(BabyVaccine babyVaccine)
        {
            _context.Set<BabyVaccine>().Update(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;
        }

        //מחיקת חיסון מתינוק
        public async Task DeleteVaccineAsync(string babyId, string vaccine)
        {
            var babyVaccine = await _context.Set<BabyVaccine>().FirstOrDefaultAsync(bv => bv.Baby.BabyId == babyId && bv.Vaccine.Name == vaccine);
            if (babyVaccine != null)
            {
                _context.Set<BabyVaccine>().Remove(babyVaccine);
                await _context.SaveChangesAsync();
            }
        }
    }
}


