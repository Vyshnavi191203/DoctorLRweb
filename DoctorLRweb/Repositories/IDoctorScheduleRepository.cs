using DoctorLRweb.Models;

namespace DoctorLRweb.Repositories
{
    public interface IDoctorScheduleRepository
    {
        void AddDoctorSchedule(DoctorSchedule schedule);
        void UpdateDoctorSchedule(DoctorSchedule schedule);
        void DeleteDoctorSchedule(int scheduleId);
        IEnumerable<DoctorSchedule> GetDoctorSchedules();
        IEnumerable<DoctorSchedule> GetByDepartment(string department);
        IEnumerable<DoctorSchedule> GetByTimeSlot(TimeOnly timeSlot);
        public IEnumerable<DoctorSchedule> GetByDoctorId(int doctorId);
        public void DeletePastSchedules();


    }
}


