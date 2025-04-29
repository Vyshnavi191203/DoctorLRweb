using DoctorLRweb.Models;
using DoctorLRweb.Repositories;

namespace DoctorLRweb.Services
{
    public class MedicalHistoryService : IMedicalHistoryService
    {
        private readonly IMedicalHistoryRepository _repository;
        public MedicalHistoryService(IMedicalHistoryRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<MedicalHistory> GetAll() => _repository.GetAll();
        public MedicalHistory GetById(int id) => _repository.GetById(id);
        public void AddMedicalHistory(MedicalHistory medicalHistory, int userId, string doctorName)
            => _repository.AddMedicalHistory(medicalHistory, userId,doctorName);
        public void UpdateMedicalHistory(int id, MedicalHistory updatedHistory, string doctorName)
            => _repository.UpdateMedicalHistory(id,updatedHistory,doctorName);
        public void DeleteMedicalHistory(int id)
            => _repository.DeleteMedicalHistory(id);
        public int? GetPatientIdFromUser(int userId)
            => _repository.GetPatientIdFromUser(userId);
        public IEnumerable<MedicalHistory> SearchByDiagnosis(string diagnosis)
          => _repository.SearchByDiagnosis(diagnosis);
        public IEnumerable<MedicalHistory> SearchByTreatment(string treatment)
            => _repository.SearchByTreatment(treatment);
        public IEnumerable<MedicalHistory> SearchByDate(DateTime dateOfVisit)
            => _repository.SearchByDate(dateOfVisit);
        public IEnumerable<MedicalHistory> GetHistoryByPatientId(int patientId)
        {
            return _repository.GetHistoryByPatientId(patientId);
        }
        public IEnumerable<MedicalHistory> GetPagedMedicalHistories(int pageNumber, int pageSize, out int totalRecords)
        {
            return _repository.GetPagedMedicalHistories(pageNumber, pageSize, out totalRecords);
        }
        public IEnumerable<MedicalHistory> SearchByPatientName(string patientName)
        {
            return _repository.SearchByPatientName(patientName);
        }
        public IEnumerable<MedicalHistory> SearchByDoctorName(string doctorName)
        {
            return _repository.SearchByDoctorName(doctorName);
        }
        public List<User> GetPatientsByDoctor(int doctorId)
        {
            return _repository.GetPatientsByDoctor(doctorId);
        }
        public List<MedicalHistory> GetHistoriesByDoctor(string doctorName)
        {
            return _repository.GetHistoriesByDoctor(doctorName);
        }
    }
}