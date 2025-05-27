using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;

using WEB_API.Models;

namespace WEB_API.DAL.Services
{
    public class AppointmentManagementDAL : IAppointmentManagementDAL
    {
        private readonly DbContext _context;
        //private readonly IMapper _mapper;

        public AppointmentManagementDAL(DbContext context)
        {
            _context = context;
        }

        // Add a new appointment
        public async Task AddAppointmentAsync(Appointment appointment)
        {
            _context.Set<Appointment>().Add(appointment);
            await _context.SaveChangesAsync();
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
        public async Task<Appointment?> GetAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time)
        {
            return await _context.Set<Appointment>().FirstOrDefaultAsync(a => a.WorkerId == workerId && a.AppointmentDate == date && a.StartTime == time);
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
                .Where(a => a.AppointmentDate == date)
                .ToListAsync();
        }

        // Check if a time slot is available
        public async Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return !await _context.Set<Appointment>().AnyAsync(a =>
                a.AppointmentDate == date &&
                ((startTime >= a.StartTime && startTime < a.EndTime) ||
                 (endTime > a.StartTime && endTime <= a.EndTime)));
        }

        //public Task AddAppointmentAsync(Appointment appointment)
        //{
        //    throw new NotImplementedException();
        //}

        Task<List<Appointment>> IAppointmentManagementDAL.GetAllAppointmentsAsync()
        {
            throw new NotImplementedException();
        }

        Task<Appointment?> IAppointmentManagementDAL.GetAppointmentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Appointment?> IAppointmentManagementDAL.GetAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time)
        {
            throw new NotImplementedException();
        }

        Task<List<Appointment>> IAppointmentManagementDAL.GetAppointmentsByDateAsync(DateOnly date)
        {
            throw new NotImplementedException();
        }

        //public Task UpdateAppointmentAsync(Appointment updatedAppointment)
        //{
        //    throw new NotImplementedException();
        //}
    }
}