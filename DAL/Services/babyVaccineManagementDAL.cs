using WEB_API.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WEB_API.DAL.Services
{
    public class BabyVaccineManagementDAL : IBabyVaccineManagementDAL
    {
        private readonly myDatabase _context;

        public BabyVaccineManagementDAL(myDatabase context)
        {
            _context = context;
        }

        public async Task<BabyVaccine> CreateAsync(string babyId, int vaccineId)
        {
            // חיפוש התינוק לפי ה-ID
           
            var baby = await _context.Set<Baby>().FirstOrDefaultAsync(b => b.BabyId.Equals(babyId) );
            if (baby == null)
           
                throw new ArgumentException("תינוק לא נמצא");
            
        // חיפוש החיסון לפי ה-ID

        var vaccine = await _context.Set<Vaccine>().FirstOrDefaultAsync(v => v.Id == vaccineId);
            if (vaccine == null)
            {
                throw new ArgumentException("חיסון לא נמצא");
            }

            // כאן תוכל להוסיף לוגיקה עסקית נוספת אם יש צורך

          
            BabyVaccine babyVaccine = new BabyVaccine(baby, vaccine);
            _context.Set<BabyVaccine>().Add(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;

        }
        //שליפת כל החיסונים של תינוק מסוים
        public async Task<List<Vaccine>> GetVaccinesAsync(string babyId)
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby) // Includes the Baby object (optional if not directly used)
                                 .Include(bv => bv.Vaccine) // Includes the Vaccine object
                                 .Where(bv => bv.Baby.BabyId == babyId) // Filters by BabyId
                                 .Select(bv => bv.Vaccine) // Selects only the Vaccine object
                                 .ToListAsync(); // Converts the result to a list
        }
        //שליפת כל החיסונים של התינוקות
        public async Task<IEnumerable<BabyVaccine>> GetAllAsync()
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby)
                                 .Include(bv => bv.Vaccine)
                                 .ToListAsync();
        }
        //עדכון פרטי תינוק
        public async Task<BabyVaccine> UpdateAsync(BabyVaccine babyVaccine)
        {
            _context.Set<BabyVaccine>().Update(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;
        }

        public async Task DeleteAsync(string babyId, string vaccine)
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


