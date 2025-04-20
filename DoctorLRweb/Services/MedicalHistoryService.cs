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
        public void AddMedicalHistory(MedicalHistory medicalHistory, int userId)
            => _repository.AddMedicalHistory(medicalHistory, userId);
        public void UpdateMedicalHistory(int id, MedicalHistory medicalHistory)
            => _repository.UpdateMedicalHistory(id,medicalHistory);
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

    }
}