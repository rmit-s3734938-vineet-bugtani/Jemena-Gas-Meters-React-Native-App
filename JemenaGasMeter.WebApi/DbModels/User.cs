using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JemenaGasMeter.WebApi.DbModels
{
    public class User
    {
        [Key]
        public string PayRollID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string LastName { get; set; }
        [Display(Name = "User Type")]
        public UserType UserType { get; set; }
        public string PIN { get; set; }
        public UserStatus UserStatus { get; set; }
        public DateTime ModifyDate { get; set; }
        public ICollection<MeterHistory> MeterHistories { get; set; }
    }
}
