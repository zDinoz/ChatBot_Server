using ChatBotServer.DTOs;

namespace ChatBotServer.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto createDto);
    Task<PatientDto?> UpdateAsync(int id, PatientDto dto);
    Task<bool> DeleteAsync(int id);
}