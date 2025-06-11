using WEB_API.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_API.DAL.API;

namespace WEB_API.DAL.Services
{
    public class AvailableAppointmentManagementDAL : IAvailableAppointmentManagementDAL
    {
        private readonly myDatabase _context;

        public AvailableAppointmentManagementDAL(myDatabase context)
        {
            _context = context;
        }

        // Add a new available appointment
        public async Task AddAvailableAppointmentAsync(AvailableAppointment appointment)
        {
            _context.Set<AvailableAppointment>().Add(appointment);
            await _context.SaveChangesAsync();
        }

        // Delete an available appointment by ID
        public async Task DeleteAvailableAppointmentAsync(int id)
        {
            var appointment = await _context.Set<AvailableAppointment>().FirstOrDefaultAsync(a => a.Id == id);
            if (appointment != null)
            {
                _context.Set<AvailableAppointment>().Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        // Get an available appointment by ID
        public async Task<AvailableAppointment?> GetAvailableAppointmentByIdAsync(int id)
        {
            return await _context.Set<AvailableAppointment>().FirstOrDefaultAsync(a => a.Id == id);
        }

        // Get all available appointments
        public async Task<List<AvailableAppointment>> GetAllAvailableAppointmentsAsync()
        {
            return await _context.Set<AvailableAppointment>().ToListAsync();
        }

        // Update an existing available appointment
        public async Task UpdateAvailableAppointmentAsync(AvailableAppointment updatedAppointment)
        {
            var existingAppointment = await _context.Set<AvailableAppointment>()
                .FirstOrDefaultAsync(a => a.Id == updatedAppointment.Id);

            if (existingAppointment == null)
                throw new KeyNotFoundException($"AvailableAppointment with Id {updatedAppointment.Id} not found.");

            existingAppointment.WorkerId = updatedAppointment.WorkerId;
            existingAppointment.AppointmentDate = updatedAppointment.AppointmentDate;
            existingAppointment.StartTime = updatedAppointment.StartTime;
            existingAppointment.EndTime = updatedAppointment.EndTime;

            await _context.SaveChangesAsync();
        }

        // Get an available appointment by worker ID, date, and time
        public async Task<AvailableAppointment?> GetAvailableAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time)
        {
            return await _context.Set<AvailableAppointment>()
                .FirstOrDefaultAsync(a => a.WorkerId == workerId && a.AppointmentDate == date && a.StartTime == time);
        }

        // Get all available appointments for a specific date
        public async Task<List<AvailableAppointment>> GetAvailableAppointmentsByDateAsync(DateOnly date)
        {
            return await _context.Set<AvailableAppointment>()
                .Where(a => a.AppointmentDate == date)
                .ToListAsync();
        }

        // Check if a time slot is available
        public async Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            return !await _context.Set<AvailableAppointment>().AnyAsync(a =>
                a.AppointmentDate == date &&
                ((startTime >= a.StartTime && startTime < a.EndTime) ||
                 (endTime > a.StartTime && endTime <= a.EndTime)));
        }

    }
}