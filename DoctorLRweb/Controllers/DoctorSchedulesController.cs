using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorLRweb.Data;
using DoctorLRweb.Models;
using DoctorLRweb.Repositories;
using DoctorLRweb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace DoctorLRweb.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //public class DoctorSchedulesController : ControllerBase
    //{
    //    private readonly Context _context;

    //    public DoctorSchedulesController(Context context)
    //    {
    //        _context = context;
    //    }

    //GET: api/DoctorSchedules
    [EnableCors("AllowAll")]
    [Authorize(Roles = "Doctor,Admin")]



    public class DoctorSchedulesController : ControllerBase
        {
            private readonly IDoctorScheduleService _service;
            public DoctorSchedulesController(IDoctorScheduleService service)
            {
                _service = service;
            }


        [HttpPost]
            public IActionResult AddDoctorSchedule(DoctorSchedule schedule)
            {
                try
                {
                    _service.AddDoctorSchedule(schedule);
                    return Ok("Schedule Added Successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            [HttpPut]
            public IActionResult UpdateDoctorSchedule(DoctorSchedule schedule)
            {
                try
                {
                    _service.UpdateDoctorSchedule(schedule);
                    return Ok("Schedule Updated Successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            [HttpDelete("{scheduleId}")]
            public IActionResult DeleteDoctorSchedule(int scheduleId)
            {
                _service.DeleteDoctorSchedule(scheduleId);
                return Ok("Schedule Deleted Successfully");
            }
            [HttpGet]
            public IActionResult GetDoctorSchedules()
            {
                return Ok(_service.GetDoctorSchedules());
            }
        [AllowAnonymous]
            [HttpGet("by-department/{department}")]
            public IActionResult GetByDepartment(string department)
            {
                return Ok(_service.GetByDepartment(department));
            }
            [HttpGet("by-timeslot/{timeSlot}")]
            public IActionResult GetByTimeSlot(TimeOnly timeSlot)
            {
                return Ok(_service.GetByTimeSlot(timeSlot));
            }
        [HttpGet("by-Id/{doctorId}")]
        public IActionResult GetSchedulesByDoctorId(int doctorId)
        {
            var schedules = _service.GetSchedulesByDoctorId(doctorId);
            if (schedules == null || !schedules.Any())
            {
                return NotFound();
            }
            return Ok(schedules);
        }
        [AllowAnonymous]
        [HttpGet("departments")]
        public IActionResult GetDepartments()
        {
            var departments = new List<string>
    {
        "Cardiology",
        "Neurology",
        "Orthopedics",
        "Pediatrics",
        "ENT",
        "Dermatology",
        "General Medicine",
        "Emergency"
    };

            return Ok(departments);
        }
        [HttpDelete("cleanup/past")]
        public IActionResult DeletePastSchedules()
        {
            _service.DeletePastSchedules();
            return Ok("Old doctor schedules deleted successfully.");
        }
    }
    }