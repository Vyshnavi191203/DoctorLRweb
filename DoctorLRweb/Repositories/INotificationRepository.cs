using DoctorLRweb.Models;
namespace DoctorLRweb.Repositories
{
    public interface INotificationRepository
    {
        string AddAppointmentNotification(int appointmentId);
        IEnumerable<string> GetNotificationsForPatient(int patientId);
        Notification GetNotificationByAppointmentId(int appointmentId);
        IEnumerable<Notification> GetNotificationsByPatientId(int patientId);
        public void GenerateNotificationsForUpcomingAppointments(int patientId);
        public void DeleteAllNotifications();


    }
}