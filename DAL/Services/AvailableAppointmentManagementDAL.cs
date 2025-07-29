using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public class AvailableAppointmentManagementDAL : IAvailableAppointmentManagementDAL
    {

        private readonly myDatabase _context;

        public AvailableAppointmentManagementDAL(myDatabase context)
        {
            _context = context;
        }
        public async Task<List<AvailableAppointment>> GetAllAvailableAppointmentsAsync()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            return await _context.Set<AvailableAppointment>()
                        .Include(a => a.Worker)
                        .Where(a => a.AppointmentDate >= currentDate)
                        .OrderBy(a => a.AppointmentDate)
                        .ThenBy(a => a.StartTime)
                        .ThenBy(a => a.Worker.WorkerType)
                        .ToListAsync();
        }

        public async Task<List<AvailableAppointment>> GetAppointmentsByDateAsync(DateOnly date)
        {
            return await _context.Set<AvailableAppointment>()
                .Include(a => a.Worker)
                .Where(a => a.AppointmentDate == date)
                .ToListAsync();
        }
        

        public async Task DeleteAvailableAppointmentAsync(int id)
        {
            var appointment = await _context.Set<AvailableAppointment>().FirstOrDefaultAsync(a => a.Id == id);
            if (appointment != null)
            {
                _context.Set<AvailableAppointment>().Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAvailableAppointmentAsync(AvailableAppointment appointment)
        {
            _context.Set<AvailableAppointment>().Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AvailableAppointment>> GetAppointmentsByWorkerType(string workerType)
        {
            return await _context.Set<AvailableAppointment>()
                .Include(a => a.Worker)
                .Where(a => a.Worker.WorkerType.Equals(workerType) &&
                            a.AppointmentDate >= DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
        }

        //public async Task<List<AvailableAppointment>> GetAppointmentsByWorkerTypeAndMonth(string workerType, int month)
        //{
        //    return await _context.Set<AvailableAppointment>()
        //        .Include(a => a.Worker.WorkerType.Equals(workerType) && a.AppointmentDate.Month == month)
        //        .ToListAsync(); ;
        //}
        public async Task<AvailableAppointment?> GetAvailableAppointmentByWorkerAndDatetime(DateOnly date, TimeOnly time, string workerType)
        {
            return await _context.Set<AvailableAppointment>()
                .Include(a => a.Worker)
                .Where(a => a.AppointmentDate == date && a.StartTime == time && a.Worker.WorkerType.Equals(workerType))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return await _context.Set<AvailableAppointment>().AnyAsync(a =>
                a.AppointmentDate == date &&
                (startTime == a.StartTime && endTime == a.EndTime));
        }

    }
}


