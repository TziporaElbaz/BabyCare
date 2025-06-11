using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    internal class AvailableAppointmentDAL : IAvailableAppointmentDAL
    {

        private readonly myDatabase _context;

        public AvailableAppointmentDAL(myDatabase context)
        {
            _context = context;
        }
        public async Task<List<AvailableAppointment>> GetAllAppointmentsAsync()
        {
            return await _context.Set<AvailableAppointment>().ToListAsync();
        }
        public async Task<List<AvailableAppointment>> GetAppointmentsByDateAsync(DateOnly date)
        {
            return await _context.Set<AvailableAppointment>()
                .Where(a => a.AppointmentDate == date)
                .ToListAsync();
        }
        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Set<AvailableAppointment>().FirstOrDefaultAsync(a => a.Id == id);
            if (appointment != null)
            {
                _context.Set<AvailableAppointment>().Remove(appointment);
                await _context.SaveChangesAsync();
            }

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

        public async Task<AvailableAppointment?> GetAppointmentById(int appointmentsId)
        {
            return await _context.Set<AvailableAppointment>().FirstOrDefaultAsync(a => a.Id == appointmentsId);
        }
        public async Task<List<AvailableAppointment?>> GetAppointmentsByWorkerType(string workerType)
        {
            return await _context.Set<AvailableAppointment>().Where(a => a.Worker.WorkerType.Equals(workerType)).ToListAsync();
        }
        public async Task<List<AvailableAppointment?>> GetAppointmentsByWorkerTypeAndMonth(string workerType, int month)
        {
            return await _context.Set<AvailableAppointment>().Include(a => a.Worker.WorkerType.Equals(workerType) && a.AppointmentDate.Month == month).ToListAsync(); ;
        }

    }

}


