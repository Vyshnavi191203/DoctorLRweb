using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DoctorLRweb.Data;
using DoctorLRweb.Models;
using System.Data;

namespace DoctorLRweb.Repositories
{

    public class NotificationRepository : INotificationRepository
    {
        private readonly Context _context;
        public NotificationRepository(Context context)
        {
            _context = context;
        }
        public string AddAppointmentNotification(int appointmentId)

        {

            var message = new SqlParameter("@Message", SqlDbType.NVarChar, 255)

            {

                Direction = ParameterDirection.Output

            };

            _context.Database.ExecuteSqlRaw("EXEC AddAppointmentNotification @p0, @Message OUTPUT",

                                            new SqlParameter("@p0", appointmentId), message);

            return message.Value?.ToString() ?? "No message generated.";

        }
        public IEnumerable<string> GetNotificationsForPatient(int patientId)
        {
            List<string> notifications = new List<string>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT Message FROM Notification WHERE AppointmentID IN (SELECT AppointmentID FROM Appointment WHERE PatientID = @PatientID)";
                command.CommandType = CommandType.Text;
                var param = command.CreateParameter();
                param.ParameterName = "@PatientID";
                param.Value = patientId;
                command.Parameters.Add(param);
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications.Add(reader.GetString(0));
                    }
                }
                _context.Database.CloseConnection();
            }
            return notifications;
        }
        public Notification GetNotificationByAppointmentId(int appointmentId)
        {
            return _context.Notifications.FirstOrDefault(n => n.AppointmentID == appointmentId);
        }
        public IEnumerable<Notification> GetNotificationsByPatientId(int patientId)
        {
            var notifications = _context.Notifications
                .FromSqlRaw("EXEC GetNotificationsByPatientID @PatientID = {0}", patientId)
                .ToList();
            return notifications;
        }
        public void GenerateNotificationsForUpcomingAppointments(int patientId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var upcomingAppointments = _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate >= today && a.Status == "Scheduled")
                .OrderBy(a => a.AppointmentDate)
                .Select(a => a.AppointmentID)
                .ToList();

            foreach (var appointmentId in upcomingAppointments)
            {
                AddAppointmentNotification(appointmentId);
            }
        }
        public void DeleteAllNotifications()
        {
            var allNotifications = _context.Notifications.ToList();
            _context.Notifications.RemoveRange(allNotifications);
            _context.SaveChanges();
        }
    }
    }
