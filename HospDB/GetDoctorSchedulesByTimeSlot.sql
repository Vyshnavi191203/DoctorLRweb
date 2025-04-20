CREATE PROCEDURE GetDoctorSchedulesByTimeSlot
   @TimeSlot TIME
AS
BEGIN
   SELECT * FROM DoctorSchedules WHERE TimeSlot = @TimeSlot;
END