using WEB_API.BL.API;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WEB_API.DAL.API;
using BL.API;

public class AppointmentBackgroundService : BackgroundService
{
    private readonly IAppointmentManagementDAL _appointmentDAL;
    private readonly IAvailableAppointmentManagementDAL _availableAppointmentDAL;
    private readonly IEmailService _emailService;
    private readonly IAvailableAppointmentsManagementBL _appointmentsManagementBL;
    public AppointmentBackgroundService(
        IAppointmentManagementDAL appointmentDAL,
        IAvailableAppointmentManagementDAL availableAppointmentDAL,
        IAvailableAppointmentsManagementBL appointmentsManagementBL,
        IEmailService emailService)
    {
        _appointmentDAL = appointmentDAL;
        _availableAppointmentDAL = availableAppointmentDAL;
        _emailService = emailService;
        _appointmentsManagementBL = appointmentsManagementBL;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await SendRemindersAsync();
            await DeleteOldAvailableAppointmentsAsync();

            // Wait 24 hours before running again
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task SendRemindersAsync()
    {
        var tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var appointments = await _appointmentDAL.GetAppointmentsByDateAsync(tomorrow);

        foreach (var appointment in appointments)
        {
            var baby = appointment.Baby;
            if (!string.IsNullOrEmpty(baby.ParentEmail))
            {
                string subject = "תזכורת לתור מחר";
                string body = $"שלום {baby.Name}, יש לך תור מחר ב-{appointment.StartTime} אצל {appointment.Worker.Name}.";
                await _emailService.SendEmail(baby.ParentEmail, subject, body);
            }
        }
    }

    private async Task DeleteOldAvailableAppointmentsAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var availableAppointments = await _availableAppointmentDAL.GetAllAvailableAppointmentsAsync();
        var expired = availableAppointments.Where(a => a.AppointmentDate < today).ToList();

        foreach (var appointment in expired)
        {
            await _availableAppointmentDAL.DeleteAvailableAppointmentAsync(appointment.Id);
        }
    }
    private DateTime _lastYearlyRun = DateTime.MinValue;

    protected async Task ExecuteAsyncApp(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await SendRemindersAsync();
            await DeleteOldAvailableAppointmentsAsync();

            // יצירת תורים לכל השנה פעם בשנה
            await RunYearlyAppointmentsIfNeeded();

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task RunYearlyAppointmentsIfNeeded()
    {
        var today = DateTime.Today;
        // בדוק אם זה 1 בינואר והשנה לא רצה עדיין
        if (today.Month == 7 && today.Day == 28 && _lastYearlyRun.Year != today.Year)
        {
            await _appointmentsManagementBL.AddAvailableAppointmentsForNextYear();
            _lastYearlyRun = today;
        }
    }
}