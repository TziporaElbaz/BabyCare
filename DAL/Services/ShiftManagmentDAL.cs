using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.Models;

namespace WEB_API.DAL.Services
{
    public class ShiftManagementDAL : IShiftManagementDAL
    {
        private readonly DbContext _context;

        public ShiftManagementDAL(DbContext context)
        {
            _context = context;
        }

        public async Task AddShiftAsync(Shift shift)
        {
            _context.Set<Shift>().Add(shift);
            await _context.SaveChangesAsync();
        }

        // מחיקת משמרת לפי ID
        public async Task DeleteShiftAsync(int day, string shiftType)
        {
            var shift = await _context.Set<Shift>().FirstOrDefaultAsync(s => s.Day == day && s.ShiftType == shiftType);
            if (shift != null)
            {
                _context.Set<Shift>().Remove(shift);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Shift?> GetShiftByIdAsync(int id)
        {
            return await _context.Set<Shift>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Shift>> GetShiftsByDayAsync(int day)
        {
            return await _context.Set<Shift>()
                         .Where(s => s.Day == day) // מסנן לפי יום
                         .ToListAsync();
        }


        public async Task<List<Shift>> GetAllShiftsAsync()
        {
            return await _context.Set<Shift>().ToListAsync();
        }


        public async Task UpdateShiftDetailsAsync(Shift updatedShift)
        {
            var existingShift = await _context.Set<Shift>().FirstOrDefaultAsync(s => s.Id == updatedShift.Id);
            if (existingShift == null)
            {
                throw new KeyNotFoundException($"Shift with ID {updatedShift.Id} not found.");
            }

            existingShift.Day = updatedShift.Day != default ? updatedShift.Day : existingShift.Day;
            existingShift.StartTime = updatedShift.StartTime != default ? updatedShift.StartTime : existingShift.StartTime;
            existingShift.EndTime = updatedShift.EndTime != default ? updatedShift.EndTime : existingShift.EndTime;
            existingShift.ShiftType = updatedShift.ShiftType ?? existingShift.ShiftType;

            await _context.SaveChangesAsync();
        }
    }
}