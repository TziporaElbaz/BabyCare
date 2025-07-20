using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public class AppointmentManagementDAL : IAppointmentManagementDAL
    {
        private readonly myDatabase _context;

        public AppointmentManagementDAL(myDatabase context)
        {
            _context = context;
        }

        // Delete an appointment by ID
        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Set<Appointment>().FirstOrDefaultAsync(a => a.Id == id);
            if (appointment != null)
            {
                _context.Set<Appointment>().Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        // Get an appointment by ID
        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _context.Set<Appointment>().FirstOrDefaultAsync(a => a.Id == id);
        }

        //public async Task<Appointment?> GetAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time)
        //{
        //    return await _context.Set<Appointment>().FirstOrDefaultAsync(a => a.WorkerId == workerId && a.AppointmentDate == date && a.StartTime == time);
        //}

        public async Task AddAppointment(Appointment appointment)
        {
            _context.Set<Appointment>().Add(appointment);
            await _context.SaveChangesAsync();
        }

        // Get all appointments
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Set<Appointment>().ToListAsync();
        }

        // Update an existing appointment
        public async Task UpdateAppointmentAsync(Appointment updatedAppointment)
        {
            var existingAppointment = await _context.Set<Appointment>().FirstOrDefaultAsync(a => a.Worker == updatedAppointment.Worker && a.AppointmentDate == updatedAppointment.AppointmentDate && a.StartTime == updatedAppointment.StartTime);
            if (existingAppointment == null)
            {
                throw new KeyNotFoundException($"Appointment with Worker {updatedAppointment.Worker}\n in date {updatedAppointment.AppointmentDate}\n in time {updatedAppointment.StartTime} not found.");
            }

            existingAppointment.WorkerId = updatedAppointment.WorkerId;
            existingAppointment.BabyId = updatedAppointment.BabyId;
            existingAppointment.AppointmentDate = updatedAppointment.AppointmentDate;
            existingAppointment.StartTime = updatedAppointment.StartTime;
            existingAppointment.EndTime = updatedAppointment.EndTime;

            await _context.SaveChangesAsync();
        }

        // Get appointments by date
        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateOnly date)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Worker)
                .Include(a => a.Baby)
                .Where(a => a.AppointmentDate == date)
                .ToListAsync();
        }
    }
}