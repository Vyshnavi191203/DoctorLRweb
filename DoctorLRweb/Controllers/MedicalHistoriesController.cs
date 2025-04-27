using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorLRweb.Models;
using DoctorLRweb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace DoctorLRweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize(Roles = "Doctor,Admin")]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalHistoryService _service;

        public MedicalHistoryController(IMedicalHistoryService service)
        {
            _service = service;
        }
        [HttpGet]
      
        public IEnumerable<MedicalHistory> Get() => _service.GetAll();
        [HttpGet("{id}")]
        public ActionResult<MedicalHistory> Get(int id)
        {
            var history = _service.GetById(id);
            return history ?? (ActionResult<MedicalHistory>)NotFound();
        }
        [HttpPost]
        public IActionResult Post([FromBody] MedicalHistory medicalHistory, [FromQuery] int userId, [FromQuery] string doctorName)
        {
            if (medicalHistory == null)
                return BadRequest("Invalid medical history data.");
            _service.AddMedicalHistory(medicalHistory, userId, doctorName);
            return CreatedAtAction(nameof(Get), new { id = medicalHistory.HistoryID }, medicalHistory);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMedicalHistory(int id, [FromBody] MedicalHistory medicalHistory, [FromQuery] string doctorName)
        {
            try
            {
                _service.UpdateMedicalHistory(id, medicalHistory, doctorName);
                return Ok("Medical history updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.DeleteMedicalHistory(id);
            return NoContent();
        }
        [HttpGet("search/diagnosis")]
        public ActionResult<IEnumerable<MedicalHistory>> SearchByDiagnosis([FromQuery] string diagnosis)
        {
            return Ok(_service.SearchByDiagnosis(diagnosis));
        }
        [HttpGet("search/treatment")]
        public ActionResult<IEnumerable<MedicalHistory>> SearchByTreatment([FromQuery] string treatment)
        {
            return Ok(_service.SearchByTreatment(treatment));
        }
        [HttpGet("search/date")]
        public ActionResult<IEnumerable<MedicalHistory>> SearchByDate([FromQuery] DateTime dateOfVisit)
        {
            return Ok(_service.SearchByDate(dateOfVisit));
        }
     
        [HttpGet("by-patient/{patientId}")]
        public IActionResult GetByPatientId(int patientId)
        {
            var history = _service.GetHistoryByPatientId(patientId);
            return Ok(history);
        }
        [HttpGet("paged")]

        public IActionResult GetPagedMedicalHistories(int pageNumber = 1, int pageSize = 5)

        {

            int totalRecords;

            var histories = _service.GetPagedMedicalHistories(pageNumber, pageSize, out totalRecords);

            return Ok(new { data = histories, totalRecords });

        }

        // Search by patient name
        [HttpGet("search/patient-name")]
        public IActionResult SearchByPatientName([FromQuery] string patientName)
        {
            var results = _service.SearchByPatientName(patientName);
            return Ok(results);
        }
        // Search by doctor name
        [HttpGet("search/doctor-name")]
        public IActionResult SearchByDoctorName([FromQuery] string doctorName)
        {
            var results = _service.SearchByDoctorName(doctorName);
            return Ok(results);
        }

    }
}