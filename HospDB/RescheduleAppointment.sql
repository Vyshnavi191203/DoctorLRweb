CREATE PROCEDURE RescheduleAppointment
    @AppointmentID INT,
    @NewAppointmentDate DATE,
    @NewTimeSlot TIME
AS
BEGIN
    DECLARE @PatientId INT, @DoctorId INT;
    -- ✅ Get `PatientId` and `DoctorId` from the existing appointment
    SELECT @PatientId = PatientId, @DoctorId = DoctorId
    FROM Appointments
    WHERE AppointmentID = @AppointmentID;
    -- ✅ If the appointment doesn't exist, return an error
    IF @PatientId IS NULL OR @DoctorId IS NULL
    BEGIN
        RAISERROR ('Appointment not found.', 16, 1);
        RETURN;
    END;
    -- ✅ Ensure the doctor is available on the new date and time slot in `DoctorSchedule`
    IF NOT EXISTS (
        SELECT 1 FROM DoctorSchedules
        WHERE DoctorId = @DoctorId
        AND AvailableDate = @NewAppointmentDate
        AND TimeSlot = @NewTimeSlot
    )
    BEGIN
        RAISERROR ('Selected date and time slot are not available for this doctor.', 16, 1);
        RETURN;
    END;
    -- ✅ Check if the patient already has an appointment at the same date and time slot
    IF EXISTS (
        SELECT 1 FROM Appointments
        WHERE PatientId = @PatientId
        AND AppointmentDate = @NewAppointmentDate
        AND TimeSlot = @NewTimeSlot
        AND AppointmentID <> @AppointmentID
    )
    BEGIN
        RAISERROR ('Patient already has an appointment at the same date and time slot.', 16, 1);
        RETURN;
    END;
    -- ✅ Check if the doctor is already booked at the new date and time slot
    IF EXISTS (
        SELECT 1 FROM Appointments
        WHERE DoctorId = @DoctorId
        AND AppointmentDate = @NewAppointmentDate
        AND TimeSlot = @NewTimeSlot
        AND AppointmentID <> @AppointmentID
    )
    BEGIN
        RAISERROR ('Doctor is already booked at this date and time slot.', 16, 1);
        RETURN;
    END;
    -- ✅ Update the appointment with the new date and time slot
    UPDATE Appointments
    SET AppointmentDate = @NewAppointmentDate,
        TimeSlot = @NewTimeSlot
    WHERE AppointmentID = @AppointmentID;
END;