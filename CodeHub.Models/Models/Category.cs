using System.ComponentModel.DataAnnotations;

namespace CodeHub.Models.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(20)]
		[MinLength(5)]
		public required string Name { get; set; }
		[Required]
		[MaxLength(200)]
		[MinLength(10)]
		public required string Description { get; set; }

		public ICollection<Product> Products { get; set; }

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}";
		}
	}
}
