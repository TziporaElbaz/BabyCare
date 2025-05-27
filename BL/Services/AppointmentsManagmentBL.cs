using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.API;
using DAL.API;
using WEB_API.DAL.API;
using WEB_API.DAL.Services;
using WEB_API.Models;

namespace BL.Services
{
    public class AppointmentsManagementBL : IAppointmentsManagementBL
    {
        IAvailableAppointmentManagementDAL availableAppointmentManagementDAL;
        IVaccineManagementBL vaccineManagementBL;
        IAppointmentManagementDAL appointmentManagementDAL;
        IBabyManagementBL babyManagementBL;
        public AppointmentsManagementBL(IVaccineManagementBL _vaccineManagementBL, IAppointmentManagementDAL _appointmentManagementDAL, IBabyManagementBL _babyManagementBL, IAvailableAppointmentManagementDAL _availableAppointmentDAL)
        {
            vaccineManagementBL = _vaccineManagementBL;
            appointmentManagementDAL = _appointmentManagementDAL;
            babyManagementBL = _babyManagementBL;
            availableAppointmentManagementDAL = _availableAppointmentDAL;
        }


        public List<Appointment> getBookedAppointmentsForBaby(string babyId)
        {
            List<Appointment> appointments = appointmentManagementDAL.GetAllAppointmentsAsync().Result;
            var babyAppointments = appointments
                .Where(a => a.BabyId.ToString() == babyId && a.AppointmentDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Today)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToList();


            return babyAppointments;
        }
        public List<Appointment> GetBabyAppointmentsHistory(string babyId)
        {
            List<Appointment> appointments = appointmentManagementDAL.GetAllAppointmentsAsync().Result;


            var babyAppointments = appointments
                .Where(a => a.BabyId.ToString() == babyId)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToList();


            return babyAppointments;
        }
        public DateTime LastVisit(string babyId)
        {

            List<Appointment> babyAppointments = GetBabyAppointmentsHistory(babyId);


            if (!babyAppointments.Any())
            {
                throw new Exception("No appointments found for the given baby ID.");
            }
            Appointment lastAppointment = babyAppointments.Last(); // מאחר והרשימה כבר מסודרת לפי תאריך
            return new DateTime(
                lastAppointment.AppointmentDate.Year,
                lastAppointment.AppointmentDate.Month,
                lastAppointment.AppointmentDate.Day,
                lastAppointment.StartTime.Hour,
                lastAppointment.StartTime.Minute,
                lastAppointment.StartTime.Second
            );
        }
    }
}

