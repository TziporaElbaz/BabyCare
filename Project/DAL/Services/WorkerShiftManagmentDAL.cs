using Microsoft.EntityFrameworkCore;
using Project.models;

namespace Project.DAL.services
{
    public class WorkerShiftManagementDAL : IWorkerShiftManagementDAL
    {
        private readonly DbContext _context;

        public WorkerShiftManagementDAL(DbContext context)
        {
            _context = context;
        }


        public async Task AddWorkerShiftAsync(WorkerShift workerShift)
        {
            _context.Set<WorkerShift>().Add(workerShift);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteWorkerShiftAsync(int id)
        {
            var workerShift = await _context.Set<WorkerShift>().FirstOrDefaultAsync(ws => ws.Id == id);
            if (workerShift != null)
            {
                _context.Set<WorkerShift>().Remove(workerShift);
                await _context.SaveChangesAsync();
            }
        }



        public async Task<WorkerShift?> GetWorkerShiftByIdAsync(int id)
        {
            return await _context.Set<WorkerShift>()
                .Include(ws => ws.Worker)
                .Include(ws => ws.Shift)
                .FirstOrDefaultAsync(ws => ws.Id == id);
        }

        // Get all WorkerShifts
        public async Task<List<WorkerShift>> GetAllWorkerShiftsAsync()
        {
            return await _context.Set<WorkerShift>()
                .Include(ws => ws.Worker)
                .Include(ws => ws.Shift)
                .ToListAsync();
        }
        public async Task<List<WorkerShift>> GetWorkerShiftsByWorkerIdAsync(int workerId)
        {
            return await _context.Set<WorkerShift>()
                .Where(ws => ws.WorkerId == workerId)
                .Include(ws => ws.Worker)
                .Include(ws => ws.Shift)
                .ToListAsync();
        }
        public async Task<List<WorkerShift>> GetWorkerShiftsByShiftIdAsync(int day)
        {
            return await _context.Set<WorkerShift>()
                .Where(ws => ws.ShiftId == day)
                .Include(ws => ws.Worker)
                .Include(ws => ws.Shift)
                .ToListAsync();
        }


        public async Task UpdateWorkerShiftDetailsAsync(WorkerShift updatedWorkerShift)
        {
            var existingWorkerShift = await _context.Set<WorkerShift>().FirstOrDefaultAsync(ws => ws.Id == updatedWorkerShift.Id);
            if (existingWorkerShift == null)
            {
                throw new KeyNotFoundException($"WorkerShift with ID {updatedWorkerShift.Id} not found.");
            }

            existingWorkerShift.WorkerId = updatedWorkerShift.WorkerId != 0 ? updatedWorkerShift.WorkerId : existingWorkerShift.WorkerId;
            existingWorkerShift.ShiftId = updatedWorkerShift.ShiftId != 0 ? updatedWorkerShift.ShiftId : existingWorkerShift.ShiftId;

            await _context.SaveChangesAsync();
        }


    }
}