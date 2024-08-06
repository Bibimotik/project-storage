using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace application.Services
{
	[Table("company")]
	public class CompanyEntity
	{
		[Key]
		[Column("CompanyID")]
		public Guid CompanyID { get; set; }

		[Required]
		[Column("INN")]
		[StringLength(12)]
		public string INN { get; set; } = string.Empty;

		[Required]
		[Column("KPP")]
		[StringLength(12)]
		public string KPP { get; set; } = string.Empty;

		[Required]
		[Column("FullName")]
		[StringLength(100)]
		public string FullName { get; set; } = string.Empty;

		[Required]
		[Column("ShortName")]
		[StringLength(100)]
		public string ShortName { get; set; } = string.Empty;

		[Required]
		[Column("LegalAddress")]
		[StringLength(1000)]
		public string LegalAddress { get; set; } = string.Empty;

		[Required]
		[Column("PostalAddress")]
		[StringLength(1000)]
		public string PostalAddress { get; set; } = string.Empty;

		[Required]
		[Column("OGRN")]
		[StringLength(13)]
		public string OGRN { get; set; } = string.Empty;

		[Required]
		[Column("Director")]
		public string Director { get; set; } = string.Empty;

		[Required]
		[Column("Logo")]
		public byte[] Logo { get; set; }

		[Required]
		[Column("IsDeleted")]
		public bool IsDeleted { get; set; }
	}

}
