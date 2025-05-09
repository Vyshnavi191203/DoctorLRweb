﻿using DoctorLRweb.Models;
using DoctorLRweb.Repositories;
using NuGet.Protocol.Core.Types;

namespace DoctorLRweb.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAllAppointments();
        }

        public int CreateAppointment(int userId, int doctorId, DateOnly appointmentDate, TimeOnly timeSlot)
        {
            int result = _appointmentRepository.CreateAppointment(userId, doctorId, appointmentDate, timeSlot);
            if (result == -1)
                throw new Exception("Invalid Patient ID.");
            if (result == -2)
                throw new Exception("Invalid Doctor ID.");
            if (result == -3)
                throw new Exception("This doctor is not available on the selected appointment date.");
            if (result == -4)
                throw new Exception("Patient already has an appointment at the same date and time slot.");
            if (result == -5)
                throw new Exception("Doctor is already booked at this date and time slot.");
            return result;
        }
        public bool DeleteAppointment(int appointmentId)
        {
            return _appointmentRepository.DeleteAppointment(appointmentId);
        }
        public List<string> GetAllDepartments()
        {
            return _appointmentRepository.GetAllDepartments();
        }
        public List<User> GetAllDoctorsByDepartment(string department)
        {
            return _appointmentRepository.GetAllDoctorsByDepartment(department);
        }
        public List<DoctorSchedule> GetDoctorsByDateWithDoctorName(DateOnly availableDate)
        {
            return _appointmentRepository.GetDoctorsByDateWithDoctorName(availableDate);
        }

        public List<dynamic> GetAppointmentsByPatientId(int patientId)
        {
            return _appointmentRepository.GetAppointmentsByPatientId(patientId);
        }

        public async Task<IEnumerable<object>> GetAppointmentsByDoctorId(int doctorId)
        {
            return await _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
        }
        public void RescheduleAppointment(int appointmentId, DateOnly newAppointmentDate, TimeOnly newTimeSlot)
        {
            _appointmentRepository.RescheduleAppointment(appointmentId, newAppointmentDate, newTimeSlot);
        }
        public List<DateOnly> GetAvailableDatesForDoctor(int doctorId)
        {
            return _appointmentRepository.GetAvailableDatesForDoctor(doctorId);
        }
        public void DeletePastAppointments()
        {
            _appointmentRepository.DeletePastAppointments();
        }
        public IEnumerable<Appointment> GetAppointmentsByPatientIdentifier(string identifier)
        {
            return _appointmentRepository.GetAppointmentsByPatientIdentifier(identifier);
        }
        public List<string> GetAvailableDepartmentsWithOpenSlots()
        {
            return _appointmentRepository.GetAvailableDepartmentsWithOpenSlots();
        }
        public List<User> GetAvailableDoctorsByDepartment(string department)
        {
            return _appointmentRepository.GetAvailableDoctorsByDepartment(department);
        }
        public List<User> GetDoctorsWithAppointments()
        {
            return _appointmentRepository.GetDoctorsWithAppointments();
        }
        public List<User> GetPatientsWithAppointments()
        {
            return _appointmentRepository.GetPatientsWithAppointments();
        }
    }
}