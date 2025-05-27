using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using WEB_API.DAL.Models;

namespace WEB_API.Models;

public partial class myDatabase : DbContext
{
    public myDatabase()
    {
    }

    public myDatabase(DbContextOptions<myDatabase> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AvailableAppointment> AvailableAppointments { get; set; }

    public virtual DbSet<Baby> Babies { get; set; }

    public virtual DbSet<BabyVaccine> BabyVaccines { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Vaccine> Vaccines { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    public virtual DbSet<WorkerShift> WorkerShifts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer(
        @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Data\Database.mdf;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC071E71E022");

            entity.HasOne(d => d.Baby).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.BabyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__BabyI__75A278F5");

            entity.HasOne(d => d.Worker).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Worke__74AE54BC");
        });

        modelBuilder.Entity<AvailableAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availabl__3214EC071D588593");

            entity.HasOne(d => d.Worker).WithMany(p => p.AvailableAppointments)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Available__Worke__71D1E811");
        });

        modelBuilder.Entity<Baby>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Babies__3214EC0785878223");

            entity.HasIndex(e => e.BabyId, "UQ__Babies__7915D7CE37AB41D4").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.BabyId)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FatherName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MotherName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ParentEmail)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ParentPhone)
                .HasMaxLength(10)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<BabyVaccine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BabyVacc__3214EC0783E636F2");

            entity.HasOne(d => d.Baby).WithMany(p => p.BabyVaccines)
                .HasForeignKey(d => d.BabyId)
                .HasConstraintName("FK__BabyVacci__BabyI__6E01572D");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.BabyVaccines)
                .HasForeignKey(d => d.VaccineId)
                .HasConstraintName("FK__BabyVacci__Vacci__6EF57B66");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shifts__3214EC07A0229852");

            entity.Property(e => e.ShiftType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vaccines__3214EC07CB5D1C48");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Workers__3214EC075101FAA7");

            entity.HasIndex(e => e.WorkerId, "UQ__Workers__077C88271664773A").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Experience).HasComputedColumnSql("(case when [StartDate]<=getdate() then datediff(year,[StartDate],getdate()) else (0) end)", false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WorkerId)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WorkerType)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<WorkerShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkerSh__3214EC07EBEFF841");

            entity.HasOne(d => d.Shift).WithMany(p => p.WorkerShifts)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkerShi__Shift__6B24EA82");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkerShifts)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkerShi__Worke__6A30C649");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
