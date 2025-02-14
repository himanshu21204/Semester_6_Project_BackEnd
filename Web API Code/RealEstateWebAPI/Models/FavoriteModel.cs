namespace RealEstateWebAPI.Models
{
	public class FavoriteModel
	{
		public int? FavoriteID { get; set; }
		public int UserID { get; set; }
		public string? UserName { get; set; }
		public int PropertyID { get; set; }
		public DateTime CreatedAt { get; set; }
	}
	public class AddFavoriteModel
	{
		public int UserID { get; set; }
		public int PropertyID { get; set; }
	}

}
