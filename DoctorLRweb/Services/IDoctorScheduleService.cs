using DoctorLRweb.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace DoctorLRweb.Services
{

    public interface IDoctorScheduleService
    {
        void AddDoctorSchedule(DoctorSchedule schedule);
        void UpdateDoctorSchedule(DoctorSchedule schedule);
        void DeleteDoctorSchedule(int scheduleId);
        IEnumerable<DoctorSchedule> GetDoctorSchedules();
        IEnumerable<DoctorSchedule> GetByDepartment(string department);
        IEnumerable<DoctorSchedule> GetByTimeSlot(TimeOnly timeSlot);
        IEnumerable<DoctorSchedule> GetSchedulesByDoctorId(int doctorId);
        public void DeletePastSchedules();

    }
}
