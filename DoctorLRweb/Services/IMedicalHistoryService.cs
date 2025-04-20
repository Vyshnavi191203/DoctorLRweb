using DoctorLRweb.Models;

namespace DoctorLRweb.Services
{
    public interface IMedicalHistoryService
    {
        IEnumerable<MedicalHistory> GetAll();
        MedicalHistory GetById(int id);
        void AddMedicalHistory(MedicalHistory medicalHistory, int userId);
        void UpdateMedicalHistory(int id,MedicalHistory medicalHistory);
        void DeleteMedicalHistory(int id);
        int? GetPatientIdFromUser(int userId);
        IEnumerable<MedicalHistory> SearchByDiagnosis(string diagnosis);
        IEnumerable<MedicalHistory> SearchByTreatment(string treatment);
        IEnumerable<MedicalHistory> SearchByDate(DateTime dateOfVisit);
        public IEnumerable<MedicalHistory> GetHistoryByPatientId(int patientId);
        IEnumerable<MedicalHistory> GetPagedMedicalHistories(int pageNumber, int pageSize, out int totalRecords);

    }
}
