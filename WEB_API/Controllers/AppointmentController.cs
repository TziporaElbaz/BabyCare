using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.BL.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {

        private readonly IAppointmentsManagementBL _appointmentsManagementBL;

        public AppointmentsController(IAppointmentsManagementBL appointmentsManagementBL)
        {
            _appointmentsManagementBL = appointmentsManagementBL;
        }

        [HttpPost("add")]
        public async Task<IActionResult> BookAppointment(string babyId, string workerType, DateOnly date, TimeOnly time)
        {
            var appointment = await _appointmentsManagementBL.BookAppointment(babyId, workerType, date, time);
            if (appointment == null)
            {
                return BadRequest("Appointment could not be created. Please check the details and try again.");
            }
            return Ok(new
            {
                appointment.Id,
                BabyName = appointment.Baby.Name,
                Worker = appointment.Worker.WorkerType,
                WorkerName = appointment.Worker.Name
            });
        }

        [HttpPost("bookVaccineAppointment")]
        public async Task<IActionResult> BookVaccineAppointment(string babyId, DateOnly date, TimeOnly time, int vaccineId)
        {
            var appointment = await _appointmentsManagementBL.BookVaccineAppointment(babyId, date, time, vaccineId);
            if (appointment == null)
            {
                return BadRequest("Appointment could not be created. Please check the details and try again.");
            }
            return Ok(new
            {
                appointment.Id,
                BabyName = appointment.Baby.Name,
                Worker = appointment.Worker.WorkerType,
                WorkerName = appointment.Worker.Name
            });
        }

        [HttpGet("booked/{babyId}")]
        public async Task<ActionResult<List<object>>> GetBookedAppointmentsForBaby(string babyId)
        {
            List<AppointmentDTO> appointments = await _appointmentsManagementBL.GetUpcomingAppointmentsForBaby(babyId);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound($"No appointments found for baby with ID: {babyId}");
            }

            var result = appointments.Select(appointment => new
            {
                appointment.Id,
                BabyName = appointment.Baby?.Name ?? "Unknown",
                WorkerName = appointment.Worker?.Name ?? "Unknown",
                WorkerType = appointment.Worker?.WorkerType ?? "Unknown",
                Date = appointment.AppointmentDate,
                Hour = appointment.StartTime
            }).ToList();

            return Ok(result);
        }

        [HttpGet("getWorkerAppointments/{workerId}")]
        public async Task<ActionResult<List<AppointmentDTO>>> GetWorkerAppointments(string workerId)
        {
            List<AppointmentDTO>? appointments = await _appointmentsManagementBL.GetWorkerAppointments(workerId);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No appointments booked or invalid id");
            }
            var result = appointments.Select(appointment => new
            {
                appointment.Id,
                WorkerName = appointment.Worker?.Name ?? "Unknown",
                WorkerType = appointment.Worker?.WorkerType ?? "Unknown",
                BabyId = appointment.Baby?.BabyId ?? "Unknown",
                BabyName = appointment.Baby?.Name ?? "Unknown",
                appointment.AppointmentDate,
                Hour = appointment.StartTime
            }).ToList();

            return Ok(result);
        }

        [HttpGet("by-date")]
        public async Task<IActionResult> GetAppointmentsByDate(DateOnly date)
        {
            var appointments = await _appointmentsManagementBL.GetAppointmentsByDateAsync(date);
            var result = appointments.Select(appointment => new
            {
                appointment.Id,
                WorkerId = appointment.Worker?.WorkerId ?? "Unknown",
                WorkerName = appointment.Worker?.Name ?? "Unknown",
                WorkerType = appointment.Worker?.WorkerType ?? "Unknown",
                BabyId = appointment.Baby?.BabyId ?? "Unknown",
                BabyName = appointment.Baby?.Name ?? "Unknown",
                appointment.AppointmentDate,
                appointment.StartTime,
                appointment.EndTime
            }).ToList();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentsManagementBL.DeleteAppointmentAsync(id);
            return NoContent();
        }

        //[HttpGet("worker/{workerId}/datetime")]
        //public async Task<IActionResult> GetAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time)
        //{
        //    var appointment = await _appointmentsManagementBL.GetAppointmentByWorkerAndDatetime(workerId, date, time);
        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(appointment);
        //}

        [HttpGet("history/{babyId}")]
        public async Task<ActionResult<List<object>>> GetBabyAppointmentsHistory(string babyId)
        {
            {
                var appointments = await _appointmentsManagementBL.GetBabyAppointmentsHistory(babyId);
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound($"No appointments found for baby with ID: {babyId}");
                }

                var result = appointments.Select(appointment => new
                {
                    appointment.Id,
                    BabyName = appointment.Baby?.Name ?? "Unknown",
                    WorkerName = appointment.Worker?.Name ?? "Unknown",
                    WorkerType = appointment.Worker?.WorkerType ?? "Unknown",
                    Date = appointment.AppointmentDate,
                    Hour = appointment.StartTime
                }).ToList();

                return Ok(result);
            }

            //[HttpGet("last-visit/{babyId}")]
            //public IActionResult LastVisit(string babyId)
            //{
            //    try
            //    {
            //        var lastVisitDate = _appointmentsManagementBL.LastVisit(babyId);
            //        return Ok(lastVisitDate);
            //    }
            //    catch (Exception ex)
            //    {
            //        return NotFound(ex.Message);
            //    }
            //}
        }

        [HttpGet("amountOfMonthlyAppointmentsStatistics")]
        public async Task<ActionResult<Dictionary<string, int>>> GetMonthlyAppointmentStatistics()
        {
            var statistics = await _appointmentsManagementBL.GetMonthlyAppointmentStatistics();
            if (statistics == null || statistics.Count == 0)
            {
                return NotFound("No appointment statistics found.");
            }
            return Ok(statistics);
        }
    }
}

