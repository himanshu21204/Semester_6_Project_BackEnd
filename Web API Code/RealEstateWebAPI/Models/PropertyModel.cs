namespace RealEstateWebAPI.Models
{
	public class PropertyModel
	{
		public int? PropertyID { get; set; }
		public int UserID { get; set; }
		public string? UserName { get; set; }
		public string? UserProfilePhoto { get; set; }
		public string PropertyTitle { get; set; }
		public string PropertyDescription { get; set; }
		public decimal PropertyPrice { get; set; }
		public string PropertyAddress { get; set; }
		public decimal PropertySize { get; set; }
		public int BedroomCount { get; set; }
		public int BathroomCount { get; set; }
		public DateTime BuildYear { get; set; }
		public string PropertyType { get; set; }
		public string TransactionType { get; set; }
		public float? ParkingSpaces { get; set; }
		public string AdditionalFeatures { get; set; }
		public string Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }

		public List<PropertyImageModel>? Images { get; set; } = new List<PropertyImageModel>();
	}
}
