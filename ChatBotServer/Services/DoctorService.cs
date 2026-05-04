using Microsoft.EntityFrameworkCore;
using ChatBotServer.Data;
using ChatBotServer.DTOs;
using ChatBotServer.Models;

namespace ChatBotServer.Services;

public class DoctorService : IDoctorService
{
    private readonly AppDbContext _context;

    public DoctorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        return await _context.Doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name,
            Specialty = d.Specialty,
            Email = d.Email,
            Phone = d.Phone,
            CreatedAt = d.CreatedAt
        }).ToListAsync();
    }

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return null;
        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty,
            Email = doctor.Email,
            Phone = doctor.Phone,
            CreatedAt = doctor.CreatedAt
        };
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto createDto)
    {
        var doctor = new Doctor
        {
            Name = createDto.Name,
            Specialty = createDto.Specialty,
            Email = createDto.Email,
            Phone = createDto.Phone
        };
        doctor.CreatedAt = DateTime.UtcNow;
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty,
            Email = doctor.Email,
            Phone = doctor.Phone,
            CreatedAt = doctor.CreatedAt
        };
    }

    public async Task<DoctorDto?> UpdateAsync(int id, DoctorDto dto)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return null;

        doctor.Name = dto.Name;
        doctor.Specialty = dto.Specialty;
        doctor.Email = dto.Email;
        doctor.Phone = dto.Phone;

        await _context.SaveChangesAsync();

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Specialty = doctor.Specialty,
            Email = doctor.Email,
            Phone = doctor.Phone,
            CreatedAt = doctor.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return false;
        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return true;
    }
}