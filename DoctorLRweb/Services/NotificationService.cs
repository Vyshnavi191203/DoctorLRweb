using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using DoctorLRweb.Models;
using DoctorLRweb.Repositories;
using DoctorLRweb.Services;

namespace DoctorLRweb.Services

{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public string SendNotification(int appointmentId)
        {
            return _notificationRepository.AddAppointmentNotification(appointmentId);
        }
        public IEnumerable<string> GetPatientNotifications(int patientId)
        {
            return _notificationRepository.GetNotificationsForPatient(patientId);
        }
        public Notification GetNotificationByAppointmentId(int appointmentId)
        {
            return _notificationRepository.GetNotificationByAppointmentId(appointmentId);
        }
        public IEnumerable<Notification> GetNotificationsByPatientId(int patientId)
        {
            return _notificationRepository.GetNotificationsByPatientId(patientId);
        }
        public void GeneratePatientUpcomingNotifications(int patientId)
        {
            _notificationRepository.GenerateNotificationsForUpcomingAppointments(patientId);
        }
        public void DeleteAllNotifications()
        {
            _notificationRepository.DeleteAllNotifications();
        }
    }
}