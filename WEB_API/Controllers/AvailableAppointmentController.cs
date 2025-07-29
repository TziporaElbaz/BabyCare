using BL.API;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableAppointmentsController : ControllerBase
    {
        private readonly IAvailableAppointmentsManagementBL _availableAppointmentsManagementBL;

        public AvailableAppointmentsController(IAvailableAppointmentsManagementBL availableAppointmentsManagementBL)
        {
            _availableAppointmentsManagementBL = availableAppointmentsManagementBL;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAvailableAppointments()
        {
            var appointments = await _availableAppointmentsManagementBL.GetAllAvailableAppointments();
            var result = appointments.Select(a => new
            {
                a.Id,
                a.Worker.WorkerId,
                a.Worker.Name,
                a.Worker.WorkerType,
                a.AppointmentDate,
                a.StartTime,
                a.EndTime
            });

            return Ok(result);
        }

        [HttpGet("findAvailableAppointmentsByDate")]
        public async Task<IActionResult> FindAvailableAppointmentsByDate(DateOnly date)
        {

            var appointments = await _availableAppointmentsManagementBL.FindAllAvailableAppointmentsByDate(date);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound($"No available appointments found for date: {date}");
            }
            var result = appointments.Select(a => new
            {
                a.Id,
                a.Worker.WorkerId,
                a.Worker.Name,
                a.Worker.WorkerType,
                a.AppointmentDate,
                a.StartTime,
                a.EndTime
            });

            return Ok(result);
        }

        [HttpGet("nurse/{babyId}")]
        public async Task<IActionResult> FindNurseAppointments(string babyId)
        {
            try
            {
                var appointments = await _availableAppointmentsManagementBL.findNurseAppointments(babyId);
                if (appointments == null || appointments.Count == 0)
                    return NotFound("No available nurse appointments found.");

                var result = appointments.Select(a => new
                {
                    a.Id,
                    a.WorkerId,
                    a.AppointmentDate,
                    a.StartTime,
                    a.EndTime
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("getAvailableAppointmentsByWorkerType")]
        public async Task<IActionResult> GetAvailableAppointmentsByWorkerType(string workerType)
        {
            List<AvailableAppointmentDTO> appointments = await _availableAppointmentsManagementBL.FindSpecificTypeOfAvailableAppointments(workerType);
            if (appointments == null || appointments.Count == 0)
                return NotFound(workerType + " appointments not found.");

            var result = appointments.Select(appointment => new
            {
                appointment.Id,
                appointment.AppointmentDate,
                appointment.StartTime,
                appointment.EndTime,
                appointment.Worker.Name,
                appointment.Worker.WorkerType
            }).ToList();

            return Ok(result);
        }

        [HttpGet("is-time-slot-available")]
        public async Task<IActionResult> IsTimeSlotAvailable(DateOnly date, TimeOnly startTime, string workerType)
        {
            var isAvailable = await _availableAppointmentsManagementBL.IsTimeSlotAvailableAsync(date, startTime, workerType);
            return Ok(isAvailable);
        }

        [HttpGet("findPhysicalTherapistAppointments")]
        public async Task<IActionResult> FindPhysicalTherapistAppointments(string physiotherapistName, DateOnly startDate, int sessionsCount)
        {
            List<AvailableAppointment> appointments = await _availableAppointmentsManagementBL.findPhysicalTherapistAppointments(physiotherapistName, startDate, sessionsCount);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound($"No available series found for {physiotherapistName} starting {startDate}");
            }
            var result = appointments.Select(a => new
            {
                a.Id,
                a.WorkerId,
                a.AppointmentDate,
                a.StartTime,
                a.EndTime
            });
            return Ok(result);
        }


        [HttpPost("add-appointments")]
        public async Task<IActionResult> AddAvailableAppointmentsToWorkers(DateTime date)
        {
            try
            {
                await _availableAppointmentsManagementBL.AddAvailableAppointmentsToAllWorkers(date);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding available appointments: {ex.Message}");
            }
        }
        [HttpPost("add-appointments-for-next-year")]
        public async Task<IActionResult> AddAvailableAppointmentsForNextYear()
        {
            try
            {
                await _availableAppointmentsManagementBL.AddAvailableAppointmentsForNextYear();
                return Ok("Appointments for next year added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

