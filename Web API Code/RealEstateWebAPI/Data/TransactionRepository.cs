using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RealEstateWebAPI.Models;

public class TransactionRepository
{
	private readonly string _connectionString;

	public TransactionRepository(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString("DefaultConnection");
	}

	#region Get All Transactions
	public List<TransactionModel> GetAllTransactions()
	{
		List<TransactionModel> transactions = new List<TransactionModel>();

		using (var connection = new SqlConnection(_connectionString))
		{
			connection.Open();
			using (var command = new SqlCommand("PR_TRANS_Transaction_SelectAll", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						transactions.Add(new TransactionModel
						{
							TransactionID = reader["TransactionID"] != DBNull.Value ? Convert.ToInt32(reader["TransactionID"]) : 0,
							TotalTransactionAmount = reader["TotalTransactionAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalTransactionAmount"]) : 0m,
							PaidAmount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,
							RemainingAmount = reader["RemainingAmount"] != DBNull.Value ? Convert.ToDecimal(reader["RemainingAmount"]) : 0m,
							TransactionDate = reader["TransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["TransactionDate"]) : DateTime.MinValue,
							PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
							PaymentStatus = reader["PaymentStatus"] != DBNull.Value ? reader["PaymentStatus"].ToString() : string.Empty,
							PaymentReferenceNumber = reader["PaymentReferenceNumber"] != DBNull.Value ? reader["PaymentReferenceNumber"].ToString() : string.Empty,
							CashPaymentAmount = reader["CashPaymentAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CashPaymentAmount"]) : 0m,
							CardNumber = reader["CardNumber"] != DBNull.Value ? reader["CardNumber"].ToString() : string.Empty,
							CardHolderName = reader["CardHolderName"] != DBNull.Value ? reader["CardHolderName"].ToString() : string.Empty,
							CardExpiryDate = reader["CardExpiryDate"] != DBNull.Value ? reader["CardExpiryDate"].ToString() : string.Empty,
							UPIID = reader["UPIID"] != DBNull.Value ? reader["UPIID"].ToString() : string.Empty,
							SellerID = reader["SellerID"] != DBNull.Value ? Convert.ToInt32(reader["SellerID"]) : 0,
							SellerName = reader["SellerName"] != DBNull.Value ? reader["SellerName"].ToString() : string.Empty,
							BuyerID = reader["BuyerID"] != DBNull.Value ? Convert.ToInt32(reader["BuyerID"]) : 0,
							BuyerName = reader["BuyerName"] != DBNull.Value ? reader["BuyerName"].ToString() : string.Empty,
							Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
							TransactionType = reader["TransactionType"] != DBNull.Value ? reader["TransactionType"].ToString() : string.Empty,
							TransactionDetail = reader["TransactionDetail"] != DBNull.Value ? reader["TransactionDetail"].ToString() : string.Empty,
							LastTransactionDate = reader["LastTransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastTransactionDate"]) : DateTime.MinValue,
							PropertyID = reader["PropertyID"] != DBNull.Value ? Convert.ToInt32(reader["PropertyID"]) : 0,
							PropertyTitle = reader["PropertyTitle"] != DBNull.Value ? reader["PropertyTitle"].ToString() : string.Empty

						});
					}
				}
			}
		}
		return transactions;
	}
	#endregion

	#region Get Transactions by SellerID
	public List<TransactionModel> GetTransactionsBySellerID(int sellerID)
	{
		List<TransactionModel> transactions = new List<TransactionModel>();

		using (var connection = new SqlConnection(_connectionString))
		{
			connection.Open();
			using (var command = new SqlCommand("PR_TRANS_Transaction_SelectBySellerID", connection))
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@SellerID", sellerID);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						transactions.Add(new TransactionModel
						{
							TransactionID = reader["TransactionID"] != DBNull.Value ? Convert.ToInt32(reader["TransactionID"]) : 0,
							TotalTransactionAmount = reader["TotalTransactionAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalTransactionAmount"]) : 0m,
							PaidAmount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,
							RemainingAmount = reader["RemainingAmount"] != DBNull.Value ? Convert.ToDecimal(reader["RemainingAmount"]) : 0m,
							TransactionDate = reader["TransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["TransactionDate"]) : DateTime.MinValue,
							PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
							PaymentStatus = reader["PaymentStatus"] != DBNull.Value ? reader["PaymentStatus"].ToString() : string.Empty,
							PaymentReferenceNumber = reader["PaymentReferenceNumber"] != DBNull.Value ? reader["PaymentReferenceNumber"].ToString() : string.Empty,
							CashPaymentAmount = reader["CashPaymentAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CashPaymentAmount"]) : 0m,
							CardNumber = reader["CardNumber"] != DBNull.Value ? reader["CardNumber"].ToString() : string.Empty,
							CardHolderName = reader["CardHolderName"] != DBNull.Value ? reader["CardHolderName"].ToString() : string.Empty,
							CardExpiryDate = reader["CardExpiryDate"] != DBNull.Value ? reader["CardExpiryDate"].ToString() : string.Empty,
							UPIID = reader["UPIID"] != DBNull.Value ? reader["UPIID"].ToString() : string.Empty,
							SellerID = reader["SellerID"] != DBNull.Value ? Convert.ToInt32(reader["SellerID"]) : 0,
							SellerName = reader["SellerName"] != DBNull.Value ? reader["SellerName"].ToString() : string.Empty,
							BuyerID = reader["BuyerID"] != DBNull.Value ? Convert.ToInt32(reader["BuyerID"]) : 0,
							BuyerName = reader["BuyerName"] != DBNull.Value ? reader["BuyerName"].ToString() : string.Empty,
							Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
							TransactionType = reader["TransactionType"] != DBNull.Value ? reader["TransactionType"].ToString() : string.Empty,
							TransactionDetail = reader["TransactionDetail"] != DBNull.Value ? reader["TransactionDetail"].ToString() : string.Empty,
							LastTransactionDate = reader["LastTransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastTransactionDate"]) : DateTime.MinValue,
							PropertyID = reader["PropertyID"] != DBNull.Value ? Convert.ToInt32(reader["PropertyID"]) : 0,
							PropertyTitle = reader["PropertyTitle"] != DBNull.Value ? reader["PropertyTitle"].ToString() : string.Empty

						});
					}
				}
			}
		}
		return transactions;
	}
	#endregion

	#region Get Transactions by BuyerID
	public List<TransactionModel> GetTransactionsByBuyerID(int buyerID)
	{
		List<TransactionModel> transactions = new List<TransactionModel>();

		using (var connection = new SqlConnection(_connectionString))
		{
			connection.Open();
			using (var command = new SqlCommand("PR_TRANS_Transaction_SelectByBuyerID", connection))
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@BuyerID", buyerID);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						transactions.Add(new TransactionModel
						{
							TransactionID = reader["TransactionID"] != DBNull.Value ? Convert.ToInt32(reader["TransactionID"]) : 0,
							TotalTransactionAmount = reader["TotalTransactionAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalTransactionAmount"]) : 0m,
							PaidAmount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,
							RemainingAmount = reader["RemainingAmount"] != DBNull.Value ? Convert.ToDecimal(reader["RemainingAmount"]) : 0m,
							TransactionDate = reader["TransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["TransactionDate"]) : DateTime.MinValue,
							PaymentType = reader["PaymentType"] != DBNull.Value ? reader["PaymentType"].ToString() : string.Empty,
							PaymentStatus = reader["PaymentStatus"] != DBNull.Value ? reader["PaymentStatus"].ToString() : string.Empty,
							PaymentReferenceNumber = reader["PaymentReferenceNumber"] != DBNull.Value ? reader["PaymentReferenceNumber"].ToString() : string.Empty,
							CashPaymentAmount = reader["CashPaymentAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CashPaymentAmount"]) : 0m,
							CardNumber = reader["CardNumber"] != DBNull.Value ? reader["CardNumber"].ToString() : string.Empty,
							CardHolderName = reader["CardHolderName"] != DBNull.Value ? reader["CardHolderName"].ToString() : string.Empty,
							CardExpiryDate = reader["CardExpiryDate"] != DBNull.Value ? reader["CardExpiryDate"].ToString() : string.Empty,
							UPIID = reader["UPIID"] != DBNull.Value ? reader["UPIID"].ToString() : string.Empty,
							SellerID = reader["SellerID"] != DBNull.Value ? Convert.ToInt32(reader["SellerID"]) : 0,
							SellerName = reader["SellerName"] != DBNull.Value ? reader["SellerName"].ToString() : string.Empty,
							BuyerID = reader["BuyerID"] != DBNull.Value ? Convert.ToInt32(reader["BuyerID"]) : 0,
							BuyerName = reader["BuyerName"] != DBNull.Value ? reader["BuyerName"].ToString() : string.Empty,
							Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
							TransactionType = reader["TransactionType"] != DBNull.Value ? reader["TransactionType"].ToString() : string.Empty,
							TransactionDetail = reader["TransactionDetail"] != DBNull.Value ? reader["TransactionDetail"].ToString() : string.Empty,
							LastTransactionDate = reader["LastTransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastTransactionDate"]) : DateTime.MinValue,
							PropertyID = reader["PropertyID"] != DBNull.Value ? Convert.ToInt32(reader["PropertyID"]) : 0,
							PropertyTitle = reader["PropertyTitle"] != DBNull.Value ? reader["PropertyTitle"].ToString() : string.Empty

						});
					}
				}
			}
		}
		return transactions;
	}
	#endregion

	#region Insert Transaction
	public int InsertTransaction(TransactionModel transaction)
	{
		using (var connection = new SqlConnection(_connectionString))
		{
			using (var command = new SqlCommand("PR_TRANS_Transaction_Insert", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@TotalTransactionAmount", transaction.TotalTransactionAmount);
				command.Parameters.AddWithValue("@PaidAmount", transaction.PaidAmount);
				command.Parameters.AddWithValue("@RemainingAmount", transaction.RemainingAmount);
				command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
				command.Parameters.AddWithValue("@PaymentType", transaction.PaymentType);
				command.Parameters.AddWithValue("@PaymentStatus", transaction.PaymentStatus ?? "Pending");
				command.Parameters.AddWithValue("@PaymentReferenceNumber", transaction.PaymentReferenceNumber ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CashPaymentAmount", transaction.CashPaymentAmount ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CardNumber", transaction.CardNumber ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CardHolderName", transaction.CardHolderName ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CardExpiryDate", transaction.CardExpiryDate ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@UPIID", transaction.UPIID ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@SellerID", transaction.SellerID);
				command.Parameters.AddWithValue("@BuyerID", transaction.BuyerID);
				command.Parameters.AddWithValue("@Status", transaction.Status ?? "Pending");
				command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@TransactionDetail", transaction.TransactionDetail ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@PropertyID", transaction.PropertyID);

				var transactionIdParameter = new SqlParameter("@TransactionID", SqlDbType.Int)
				{
					Direction = ParameterDirection.Output
				};
				command.Parameters.Add(transactionIdParameter);

				connection.Open();
				command.ExecuteNonQuery();

				int transactionId = (int)transactionIdParameter.Value;
				Console.WriteLine($"Transaction ID: {transactionId}");
				return transactionId;
			}
		}
	}
	#endregion

	#region Update Transaction
	public bool UpdateTransaction(TransactionModel transaction)
	{
		using (var connection = new SqlConnection(_connectionString))
		{
			connection.Open();
			using (var command = new SqlCommand("PR_TRANS_Transaction_Update", connection))
			{
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
				command.Parameters.AddWithValue("@TotalTransactionAmount", transaction.TotalTransactionAmount);
				command.Parameters.AddWithValue("@PaidAmount", transaction.PaidAmount);
				command.Parameters.AddWithValue("@RemainingAmount", transaction.RemainingAmount);
				command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
				command.Parameters.AddWithValue("@PaymentType", transaction.PaymentType);
				command.Parameters.AddWithValue("@PaymentStatus", transaction.PaymentStatus);
				command.Parameters.AddWithValue("@PaymentReferenceNumber", transaction.PaymentReferenceNumber ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CashPaymentAmount", transaction.CashPaymentAmount ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CardNumber", transaction.CardNumber ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CardHolderName", transaction.CardHolderName ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@CardExpiryDate", transaction.CardExpiryDate ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@UPIID", transaction.UPIID ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@SellerID", transaction.SellerID);
				command.Parameters.AddWithValue("@BuyerID", transaction.BuyerID);
				command.Parameters.AddWithValue("@Status", transaction.Status);
				command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@TransactionDetail", transaction.TransactionDetail ?? (object)DBNull.Value);
				command.Parameters.AddWithValue("@PropertyID", transaction.PropertyID);

				int rowsAffected = command.ExecuteNonQuery();
				return rowsAffected > 0;
			}
		}
	}
	#endregion

	#region Transaction Property Drop Down
	public IEnumerable<TransactionPropertyDropDownModel> TransactionPropertyDropDown(int userId)
	{
		var properties = new List<TransactionPropertyDropDownModel>();
		using (SqlConnection connection = new SqlConnection(_connectionString))
		{
			SqlCommand command = new SqlCommand("PR_LOC_Transaction_PropertyDropdown_ByUserID", connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			command.Parameters.AddWithValue("@UserID", userId);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				properties.Add(new TransactionPropertyDropDownModel
				{
					PropertyID = Convert.ToInt32(reader["PropertyID"]),
					PropertyTitle = reader["PropertyTitle"].ToString(),
					PropertyPrice = Convert.ToDecimal(reader["PropertyPrice"])
				});
			}
		}
		return properties;
	}
	#endregion
}
