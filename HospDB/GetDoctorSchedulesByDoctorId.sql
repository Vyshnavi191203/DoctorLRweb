CREATE PROCEDURE GetDoctorSchedulesByDoctorId
    @DoctorId INT
AS
BEGIN
    SELECT ScheduleId, DoctorId,Department, AvailableDate, TimeSlot
    FROM DoctorSchedules
    WHERE DoctorId = @DoctorId
END