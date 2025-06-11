using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public class VaccineManagementDAL : IVaccineManagementDAL
    {
        private readonly myDatabase _context;

        public VaccineManagementDAL(myDatabase context)
        {
            _context = context;
        }


        public async Task AddVaccineAsync(Vaccine vaccine)
        {
            _context.Set<Vaccine>().Add(vaccine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVaccineAsync(string name)
        {
            var vaccine = await _context.Set<Vaccine>().FirstOrDefaultAsync(v => v.Name == name);
            if (vaccine != null)
            {
                _context.Set<Vaccine>().Remove(vaccine);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Vaccine?> GetVaccineByIdAsync(string name)
        {
            return await _context.Set<Vaccine>().FirstOrDefaultAsync(v => v.Name == name);
        }


        public async Task<List<Vaccine>> GetAllVaccinesAsync()
        {
            return await _context.Set<Vaccine>().ToListAsync();
        }


        public async Task UpdateVaccineDetailsAsync(Vaccine updatedVaccine)
        {
            var existingVaccine = await _context.Set<Vaccine>().FirstOrDefaultAsync(v => v.Id == updatedVaccine.Id);
            if (existingVaccine == null)
            {
                throw new KeyNotFoundException($"Vaccine with ID {updatedVaccine.Id} not found.");
            }

            existingVaccine.Name = updatedVaccine.Name ?? existingVaccine.Name;
            existingVaccine.MinAgeMonths = updatedVaccine.MinAgeMonths != default ? updatedVaccine.MinAgeMonths : existingVaccine.MinAgeMonths;
            existingVaccine.MaxAgeMonths = updatedVaccine.MaxAgeMonths != default ? updatedVaccine.MaxAgeMonths : existingVaccine.MaxAgeMonths;
            existingVaccine.IsMandatory = updatedVaccine.IsMandatory;

            await _context.SaveChangesAsync();
        }
    }
}