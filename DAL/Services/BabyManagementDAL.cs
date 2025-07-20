using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
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
            return await _context.Set<Baby>().Include(b => b.Appointments)
                         .FirstOrDefaultAsync(b => b.BabyId.Equals(id));
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

        public async Task AddAppointmentToBaby(Baby baby, Appointment appointment)
        {
            baby.Appointments.Add(appointment);
            _context.Set<Baby>().Update(baby);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetBabyAppointments(Baby baby)
        {
            var appointments = await _context.Set<Appointment>()
                .Include(a => a.Worker)
                .Where(a => a.BabyId == baby.Id)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return appointments;
        }
    }
}
