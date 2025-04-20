using DoctorLRweb.Data;

using DoctorLRweb.Models;

using DoctorLRweb.Services;

using DoctorLRweb.Repositories;

using Microsoft.AspNetCore.Cors;

using Microsoft.AspNetCore.Mvc;

namespace DoctorLRweb.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    [EnableCors("AllowAll")]

    public class AuthController : ControllerBase

    {

        private readonly IAuth _authService;

        private readonly INotificationService _notificationService;

        private readonly Context _context;

        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IDoctorScheduleRepository _scheduleRepository;

        public AuthController(

            IAuth authService,

            INotificationService notificationService,

            Context context,

            IAppointmentRepository appointmentRepository,

            IDoctorScheduleRepository scheduleRepository)

        {

            _authService = authService;

            _notificationService = notificationService;

            _context = context;

            _appointmentRepository = appointmentRepository;

            _scheduleRepository = scheduleRepository;

        }

        [HttpPost("login")]

        public IActionResult Login([FromBody] LoginRequest request)

        {

            if (request == null)

                return BadRequest("Request body is null");

            var token = _authService.Authentication(request.Identifier, request.Password, request.Role);

            if (token == null)

                return Unauthorized("Invalid credentials or role");

            // ✅ Identify user after successful authentication

            var user = _context.Users

                .FirstOrDefault(u =>

                    (u.Email == request.Identifier || u.UserId.ToString() == request.Identifier) &&

                    u.Role.ToLower() == request.Role.ToLower());

            // ✅ Cleanup past appointments and schedules

            try

            {

                _appointmentRepository.DeletePastAppointments();

                _scheduleRepository.DeletePastSchedules();

            }

            catch (Exception ex)

            {

                Console.WriteLine("⚠️ Cleanup failed: " + ex.Message);

            }

            // ✅ Notification logic for patient

            if (user != null && user.Role.ToLower() == "patient")

            {

                Console.WriteLine($"✅ Login: Patient ID = {user.UserId}");

                // Clear all previous notifications

                _notificationService.DeleteAllNotifications();

                // Generate only this patient's upcoming appointment notifications

                _notificationService.GeneratePatientUpcomingNotifications(user.UserId);

            }

            return Ok(new { Token = token, userId = user?.UserId });

        }

        [HttpPost("logout")]

        public IActionResult Logout()

        {

            try

            {

                // ✅ Clear all notifications on logout

                _notificationService.DeleteAllNotifications();

                return Ok(new { message = "Logged out and notifications cleared." });

            }

            catch (Exception ex)

            {

                return BadRequest(new { message = ex.Message });

            }

        }
        [HttpGet("getRole/{identifier}")]
        public IActionResult GetRole(string identifier)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == identifier || u.UserId.ToString() == identifier);

            if (user == null)
                return NotFound("User not found");

            return Ok(new { role = user.Role });
        }


    }

}

