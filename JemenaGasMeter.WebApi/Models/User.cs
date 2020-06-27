using System;
using System.ComponentModel.DataAnnotations;

namespace JemenaGasMeter.WebApi.Models
{
    public class User
    {
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
    }
}
