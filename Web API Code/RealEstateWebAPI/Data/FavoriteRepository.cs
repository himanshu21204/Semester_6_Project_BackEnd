using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class FavoriteRepository
	{
		private readonly string _connectionString;

		public FavoriteRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		#region Add Favorite
		public bool AddFavorite(int userId, int propertyId)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Favorites_Add", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@UserID", userId);
				cmd.Parameters.AddWithValue("@PropertyID", propertyId);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Remove Favorite
		public bool RemoveFavorite(int favoriteId)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Favorites_Remove", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@FavoriteID", favoriteId);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}

		#endregion
		#region Get Favorites by UserID
		public IEnumerable<FavoriteModel> GetFavoritesByUser(int userId)
		{
			List<FavoriteModel> favorites = new List<FavoriteModel>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Favorites_GetByUser", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@UserID", userId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					favorites.Add(new FavoriteModel
					{
						FavoriteID = Convert.ToInt32(reader["FavoriteID"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						PropertyID = Convert.ToInt32(reader["PropertyID"]),
						CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
					});
				}
			}

			return favorites;
		}
		#endregion
	}
}
