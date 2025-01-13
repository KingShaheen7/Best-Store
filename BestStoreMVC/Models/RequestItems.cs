using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class RequestItems
    {
		public int Id { get; set; }
		public Product? RequestProduct { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
		public int Cuantity { get; set; }

        [Required]
		public string UserId { get; set; } = "";

		public DateTime RequestDate { get; set; }

	}
}
