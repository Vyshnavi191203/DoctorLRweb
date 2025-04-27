CREATE PROCEDURE GetDoctorsByDepartment
   @Department NVARCHAR(100)
AS
BEGIN
   SELECT DISTINCT
       u.UserId AS DoctorId,
       u.Name AS DoctorName,
       ds.Department
   FROM DoctorSchedules ds
   INNER JOIN Users u ON ds.DoctorId = u.UserId
   WHERE ds.Department = @Department
   AND u.Role = 'Doctor';
END;