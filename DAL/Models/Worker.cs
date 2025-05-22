using System;
using System.Collections.Generic;
using Project.DAL.Models;

namespace Project.Models;

public partial class Worker
{
    public int Id { get; set; }

    public string WorkerId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool Gender { get; set; }

    public DateOnly Birthdate { get; set; }

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string WorkerType { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateOnly StartDate { get; set; }

    public int? Experience { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<AvailableAppointment> AvailableAppointments { get; set; } = new List<AvailableAppointment>();

    public virtual ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
}
