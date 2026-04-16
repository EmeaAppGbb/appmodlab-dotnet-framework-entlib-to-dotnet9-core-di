using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int patientId);
        int InsertPatient(Patient patient);
        void UpdatePatient(Patient patient);
    }
}
