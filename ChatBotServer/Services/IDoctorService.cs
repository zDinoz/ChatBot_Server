using ChatBotServer.DTOs;

namespace ChatBotServer.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(int id);
    Task<DoctorDto> CreateAsync(CreateDoctorDto createDto);
    Task<DoctorDto?> UpdateAsync(int id, DoctorDto dto);
    Task<bool> DeleteAsync(int id);
}