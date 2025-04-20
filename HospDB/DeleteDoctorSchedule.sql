CREATE PROCEDURE DeleteDoctorSchedule
   @ScheduleId INT
AS
BEGIN
   DELETE FROM DoctorSchedules WHERE ScheduleId = @ScheduleId;
END