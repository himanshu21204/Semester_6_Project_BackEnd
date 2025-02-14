using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Models;
using RealEstateWebAPI.Repositories;
using RealEstateWebAPI.Models;
using RealEstateWebAPI.Repositories;

namespace RealEstateWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class InstallmentController : ControllerBase
	{
		private readonly InstallmentRepository _installmentRepository;

		public InstallmentController(InstallmentRepository installmentRepository)
		{
			_installmentRepository = installmentRepository;
		}

		#region Insert Installment
		[HttpPost]
		public async Task<IActionResult> InsertInstallment([FromBody] InstallmentModel installment)
		{
			if (installment == null)
			{
				return BadRequest(new { Message = "Invalid installment data provided." });
			}

			var result = await _installmentRepository.InsertInstallmentAsync(installment);
			if (result > 0)
			{
				return Ok(new { Message = "Installment entry added successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while adding the installment entry." });
		}
		#endregion

		#region Get All Installments
		[HttpGet]
		public async Task<IActionResult> GetAllInstallments()
		{
			var installments = await _installmentRepository.GetAllInstallmentsAsync();
			if (installments == null || !installments.Any())
			{
				return NotFound(new { Message = "No installment entries found." });
			}
			return Ok(installments);
		}
		#endregion

		#region Get Installment By ID
		[HttpGet("{id}")]
		public async Task<IActionResult> GetInstallmentById(int id)
		{
			var installment = await _installmentRepository.GetInstallmentByIdAsync(id);
			if (installment == null)
			{
				return NotFound(new { Message = "Installment entry not found." });
			}
			return Ok(installment);
		}
		#endregion

		#region Get Installments By Transaction ID
		[HttpGet("ByTransactionId/{transactionId}")]
		public async Task<IActionResult> GetInstallmentsByTransactionId(int transactionId)
		{
			var installments = await _installmentRepository.GetInstallmentsByTransactionIdAsync(transactionId);
			if (installments == null || !installments.Any())
			{
				return NotFound(new { Message = "No installments found for the specified transaction ID." });
			}
			return Ok(installments);
		}
		#endregion

		#region Update Installment
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateInstallment(int id, [FromBody] InstallmentModel installment)
		{
			if (id != installment.InstallmentID)
			{
				return BadRequest(new { Message = "Installment ID mismatch." });
			}

			var result = await _installmentRepository.UpdateInstallmentAsync(installment);
			if (result > 0)
			{
				return Ok(new { Message = "Installment updated successfully." });
			}
			return StatusCode(500, new { Message = "An error occurred while updating the installment." });
		}
		#endregion
	}
}
