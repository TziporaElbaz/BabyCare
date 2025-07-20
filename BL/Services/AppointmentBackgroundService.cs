//using WEB_API.BL.API;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using WEB_API.DAL.API;

//public class AppointmentBackgroundService : BackgroundService
//{
//    private readonly IAppointmentManagementDAL _appointmentDAL;
//    private readonly IAvailableAppointmentManagementDAL _availableAppointmentDAL;
//    private readonly IEmailService _emailService;

//    public AppointmentBackgroundService(
//        IAppointmentManagementDAL appointmentDAL,
//        IAvailableAppointmentManagementDAL availableAppointmentDAL,
//        IEmailService emailService)
//    {
//        _appointmentDAL = appointmentDAL;
//        _availableAppointmentDAL = availableAppointmentDAL;
//        _emailService = emailService;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            await SendRemindersAsync();
//            await DeleteOldAvailableAppointmentsAsync();

//            // Wait 24 hours before running again
//            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
//        }
//    }

//    private async Task SendRemindersAsync()
//    {
//        var tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
//        var appointments = await _appointmentDAL.GetAppointmentsByDateAsync(tomorrow);

//        foreach (var appointment in appointments)
//        {
//            var baby = appointment.Baby;
//            if (!string.IsNullOrEmpty(baby.ParentEmail))
//            {
//                string subject = "תזכורת לתור מחר";
//                string body = $"שלום {baby.Name}, יש לך תור מחר ב-{appointment.StartTime} אצל {appointment.Worker.Name}.";
//                await _emailService.SendEmail(baby.ParentEmail, subject, body);
//            }
//        }
//    }

//    private async Task DeleteOldAvailableAppointmentsAsync()
//    {
//        var today = DateOnly.FromDateTime(DateTime.Now);
//        var availableAppointments = await _availableAppointmentDAL.GetAllAvailableAppointmentsAsync();
//        var expired = availableAppointments.Where(a => a.AppointmentDate < today).ToList();

//        foreach (var appointment in expired)
//        {
//            await _availableAppointmentDAL.DeleteAvailableAppointmentAsync(appointment.Id);
//        }
//    }
//}