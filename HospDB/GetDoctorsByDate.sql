CREATE PROCEDURE GetDoctorsByDate
   @AvailableDate DATE
AS
BEGIN
   SELECT
       ds.ScheduleId,  -- Include this column
       u.UserId AS DoctorId,
       u.Name AS DoctorName,
       ds.Department,
       ds.AvailableDate,
       ds.TimeSlot
   FROM DoctorSchedules ds
   INNER JOIN Users u ON ds.DoctorId = u.UserId
   WHERE ds.AvailableDate = @AvailableDate
   AND u.Role = 'Doctor';
END;