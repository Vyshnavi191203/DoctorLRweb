using DoctorLRweb.Models;
namespace DoctorLRweb.Services
{
    public interface INotificationService
    {
        string SendNotification(int appointmentId);
        IEnumerable<string> GetPatientNotifications(int patientId);
        Notification GetNotificationByAppointmentId(int appointmentId);
        IEnumerable<Notification> GetNotificationsByPatientId(int patientId);
        public void GeneratePatientUpcomingNotifications(int patientId);
        public void DeleteAllNotifications();


    }
}