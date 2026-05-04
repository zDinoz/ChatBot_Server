using ChatBotServer.DTOs;

namespace ChatBotServer.Services;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto createDto);
    Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentDto updateDto);
    Task<bool> DeleteAsync(int id);
}