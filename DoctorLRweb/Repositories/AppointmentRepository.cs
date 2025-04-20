using System.Data;
using DoctorLRweb.Models;
using DoctorLRweb.Data;
using DoctorLRweb.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DoctorLRweb.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly Context _context;
        public AppointmentRepository(Context context)
        {
            _context = context;
        }
        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.ToList();
        }

        public int CreateAppointment(int userId, int doctorId, DateOnly appointmentDate, TimeOnly timeSlot)

        {

            try

            {

                var appointmentIdParam = new SqlParameter("@AppointmentID", System.Data.SqlDbType.Int)

                {

                    Direction = System.Data.ParameterDirection.Output

                };

                _context.Database.ExecuteSqlRaw(

                    "EXEC InsertAppointment @UserId, @DoctorId, @AppointmentDate, @TimeSlot, @AppointmentID OUTPUT",

                    new SqlParameter("@UserId", userId),

                    new SqlParameter("@DoctorId", doctorId),

                    new SqlParameter("@AppointmentDate", appointmentDate),

                    new SqlParameter("@TimeSlot", timeSlot),

                    appointmentIdParam

                );

                return (int)appointmentIdParam.Value;

            }

            catch (Exception ex)

            {

                if (ex.Message.Contains("Invalid Patient ID"))

                    return -1; // ❌ Patient does not exist

                if (ex.Message.Contains("Invalid Doctor ID"))

                    return -2; // ❌ Doctor does not exist

                if (ex.Message.Contains("This doctor is not available on the selected appointment date"))

                    return -3; // ❌ Doctor is unavailable

                if (ex.Message.Contains("Patient already has an appointment at the same date and time slot"))

                    return -4; // ❌ Patient double booking

                if (ex.Message.Contains("Doctor is already booked at this date and time slot"))

                    return -5; // ❌ Doctor double booking

                throw;

            }

        }
        public bool DeleteAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.Find(appointmentId);
            if (appointment == null)
                return false;
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return true;
        }
        public List<string> GetAvailableDepartments()
        {
            return _context.DoctorSchedules
                .Select(ds => ds.Department)
                .Distinct()
                .ToList();
        }
        public List<User> GetDoctorsByDepartment(string department)
        {
            return _context.Users
                .Where(u => u.Role == "doctor" && _context.DoctorSchedules.Any(ds => ds.DoctorId == u.UserId && ds.Department == department))
                .ToList();
        }
        // ✅ Updated: Get available doctors with their names and time slots
        public List<DoctorSchedule> GetDoctorsByDateWithDoctorName(DateOnly availableDate)
        {
            return _context.DoctorSchedules
       .FromSqlRaw("EXEC GetAvailableDoctorsByDate @p0", availableDate)
       .ToList();
        }

        public List<dynamic> GetAppointmentsByPatientId(int patientId)
        {
            var result = (from a in _context.Appointments
                          join d in _context.Users on a.DoctorID equals d.UserId
                          where a.PatientId == patientId
                          select new
                          {
                              a.AppointmentID,
                              a.AppointmentDate,
                              a.TimeSlot,
                              a.Status,
                              DoctorID = a.DoctorID,
                              DoctorName = d.Name
                          }).ToList<dynamic>();

            return result;
        }

        public List<Appointment> GetAppointmentsByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.DoctorID == doctorId)
                .ToList();
        }

        public bool RescheduleAppointment(int appointmentId, DateOnly newAppointmentDate, TimeOnly newTimeSlot)

        {

            try

            {

                _context.Database.ExecuteSqlRaw(

                    "EXEC RescheduleAppointment @AppointmentID, @NewAppointmentDate, @NewTimeSlot",

                    new SqlParameter("@AppointmentID", appointmentId),

                    new SqlParameter("@NewAppointmentDate", newAppointmentDate),

                    new SqlParameter("@NewTimeSlot", newTimeSlot)

                );

                return true;

            }

            catch (Exception ex)

            {

                if (ex.Message.Contains("Appointment not found."))

                    throw new Exception("Appointment not found.");

                if (ex.Message.Contains("Selected date and time slot are not available for this doctor."))

                    throw new Exception("Selected date and time slot are not available for this doctor. Please choose an available time slot.");

                if (ex.Message.Contains("Patient already has an appointment at the same date and time slot."))

                    throw new Exception("Patient already has an appointment at the same date and time slot.");

                if (ex.Message.Contains("Doctor is already booked at this date and time slot."))

                    throw new Exception("Doctor is already booked at this date and time slot.");

                throw;

            }

        }
        public List<DateOnly> GetAvailableDatesForDoctor(int doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId && ds.AvailableDate >= today)
                .Where(ds => !_context.Appointments.Any(a =>
                    a.DoctorID == ds.DoctorId &&
                    a.AppointmentDate == ds.AvailableDate &&
                    a.TimeSlot == ds.TimeSlot))
                .Select(ds => ds.AvailableDate)
                .Distinct()
                .OrderBy(date => date)
                .ToList();
        }
        public void DeletePastAppointments()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var pastAppointments = _context.Appointments
                .Where(a => a.AppointmentDate < today)
                .ToList();

            if (pastAppointments.Any())
            {
                _context.Appointments.RemoveRange(pastAppointments);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Appointment> GetAppointmentsByPatientIdentifier(string identifier)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Role == "Patient" && (u.UserId.ToString() == identifier || u.Name.Contains(identifier)));

            if (user == null)
                return new List<Appointment>();

            return _context.Appointments.Where(a => a.PatientId == user.UserId).ToList();
        }
    }
}