using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace application.Services
{
	[Table("STATUS")]
	public class StatusModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("StatusID")]
		public int StatusID { get; set; }

		[Required]
		[Column("Title")]
		[StringLength(100)]
		public string Title { get; set; } = String.Empty;
	}

}
