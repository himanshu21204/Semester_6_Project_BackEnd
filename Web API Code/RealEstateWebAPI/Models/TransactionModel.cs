namespace RealEstateWebAPI.Models
{
	public class TransactionModel
	{
		public int? TransactionID { get; set; }
		public decimal TotalTransactionAmount { get; set; }
		public decimal PaidAmount { get; set; }
		public decimal RemainingAmount { get; set; }
		public DateTime TransactionDate { get; set; }
		public string PaymentType { get; set; } // Cash,CreditCard,DebitCard,UPI
		public string PaymentStatus { get; set; } = "Complete"; // Pending,Completed,Failed,Cancelled
		public string? PaymentReferenceNumber { get; set; }
		public decimal? CashPaymentAmount { get; set; }
		public string? CardNumber { get; set; }
		public string? CardHolderName { get; set; }
		public string? CardExpiryDate { get; set; }
		public string? UPIID { get; set; }
		public int SellerID { get; set; }
		public string? SellerName { get; set; }
		public int BuyerID { get; set; }
		public string? BuyerName { get; set; }
		public string Status { get; set; } = "Complete"; // Pending,Completed,Cancelled
		public string? TransactionType { get; set; } // Sale,Installment
		public string? TransactionDetail { get; set; }
		public DateTime? LastTransactionDate { get; set; }
		public int PropertyID { get; set; }
		public string? PropertyTitle { get; set; }
	}
	public class TransactionPropertyDropDownModel
	{
		public int PropertyID { get; set; }
		public string PropertyTitle { get; set; } = string.Empty;
		public decimal PropertyPrice { get; set; }
	}
}
