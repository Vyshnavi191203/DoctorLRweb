using Microsoft.AspNetCore.Mvc;
using DoctorLRweb.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using DoctorLRweb.Repositories;

namespace DoctorLRweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("send/{appointmentId}")]
        public IActionResult SendNotification(int appointmentId)
        {
            var message = _notificationService.SendNotification(appointmentId);
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Failed to generate notification message.");
            }
            return Ok(new { Message = message });
        }
        [HttpGet("getByAppointment/{appointmentId}")]
        public IActionResult GetByAppointment(int appointmentId)
        {
            var notification = _notificationService.GetNotificationByAppointmentId(appointmentId);
            if (notification == null)
            {
                return NotFound(new { message = "No notification found for the given appointment ID." });
            }
            return Ok(notification);
        }
        [HttpGet("getByPatient/{patientId}")]
        public IActionResult GetByPatientId(int patientId)
        {
            var notifications = _notificationService.GetNotificationsByPatientId(patientId);
            if (!notifications.Any())
                return NotFound($"No notifications found for PatientID: {patientId}");
            return Ok(notifications);
        }
        [HttpPost("generate-patient-upcoming/{patientId}")]
        public IActionResult GeneratePatientUpcomingNotifications(int patientId)
        {
            try
            {
                _notificationService.GeneratePatientUpcomingNotifications(patientId);
                return Ok(new { message = "Notifications generated for upcoming appointments." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
      
    }
}