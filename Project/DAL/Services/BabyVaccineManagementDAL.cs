namespace Project.DAL.services
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Project.DAL.models;
    using Project.models;

    public class BabyVaccineManagement
    {
        private readonly DbContext _context;

        public BabyVaccineManagement(DbContext context)
        {
            _context = context;
        }

        public async Task<BabyVaccine> CreateAsync(BabyVaccine babyVaccine)
        {
            _context.Set<BabyVaccine>().Add(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;
        }

        public async Task<BabyVaccine> GetAsync(int id)
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby)
                                 .Include(bv => bv.Vaccine)
                                 .FirstOrDefaultAsync(bv => bv.Id == id);
        }

        public async Task<IEnumerable<BabyVaccine>> GetAllAsync()
        {
            return await _context.Set<BabyVaccine>()
                                 .Include(bv => bv.Baby)
                                 .Include(bv => bv.Vaccine)
                                 .ToListAsync();
        }

        public async Task<BabyVaccine> UpdateAsync(BabyVaccine babyVaccine)
        {
            _context.Set<BabyVaccine>().Update(babyVaccine);
            await _context.SaveChangesAsync();
            return babyVaccine;
        }

        public async Task DeleteAsync(int id)
        {
            var babyVaccine = await _context.Set<BabyVaccine>().FindAsync(id);
            if (babyVaccine != null)
            {
                _context.Set<BabyVaccine>().Remove(babyVaccine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
