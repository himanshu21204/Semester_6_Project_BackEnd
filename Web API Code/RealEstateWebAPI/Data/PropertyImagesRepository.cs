using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class PropertyImagesRepository
	{
		#region Configuration Connection String
		private readonly string _connectionString;

		public PropertyImagesRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#endregion

		#region Insert Property Image
		public bool Insert(int propertyID, string imageURL)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_PropertyImage_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@PropertyID", propertyID);
				command.Parameters.AddWithValue("@ImageURL", imageURL);

				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Get Property Images by PropertyID
		public IEnumerable<PropertyImageModel> GetByPropertyID(int propertyID)
		{
			List<PropertyImageModel> images = new List<PropertyImageModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_PropertyImage_GetByPropertyID", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@PropertyID", propertyID);

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					images.Add(new PropertyImageModel
					{
						ImageID = Convert.ToInt32(reader["ImageID"]),
						PropertyID = Convert.ToInt32(reader["PropertyID"]),
						ImageURL = reader["ImageURL"].ToString(),
						UploadedAt = Convert.ToDateTime(reader["UploadedAt"])
					});
				}
			}
			return images;
		}
		#endregion

		#region Delete Property Image by ImageID
		public bool DeleteByImageID(int imageID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_PropertyImage_DeleteByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@ImageID", imageID);

				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Delete Property Images by PropertyID
		public bool DeleteByPropertyID(int propertyID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_PropertyImage_DeleteByPropertyID", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@PropertyID", propertyID);

				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Update Images
		public bool Update(int imageID, int propertyID, string imageURL)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_PropertyImage_Update", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@PropertyID", propertyID);
				command.Parameters.AddWithValue("@ImageURL", imageURL);
				command.Parameters.AddWithValue("@ImageID", imageID);

				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
	}
}
