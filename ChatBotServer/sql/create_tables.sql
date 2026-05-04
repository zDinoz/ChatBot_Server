-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ChatBotDB')
BEGIN
    CREATE DATABASE ChatBotDB;
END
GO

USE ChatBotDB;
GO

-- Drop existing tables in correct order (foreign key constraints)
IF OBJECT_ID('dbo.MedicalHistories', 'U') IS NOT NULL DROP TABLE dbo.MedicalHistories;
IF OBJECT_ID('dbo.Appointments', 'U') IS NOT NULL DROP TABLE dbo.Appointments;
IF OBJECT_ID('dbo.Rooms', 'U') IS NOT NULL DROP TABLE dbo.Rooms;
IF OBJECT_ID('dbo.Doctors', 'U') IS NOT NULL DROP TABLE dbo.Doctors;
IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL DROP TABLE dbo.Patients;
GO

-- Create Patients table
CREATE TABLE Patients (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    DateOfBirth DATETIME NOT NULL,
    Address NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Create Doctors table
CREATE TABLE Doctors (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Create Rooms table
CREATE TABLE Rooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    Location NVARCHAR(100) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Create Appointments table
CREATE TABLE Appointments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    RoomId INT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Scheduled',
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE,
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoomId) REFERENCES Rooms(Id) ON DELETE SET NULL
);
GO

-- Create MedicalHistories table
CREATE TABLE MedicalHistories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    VisitDate DATETIME NOT NULL,
    Diagnosis NVARCHAR(MAX) NOT NULL DEFAULT '',
    Symptoms NVARCHAR(MAX) NULL,
    Treatment NVARCHAR(MAX) NULL,
    Prescription NVARCHAR(MAX) NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE,
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
);
GO

-- Insert seed data for Patients
INSERT INTO Patients (Name, Email, Phone, DateOfBirth, Address) VALUES
('John Smith', 'john@email.com', '0912345678', '1985-03-15', '123 Main St'),
('Mary Johnson', 'mary@email.com', '0912345679', '1990-07-22', '456 Oak Ave'),
('Bob Williams', 'bob@email.com', '0912345680', '1978-11-05', '789 Pine Rd');
GO

-- Insert seed data for Doctors
INSERT INTO Doctors (Name, Specialty, Email, Phone) VALUES
('Dr. Sarah Lee', 'Cardiology', 'sarah@clinic.com', '0111111111'),
('Dr. Michael Chen', 'General Medicine', 'michael@clinic.com', '0111111112'),
('Dr. Emily Brown', 'Pediatrics', 'emily@clinic.com', '0111111113');
GO

-- Insert seed data for Rooms
INSERT INTO Rooms (Name, Capacity, Location, IsActive) VALUES
('Room 101', 2, 'Floor 1', 1),
('Room 102', 4, 'Floor 1', 1),
('Room 201', 6, 'Floor 2', 1);
GO

-- Insert seed data for Appointments
INSERT INTO Appointments (PatientId, DoctorId, RoomId, AppointmentDate, Status, Notes) VALUES
(1, 1, 1, '2025-01-15 09:00:00', 'Completed', 'Regular checkup'),
(2, 2, 2, '2025-01-16 10:30:00', 'Completed', 'Follow-up visit'),
(3, 3, 3, '2025-01-17 14:00:00', 'Scheduled', 'Annual physical');
GO

-- Insert seed data for MedicalHistories
INSERT INTO MedicalHistories (PatientId, DoctorId, VisitDate, Diagnosis, Symptoms, Treatment, Prescription, Notes, CreatedAt, UpdatedAt) VALUES
(1, 1, '2025-01-15', 'Hypertension', 'High blood pressure, headache', 'Prescribed medication, lifestyle changes', 'Amlodipine 5mg daily', 'Patient advised to reduce salt intake', GETUTCDATE(), GETUTCDATE()),
(1, 2, '2024-11-20', 'Common Cold', 'Cough, sore throat, mild fever', 'Rest, fluids, OTC medication', 'Paracetamol 500mg as needed', 'Recovery expected in 5-7 days', GETUTCDATE(), GETUTCDATE()),
(2, 2, '2025-01-16', 'Type 2 Diabetes', 'Elevated blood sugar levels', 'Diet control, medication', 'Metformin 500mg twice daily', 'Monthly blood sugar monitoring required', GETUTCDATE(), GETUTCDATE()),
(2, 1, '2024-10-05', 'Migraine', 'Recurring headaches, sensitivity to light', 'Pain management, trigger avoidance', 'Ibuprofen 400mg as needed', 'Stress management recommended', GETUTCDATE(), GETUTCDATE()),
(3, 3, '2025-01-17', 'Asthma', 'Shortness of breath, wheezing', 'Inhaler therapy, trigger avoidance', 'Albuterol inhaler as needed', 'Avoid dust and smoke exposure', GETUTCDATE(), GETUTCDATE()),
(3, 2, '2024-08-12', 'Gastritis', 'Stomach pain, nausea, bloating', 'Dietary changes, antacids', 'Omeprazole 20mg daily', 'Avoid spicy and acidic foods', GETUTCDATE(), GETUTCDATE());
GO
