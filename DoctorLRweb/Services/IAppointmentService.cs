using DoctorLRweb.Models;

namespace DoctorLRweb.Services
{
    public interface IAppointmentService
    {
        List<Appointment> GetAllAppointments();
      
        int CreateAppointment(int userId, int doctorId, DateOnly appointmentDate, TimeOnly timeSlot);
        bool DeleteAppointment(int appointmentId);
        void RescheduleAppointment(int appointmentId, DateOnly newAppointmentDate, TimeOnly newTimeSlot);
        List<string> GetAvailableDepartments();
        List<User> GetDoctorsByDepartment(string department);
        List<DoctorSchedule> GetDoctorsByDateWithDoctorName(DateOnly availableDate);
        public List<dynamic> GetAppointmentsByPatientId(int patientId);
        public List<Appointment> GetAppointmentsByDoctorId(int doctorId);
        List<DateOnly> GetAvailableDatesForDoctor(int doctorId);
        public void DeletePastAppointments();
        public IEnumerable<Appointment> GetAppointmentsByPatientIdentifier(string identifier);



    }
}