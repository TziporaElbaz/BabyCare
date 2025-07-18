﻿
using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public class WorkersManagementDAL : IWorkersManagmentDAL
    {
        private readonly myDatabase _context;

        public WorkersManagementDAL(myDatabase context)
        {
            _context = context;
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            _context.Set<Worker>().Add(worker);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkerAsync(string id, string name)
        {
            var worker = await _context.Set<Worker>().FirstOrDefaultAsync(w => w.WorkerId.Equals(id )&& w.Name == name);
            if (worker != null)
            {
                _context.Set<Worker>().Remove(worker);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Worker?> GetWorkerByIdAsync(string id)
        {
            return await _context.Set<Worker>().FirstOrDefaultAsync(w => w.WorkerId.Equals(id));
        }


        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _context.Set<Worker>().ToListAsync();
        }
        public async Task UpdateWorkerDetailsAsync(Worker updatedWorker)
        {
            var existingWorker = await _context.Set<Worker>().FirstOrDefaultAsync(w => w.WorkerId.Equals(updatedWorker.WorkerId));
            if (existingWorker == null)
            {
                throw new KeyNotFoundException($"Worker with ID {updatedWorker.WorkerId} not found.");
            }
            existingWorker.Name = updatedWorker.Name ?? existingWorker.Name;
            existingWorker.Address = updatedWorker.Address ?? existingWorker.Address;
            existingWorker.Phone = updatedWorker.Phone ?? existingWorker.Phone;
            existingWorker.Email = updatedWorker.Email ?? existingWorker.Email;
            existingWorker.WorkerType = updatedWorker.WorkerType ?? existingWorker.WorkerType;
            existingWorker.Salary = updatedWorker.Salary != default ? updatedWorker.Salary : existingWorker.Salary;
            existingWorker.Experience = updatedWorker.Experience ?? existingWorker.Experience;
            _context.Set<Worker>().Update(existingWorker);
           
            await _context.SaveChangesAsync();
        }
    }
}