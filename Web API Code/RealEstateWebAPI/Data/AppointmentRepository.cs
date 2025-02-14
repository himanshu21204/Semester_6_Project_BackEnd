using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class AppointmentRepository
	{
		private readonly string _connectionString;

		public AppointmentRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		#region Schedule Appointment
		public bool ScheduleAppointment(AppointmentModel appointment)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Appointment_Schedule", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@BookerUserID", appointment.BookerUserID);
				cmd.Parameters.AddWithValue("@AppointmentUserID", appointment.AppointmentUserID);
				cmd.Parameters.AddWithValue("@PropertyID", appointment.PropertyID);
				cmd.Parameters.AddWithValue("@AppointmentStartDate", appointment.AppointmentStartDate);
				cmd.Parameters.AddWithValue("@AppointmentEndDate", appointment.AppointmentEndDate);
				cmd.Parameters.AddWithValue("@Status", appointment.Status);
				cmd.Parameters.AddWithValue("@Notes", appointment.Notes ?? (object)DBNull.Value);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Update Appointment Status
		public bool UpdateAppointmentStatus(int appointmentId, AppointmentStatus status)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Appointment_UpdateStatus", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
				cmd.Parameters.AddWithValue("@Status", status.Status);

				conn.Open();
				int affectedRows = cmd.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion

		#region Get Appointments by User ID
		public IEnumerable<AppointmentModel> GetAppointmentsByUser(int bookerUserID)
		{
			List<AppointmentModel> appointments = new List<AppointmentModel>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Appointment_GetByUser", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@BookerUserID", bookerUserID);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					appointments.Add(MapToAppointmentModel(reader));
				}
			}

			return appointments;
		}
		#endregion

		#region Get Appointments by Property ID
		public IEnumerable<AppointmentModel> GetAppointmentsByProperty(int propertyId)
		{
			List<AppointmentModel> appointments = new List<AppointmentModel>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Appointment_GetByProperty", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@PropertyID", propertyId);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					appointments.Add(MapToAppointmentModel(reader));
				}
			}

			return appointments;
		}
		#endregion

		#region Get Appointments by Status
		public IEnumerable<AppointmentModel> GetAppointmentsByStatus(string status)
		{
			List<AppointmentModel> appointments = new List<AppointmentModel>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("PR_LOC_Appointment_GetByStatus", conn)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@Status", status);

				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					appointments.Add(MapToAppointmentModel(reader));
				}
			}

			return appointments;
		}
		#endregion

		private AppointmentModel MapToAppointmentModel(SqlDataReader reader)
		{
			return new AppointmentModel
			{
				AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
				BookerUserID = Convert.ToInt32(reader["BookerUserID"]),
				BookerName = reader["BookerName"].ToString(),
				AppointmentUserID = Convert.ToInt32(reader["AppointmentUserID"]),
				AppointmentUserName = reader["AppointmentUserName"].ToString(),
				PropertyID = Convert.ToInt32(reader["PropertyID"]),
				PropertyTitle = reader["PropertyTitle"].ToString(),
				AppointmentStartDate = Convert.ToDateTime(reader["AppointmentStartDate"]),
				AppointmentEndDate = Convert.ToDateTime(reader["AppointmentEndDate"]),
				Status = reader["Status"].ToString(),
				Notes = reader["Notes"]?.ToString(),
				CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
				ModifiedAt = reader["ModifiedAt"] as DateTime?
			};
		}

		#region User Drop Down
		public IEnumerable<UserDropDownModel> UserDropDown()
		{
			var users = new List<UserDropDownModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_AgentSeller_Dropdown", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					users.Add(new UserDropDownModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						FullName = reader["FullName"].ToString()
					});
				}
			}
			return users;
		}
		#endregion

		#region Property Drop Down
		public IEnumerable<PropertyDropDownModel> PropertyDropDown(int userId)
		{
			var properties = new List<PropertyDropDownModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_PropertyDropdown_ByUserID", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("@UserID", userId);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					properties.Add(new PropertyDropDownModel
					{
						PropertyID = Convert.ToInt32(reader["PropertyID"]),
						PropertyTitle = reader["PropertyTitle"].ToString()
					});
				}
			}
			return properties;
		}
		#endregion
	}
}
