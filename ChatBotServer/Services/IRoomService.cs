using ChatBotServer.DTOs;

namespace ChatBotServer.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync();
    Task<RoomDto?> GetByIdAsync(int id);
    Task<RoomDto> CreateAsync(CreateRoomDto createDto);
    Task<RoomDto?> UpdateAsync(int id, RoomDto dto);
    Task<bool> DeleteAsync(int id);
}