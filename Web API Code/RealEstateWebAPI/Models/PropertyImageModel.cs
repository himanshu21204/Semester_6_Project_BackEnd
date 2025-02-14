namespace RealEstateWebAPI.Models
{
	public class PropertyImageModel
	{
		public int? ImageID { get; set; }
		public int PropertyID { get; set; }
		public string ImageURL { get; set; }
		public DateTime UploadedAt { get; set; }
	}
}
