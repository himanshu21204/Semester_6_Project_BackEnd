using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealEstateWebAPI.Models
{
	public class InstallmentModel
	{
		public int? InstallmentID { get; set; }

		public int TransactionID { get; set; }

		public decimal InstallmentAmount { get; set; }

		public DateTime InstallmentDate { get; set; }

		public decimal PaidAmount { get; set; } = 0;

		public string PaymentStatus { get; set; } = "Pending";

		public string? PaymentReferenceNumber { get; set; }

		public string PaymentType { get; set; }

		public DateTime? LastPaymentDate { get; set; }

		public decimal? CashPaymentAmount { get; set; }

		public string? CardNumber { get; set; }

		public string? CardHolderName { get; set; }

		public string? CardExpiryDate { get; set; }

		public string? UPIID { get; set; }
	}
}
