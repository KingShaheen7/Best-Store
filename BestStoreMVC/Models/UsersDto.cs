using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class UsersDto
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = "";

        [Required]
        [StringLength(255)]
        public string ConPasswordHash { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = "";

        public string remember { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = "";
    }
}
