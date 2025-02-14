using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RealEstateWebAPI.Models;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Repositories
{
	public class InstallmentRepository
	{
		private readonly string _connectionString;

		public InstallmentRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		#region Get All Installments
		public async Task<IEnumerable<InstallmentModel>> GetAllInstallmentsAsync()
		{
			var installments = new List<InstallmentModel>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("PR_INST_Installment_SelectAll", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					var reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						installments.Add(new InstallmentModel
						{
							InstallmentID = Convert.ToInt32(reader["InstallmentID"]),
							TransactionID = Convert.ToInt32(reader["TransactionID"]),
							InstallmentAmount = Convert.ToDecimal(reader["InstallmentAmount"]),
							InstallmentDate = Convert.ToDateTime(reader["InstallmentDate"]),
							PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
							PaymentStatus = reader["PaymentStatus"].ToString(),
							PaymentReferenceNumber = reader["PaymentReferenceNumber"]?.ToString(),
							LastPaymentDate = reader["LastPaymentDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["LastPaymentDate"]),
							CashPaymentAmount = reader["CashPaymentAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CashPaymentAmount"]),
							CardNumber = reader["CardNumber"]?.ToString(),
							CardHolderName = reader["CardHolderName"]?.ToString(),
							CardExpiryDate = reader["CardExpiryDate"]?.ToString(),
							UPIID = reader["UPIID"]?.ToString(),
							PaymentType = reader["PaymentType"]?.ToString() // Added PaymentType
						});
					}
				}
			}
			return installments;
		}
		#endregion

		#region Get Installment by ID
		public async Task<InstallmentModel> GetInstallmentByIdAsync(int installmentId)
		{
			InstallmentModel installment = null;
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("PR_INST_Installment_SelectByID", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@InstallmentID", installmentId);
					var reader = await command.ExecuteReaderAsync();
					if (await reader.ReadAsync())
					{
						installment = new InstallmentModel
						{
							InstallmentID = Convert.ToInt32(reader["InstallmentID"]),
							TransactionID = Convert.ToInt32(reader["TransactionID"]),
							InstallmentAmount = Convert.ToDecimal(reader["InstallmentAmount"]),
							InstallmentDate = Convert.ToDateTime(reader["InstallmentDate"]),
							PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
							PaymentStatus = reader["PaymentStatus"].ToString(),
							PaymentReferenceNumber = reader["PaymentReferenceNumber"]?.ToString(),
							LastPaymentDate = reader["LastPaymentDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["LastPaymentDate"]),
							CashPaymentAmount = reader["CashPaymentAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CashPaymentAmount"]),
							CardNumber = reader["CardNumber"]?.ToString(),
							CardHolderName = reader["CardHolderName"]?.ToString(),
							CardExpiryDate = reader["CardExpiryDate"]?.ToString(),
							UPIID = reader["UPIID"]?.ToString(),
							PaymentType = reader["PaymentType"]?.ToString() // Added PaymentType
						};
					}
				}
			}
			return installment;
		}
		#endregion

		#region Select By Transaction ID
		public async Task<IEnumerable<InstallmentModel>> GetInstallmentsByTransactionIdAsync(int transactionId)
		{
			var installments = new List<InstallmentModel>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("PR_INST_Installment_SelectByTransactionID", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@TransactionID", transactionId);
					var reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						installments.Add(new InstallmentModel
						{
							InstallmentID = Convert.ToInt32(reader["InstallmentID"]),
							TransactionID = Convert.ToInt32(reader["TransactionID"]),
							InstallmentAmount = Convert.ToDecimal(reader["InstallmentAmount"]),
							InstallmentDate = Convert.ToDateTime(reader["InstallmentDate"]),
							PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
							PaymentStatus = reader["PaymentStatus"].ToString(),
							PaymentReferenceNumber = reader["PaymentReferenceNumber"]?.ToString(),
							LastPaymentDate = reader["LastPaymentDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["LastPaymentDate"]),
							CashPaymentAmount = reader["CashPaymentAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CashPaymentAmount"]),
							CardNumber = reader["CardNumber"]?.ToString(),
							CardHolderName = reader["CardHolderName"]?.ToString(),
							CardExpiryDate = reader["CardExpiryDate"]?.ToString(),
							UPIID = reader["UPIID"]?.ToString(),
							PaymentType = reader["PaymentType"]?.ToString() // Added PaymentType
						});
					}
				}
			}
			return installments;
		}
		#endregion

		#region Insert Installment
		public async Task<int> InsertInstallmentAsync(InstallmentModel installment)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("PR_INST_Installment_Insert", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@TransactionID", installment.TransactionID);
					command.Parameters.AddWithValue("@InstallmentAmount", installment.InstallmentAmount);
					command.Parameters.AddWithValue("@InstallmentDate", installment.InstallmentDate);
					command.Parameters.AddWithValue("@PaidAmount", installment.PaidAmount);
					command.Parameters.AddWithValue("@PaymentStatus", installment.PaymentStatus);
					command.Parameters.AddWithValue("@PaymentReferenceNumber", installment.PaymentReferenceNumber ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@LastPaymentDate", installment.LastPaymentDate ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CashPaymentAmount", installment.CashPaymentAmount ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CardNumber", installment.CardNumber ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CardHolderName", installment.CardHolderName ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CardExpiryDate", installment.CardExpiryDate ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@UPIID", installment.UPIID ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@PaymentType", installment.PaymentType ?? (object)DBNull.Value); // Added PaymentType
					return await command.ExecuteNonQueryAsync();
				}
			}
		}
		#endregion

		#region Update Installment
		public async Task<int> UpdateInstallmentAsync(InstallmentModel installment)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("PR_INST_Installment_Update", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@InstallmentID", installment.InstallmentID);
					command.Parameters.AddWithValue("@TransactionID", installment.TransactionID);
					command.Parameters.AddWithValue("@InstallmentAmount", installment.InstallmentAmount);
					command.Parameters.AddWithValue("@InstallmentDate", installment.InstallmentDate);
					command.Parameters.AddWithValue("@PaidAmount", installment.PaidAmount);
					command.Parameters.AddWithValue("@PaymentStatus", installment.PaymentStatus);
					command.Parameters.AddWithValue("@PaymentReferenceNumber", installment.PaymentReferenceNumber ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@LastPaymentDate", installment.LastPaymentDate ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CashPaymentAmount", installment.CashPaymentAmount ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CardNumber", installment.CardNumber ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CardHolderName", installment.CardHolderName ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@CardExpiryDate", installment.CardExpiryDate ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@UPIID", installment.UPIID ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@PaymentType", installment.PaymentType ?? (object)DBNull.Value); // Added PaymentType
					return await command.ExecuteNonQueryAsync();
				}
			}
		}
		#endregion
	}
}
