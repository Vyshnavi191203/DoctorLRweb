CREATE PROCEDURE GetAvailableDepartments
AS
BEGIN
   SELECT DISTINCT Department FROM DoctorSchedules;
END;