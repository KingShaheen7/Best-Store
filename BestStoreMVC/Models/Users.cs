using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
	public int UserId { get; set; }  

	[Required]
	[StringLength(100)] 
	public string Username { get; set; } = "";

	[Required]
	[StringLength(255)] 
	public string PasswordHash { get; set; } = "";   

	[Required]
	[StringLength(100)]
	public string FullName { get; set; } = "";

	[Required]
	[StringLength(100)]
	public string Email { get; set; } = "";
}
