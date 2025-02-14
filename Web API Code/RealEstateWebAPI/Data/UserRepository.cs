using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class UserRepository
	{
		#region configuration Connection string
		private readonly string _connectionString;
		public UserRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#endregion

		#region Select All User
		public IEnumerable<UserModel> SelectAll()
		{
			List<UserModel> users = new List<UserModel>();
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_User_SelectAll", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					users.Add(new UserModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						PhoneNumber = reader["PhoneNumber"].ToString(),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						Email = reader["Email"].ToString(),
						Password = reader["Password"].ToString(),
						Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
						UserRole = reader["UserRole"].ToString(),
						ProfilePhoto = reader["ProfilePhoto"] != DBNull.Value ? reader["ProfilePhoto"].ToString() : null,
						Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
						CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
						ModifiedAt = reader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedAt"]) : null,
						IsActive = Convert.ToBoolean(reader["IsActive"])
					});
				}
			}
			return users;
		}
		#endregion
		#region Select User By ID
		public UserModel SelectByID(int UserID)
		{
			UserModel user = new UserModel();
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_User_SelectByPK", conn)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@UserID", UserID);
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					user = new UserModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						PhoneNumber = reader["PhoneNumber"].ToString(),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						Email = reader["Email"].ToString(),
						Password = reader["Password"].ToString(),
						Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
						UserRole = reader["UserRole"].ToString(),
						ProfilePhoto = reader["ProfilePhoto"] != DBNull.Value ? reader["ProfilePhoto"].ToString() : null,
						Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
						CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
						ModifiedAt = reader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedAt"]) : null,
						IsActive = Convert.ToBoolean(reader["IsActive"])
					};
				}
			}
			return user;
		}
		#endregion
		#region Inser User
		public bool Insert(UserModel userModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@UserName", userModel.UserName);
				command.Parameters.AddWithValue("@Email", userModel.Email);
				command.Parameters.AddWithValue("@Password", userModel.Password);
				command.Parameters.AddWithValue("@PhoneNumber", userModel.PhoneNumber);
				command.Parameters.AddWithValue("@Address", userModel.Address ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@FirstName", userModel.FirstName);
				command.Parameters.AddWithValue("@LastName", userModel.LastName);
				command.Parameters.AddWithValue("@Description", userModel.Description ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@UserRole", userModel.UserRole);
				command.Parameters.AddWithValue("@ProfilePhoto", userModel.ProfilePhoto ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@IsActive", userModel.IsActive);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Update User
		public bool Update(UserModel userModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_UpdateByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@UserID", userModel.UserID);
				command.Parameters.AddWithValue("@UserName", userModel.UserName);
				command.Parameters.AddWithValue("@Email", userModel.Email);
				command.Parameters.AddWithValue("@Password", userModel.Password);
				command.Parameters.AddWithValue("@PhoneNumber", userModel.PhoneNumber);
				command.Parameters.AddWithValue("@Address", userModel.Address ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@FirstName", userModel.FirstName);
				command.Parameters.AddWithValue("@LastName", userModel.LastName);
				command.Parameters.AddWithValue("@Description", userModel.Description ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@UserRole", userModel.UserRole);
				command.Parameters.AddWithValue("@ProfilePhoto", userModel.ProfilePhoto ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@IsActive", userModel.IsActive);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Delete User
		public bool Delete(int userID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_DeleteByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@UserID", userID);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Deactivate User
		public bool DeactivateUser(int userID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_DeactivateByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@UserID", userID);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Update Profile Photo
		public bool UpdateProfilePhoto(int userID, string profilePhoto)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				using (SqlCommand command = new SqlCommand("PR_LOC_Property_UpdateUserProfilePhoto", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					// Add parameters
					command.Parameters.AddWithValue("@UserID", userID);
					command.Parameters.AddWithValue("@ProfilePhoto", profilePhoto);

					connection.Open();
					int rowsAffected = command.ExecuteNonQuery();
					return rowsAffected > 0;
				}
			}
		}
		#endregion
		#region Get Profile Photo
		public string GetUserProfilePhoto(int userID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_UserProfilePhotoByID", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@UserID", userID);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					return reader["ProfilePhoto"].ToString();
				}
				return null;
			}
		}
		#endregion
		#region Update/Change Password
		public bool ChangePassword(int userId, string oldPassword, string newPassword)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand("ChangeUserPassword", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@UserId", userId);
					cmd.Parameters.AddWithValue("@OldPassword", oldPassword);
					cmd.Parameters.AddWithValue("@NewPassword", newPassword);

					try
					{
						int affectedRow = cmd.ExecuteNonQuery();
						return affectedRow > 0;
					}
					catch (SqlException ex)
					{
						return false;
					}
				}
			}
		}
		#endregion
		#region Save OTP In OTP Table
		public bool SaveOTP(string email, string otp, DateTime expiry)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = new SqlCommand("PR_LOC_OTP_SaveUserOTP", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@Email", email);
					command.Parameters.AddWithValue("@OTP", otp);
					command.Parameters.AddWithValue("@OTPExpiry", expiry);

					return command.ExecuteNonQuery() > 0;
				}
			}
		}
		#endregion
		#region Verify Recieved OTP From User
		public string VerifyOTPAndResetPassword(string email, string otp, string newPasswordHash)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = new SqlCommand("PR_LOC_OTP_VerifyOTPAndResetPassword", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@Email", email);
					command.Parameters.AddWithValue("@OTP", otp);
					command.Parameters.AddWithValue("@NewPassword", newPasswordHash);

					return (string)command.ExecuteScalar();
				}
			}
			#endregion
		}
	}
}
