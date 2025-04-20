CREATE PROCEDURE AddAppointmentNotification  
   @AppointmentID INT,
   @Message NVARCHAR(255) OUTPUT  
AS  
BEGIN  
   DECLARE @PatientID INT, @DoctorID INT, @AppointmentDate DATE, @TimeSlot NVARCHAR(8), @DoctorName NVARCHAR(100)
   -- Fetch details
   SELECT
       @PatientID = PatientID,
       @DoctorID = DoctorID,
       @AppointmentDate = CAST(AppointmentDate AS DATE), -- Only Date (YYYY-MM-DD)
       @TimeSlot = CONVERT(VARCHAR(8), TimeSlot, 108) -- Format Time as HH:mm:ss
   FROM Appointments WHERE AppointmentID = @AppointmentID
   -- Fetch Doctor Name
   SELECT @DoctorName = Name FROM [Users] WHERE UserID = @DoctorID AND Role = 'Doctor'
   -- Generate Notification Message
   IF @PatientID IS NOT NULL AND @DoctorID IS NOT NULL AND @AppointmentDate IS NOT NULL AND @TimeSlot IS NOT NULL AND @DoctorName IS NOT NULL
   BEGIN
       SET @Message = 'Dear Patient, you have an appointment on ' + CONVERT(NVARCHAR, @AppointmentDate, 23)
                      + ' at ' + @TimeSlot + ' with Dr. ' + @DoctorName + '.'
       -- Insert into Notifications table
       INSERT INTO Notifications (AppointmentID, Message, Timestamp)
       VALUES (@AppointmentID, @Message, GETDATE())
   END
   ELSE
   BEGIN
       SET @Message = 'Error: Missing data for notification.'
   END
END