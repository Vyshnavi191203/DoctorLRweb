using DoctorLRweb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorLRweb.Data
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public Context() { }

        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure DoctorSchedule
            modelBuilder.Entity<DoctorSchedule>()
                .HasKey(ds => ds.ScheduleId);


            // Configure MedicalHistory
            modelBuilder.Entity<MedicalHistory>()
                .HasKey(mh => mh.HistoryID);

            // Configure User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}