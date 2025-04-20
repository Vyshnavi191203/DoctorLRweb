CREATE PROCEDURE GetAvailableDoctorsByDate
   @AvailableDate DATE
AS
BEGIN
   SELECT
       ds.ScheduleId,
       ds.DoctorId,
	   ds.Department,
       u.Name AS DoctorName,
       ds.AvailableDate,
       ds.TimeSlot
   FROM DoctorSchedules ds
   INNER JOIN Users u ON ds.DoctorId = u.UserId
   WHERE ds.AvailableDate = @AvailableDate
   -- ✅ Exclude time slots that are already booked in `Appointment`
   AND NOT EXISTS (
       SELECT 1 FROM Appointments a
       WHERE a.DoctorId = ds.DoctorId
       AND a.AppointmentDate = ds.AvailableDate
       AND a.TimeSlot = ds.TimeSlot
   );
END;