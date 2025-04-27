CREATE PROCEDURE UpdateDoctorSchedule
    @ScheduleId INT,
    @UserId INT,
    @Department NVARCHAR(100),
    @AvailableDate DATE,
    @TimeSlot TIME
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @DoctorId INT;
    -- Validate Doctor
    SELECT @DoctorId = UserId FROM Users WHERE UserId = @UserId AND Role = 'Doctor';
    IF @DoctorId IS NULL
    BEGIN
        RAISERROR('Invalid Doctor ID or User is not a Doctor.', 16, 1);
        RETURN;
    END
    -- Prevent updating to past dates
    IF @AvailableDate < CAST(GETDATE() AS DATE)
    BEGIN
        RAISERROR('Cannot update schedule to a past date.', 16, 1);
        RETURN;
    END
    -- Update Doctor Schedule
    UPDATE DoctorSchedules
    SET Department = @Department,
        AvailableDate = @AvailableDate,
        TimeSlot = @TimeSlot
    WHERE ScheduleId = @ScheduleId AND DoctorId = @DoctorId;
END;