namespace RealEstateWebAPI.Models
{
	public class ContactUsModel
	{
		public int? ContactID { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string? Subject { get; set; }
		public string Message { get; set; }
		public string? Status { get; set; }
		public DateTime? SubmittedAt { get; set; }
	}
	public class StatusModel
	{
		public string? Status { get; set; }
	}
}
