using Microsoft.EntityFrameworkCore;
using BabyCare.DAL.API;
using BabyCare.DAL.Models;

namespace BabyCare.DAL.Services
{
    public class BabyManagementDAL : IBabyManagementDAL
    {
        private readonly myDatabase _context;

        public BabyManagementDAL(myDatabase context)
        {
            _context = context;
        }

        public async Task<Baby?> GetBabyByIdAsync(string id)
        {
            return await _context.Set<Baby>().FirstOrDefaultAsync(b => b.BabyId.Equals(id));
        }

        public async Task AddBabyAsync(Baby baby)
        {
            await _context.Set<Baby>().AddAsync(baby);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBabyAsync(Baby baby)
        {
            _context.Set<Baby>().Remove(baby);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Baby>> GetAllBabiesAsync()
        {
            return await _context.Set<Baby>().ToListAsync();
        }

        public async Task UpdateBabyDetailsAsync(Baby updatedBaby)
        {
            _context.Set<Baby>().Update(updatedBaby);
            await _context.SaveChangesAsync();
        }
    }
}
