using Crayon.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Entity.Data
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Employee> EmployeeSet { get; set; }
        public DbSet<Department> DepartmentSet { get; set; }
        public DbSet<Designation> DesignationSet { get; set; }
        public DbSet<Workplace> WorkplaceSet { get; set; }
        public DbSet<Attendance> AttendanceSet { get; set; }
        public DbSet<AttendanceLog> AttendanceLogSet { get; set; }
        public DbSet<LeaveType> LeaveTypeSet { get; set; }
        public DbSet<LeaveBalance> LeaveBalanceSet { get; set; }
        public DbSet<LeaveApplication> LeaveApplicationSet { get; set; }
        public DbSet<Project> ProjectSet { get; set; }
        public DbSet<ProjectTask> ProjectTaskSet { get; set; }
        public DbSet<Timesheet> TimesheetSet { get; set; }
        public DbSet<Holiday> HolidaySet { get; set; }
        public DbSet<ApprovalWorkflow> ApprovalWorkflowSet { get; set; }
        public DbSet<Notification> NotificationSet { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.Project)
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Timesheet>()
                .HasOne(t => t.ProjectTask)
                .WithMany()
                .HasForeignKey(t => t.ProjectTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Attendance>()
            .Property(a => a.HoursWorked)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Timesheet>()
                .Property(t => t.HoursWorked)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LeaveBalance>()
                .Property(l => l.AvailableDays)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Employee>()
        .HasOne(e => e.ReportingToEmployee)
        .WithMany(e => e.Subordinates)
        .HasForeignKey(e => e.ReportingToEmployeeId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
