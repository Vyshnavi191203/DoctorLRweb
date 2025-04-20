
using System.Collections.Generic;
using DoctorLRweb.Models;
using DoctorLRweb.Repositories;
using DoctorLRweb.Services;


namespace DoctorLRweb.Services
{
    public class DoctorScheduleServices : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _repository;
        public DoctorScheduleServices(IDoctorScheduleRepository repository)
        {
            _repository = repository;
        }
        public void AddDoctorSchedule(DoctorSchedule schedule)
        {
            _repository.AddDoctorSchedule(schedule);
        }
        public void UpdateDoctorSchedule(DoctorSchedule schedule)
        {
            _repository.UpdateDoctorSchedule(schedule);
        }
        public void DeleteDoctorSchedule(int scheduleId)
        {
            _repository.DeleteDoctorSchedule(scheduleId);
        }
        public IEnumerable<DoctorSchedule> GetDoctorSchedules()
        {
            return _repository.GetDoctorSchedules();
        }
        public IEnumerable<DoctorSchedule> GetByDepartment(string department)
        {
            return _repository.GetByDepartment(department);
        }
        public IEnumerable<DoctorSchedule> GetByTimeSlot(TimeOnly timeSlot)
        {
            return _repository.GetByTimeSlot(timeSlot);
        }
        public IEnumerable<DoctorSchedule> GetSchedulesByDoctorId(int doctorId)
        {
            return _repository.GetByDoctorId(doctorId);
        }
        public void DeletePastSchedules()
        {
            _repository.DeletePastSchedules();
        }
    }
}