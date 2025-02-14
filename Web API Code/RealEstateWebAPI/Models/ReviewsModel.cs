using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealEstateWebAPI.Models
{
	public class PropertyReview
	{
		public int ReviewID { get; set; }

		public int PropertyID { get; set; }

		public int UserID { get; set; }
		public string? UserName { get; set; }

		public int Rating { get; set; }

		public string ReviewText { get; set; }

		public string Keywords { get; set; }

		public DateTime SubmittedAt { get; set; }
	}
	public class AgentReview
	{
		public int ReviewID { get; set; }

		public int AgentID { get; set; }

		public int UserID { get; set; }
		public string? UserName { get; set; }

		public int Rating { get; set; }

		public string ReviewText { get; set; }

		public string Keywords { get; set; }

		public DateTime SubmittedAt { get; set; }
	}
}
