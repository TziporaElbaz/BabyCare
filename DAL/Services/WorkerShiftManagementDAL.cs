using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BabyCare.DAL.API;
using BabyCare.DAL.Models;

namespace BabyCare.DAL.Services
{
    public class WorkerShiftManagementDAL : IWorkerShiftManagementDAL
    {
        private readonly DbContext _context;
        private readonly IWorkersManagmentDAL _workersManagmentDAL;

        // Constructor שמקבל DbContext
        public WorkerShiftManagementDAL(DbContext context, IWorkersManagmentDAL workersManagmentDAL)
        {
            _context = context;
            _workersManagmentDAL = workersManagmentDAL;
        }

        // פונקציה שמוסיפה קשר בין עובד ומשמרת ושומרת בדאטה בייס
        public async Task AddWorkerShiftAsync(Worker worker, Shift shift)
        {
            // בדיקה אם העובד והמשמרת אינם null
            if (worker == null) throw new ArgumentNullException(nameof(worker), "Worker cannot be null.");
            if (shift == null) throw new ArgumentNullException(nameof(shift), "Shift cannot be null.");

            // יצירת אובייקט חדש של WorkerShift
            var workerShift = new WorkerShift(shift, worker)
            {
                WorkerId = worker.Id,   // מזהה העובד
                ShiftId = shift.Id      // מזהה המשמרת
            };

            // הוספת הקשר לטבלה
            _context.Set<WorkerShift>().Add(workerShift);

            // שמירת השינויים בדאטה בייס
            await _context.SaveChangesAsync();
        }

        // מחיקת קשר בין עובד למשמרת לפי ID
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

        // שליפת כל הקשרים בין עובדים למשמרות
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
                        .Where(ws => ws.ShiftId == shiftId)
                        .Include(ws => ws.Worker)
                        .Select(ws => ws.Worker) // Select only the Worker entities
                        .ToListAsync();
        }
        public async Task<List<Shift>> GetShiftsByWorkerID(int workerId)
        {
            return await _context.Set<WorkerShift>()
                        .Where(ws => ws.WorkerId == workerId)
                        .Include(ws => ws.Shift)
                        .Select(ws => ws.Shift) // Select only the Worker entities
                        .ToListAsync();
        }

        // עדכון פרטי קשר בין עובד למשמרת
        public async Task UpdateWorkerShiftDetailsAsync(WorkerShift updatedWorkerShift)
        {
            if (updatedWorkerShift == null) throw new ArgumentNullException(nameof(updatedWorkerShift));

            var existingWorkerShift = await _context.Set<WorkerShift>()
                                                    .FindAsync(updatedWorkerShift.Id);

            if (existingWorkerShift == null)
            {
                throw new KeyNotFoundException($"WorkerShift with ID {updatedWorkerShift.Id} was not found.");
            }

            // עדכון השדות
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

        public Task<List<Shift>> GetShiftByWorkerID(int workerId)
        {
            throw new NotImplementedException();
        }
    }
}