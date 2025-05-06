using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.DAL.API;
using Project.Models;

namespace Project.DAL.Services
{
    public class WorkerShiftManagementDAL : IWorkerShiftManagementDAL
    {
        private readonly DbContext _context;

        // Constructor שמקבל DbContext
        public WorkerShiftManagementDAL(DbContext context)
        {
            _context = context;
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

            
            // שליפת כל הקשרים לפי מזהה משמרת
            public async Task<List<WorkerShift>> GetWorkerShiftsByShiftIdAsync(int shiftId)
            {
                return await _context.Set<WorkerShift>()
                                     .Where(ws => ws.ShiftId == shiftId)
                                     .Include(ws => ws.Worker)
                                     .Include(ws => ws.Shift)
                                     .ToListAsync();
            }

            // שליפת כל הקשרים לפי מזהה עובד
            public async Task<List<WorkerShift>> GetWorkerShiftsByWorkerIdAsync(int workerId)
            {
                return await _context.Set<WorkerShift>()
                                     .Where(ws => ws.WorkerId == workerId)
                                     .Include(ws => ws.Worker)
                                     .Include(ws => ws.Shift)
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