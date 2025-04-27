using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLRweb.Models
{

    public class MedicalHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HistoryID { get; set; }

        [Required]
        [ForeignKey("User")]
        public int PatientId { get; set; }

        [Required]
        public string DoctorNames { get; set; }


        [Required]
        [MaxLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        [MaxLength(500)]
        public string Treatment { get; set; }

        [Required]
        public DateTime DateOfVisit { get; set; }

    }
}