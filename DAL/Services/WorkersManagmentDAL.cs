
using Microsoft.EntityFrameworkCore;
using Project.DAL.API;
using Project.Models;

namespace Project.DAL.Services
{
    public class WorkersManagmentDAL : IWorkersManagmentDAL
    {
        private readonly DbContext _context;

        public WorkersManagmentDAL(DbContext context)
        {
            _context = context;
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            _context.Set<Worker>().Add(worker);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkerAsync(int id, string name)
        {
            var worker = await _context.Set<Worker>().FirstOrDefaultAsync(w => w.Id == id && w.Name == name);
            if (worker != null)
            {
                _context.Set<Worker>().Remove(worker);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Worker?> GetWorkerByIdAsync(int id)
        {
            return await _context.Set<Worker>().FirstOrDefaultAsync(w => w.Id == id);
        }


        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _context.Set<Worker>().ToListAsync();
        }
        public async Task UpdateWorkerDetailsAsync(Worker updatedWorker)
        {
            var existingWorker = await _context.Set<Worker>().FirstOrDefaultAsync(w => w.Id == updatedWorker.Id);
            if (existingWorker == null)
            {
                throw new KeyNotFoundException($"Worker with ID {updatedWorker.Id} not found.");
            }
            existingWorker.Name = updatedWorker.Name ?? existingWorker.Name;
            existingWorker.Address = updatedWorker.Address ?? existingWorker.Address;
            existingWorker.Phone = updatedWorker.Phone ?? existingWorker.Phone;
            existingWorker.Email = updatedWorker.Email ?? existingWorker.Email;
            existingWorker.WorkerType = updatedWorker.WorkerType ?? existingWorker.WorkerType;
            existingWorker.Salary = updatedWorker.Salary != default ? updatedWorker.Salary : existingWorker.Salary;
            existingWorker.Experience = updatedWorker.Experience ?? existingWorker.Experience;
            await _context.SaveChangesAsync();
        }
    }
}