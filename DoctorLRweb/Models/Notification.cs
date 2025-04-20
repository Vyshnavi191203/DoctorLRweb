using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoctorLRweb.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }
        [Required]
        public int AppointmentID { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

    }
}