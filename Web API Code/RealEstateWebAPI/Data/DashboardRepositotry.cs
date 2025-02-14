using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RealEstateWebAPI.Models;
using System.Data;

namespace RealEstateWebAPI.Data
{
	public class DashboardRepository
	{
		private readonly string _connectionString;
		public DashboardRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		#region Retrive All Related Dashboard
		public async Task<Dashboard> Index()
		{
			var dashboardData = new Dashboard
			{
				Counts = new List<RealEstateDashboardSummaryCountsModel>(),
				RecentProperties = new List<PropertyDashboardModel>(),
				RecentAgents = new List<AgentDashboardModel>()
			};

			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (var command = new SqlCommand("usp_GetRealEstateSummaryData", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (reader.HasRows)
						{
							while (await reader.ReadAsync())
							{
								dashboardData.Counts.Add(new RealEstateDashboardSummaryCountsModel
								{
									Metric = reader["Metric"].ToString(),
									Value = Convert.ToInt32(reader["Value"])
								});
							}

							// Fetch recent properties
							if (await reader.NextResultAsync())
							{
								while (await reader.ReadAsync())
								{
									dashboardData.RecentProperties.Add(new PropertyDashboardModel
									{
										PropertyID = Convert.ToInt32(reader["PropertyID"]),
										PropertyTitle = reader["PropertyTitle"].ToString(),
										TransactionType = reader["TransactionType"].ToString(),
										PropertyPrice = Convert.ToDecimal(reader["PropertyPrice"]),
										CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
									});
								}
							}

							// Fetch recent agents
							if (await reader.NextResultAsync())
							{
								while (await reader.ReadAsync())
								{
									dashboardData.RecentAgents.Add(new AgentDashboardModel
									{
										AgentID = Convert.ToInt32(reader["AgentID"]),
										AgentName = reader["AgentName"].ToString(),
										PropertiesListed = Convert.ToInt32(reader["PropertiesListed"]),
										Email = reader["Email"].ToString(),
										CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
									});
								}
							}
						}
					}
				}
			}

			return dashboardData;
		}
		#endregion

		#region Agent/Seller Dashboard
		public async Task<RealEstateSummarySeller> GetRealEstateSummaryForAgentAsync(int userId)
		{
			var dashboardData = new RealEstateSummarySeller
			{
				Counts = new List<CountMetricSeller>(),
				RecentProperties = new List<RecentPropertySeller>(),
				RecentAppointments = new List<RecentAppointmentSeller>()
			};

			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using (var command = new SqlCommand("usp_GetRealEstateSummaryDataForAgent", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@UserID", userId);

					using (var reader = await command.ExecuteReaderAsync())
					{
						// Fetch Counts
						while (await reader.ReadAsync())
						{
							dashboardData.Counts.Add(new CountMetricSeller
							{
								Metric = reader["Metric"].ToString(),
								Value = Convert.ToInt32(reader["Value"])
							});
						}

						// Fetch Recent Properties
						if (await reader.NextResultAsync())
						{
							while (await reader.ReadAsync())
							{
								dashboardData.RecentProperties.Add(new RecentPropertySeller
								{
									PropertyID = Convert.ToInt32(reader["PropertyID"]),
									PropertyTitle = reader["PropertyTitle"].ToString(),
									TransactionType = reader["TransactionType"].ToString(),
									PropertyPrice = Convert.ToDecimal(reader["PropertyPrice"]),
									CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
									AgentID = Convert.ToInt32(reader["AgentID"])
								});
							}
						}

						// Fetch Recent Appointments
						if (await reader.NextResultAsync())
						{
							while (await reader.ReadAsync())
							{
								dashboardData.RecentAppointments.Add(new RecentAppointmentSeller
								{
									AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
									BookerUserID = Convert.ToInt32(reader["BookerUserID"]),
									AppointmentUserID = Convert.ToInt32(reader["AppointmentUserID"]),
									PropertyID = Convert.ToInt32(reader["PropertyID"]),
									AppointmentStartDate = Convert.ToDateTime(reader["AppointmentStartDate"]),
									AppointmentEndDate = Convert.ToDateTime(reader["AppointmentEndDate"]),
									Status = reader["Status"].ToString(),
									Notes = reader["Notes"].ToString(),
									CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
								});
							}
						}
					}
				}
			}

			return dashboardData;
		}
		#endregion
		}
}
