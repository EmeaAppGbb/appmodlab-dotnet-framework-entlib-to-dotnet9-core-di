using HealthClaimsProcessor.Core.Models;

namespace HealthClaimsProcessor.Core.Interfaces
{
    public interface IPatientService
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int patientId);
        int CreatePatient(Patient patient);
        void UpdatePatient(Patient patient);
    }
}
