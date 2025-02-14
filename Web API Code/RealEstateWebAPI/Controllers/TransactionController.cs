using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Data;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class TransactionController : ControllerBase
	{
		private readonly TransactionRepository _transactionRepository;

		public TransactionController(TransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		#region Insert Transaction
		[HttpPost]
		public IActionResult InsertTransaction([FromBody] TransactionModel transaction)
		{
			if (transaction == null)
			{
				return BadRequest(new { Message = "Invalid transaction data provided." });
			}

			int isInserted = _transactionRepository.InsertTransaction(transaction);
			if (isInserted > 0)
			{
				return Ok(new { TransactionId = isInserted, Message = "Transaction added successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while adding the transaction." });
		}
		#endregion

		#region Get All Transactions
		[HttpGet]
		public IActionResult GetAllTransactions()
		{
			var transactions = _transactionRepository.GetAllTransactions();
			if (transactions == null || transactions.Count == 0)
			{
				return NotFound(new { Message = "No transactions found." });
			}
			return Ok(transactions);
		}
		#endregion

		//#region Get Transaction By ID
		//[HttpGet("{id}")]
		//public IActionResult GetTransactionById(int id)
		//{
		//	var transaction = _transactionRepository.GetT(id);
		//	if (transaction == null)
		//	{
		//		return NotFound(new { Message = "Transaction not found." });
		//	}
		//	return Ok(transaction);
		//}
		//#endregion

		#region Get Transactions by SellerID
		[HttpGet("BySeller/{sellerID}")]
		public IActionResult GetTransactionsBySellerID(int sellerID)
		{
			var transactions = _transactionRepository.GetTransactionsBySellerID(sellerID);
			if (transactions == null || !transactions.Any())
			{
				return NotFound(new { Message = "No transactions found for the given seller." });
			}
			return Ok(transactions);
		}
		#endregion

		#region Get Transactions by BuyerID
		[HttpGet("ByBuyer/{buyerID}")]
		public IActionResult GetTransactionsByBuyerID(int buyerID)
		{
			var transactions = _transactionRepository.GetTransactionsByBuyerID(buyerID);
			if (transactions == null || !transactions.Any())
			{
				return NotFound(new { Message = "No transactions found for the given buyer." });
			}
			return Ok(transactions);
		}
		#endregion

		#region Update Transaction
		[HttpPut("{transactionID}")]
		public IActionResult UpdateTransaction(int transactionID, [FromBody] TransactionModel transaction)
		{
			if (transactionID == 0 || transaction == null)
			{
				return BadRequest(new { Message = "Invalid transaction data." });
			}

			bool isUpdated = _transactionRepository.UpdateTransaction(transaction);
			if (isUpdated)
			{
				return Ok(new { Message = "Transaction updated successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the transaction." });
		}
		#endregion

		#region Property Drop Down for Transaction
		[HttpGet("{userId}")]
		public IActionResult GetTransactionPropertyDropDown(int userId)
		{
			var properties = _transactionRepository.TransactionPropertyDropDown(userId);
			return Ok(properties);
		}
		#endregion
	}

}
