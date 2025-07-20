using Microsoft.EntityFrameworkCore;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.DAL.Services
{
    public class WorkerShiftManagementDAL : IWorkerShiftManagementDAL
    {
        private readonly myDatabase _context;

        public WorkerShiftManagementDAL(myDatabase context)
        {
            _context = context;
        }

        public async Task AddWorkerShiftAsync(Worker worker, Shift shift)
        {
            var workerShift = new WorkerShift(shift, worker);
            _context.Set<WorkerShift>().Add(workerShift);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkerShiftAsync(int id)
        {
            var workerShift = await _context.Set<WorkerShift>().FindAsync(id);
            if (workerShift == null)
            {
                throw new KeyNotFoundException($"WorkerShift with ID {id} was not found.");
            }

            _context.Set<WorkerShift>().Remove(workerShift);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WorkerShift>> GetAllWorkerShiftsAsync()
        {
            return await _context.Set<WorkerShift>()
                                 .Include(ws => ws.Worker)
                                 .Include(ws => ws.Shift)
                                 .ToListAsync();
        }

        public async Task<List<Worker>> GetWorkersByShiftID(int shiftId)
        {
            return await _context.Set<WorkerShift>()
                       .Include(ws => ws.Worker)
             .Where(ws => ws.ShiftId == shiftId)
             .Select(ws => ws.Worker)
             .ToListAsync();
        }

        public async Task<List<Shift>> GetShiftByWorkerID(int workerId)
        {
            return await _context.Set<WorkerShift>()
                        .Include(ws => ws.Shift)
                        .Where(ws => ws.WorkerId == workerId)
                        .Select(ws => ws.Shift)
                        .ToListAsync();
        }

        public async Task UpdateWorkerShiftDetailsAsync(WorkerShift updatedWorkerShift)
        {
            if (updatedWorkerShift == null) throw new ArgumentNullException(nameof(updatedWorkerShift));

            var existingWorkerShift = await _context.Set<WorkerShift>()
                                                    .FindAsync(updatedWorkerShift.Id);

            if (existingWorkerShift == null)
            {
                throw new KeyNotFoundException($"WorkerShift with ID {updatedWorkerShift.Id} was not found.");
            }

            existingWorkerShift.WorkerId = updatedWorkerShift.WorkerId;
            existingWorkerShift.ShiftId = updatedWorkerShift.ShiftId;
            existingWorkerShift.Worker = updatedWorkerShift.Worker;
            existingWorkerShift.Shift = updatedWorkerShift.Shift;

            _context.Set<WorkerShift>().Update(existingWorkerShift);
            await _context.SaveChangesAsync();
        }

        public Task<WorkerShift?> GetWorkerShiftByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<WorkerShift>> GetWorkerShiftsByShiftIdAsync(int day)
        {
            throw new NotImplementedException();
        }

        //public async Task AssignAllWorkersToAllShifts()
        //{
        //    var shifts = _context.WorkerShifts.OrderBy(s => s.Id).ToList();

        //    foreach (var shift in shifts)
        //    {
        //        var newShift = new WorkerShift
        //        {
        //            WorkerId = shift.WorkerId,
        //            ShiftId = shift.ShiftId,
        //            Shift = shift.Shift, // Assuming you want to keep the same Shift object
        //            Worker = shift.Worker  // Assuming you want to keep the same Worker object
        //        };

        //        _context.WorkerShifts.Add(newShift);
        //    }

        //    await _context.SaveChangesAsync();

        //    _context.WorkerShifts.RemoveRange(shifts);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task AssignAllWorkersToShiftsForDayAsync(int dayOfWeek)
        //{
        //    var shifts = _context.Shifts.Where(s => s.Day == dayOfWeek).ToList();
        //    var workers = _context.Workers.ToList();

        //    foreach (var shift in shifts)
        //    {
        //        foreach (var worker in workers)
        //        {
        //            bool exists = _context.WorkerShifts.Any(ws => ws.ShiftId == shift.Id && ws.WorkerId == worker.Id);
        //            if (!exists)
        //            {
        //                _context.WorkerShifts.Add(new WorkerShift
        //                {
        //                    WorkerId = worker.Id,
        //                    ShiftId = shift.Id
        //                });
        //            }
        //        }
        //    }
        //    await _context.SaveChangesAsync();
        //}
    }
}
