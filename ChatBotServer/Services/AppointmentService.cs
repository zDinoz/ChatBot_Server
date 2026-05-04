using Microsoft.EntityFrameworkCore;
using ChatBotServer.Data;
using ChatBotServer.DTOs;
using ChatBotServer.Models;

namespace ChatBotServer.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Room)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient != null ? a.Patient.Name : "",
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor != null ? a.Doctor.Name : "",
                RoomId = a.RoomId,
                RoomName = a.Room != null ? a.Room.Name : null,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt
            }).ToListAsync();
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Room)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (appointment == null) return null;
        
        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientName = appointment.Patient?.Name ?? "",
            DoctorId = appointment.DoctorId,
            DoctorName = appointment.Doctor?.Name ?? "",
            RoomId = appointment.RoomId,
            RoomName = appointment.Room?.Name,
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        };
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto createDto)
    {
        var appointment = new Appointment
        {
            PatientId = createDto.PatientId,
            DoctorId = createDto.DoctorId,
            RoomId = createDto.RoomId,
            AppointmentDate = createDto.AppointmentDate,
            Notes = createDto.Notes
        };
        appointment.CreatedAt = DateTime.UtcNow;
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        var patient = await _context.Patients.FindAsync(appointment.PatientId);
        var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
        Room? room = null;
        if (appointment.RoomId.HasValue)
            room = await _context.Rooms.FindAsync(appointment.RoomId.Value);

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientName = patient?.Name ?? "",
            DoctorId = appointment.DoctorId,
            DoctorName = doctor?.Name ?? "",
            RoomId = appointment.RoomId,
            RoomName = room?.Name,
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        };
    }

    public async Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentDto updateDto)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return null;

        appointment.PatientId = updateDto.PatientId;
        appointment.DoctorId = updateDto.DoctorId;
        appointment.RoomId = updateDto.RoomId;
        appointment.AppointmentDate = updateDto.AppointmentDate;
        appointment.Status = updateDto.Status;
        appointment.Notes = updateDto.Notes;

        await _context.SaveChangesAsync();

        var patient = await _context.Patients.FindAsync(appointment.PatientId);
        var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
        Room? room = null;
        if (appointment.RoomId.HasValue)
            room = await _context.Rooms.FindAsync(appointment.RoomId.Value);

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            PatientName = patient?.Name ?? "",
            DoctorId = appointment.DoctorId,
            DoctorName = doctor?.Name ?? "",
            RoomId = appointment.RoomId,
            RoomName = room?.Name,
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return false;
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }
}