using Microsoft.EntityFrameworkCore;
using ChatBotServer.Data;
using ChatBotServer.DTOs;
using ChatBotServer.Models;

namespace ChatBotServer.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
    {
        return await _context.Rooms.Select(r => new RoomDto
        {
            Id = r.Id,
            Name = r.Name,
            Capacity = r.Capacity,
            Location = r.Location,
            IsActive = r.IsActive,
            CreatedAt = r.CreatedAt
        }).ToListAsync();
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return null;
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Location = room.Location,
            IsActive = room.IsActive,
            CreatedAt = room.CreatedAt
        };
    }

    public async Task<RoomDto> CreateAsync(CreateRoomDto createDto)
    {
        var room = new Room
        {
            Name = createDto.Name,
            Capacity = createDto.Capacity,
            Location = createDto.Location
        };
        room.CreatedAt = DateTime.UtcNow;
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Location = room.Location,
            IsActive = room.IsActive,
            CreatedAt = room.CreatedAt
        };
    }

    public async Task<RoomDto?> UpdateAsync(int id, RoomDto dto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return null;

        room.Name = dto.Name;
        room.Capacity = dto.Capacity;
        room.Location = dto.Location;
        room.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Location = room.Location,
            IsActive = room.IsActive,
            CreatedAt = room.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return false;
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }
}