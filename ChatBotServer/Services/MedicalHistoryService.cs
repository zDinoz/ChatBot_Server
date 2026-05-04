using Microsoft.EntityFrameworkCore;
using ChatBotServer.Data;
using ChatBotServer.DTOs;
using ChatBotServer.Models;

namespace ChatBotServer.Services;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly AppDbContext _context;

    public MedicalHistoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedicalHistoryDto>> GetByPatientIdAsync(int patientId)
    {
        return await _context.MedicalHistories
            .Include(mh => mh.Patient)
            .Include(mh => mh.Doctor)
            .Where(mh => mh.PatientId == patientId)
            .OrderByDescending(mh => mh.VisitDate)
            .Select(mh => new MedicalHistoryDto
            {
                Id = mh.Id,
                PatientId = mh.PatientId,
                PatientName = mh.Patient != null ? mh.Patient.Name : "",
                DoctorId = mh.DoctorId,
                DoctorName = mh.Doctor != null ? mh.Doctor.Name : "",
                DoctorSpecialty = mh.Doctor != null ? mh.Doctor.Specialty : "",
                VisitDate = mh.VisitDate,
                Diagnosis = mh.Diagnosis,
                Symptoms = mh.Symptoms,
                Treatment = mh.Treatment,
                Prescription = mh.Prescription,
                Notes = mh.Notes,
                CreatedAt = mh.CreatedAt,
                UpdatedAt = mh.UpdatedAt
            }).ToListAsync();
    }

    public async Task<IEnumerable<MedicalHistoryDto>> QueryAsync(string? patientName, string? phone)
    {
        var query = _context.MedicalHistories
            .Include(mh => mh.Patient)
            .Include(mh => mh.Doctor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(patientName))
        {
            query = query.Where(mh => mh.Patient != null && EF.Functions.Like(mh.Patient.Name, $"%{patientName}%"));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(mh => mh.Patient != null && EF.Functions.Like(mh.Patient.Phone, $"%{phone}%"));
        }

        return await query
            .OrderByDescending(mh => mh.VisitDate)
            .Select(mh => new MedicalHistoryDto
            {
                Id = mh.Id,
                PatientId = mh.PatientId,
                PatientName = mh.Patient != null ? mh.Patient.Name : "",
                DoctorId = mh.DoctorId,
                DoctorName = mh.Doctor != null ? mh.Doctor.Name : "",
                DoctorSpecialty = mh.Doctor != null ? mh.Doctor.Specialty : "",
                VisitDate = mh.VisitDate,
                Diagnosis = mh.Diagnosis,
                Symptoms = mh.Symptoms,
                Treatment = mh.Treatment,
                Prescription = mh.Prescription,
                Notes = mh.Notes,
                CreatedAt = mh.CreatedAt,
                UpdatedAt = mh.UpdatedAt
            }).ToListAsync();
    }

    public async Task<MedicalHistoryDto?> GetByIdAsync(int id)
    {
        var history = await _context.MedicalHistories
            .Include(mh => mh.Patient)
            .Include(mh => mh.Doctor)
            .FirstOrDefaultAsync(mh => mh.Id == id);

        if (history == null) return null;

        return new MedicalHistoryDto
        {
            Id = history.Id,
            PatientId = history.PatientId,
            PatientName = history.Patient?.Name ?? "",
            DoctorId = history.DoctorId,
            DoctorName = history.Doctor?.Name ?? "",
            DoctorSpecialty = history.Doctor?.Specialty ?? "",
            VisitDate = history.VisitDate,
            Diagnosis = history.Diagnosis,
            Symptoms = history.Symptoms,
            Treatment = history.Treatment,
            Prescription = history.Prescription,
            Notes = history.Notes,
            CreatedAt = history.CreatedAt,
            UpdatedAt = history.UpdatedAt
        };
    }

    public async Task<MedicalHistoryDto> CreateAsync(CreateMedicalHistoryDto createDto)
    {
        var history = new MedicalHistory
        {
            PatientId = createDto.PatientId,
            DoctorId = createDto.DoctorId,
            VisitDate = createDto.VisitDate,
            Diagnosis = createDto.Diagnosis,
            Symptoms = createDto.Symptoms,
            Treatment = createDto.Treatment,
            Prescription = createDto.Prescription,
            Notes = createDto.Notes
        };

        _context.MedicalHistories.Add(history);
        await _context.SaveChangesAsync();

        var patient = await _context.Patients.FindAsync(history.PatientId);
        var doctor = await _context.Doctors.FindAsync(history.DoctorId);

        return new MedicalHistoryDto
        {
            Id = history.Id,
            PatientId = history.PatientId,
            PatientName = patient?.Name ?? "",
            DoctorId = history.DoctorId,
            DoctorName = doctor?.Name ?? "",
            DoctorSpecialty = doctor?.Specialty ?? "",
            VisitDate = history.VisitDate,
            Diagnosis = history.Diagnosis,
            Symptoms = history.Symptoms,
            Treatment = history.Treatment,
            Prescription = history.Prescription,
            Notes = history.Notes,
            CreatedAt = history.CreatedAt,
            UpdatedAt = history.UpdatedAt
        };
    }

    public async Task<MedicalHistoryDto?> UpdateAsync(int id, UpdateMedicalHistoryDto updateDto)
    {
        var history = await _context.MedicalHistories.FindAsync(id);
        if (history == null) return null;

        history.VisitDate = updateDto.VisitDate;
        history.Diagnosis = updateDto.Diagnosis;
        history.Symptoms = updateDto.Symptoms;
        history.Treatment = updateDto.Treatment;
        history.Prescription = updateDto.Prescription;
        history.Notes = updateDto.Notes;
        history.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var patient = await _context.Patients.FindAsync(history.PatientId);
        var doctor = await _context.Doctors.FindAsync(history.DoctorId);

        return new MedicalHistoryDto
        {
            Id = history.Id,
            PatientId = history.PatientId,
            PatientName = patient?.Name ?? "",
            DoctorId = history.DoctorId,
            DoctorName = doctor?.Name ?? "",
            DoctorSpecialty = doctor?.Specialty ?? "",
            VisitDate = history.VisitDate,
            Diagnosis = history.Diagnosis,
            Symptoms = history.Symptoms,
            Treatment = history.Treatment,
            Prescription = history.Prescription,
            Notes = history.Notes,
            CreatedAt = history.CreatedAt,
            UpdatedAt = history.UpdatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var history = await _context.MedicalHistories.FindAsync(id);
        if (history == null) return false;

        _context.MedicalHistories.Remove(history);
        await _context.SaveChangesAsync();
        return true;
    }
}
