using FluentValidation;
using RealEstateWebAPI.Models;
using System;

public class TransactionValidation : AbstractValidator<TransactionModel>
{
	public TransactionValidation()
	{
		// Validate SenderID
		RuleFor(transaction => transaction.SellerID)
			.NotEmpty().WithMessage("Seller ID is required.")
			.GreaterThanOrEqualTo(1).WithMessage("Seller ID must be greater than or equal to 1.")
			.WithName("Seller ID");

		// Validate ReceiverID (BuyerID)
		RuleFor(transaction => transaction.BuyerID)
			.NotEmpty().WithMessage("Buyer ID is required.")
			.GreaterThanOrEqualTo(1).WithMessage("Buyer ID must be greater than or equal to 1.")
			.WithName("Buyer ID");

		// Validate PropertyID
		RuleFor(transaction => transaction.PropertyID)
			.NotEmpty().WithMessage("Property ID is required.")
			.GreaterThanOrEqualTo(1).WithMessage("Property ID must be greater than or equal to 1.")
			.WithName("Property ID");

		// Validate TotalTransactionAmount
		RuleFor(transaction => transaction.TotalTransactionAmount)
			.NotEmpty().WithMessage("Total transaction amount is required.")
			.GreaterThan(0).WithMessage("Total transaction amount must be greater than 0.")
			.WithName("Total Transaction Amount");

		// Validate PaidAmount
		RuleFor(transaction => transaction.PaidAmount)
			.NotEmpty().WithMessage("Paid amount is required.")
			.GreaterThanOrEqualTo(0).WithMessage("Paid amount must be greater than or equal to 0.")
			.WithName("Paid Amount");

		// Validate RemainingAmount
		RuleFor(transaction => transaction.RemainingAmount)
			.NotEmpty().WithMessage("Remaining amount is required.")
			.GreaterThanOrEqualTo(0).WithMessage("Remaining amount must be greater than or equal to 0.")
			.WithName("Remaining Amount");

		// Validate TransactionDate
		RuleFor(transaction => transaction.TransactionDate)
			.NotEmpty().WithMessage("Transaction date is required.")
			.GreaterThanOrEqualTo(DateTime.Now).WithMessage("Transaction date cannot be in the past.")
			.WithName("Transaction Date");

		// Validate PaymentType
		RuleFor(transaction => transaction.PaymentType)
			.NotEmpty().WithMessage("Payment type is required.")
			.Must(pt => pt == "Cash" || pt == "CreditCard" || pt == "DebitCard" || pt == "UPI")
			.WithMessage("Payment type must be 'Cash', 'CreditCard', 'DebitCard', or 'UPI'.")
			.WithName("Payment Type");

		// Validate PaymentStatus
		RuleFor(transaction => transaction.PaymentStatus)
			.NotEmpty().WithMessage("Payment status is required.")
			.Must(ps => ps == "Pending" || ps == "Completed" || ps == "Failed" || ps == "Cancelled")
			.WithMessage("Payment status must be 'Pending', 'Completed', 'Failed', or 'Cancelled'.")
			.WithName("Payment Status");

		// Validate PaymentReferenceNumber (optional)
		RuleFor(transaction => transaction.PaymentReferenceNumber)
			.Matches(@"^[A-Za-z0-9]+$").WithMessage("Payment Reference Number must be alphanumeric.")
			.When(transaction => !string.IsNullOrEmpty(transaction.PaymentReferenceNumber))
			.WithName("Payment Reference Number");

		// Validate CashPaymentAmount (optional)
		RuleFor(transaction => transaction.CashPaymentAmount)
			.GreaterThanOrEqualTo(0).WithMessage("Cash payment amount must be greater than or equal to 0.")
			.When(transaction => transaction.PaymentType == "Cash")
			.WithName("Cash Payment Amount");

		// Validate CardNumber (optional)
		RuleFor(transaction => transaction.CardNumber)
			.Matches(@"^\d{16}$").WithMessage("Card number must be 16 digits.")
			.When(transaction => !string.IsNullOrEmpty(transaction.CardNumber))
			.WithName("Card Number");

		// Validate CardHolderName (optional)
		RuleFor(transaction => transaction.CardHolderName)
			.NotEmpty().WithMessage("Cardholder name is required.")
			.When(transaction => !string.IsNullOrEmpty(transaction.CardHolderName))
			.WithName("Card Holder Name");

		// Validate CardExpiryDate (optional)
		RuleFor(transaction => transaction.CardExpiryDate)
			.Matches(@"^(0[1-9]|1[0-2])\/[0-9]{2}$").WithMessage("Card expiry date must be in MM/YY format.")
			.When(transaction => !string.IsNullOrEmpty(transaction.CardExpiryDate))
			.WithName("Card Expiry Date");

		// Validate UPIID (optional)
		RuleFor(transaction => transaction.UPIID)
			.Matches(@"^[a-zA-Z0-9]+@upi$").WithMessage("UPI ID must be in a valid format (e.g., example@upi).")
			.When(transaction => !string.IsNullOrEmpty(transaction.UPIID))
			.WithName("UPI ID");

		// Validate TransactionType
		RuleFor(transaction => transaction.TransactionType)
			.NotEmpty().WithMessage("Transaction type is required.")
			.Must(tt => tt == "Sale" || tt == "Installment")
			.WithMessage("Transaction type must be 'Sale' or 'Installment'.")
			.WithName("Transaction Type");

		// Validate Status
		RuleFor(transaction => transaction.Status)
			.NotEmpty().WithMessage("Transaction status is required.")
			.Must(s => s == "Pending" || s == "Completed" || s == "Cancelled")
			.WithMessage("Transaction status must be 'Pending', 'Completed', or 'Cancelled'.")
			.WithName("Status");
	}
}
