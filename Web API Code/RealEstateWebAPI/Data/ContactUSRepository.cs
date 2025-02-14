using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class ContactUSRepository
	{
		private readonly string _connectionString;

		public ContactUSRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		#region Insert ContactUs
		public bool InsertContactUs(ContactUsModel contactUs)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_ContactUS_Insert", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@Name", contactUs.Name);
				cmd.Parameters.AddWithValue("@Email", contactUs.Email);
				cmd.Parameters.AddWithValue("@PhoneNumber", contactUs.PhoneNumber);
				cmd.Parameters.AddWithValue("@Subject", contactUs.Subject ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@Message", contactUs.Message);
				cmd.Parameters.AddWithValue("@SubmittedAt", contactUs.SubmittedAt);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Get All ContactUs Entries
		public IEnumerable<ContactUsModel> GetAllContactUs()
		{
			List<ContactUsModel> contactUsList = new List<ContactUsModel>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_ContactUS_SelectAll", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					contactUsList.Add(new ContactUsModel
					{
						ContactID = Convert.ToInt32(reader["ContactID"]),
						Name = reader["Name"].ToString(),
						Email = reader["Email"].ToString(),
						PhoneNumber = reader["PhoneNumber"].ToString(),
						Subject = reader["Subject"]?.ToString(),
						Message = reader["Message"].ToString(),
						Status = reader["Status"].ToString(),
						SubmittedAt = Convert.ToDateTime(reader["SubmittedAt"])
					});
				}
			}

			return contactUsList;
		}
		#endregion

		#region Get ContactUs Entry By ID
		public ContactUsModel GetContactUsById(int contactId)
		{
			ContactUsModel contactUs = null;

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_ContactUS_SelectByPK", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@ContactID", contactId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					contactUs = new ContactUsModel
					{
						ContactID = Convert.ToInt32(reader["ContactID"]),
						Name = reader["Name"].ToString(),
						Email = reader["Email"].ToString(),
						PhoneNumber = reader["PhoneNumber"].ToString(),
						Subject = reader["Subject"]?.ToString(),
						Message = reader["Message"].ToString(),
						Status = reader["Status"].ToString(),
						SubmittedAt = Convert.ToDateTime(reader["SubmittedAt"])
					};
				}
			}

			return contactUs;
		}
		#endregion
		#region Update Contact US Status
		public bool UpdateContactUsStatus(int ContactID,StatusModel status)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_ContactUS_UpdateStatus", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@ContactID", ContactID);
				command.Parameters.AddWithValue("@Status", status.Status);
				connection.Open();
				int rowAffected = command.ExecuteNonQuery();
				return rowAffected > 0;
			}
		}
		#endregion
	}
}
