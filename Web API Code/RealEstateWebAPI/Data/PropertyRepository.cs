using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace RealEstateWebAPI.Data
{
	public class PropertyRepository
	{
		#region Configuration Connection String
		private readonly string _connectionString;
		private readonly PropertyImagesRepository _propertyImageRepository;
		public PropertyRepository(IConfiguration configuration, PropertyImagesRepository propertyImagesRepository)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_propertyImageRepository = propertyImagesRepository;
		}
		#endregion

		#region Select All Properties
		public IEnumerable<PropertyModel> SelectAll()
		{
			List<PropertyModel> properties = new List<PropertyModel>();
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Property_SelectAll", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var images = _propertyImageRepository.GetByPropertyID(Convert.ToInt32(reader["PropertyID"])) ?? new List<PropertyImageModel>();

						properties.Add(new PropertyModel
						{
							PropertyID = Convert.ToInt32(reader["PropertyID"]),
							UserID = Convert.ToInt32(reader["UserID"]),
							UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : null,
							UserProfilePhoto = reader["ProfilePhoto"] != DBNull.Value ? reader["ProfilePhoto"].ToString() : null,
							PropertyTitle = reader["PropertyTitle"].ToString(),
							PropertyDescription = reader["PropertyDescription"].ToString(),
							PropertyPrice = Convert.ToDecimal(reader["PropertyPrice"]),
							PropertyAddress = reader["PropertyAddress"].ToString(),
							PropertySize = Convert.ToDecimal(reader["PropertySize"]),
							BedroomCount = Convert.ToInt32(reader["BedroomCount"]),
							BathroomCount = Convert.ToInt32(reader["BathroomCount"]),
							BuildYear = Convert.ToDateTime(reader["BuildYear"]),
							TransactionType = reader["TransactionType"].ToString(),
							PropertyType = reader["PropertyType"].ToString(),
							ParkingSpaces = reader["ParkingSpaces"] != DBNull.Value ? Convert.ToSingle(reader["ParkingSpaces"]) : (float?)null,
							Status = reader["Status"].ToString(),
							CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
							ModifiedAt = reader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedAt"]) : (DateTime?)null,
							AdditionalFeatures = reader["AdditionalFeatures"] != DBNull.Value ? reader["AdditionalFeatures"].ToString() : null,
							Images = (List<PropertyImageModel>)images
						});
					}
				}
			}

			return properties;
		}
		#endregion


		#region Select Property By ID
		public PropertyModel SelectByID(int propertyID)
		{
			PropertyModel property = null;
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Property_SelectByPK", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@PropertyID", propertyID);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					var images = _propertyImageRepository.GetByPropertyID(Convert.ToInt32(reader["PropertyID"])) ?? new List<PropertyImageModel>();
					property = new PropertyModel
					{
						PropertyID = Convert.ToInt32(reader["PropertyID"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : null,
						PropertyTitle = reader["PropertyTitle"].ToString(),
						PropertyDescription = reader["PropertyDescription"].ToString(),
						PropertyPrice = Convert.ToDecimal(reader["PropertyPrice"]),
						PropertyAddress = reader["PropertyAddress"].ToString(),
						PropertySize = Convert.ToDecimal(reader["PropertySize"]),
						BedroomCount = Convert.ToInt32(reader["BedroomCount"]),
						BathroomCount = Convert.ToInt32(reader["BathroomCount"]),
						BuildYear = Convert.ToDateTime(reader["BuildYear"]),
						TransactionType = reader["TransactionType"].ToString(),
						PropertyType = reader["PropertyType"].ToString(),
						Status = reader["Status"].ToString(),
						ParkingSpaces = reader["ParkingSpaces"] != DBNull.Value ? Convert.ToSingle(reader["ParkingSpaces"]) : (float?)null,
						CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
						ModifiedAt = reader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedAt"]) : (DateTime?)null,
						AdditionalFeatures = reader["AdditionalFeatures"] != DBNull.Value ? reader["AdditionalFeatures"].ToString() : null,
						Images = (List<PropertyImageModel>)images
					};
				}
			}
			return property;
		}
		#endregion

		#region Insert Property
		public bool Insert(PropertyModel propertyModel)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Property_Insert", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@UserID", propertyModel.UserID);
				cmd.Parameters.AddWithValue("@PropertyTitle", propertyModel.PropertyTitle);
				cmd.Parameters.AddWithValue("@PropertyDescription", propertyModel.PropertyDescription);
				cmd.Parameters.AddWithValue("@PropertyPrice", propertyModel.PropertyPrice);
				cmd.Parameters.AddWithValue("@PropertyAddress", propertyModel.PropertyAddress);
				cmd.Parameters.AddWithValue("@PropertySize", propertyModel.PropertySize);
				cmd.Parameters.AddWithValue("@BedroomCount", propertyModel.BedroomCount);
				cmd.Parameters.AddWithValue("@BathroomCount", propertyModel.BathroomCount);
				cmd.Parameters.AddWithValue("@BuildYear", propertyModel.BuildYear);
				cmd.Parameters.AddWithValue("@PropertyType", propertyModel.PropertyType);
				cmd.Parameters.AddWithValue("@TransactionType", propertyModel.TransactionType);
				cmd.Parameters.AddWithValue("@ParkingSpaces", propertyModel.ParkingSpaces ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@AdditionalFeatures", propertyModel.AdditionalFeatures ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@Status",propertyModel.Status);
				SqlParameter outputPropertyID = new SqlParameter
				{
					ParameterName = "@InsertedPropertyID",
					SqlDbType = SqlDbType.Int,
					Direction = ParameterDirection.Output
				};
				cmd.Parameters.Add(outputPropertyID);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();

				int insertedPropertyID = (int)outputPropertyID.Value;

				if (affectedRows > 0 && propertyModel.Images != null)
				{
					foreach (var image in propertyModel.Images)
					{
						_propertyImageRepository.Insert(insertedPropertyID, image.ImageURL);
					}
				}

				return affectedRows > 0;
			}
		}

		#endregion

		#region Update Property
		public bool Update(PropertyModel propertyModel)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Property_UpdateByPK", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@PropertyID", propertyModel.PropertyID);
				cmd.Parameters.AddWithValue("@UserID", propertyModel.UserID);
				cmd.Parameters.AddWithValue("@PropertyTitle", propertyModel.PropertyTitle);
				cmd.Parameters.AddWithValue("@PropertyDescription", propertyModel.PropertyDescription);
				cmd.Parameters.AddWithValue("@PropertyPrice", propertyModel.PropertyPrice);
				cmd.Parameters.AddWithValue("@PropertyAddress", propertyModel.PropertyAddress);
				cmd.Parameters.AddWithValue("@PropertySize", propertyModel.PropertySize);
				cmd.Parameters.AddWithValue("@BedroomCount", propertyModel.BedroomCount);
				cmd.Parameters.AddWithValue("@BathroomCount", propertyModel.BathroomCount);
				cmd.Parameters.AddWithValue("@BuildYear", propertyModel.BuildYear);
				cmd.Parameters.AddWithValue("@PropertyType", propertyModel.PropertyType);
				cmd.Parameters.AddWithValue("@TransactionType", propertyModel.TransactionType);
				cmd.Parameters.AddWithValue("@ParkingSpaces", propertyModel.ParkingSpaces ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@AdditionalFeatures", propertyModel.AdditionalFeatures ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@Status", propertyModel.Status);
				// Add AdditionalFeatures						  
				// Assuming ModifiedAt is handled by the database with a default value or trigger																	  
				// If not, uncomment the following line:			  
				// cmd.Parameters.AddWithValue("@ModifiedAt", propertyModel.ModifiedAt ?? (object)DBNull.Value);
				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				if (affectedRows > 0 && propertyModel.Images != null && propertyModel.Images.Count > 0)
				{
					//_propertyImageRepository.DeleteByPropertyID((int)propertyModel.PropertyID);
					foreach (var image in propertyModel.Images)
					{
						if(image.ImageID == 0)
						{
							_propertyImageRepository.Insert((int)propertyModel.PropertyID, image.ImageURL);
						}
					}
				}
				return affectedRows > 0;
			}
		}
		#endregion

		#region Delete Property
		public bool Delete(int propertyID)
		{
			_propertyImageRepository.DeleteByPropertyID(propertyID);
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Property_DeleteByPK", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@PropertyID", propertyID);
				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
	}
}
