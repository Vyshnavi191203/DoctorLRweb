CREATE PROCEDURE GetDoctorSchedulesByDepartment
   @Department NVARCHAR(100)
AS
BEGIN
   SELECT * FROM DoctorSchedules WHERE Department = @Department;
END