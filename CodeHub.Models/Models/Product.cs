
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CodeHub.Models.Models
{
	public class Product
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(60)]
		[MinLength(5)]
		public string Name { get; set; }
		[Required]
		[MaxLength]
		public string Description { get; set; }
		public long Downloads { get; set; } = 0;
		[Required]
		public double Price { get; set; }
		[Required]
		[RegularExpression(@"^\d+(\.\d+){0,2}$", ErrorMessage = "Invalid version format. Must be in the format of x.y.z")]
		public string Version { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		[ValidateNever]
		public string ImageUrl { get; set; }
		[ValidateNever]
		public string LogoUrl { get; set; }
		[ValidateNever]
		public string SourceCodeUrl { get; set; }
		[Required]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(Downloads)}: {Downloads}, {nameof(Price)}: {Price}, {nameof(Version)}: {Version}, {nameof(CreatedAt)}: {CreatedAt}, {nameof(UpdatedAt)}: {UpdatedAt}, {nameof(ImageUrl)}: {ImageUrl}, {nameof(LogoUrl)}: {LogoUrl}, {nameof(SourceCodeUrl)}: {SourceCodeUrl}, {nameof(Category)}: {Category}";
		}
	}
}
