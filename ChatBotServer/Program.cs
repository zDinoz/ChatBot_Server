using Microsoft.EntityFrameworkCore;
using ChatBotServer.Data;
using ChatBotServer.Services;
using ChatBotServer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.EnsureCreated();

app.Run();

{
    if (!db.Patients.Any())
    {
        db.Patients.AddRange(
            new Patient { Name = "John Smith", Email = "john@email.com", Phone = "0912345678", DateOfBirth = new DateTime(1985, 3, 15), Address = "123 Main St" },
            new Patient { Name = "Mary Johnson", Email = "mary@email.com", Phone = "0912345679", DateOfBirth = new DateTime(1990, 7, 22), Address = "456 Oak Ave" },
            new Patient { Name = "Bob Williams", Email = "bob@email.com", Phone = "0912345680", DateOfBirth = new DateTime(1978, 11, 5), Address = "789 Pine Rd" }
        );
    }

    if (!db.Doctors.Any())
    {
        db.Doctors.AddRange(
            new Doctor { Name = "Dr. Sarah Lee", Specialty = "Cardiology", Email = "sarah@clinic.com", Phone = "0111111111" },
            new Doctor { Name = "Dr. Michael Chen", Specialty = "General Medicine", Email = "michael@clinic.com", Phone = "0111111112" },
            new Doctor { Name = "Dr. Emily Brown", Specialty = "Pediatrics", Email = "emily@clinic.com", Phone = "0111111113" }
        );
    }

    if (!db.Rooms.Any())
    {
        db.Rooms.AddRange(
            new Room { Name = "Room 101", Capacity = 2, Location = "Floor 1" },
            new Room { Name = "Room 102", Capacity = 4, Location = "Floor 1" },
            new Room { Name = "Room 201", Capacity = 6, Location = "Floor 2" }
        );
    }

    db.SaveChanges();
}