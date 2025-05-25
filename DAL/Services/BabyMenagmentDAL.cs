using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.Models;

namespace WEB_API.DAL.Services
{
    public class BabyManagementDAL : IBabyManagementDAL
    {
        private readonly DbContext _context;

        public BabyManagementDAL(DbContext context)
        {
            _context = context;
        }

        public async Task AddBabyAsync(Baby baby)
        {
            _context.Set<Baby>().Add(baby);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBabyAsync(int id)
        {
            var baby = await _context.Set<Baby>().FirstOrDefaultAsync(b => b.Id == id);
            if (baby != null)
            {
                _context.Set<Baby>().Remove(baby);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Baby?> GetBabyByIdAsync(int id)
        {
            return await _context.Set<Baby>().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Baby>> GetAllBabiesAsync()
        {
            return await _context.Set<Baby>().ToListAsync();
        }

        public async Task UpdateBabyDetailsAsync(Baby updatedBaby)
        {
            var existingBaby = await _context.Set<Baby>().FirstOrDefaultAsync(b => b.Id == updatedBaby.Id);
            if (existingBaby == null)
            {
                throw new KeyNotFoundException($"Baby with ID {updatedBaby.Id} not found.");
            }

            existingBaby.Name = updatedBaby.Name ?? existingBaby.Name;
            existingBaby.Birthdate = updatedBaby.Birthdate != default ? updatedBaby.Birthdate : existingBaby.Birthdate;
            existingBaby.Gender = updatedBaby.Gender;
            existingBaby.Weight = updatedBaby.Weight != default ? updatedBaby.Weight : existingBaby.Weight;
            existingBaby.Height = updatedBaby.Height != default ? updatedBaby.Height : existingBaby.Height;
            existingBaby.HeadCircumference = updatedBaby.HeadCircumference != default ? updatedBaby.HeadCircumference : existingBaby.HeadCircumference;
            existingBaby.BirthWeight = updatedBaby.BirthWeight != default ? updatedBaby.BirthWeight : existingBaby.BirthWeight;
            existingBaby.IsInGrowthCurve = updatedBaby.IsInGrowthCurve;
            existingBaby.MotherName = updatedBaby.MotherName ?? existingBaby.MotherName;
            existingBaby.FatherName = updatedBaby.FatherName ?? existingBaby.FatherName;
            existingBaby.ParentPhone = updatedBaby.ParentPhone ?? existingBaby.ParentPhone;
            existingBaby.ParentEmail = updatedBaby.ParentEmail ?? existingBaby.ParentEmail;
            existingBaby.Address = updatedBaby.Address ?? existingBaby.Address;

            await _context.SaveChangesAsync();
        }
    }
}