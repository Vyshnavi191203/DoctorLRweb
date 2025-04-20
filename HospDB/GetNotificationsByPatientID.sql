CREATE PROCEDURE GetNotificationsByPatientID
   @PatientID INT
AS
BEGIN
   SELECT
       N.NotificationID,
       N.Message,
       N.Timestamp,
       N.AppointmentID
   FROM Notifications N
   INNER JOIN Appointments A ON N.AppointmentID = A.AppointmentID
   WHERE A.PatientID = @PatientID
   ORDER BY N.Timestamp DESC;
END;