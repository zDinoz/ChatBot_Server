using Microsoft.EntityFrameworkCore;
using ChatBotServer.Data;
using ChatBotServer.DTOs;
using ChatBotServer.Models;

namespace ChatBotServer.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        return await _context.Patients.Select(p => new PatientDto
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            Phone = p.Phone,
            DateOfBirth = p.DateOfBirth,
            Address = p.Address,
            CreatedAt = p.CreatedAt
        }).ToListAsync();
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return null;
        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name,
            Email = patient.Email,
            Phone = patient.Phone,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address,
            CreatedAt = patient.CreatedAt
        };
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto createDto)
    {
        var patient = new Patient
        {
            Name = createDto.Name,
            Email = createDto.Email,
            Phone = createDto.Phone,
            DateOfBirth = createDto.DateOfBirth,
            Address = createDto.Address
        };
        patient.CreatedAt = DateTime.UtcNow;
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name,
            Email = patient.Email,
            Phone = patient.Phone,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address,
            CreatedAt = patient.CreatedAt
        };
    }

    public async Task<PatientDto?> UpdateAsync(int id, PatientDto dto)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return null;

        patient.Name = dto.Name;
        patient.Email = dto.Email;
        patient.Phone = dto.Phone;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Address = dto.Address;

        await _context.SaveChangesAsync();

        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name,
            Email = patient.Email,
            Phone = patient.Phone,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address,
            CreatedAt = patient.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return false;
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }
}