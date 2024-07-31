using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace application.Services
{
	[Table("company")]
	public class CompanyModel
	{
		[Key]
		[Column("CompanyID")]
		public Guid CompanyID { get; set; }

		[Required]
		[Column("INN")]
		[StringLength(12)]
		public string INN { get; set; } = String.Empty;

		[Required]
		[Column("KPP")]
		[StringLength(12)]
		public string KPP { get; set; } = String.Empty;

		[Required]
		[Column("OGRN")]
		[StringLength(13)]
		public string OGRN { get; set; } = String.Empty;

		[Required]
		[Column("FullName")]
		[StringLength(100)]
		public string FullName { get; set; } = String.Empty;

		[Required]
		[Column("ShortName")]
		[StringLength(100)]
		public string ShortName { get; set; } = String.Empty;

		[Required]
		[Column("LegalAddress")]
		[StringLength(1000)]
		public string LegalAddress { get; set; } = String.Empty;

		[Required]
		[Column("PostalAddress")]
		[StringLength(1000)]
		public string PostalAddress { get; set; } = String.Empty;

		[Required]
		[Column("Director")]
		public string Director { get; set; } = String.Empty;

		[Column("Logo")]
		public byte[] Logo { get; set; }

		[Required]
		[Column("IsDeleted")]
		public bool IsDeleted { get; set; }
	}

}
