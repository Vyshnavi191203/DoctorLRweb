using System.Data;
using DoctorLRweb.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DoctorLRweb.Data;
using NuGet.Protocol.Core.Types;



   

    namespace DoctorLRweb.Repositories

    {

        public class MedicalHistoryRepository : IMedicalHistoryRepository

        {

            private readonly Context _context;

            public MedicalHistoryRepository(Context context)

            {

                _context = context;

            }

            public IEnumerable<MedicalHistory> GetAll()

            {

                return _context.MedicalHistories.ToList();

            }

            public MedicalHistory GetById(int id)

            {

                return _context.MedicalHistories.Find(id);

            }

        public void AddMedicalHistory(MedicalHistory medicalHistory, int userId, string doctorName)
        {
            int? patientId = GetPatientIdFromUser(userId);
            if (patientId == null)
                throw new KeyNotFoundException("User is not a patient or does not exist.");
            medicalHistory.PatientId = patientId.Value;
            medicalHistory.DoctorNames = doctorName; // Set initial doctor
            _context.MedicalHistories.Add(medicalHistory);
            _context.SaveChanges();
        }

        public void UpdateMedicalHistory(int id, MedicalHistory updatedHistory, string doctorName)
        {
            var existingHistory = _context.MedicalHistories.FirstOrDefault(h => h.HistoryID == id);
            if (existingHistory == null)
            {
                throw new KeyNotFoundException("Medical history record not found.");
            }
            existingHistory.Diagnosis = updatedHistory.Diagnosis;
            existingHistory.Treatment = updatedHistory.Treatment;
            existingHistory.DateOfVisit = updatedHistory.DateOfVisit;
            if (!existingHistory.DoctorNames.Contains(doctorName))
            {
                existingHistory.DoctorNames += ", " + doctorName; // Append new doctor name
            }
            _context.SaveChanges();
        }

        public void DeleteMedicalHistory(int id)

            {

                var history = _context.MedicalHistories.Find(id);

                if (history != null)

                {

                    _context.MedicalHistories.Remove(history);

                    _context.SaveChanges();

                }

            }

        public int? GetPatientIdFromUser(int userId)
        {
            var patientIdParam = new SqlParameter("@PatientId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            _context.Database.ExecuteSqlRaw(
                "EXEC GetPatientIdFromUser @UserId, @PatientId OUTPUT",
                new SqlParameter("@UserId", userId),
                patientIdParam);
            // Handle DBNull safely
            if (patientIdParam.Value == DBNull.Value)
            {
                return null; // Return null when no patient exists
            }
            return (int?)patientIdParam.Value;
        }
        public IEnumerable<MedicalHistory> SearchByDiagnosis(string diagnosis)
        {
            return _context.MedicalHistories.Where(m => m.Diagnosis.Contains(diagnosis)).ToList();
        }
        public IEnumerable<MedicalHistory> SearchByTreatment(string treatment)
        {
            return _context.MedicalHistories.Where(m => m.Treatment.Contains(treatment)).ToList();
        }
        public IEnumerable<MedicalHistory> SearchByDate(DateTime dateOfVisit)
        {
            return _context.MedicalHistories.Where(m => m.DateOfVisit.Date == dateOfVisit.Date).ToList();
        }
        public IEnumerable<MedicalHistory> GetHistoryByPatientId(int patientId)
        {
            return _context.MedicalHistories.Where(m => m.PatientId == patientId).ToList();
        }
        public IEnumerable<MedicalHistory> GetPagedMedicalHistories(int pageNumber, int pageSize, out int totalRecords)
        {
            totalRecords = _context.MedicalHistories.Count();

            return _context.MedicalHistories
                .OrderBy(h => h.HistoryID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public IEnumerable<MedicalHistory> SearchByPatientName(string patientName)
        {
            return _context.MedicalHistories
                .Where(h => _context.Users
                    .Any(u => u.UserId == h.PatientId && u.Name.Contains(patientName)))
                .ToList();
        }
        public IEnumerable<MedicalHistory> SearchByDoctorName(string doctorName)
        {
            return _context.MedicalHistories
                .Where(h => h.DoctorNames.Contains(doctorName))
                .ToList();
        }
        public List<User> GetPatientsByDoctor(int doctorId)

        {

            var patients = (from appointment in _context.Appointments

                            join user in _context.Users on appointment.PatientId equals user.UserId

                            where appointment.DoctorID == doctorId

                            select user)

                            .Distinct()

                            .ToList();

            return patients;

        }
        public List<MedicalHistory> GetHistoriesByDoctor(string doctorName)
        {
            return _context.MedicalHistories
                           .Where(h => h.DoctorNames == doctorName)
                           .OrderByDescending(h => h.DateOfVisit)
                           .ToList();
        }


    }

    } 