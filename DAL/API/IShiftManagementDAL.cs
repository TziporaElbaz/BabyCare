
using Project.Models;

namespace Project.DAL.API
{
    public interface IShiftManagementDAL
    {
        Task AddShiftAsync(Shift shift);
        Task DeleteShiftAsync(int day, string shiftType);
        Task<List<Shift>> GetAllShiftsAsync();
        Task<List<Shift>> GetShiftsByDayAsync(int day);
        Task<Shift?> GetShiftByIdAsync(int id);
        Task UpdateShiftDetailsAsync(Shift updatedShift);
    }
}