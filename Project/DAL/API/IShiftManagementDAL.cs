using Project.models;

namespace Project.DAL.API
{
    public interface IShiftManagementDAL
    {
        Task AddShiftAsync(Shift shift);
        Task DeleteShiftAsync(int day, string shiftType);
        Task<List<Shift>> GetAllShiftsAsync();
        Task<Shift?> GetShiftByDayAsync(int day, string shiftType);
        Task<Shift?> GetShiftByIdAsync(int day, string shiftType);
        Task UpdateShiftDetailsAsync(Shift updatedShift);
    }
}