using ChatBotServer.DTOs;

namespace ChatBotServer.Services;

public interface IMedicalHistoryService
{
    Task<IEnumerable<MedicalHistoryDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<MedicalHistoryDto>> QueryAsync(string? patientName, string? phone);
    Task<MedicalHistoryDto?> GetByIdAsync(int id);
    Task<MedicalHistoryDto> CreateAsync(CreateMedicalHistoryDto createDto);
    Task<MedicalHistoryDto?> UpdateAsync(int id, UpdateMedicalHistoryDto updateDto);
    Task<bool> DeleteAsync(int id);
}
