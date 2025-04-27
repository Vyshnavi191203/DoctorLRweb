CREATE PROCEDURE InsertAppointment
    @UserId INT,
    @DoctorId INT,
    @AppointmentDate DATE,
    @TimeSlot TIME,
    @AppointmentID INT OUTPUT
AS
BEGIN
    DECLARE @PatientId INT;
    -- ✅ Ensure `UserId` exists in `Users` table where Role = 'patient'
    SELECT @PatientId = UserId FROM Users WHERE UserId = @UserId AND Role = 'patient';
    IF @PatientId IS NULL
    BEGIN
        RAISERROR ('Invalid Patient ID', 16, 1);
        RETURN;
    END;
    -- ✅ Ensure `DoctorId` exists in `Users` table where Role = 'doctor'
    IF NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @DoctorId AND Role = 'doctor')
    BEGIN
        RAISERROR ('Invalid Doctor ID', 16, 1);
        RETURN;
    END;
    -- ✅ Ensure `AppointmentDate` exists in `DoctorSchedule` for the given `DoctorId`
    IF NOT EXISTS (
        SELECT 1 FROM DoctorSchedules
        WHERE DoctorId = @DoctorId
        AND AvailableDate = @AppointmentDate
    )
    BEGIN
        RAISERROR ('This doctor is not available on the selected appointment date', 16, 1);
        RETURN;
    END;
    -- ✅ Check if the patient already has an appointment at the same date and time
    IF EXISTS (
        SELECT 1 FROM Appointments
        WHERE PatientId = @PatientId
        AND AppointmentDate = @AppointmentDate
        AND TimeSlot = @TimeSlot
    )
    BEGIN
        RAISERROR ('Patient already has an appointment at the same date and time slot', 16, 1);
        RETURN;
    END;
    -- ✅ Check if the doctor is already booked at the same date and time
    IF EXISTS (
        SELECT 1 FROM Appointments
        WHERE DoctorId = @DoctorId
        AND AppointmentDate = @AppointmentDate
        AND TimeSlot = @TimeSlot
    )
    BEGIN
        RAISERROR ('Doctor is already booked at this date and time slot', 16, 1);
        RETURN;
    END;
    -- ✅ Insert into `Appointment` table with default `Status = 'Scheduled'`
    INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, TimeSlot, Status)
    VALUES (@PatientId, @DoctorId, @AppointmentDate, @TimeSlot, 'Scheduled');
    -- ✅ Return the new Appointment ID
    SET @AppointmentID = SCOPE_IDENTITY();
END;