CREATE PROCEDURE GetPatientIdFromUser
   @UserId INT,
   @PatientId INT OUTPUT
AS
BEGIN
   -- Fetch the PatientId from User table where Role = 'Patient'
   SELECT @PatientId = UserId FROM [Users] WHERE UserId = @UserId AND Role = 'Patient';
END;