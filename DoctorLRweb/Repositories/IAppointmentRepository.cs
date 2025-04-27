using DoctorLRweb.Models;

namespace DoctorLRweb.Repositories
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAllAppointments();
      
        int CreateAppointment(int userId, int doctorId, DateOnly appointmentDate, TimeOnly timeSlot);
        bool DeleteAppointment(int appointmentId);
        bool RescheduleAppointment(int appointmentId, DateOnly newAppointmentDate, TimeOnly newTimeSlot);
        List<string> GetAllDepartments();
        List<User> GetAllDoctorsByDepartment(string department);
        List<DoctorSchedule> GetDoctorsByDateWithDoctorName(DateOnly availableDate);
        public List<dynamic> GetAppointmentsByPatientId(int patientId);
         Task<IEnumerable<object>> GetAppointmentsByDoctorId(int doctorId);
        List<DateOnly> GetAvailableDatesForDoctor(int doctorId);
        public void DeletePastAppointments();
        public IEnumerable<Appointment> GetAppointmentsByPatientIdentifier(string identifier);
        public List<User> GetAvailableDoctorsByDepartment(string department);
        public List<string> GetAvailableDepartmentsWithOpenSlots();
        public List<User> GetPatientsWithAppointments();
        public List<User> GetDoctorsWithAppointments();


    }
}