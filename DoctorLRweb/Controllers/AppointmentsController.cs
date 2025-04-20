using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorLRweb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using DoctorLRweb.Data;

namespace DoctorLRweb.Controllers
{

    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]

    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_appointmentService.GetAllAppointments());
        }
        [Authorize(Roles = "Patient,Admin")]
        [HttpPost]
        public IActionResult CreateAppointment([FromBody] AppointmentRequest request)
        {
            try
            {
                var appointmentId = _appointmentService.CreateAppointment(
                    request.UserId, request.DoctorId, request.AppointmentDate, request.TimeSlot);
                return Ok(new { AppointmentID = appointmentId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        // ✅ Request Model for Creating Appointment
        public class AppointmentRequest
        {
            public int UserId { get; set; }  // ✅ Patient ID
            public int DoctorId { get; set; }  // ✅ Doctor ID
            public DateOnly AppointmentDate { get; set; }
            public TimeOnly TimeSlot { get; set; }
        }

        [Authorize(Roles = "Patient,Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _appointmentService.DeleteAppointment(id);
            if (!isDeleted)
                return NotFound("Appointment not found");
            return Ok("Appointment deleted successfully.");
        }

        [AllowAnonymous]
        [HttpGet("departments")]
        public IActionResult GetAvailableDepartments()
        {
            var departments = _appointmentService.GetAvailableDepartments();
            if (departments.Count == 0)
                return NotFound("No available departments found.");
            return Ok(departments);
        }

        [AllowAnonymous]
        [HttpGet("doctors-by-department")]
        public IActionResult GetDoctorsByDepartment([FromQuery] string department)
        {
            var doctors = _appointmentService.GetDoctorsByDepartment(department);
            if (doctors.Count == 0)
                return NotFound("No doctors found for the selected department.");
            return Ok(doctors);
        }
        // ✅ Updated: Get available doctors with their names and time slots


        [Authorize(Roles = "Patient,Admin")]
        [HttpGet("doctors-by-date")]
        public IActionResult GetDoctorsByDateWithDoctorName([FromQuery] DateOnly availableDate)
        {
            var doctors = _appointmentService.GetDoctorsByDateWithDoctorName(availableDate);
            if (doctors.Count == 0)
                return NotFound("No available doctors or time slots for the selected date.");
            return Ok(doctors);
        }
        [Authorize(Roles = "Patient,Admin")]
        [HttpGet("by-patient/{patientId}")]
        
        public IActionResult GetAppointmentsByPatientId(int patientId)
        {
            var appointments = _appointmentService.GetAppointmentsByPatientId(patientId);
            if (appointments == null || !appointments.Any())
                return NotFound("No appointments found for this patient.");

            return Ok(appointments);
        }
        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("by-doctor/{doctorId}")]
        public IActionResult GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = _appointmentService.GetAppointmentsByDoctorId(doctorId);
            if (appointments == null || !appointments.Any())
                return NotFound("No appointments found for this doctor.");
            return Ok(appointments);
        }

        [Authorize(Roles = "Patient,Admin")]
        [HttpPut("reschedule/{appointmentId}")]
        public IActionResult RescheduleAppointment(int appointmentId, [FromBody] RescheduleRequest request)
        {
            try
            {
                _appointmentService.RescheduleAppointment(appointmentId, request.NewAppointmentDate, request.NewTimeSlot);
                return Ok(new { Message = "Appointment rescheduled successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [Authorize(Roles = "Patient,Admin")]
        [HttpGet("doctor-available-dates")]
        public IActionResult GetDoctorAvailableDates([FromQuery] int doctorId)
        {
            var availableDates = _appointmentService.GetAvailableDatesForDoctor(doctorId);
            if (availableDates == null || !availableDates.Any())
            {
                return NotFound("No available dates found for this doctor.");
            }
            return Ok(availableDates);
        }

        [HttpDelete("delete-past")]
        public IActionResult DeletePastAppointments()
        {
            _appointmentService.DeletePastAppointments();
            return Ok("Past appointments deleted successfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("search-patient")]
        public IActionResult SearchAppointmentsByPatient(string identifier)
        {
            var result = _appointmentService.GetAppointmentsByPatientIdentifier(identifier);
            return Ok(result);
        }
    }
    public class AppointmentRequest
    {
        public int UserId { get; set; }  // ✅ Patient ID (from User table where Role = 'patient')
        public int DoctorId { get; set; }  // ✅ Doctor ID (from DoctorSchedule, which gets it from User where Role = 'doctor')
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public string Status { get; set; }  // Example: "Scheduled", "Completed", "Cancelled"
    }
    public class RescheduleRequest
    {
        public DateOnly NewAppointmentDate { get; set; }
        public TimeOnly NewTimeSlot { get; set; }
    }

}