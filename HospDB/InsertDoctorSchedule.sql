CREATE PROCEDURE InsertDoctorSchedule
    @UserId INT,
    @Department NVARCHAR(100),
    @AvailableDate DATE,
    @TimeSlot TIME
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @DoctorId INT;
    -- Validate if the UserId belongs to a Doctor
    SELECT @DoctorId = UserId FROM Users WHERE UserId = @UserId AND Role = 'Doctor';
    IF @DoctorId IS NULL
    BEGIN
        RAISERROR('Invalid Doctor ID or User is not a Doctor.', 16, 1);
        RETURN;
    END
    -- Prevent inserting schedules for past dates
    IF @AvailableDate < CAST(GETDATE() AS DATE)
    BEGIN
        RAISERROR('Cannot add schedule for a past date.', 16, 1);
        RETURN;
    END
    -- Prevent the same doctor from having schedules in different departments
    IF EXISTS (
        SELECT 1 FROM DoctorSchedules
        WHERE DoctorId = @DoctorId AND Department <> @Department
    )
    BEGIN
        RAISERROR('Doctor cannot have schedules in different departments.', 16, 1);
        RETURN;
    END
    -- Insert the doctor schedule if all checks pass
    INSERT INTO DoctorSchedules (DoctorId, Department, AvailableDate, TimeSlot)
    VALUES (@DoctorId, @Department, @AvailableDate, @TimeSlot);
END;