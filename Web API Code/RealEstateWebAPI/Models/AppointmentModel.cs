namespace RealEstateWebAPI.Models
{
	public class AppointmentModel
	{
		public int? AppointmentID { get; set; }
		public int BookerUserID { get; set; }
		public string? BookerName { get; set; }
		public int? AppointmentUserID { get; set; }
		public string? AppointmentUserName { get; set; }
		public int? PropertyID { get; set; }
		public string? PropertyTitle { get; set; }
		public DateTime AppointmentStartDate { get; set; }
		public DateTime AppointmentEndDate { get; set; }
		public string Status { get; set; } = string.Empty;
		public string? Notes { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
	}

	public class UserDropDownModel
	{
		public int UserID { get; set; }
		public string FullName { get; set; } = string.Empty;
	}

	public class PropertyDropDownModel
	{
		public int PropertyID { get; set; }
		public string PropertyTitle { get; set; } = string.Empty;
	}

	public class AppointmentStatus
	{
		public string? Status { get; set; }
	}
}