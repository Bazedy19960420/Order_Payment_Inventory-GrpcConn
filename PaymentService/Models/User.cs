using System.ComponentModel.DataAnnotations;

namespace PaymentService.Models
{
    public class User{
        [Key]
        public int UserId { get; set;}
        public int Balance { get; set;}
    }
}