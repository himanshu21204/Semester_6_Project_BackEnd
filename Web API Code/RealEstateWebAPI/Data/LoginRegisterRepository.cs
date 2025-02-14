using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;
using Google.Apis.Auth;

namespace RealEstateWebAPI.Data
{
	public class LoginRegisterRepository
	{
		#region Configuration Connection String
		private readonly string _connectionString;

		public LoginRegisterRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#endregion

		#region Register User
		public bool RegisterUser(UserModel user)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var cmd = new SqlCommand("PR_LOC_User_Register", connection);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@UserName", user.UserName);
				cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
				cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
				cmd.Parameters.AddWithValue("@LastName", user.LastName);
				cmd.Parameters.AddWithValue("@Email", user.Email);
				cmd.Parameters.AddWithValue("@Password", user.Password);
				cmd.Parameters.AddWithValue("@UserRole", user.UserRole);

				connection.Open();
				var result = cmd.ExecuteNonQuery();
				return result > 0;
			}
		}
		#endregion

		#region Register User Using Goole
		public bool RegisterUserUsingGoogle(UserModel user)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var cmd = new SqlCommand("PR_LOC_User_RegisterUsingGoogle", connection);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@UserName", user.UserName);
				cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
				cmd.Parameters.AddWithValue("@LastName", user.LastName);
				cmd.Parameters.AddWithValue("@Email", user.Email);
				cmd.Parameters.AddWithValue("@ProfilePhoto", user.ProfilePhoto);
				cmd.Parameters.AddWithValue("@UserRole", user.UserRole);

				connection.Open();
				var result = cmd.ExecuteNonQuery();
				return result > 0;
			}
		}
		#endregion

		#region Login User
		public UserModel LoginUser(string userName, string password)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var cmd = new SqlCommand("PR_LOC_User_Login", connection);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@UserName", userName);
				cmd.Parameters.AddWithValue("@Password", password);

				connection.Open();
				var reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					return new UserModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						UserRole = reader["UserRole"].ToString(),
						Password = reader["Password"].ToString(),
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						Email = reader["Email"].ToString()
					};
				}
			}
			return null; // Return null if no matching user or invalid password
		}
		#endregion

		#region Google Login
		public async Task<UserModel> GoogleLogin(string googleToken, string userRole)
		{
			try
			{
				var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken);

				using (var connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					using (var cmd = new SqlCommand("PR_LOC_GetUserByEmail", connection))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Email", payload.Email);

						var reader = await cmd.ExecuteReaderAsync();
						if (reader.Read())
						{
							return new UserModel
							{
								UserID = Convert.ToInt32(reader["UserID"]),
								Email = reader["Email"].ToString(),
								FirstName = reader["FirstName"].ToString(),
								LastName = reader["LastName"].ToString(),
								UserRole = reader["UserRole"].ToString()
							};
						}
						reader.Close();

						using (var insertCmd = new SqlCommand("PR_LOC_User_Register", connection))
						{
							insertCmd.CommandType = CommandType.StoredProcedure;
							insertCmd.Parameters.AddWithValue("@UserName", payload.Email);
							insertCmd.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);
							insertCmd.Parameters.AddWithValue("@FirstName", payload.GivenName);
							insertCmd.Parameters.AddWithValue("@LastName", payload.FamilyName);
							insertCmd.Parameters.AddWithValue("@Email", payload.Email);
							insertCmd.Parameters.AddWithValue("@Password", DBNull.Value);
							insertCmd.Parameters.AddWithValue("@UserRole", userRole);

							await insertCmd.ExecuteNonQueryAsync();
						}

						return new UserModel
						{
							Email = payload.Email,
							FirstName = payload.GivenName,
							LastName = payload.FamilyName,
							UserRole = userRole
						};
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Google authentication failed: " + ex.Message);
			}
		}
		#endregion
		#region Get User By Email (For Google Login)
		public UserModel GetUserByEmail(string email)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				var cmd = new SqlCommand("PR_LOC_User_GetByEmail", connection);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@Email", email);

				connection.Open();
				var reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					return new UserModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						UserRole = reader["UserRole"].ToString(),
						Password = null, // Never return password for security reasons
						FirstName = reader["FirstName"].ToString(),
						LastName = reader["LastName"].ToString(),
						Email = reader["Email"].ToString(),
						ProfilePhoto = reader["ProfilePhoto"]?.ToString()
					};
				}
			}
			return null;  // Return null if no user found
		}
		#endregion
	}
}
