﻿namespace WEB_API.BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        Task AddAvailableAppointmentsToWorkers(DateTime day);
        Task<bool> IsHoliday(DateTime date);
    }
}