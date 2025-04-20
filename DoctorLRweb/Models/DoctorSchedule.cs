using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLRweb.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int DoctorId { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public DateOnly AvailableDate { get; set; }

        [Required]
        public TimeOnly TimeSlot { get; set; }

    }
}