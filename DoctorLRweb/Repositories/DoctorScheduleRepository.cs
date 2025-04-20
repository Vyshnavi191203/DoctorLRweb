using DoctorLRweb.Models;
using DoctorLRweb.Data;
using Microsoft.EntityFrameworkCore;

namespace DoctorLRweb.Repositories
{

    public class DoctorScheduleRepository : IDoctorScheduleRepository
    {
        private readonly Context _context;
        public DoctorScheduleRepository(Context context)
        {
            _context = context;
        }
        public void AddDoctorSchedule(DoctorSchedule schedule)
        {
            try
            {
                _context.Database.ExecuteSqlInterpolated($@"
               EXEC InsertDoctorSchedule {schedule.DoctorId}, {schedule.Department}, {schedule.AvailableDate}, {schedule.TimeSlot}");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add schedule: " + ex.Message);
            }
        }
        public void UpdateDoctorSchedule(DoctorSchedule schedule)
        {
            try
            {
                _context.Database.ExecuteSqlInterpolated($@"
               EXEC UpdateDoctorSchedule {schedule.ScheduleId}, {schedule.DoctorId}, {schedule.Department}, {schedule.AvailableDate}, {schedule.TimeSlot}");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update schedule: " + ex.Message);
            }
        }
        public void DeleteDoctorSchedule(int scheduleId)
        {
            _context.Database.ExecuteSqlInterpolated($@"
           EXEC DeleteDoctorSchedule {scheduleId}");
        }
        public IEnumerable<DoctorSchedule> GetDoctorSchedules()
        {
            return _context.DoctorSchedules.FromSqlRaw("SELECT * FROM DoctorSchedules").ToList();
        }
        public IEnumerable<DoctorSchedule> GetByDepartment(string department)
        {
            return _context.DoctorSchedules.FromSqlInterpolated($@"
           EXEC GetDoctorSchedulesByDepartment {department}").ToList();
        }
        public IEnumerable<DoctorSchedule> GetByTimeSlot(TimeOnly timeSlot)
        {
            return _context.DoctorSchedules.FromSqlInterpolated($@"
           EXEC GetDoctorSchedulesByTimeSlot {timeSlot}").ToList();
        }
        public IEnumerable<DoctorSchedule> GetByDoctorId(int doctorId)
        {
            return _context.DoctorSchedules.FromSqlInterpolated($@"
           EXEC GetDoctorSchedulesByDoctorId {doctorId}").ToList();
        }
        public void DeletePastSchedules()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var pastSchedules = _context.DoctorSchedules
                .Where(s => s.AvailableDate < today)
                .ToList();

            if (pastSchedules.Any())
            {
                _context.DoctorSchedules.RemoveRange(pastSchedules);
                _context.SaveChanges();
            }
        }
    }
}
