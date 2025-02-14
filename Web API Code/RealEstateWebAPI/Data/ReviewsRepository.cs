using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class ReviewsRepository
	{
		private readonly string _connectionString;

		public ReviewsRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		#region Insert Agent Review
		public bool InsertAgentReview(AgentReview agentReview)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Insert_AgentReview", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@AgentID", agentReview.AgentID);
				cmd.Parameters.AddWithValue("@UserID", agentReview.UserID);
				cmd.Parameters.AddWithValue("@Rating", agentReview.Rating);
				cmd.Parameters.AddWithValue("@ReviewText", agentReview.ReviewText);
				cmd.Parameters.AddWithValue("@Keywords", agentReview.Keywords);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Insert Property Review
		public bool InsertPropertyReview(PropertyReview propertyReview)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Insert_PropertyReview", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@PropertyID", propertyReview.PropertyID);
				cmd.Parameters.AddWithValue("@UserID", propertyReview.UserID);
				cmd.Parameters.AddWithValue("@Rating", propertyReview.Rating);
				cmd.Parameters.AddWithValue("@ReviewText", propertyReview.ReviewText);
				cmd.Parameters.AddWithValue("@Keywords", propertyReview.Keywords);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Get Agent Review by Agent ID
		public IEnumerable<AgentReview> GetAgentReviewByAgentID(int agentId)
		{
			List<AgentReview> agentReviews = new List<AgentReview>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Get_AgentReviews", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@AgentID", agentId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					agentReviews.Add(new AgentReview
					{
						ReviewID = Convert.ToInt32(reader["ReviewID"]),
						AgentID = Convert.ToInt32(reader["AgentID"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["FullName"].ToString(),
						Rating = Convert.ToInt32(reader["Rating"]),
						ReviewText = reader["ReviewText"].ToString(),
						Keywords = reader["Keywords"] != DBNull.Value ? reader["Keywords"].ToString() : null,
						SubmittedAt = Convert.ToDateTime(reader["SubmittedAt"])
					});
				}
				conn.Close();
			}

			return agentReviews;
		}
		#endregion

		#region Get Property Review by Property ID
		public IEnumerable<PropertyReview> GetPropertyReviewByPropertyID(int propertyId)
		{
			List<PropertyReview> propertyReviews = new List<PropertyReview>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Get_PropertyReviews", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@PropertyID", propertyId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					propertyReviews.Add(new PropertyReview
					{
						ReviewID = Convert.ToInt32(reader["ReviewID"]),
						PropertyID = Convert.ToInt32(reader["PropertyID"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["FullName"].ToString(),
						Rating = Convert.ToInt32(reader["Rating"]),
						ReviewText = reader["ReviewText"].ToString(),
						Keywords = reader["Keywords"] != DBNull.Value ? reader["Keywords"].ToString() : null,
						SubmittedAt = Convert.ToDateTime(reader["SubmittedAt"])
					});
				}
				conn.Close();
			}

			return propertyReviews;
		}
		#endregion

		#region Update Property Review
		public bool UpdatePropertyReview(PropertyReview propertyReview)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Edit_PropertyReview", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@ReviewID", propertyReview.ReviewID);
				cmd.Parameters.AddWithValue("@Rating", propertyReview.Rating);
				cmd.Parameters.AddWithValue("@ReviewText", propertyReview.ReviewText);
				cmd.Parameters.AddWithValue("@Keywords", propertyReview.Keywords);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Delete Property Review
		public bool DeletePropertyReview(int reviewId)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Delete_PropertyReview", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@ReviewID", reviewId);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Update Agent Review
		public bool UpdateAgentReview(AgentReview agentReview)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Edit_AgentReview", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@ReviewID", agentReview.ReviewID);
				cmd.Parameters.AddWithValue("@Rating", agentReview.Rating);
				cmd.Parameters.AddWithValue("@ReviewText", agentReview.ReviewText);
				cmd.Parameters.AddWithValue("@Keywords", agentReview.Keywords);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Delete Agent Review
		public bool DeleteAgentReview(int reviewId)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("Delete_AgentReview", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@ReviewID", reviewId);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

	}
}
