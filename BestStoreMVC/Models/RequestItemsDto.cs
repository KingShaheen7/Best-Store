using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
	public class RequestItemsDto
	{
        [Required, MaxLength(100)]
		public string Name { get; set; } = "";

		[Required]
		public decimal Price { get; set; }

		public IFormFile? ImageFile { get; set; }

		public int Cuantity { get; set; }

		public string Category { get; set; } = "";

		public string UserEmail { get; set; } = "";

	}
}
