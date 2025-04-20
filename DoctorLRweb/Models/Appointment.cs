using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLRweb.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required]


        public int PatientId { get; set; }

        [Required]
        public int DoctorID { get; set; }

        [Required]
        public DateOnly AppointmentDate { get; set; }

        [Required]
        public TimeOnly TimeSlot { get; set; }

        [Required]
        public string Status { get; set; }

        //[ForeignKey("PatientId")]
        //public User Patient { get; set; }

        //[ForeignKey("DoctorID")]
        //public User Doctor { get; set; }
    }
}