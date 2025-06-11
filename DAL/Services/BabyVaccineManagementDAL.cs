using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WEB_API.Models;


namespace DAL.Services
{
    public class BabyVaccineManagementDAL
    {
        
            private readonly DbContext _context;

            public BabyVaccineManagementDAL(DbContext context)
            {
                _context = context;
            }

            public async Task<BabyVaccine> CreateAsync(Baby baby, Vaccine vaccine)
            {
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


