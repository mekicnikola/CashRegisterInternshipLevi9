using CashRegister.Application.Services;
using CashRegister.Application.Services.MediatR;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CashRegister.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillController : ControllerBase
    {
        private readonly BillService _billService;
        private readonly ILogger<BillService> _logger;
        private readonly IBillRepository _billRepository;
        private readonly IMediator _mediator;

        public BillController(BillService billService, ILogger<BillService> logger, IBillRepository billRepository, IMediator mediator)
        {
            _billService = billService;
            _logger = logger;
            _billRepository = billRepository;
            _mediator = mediator;
        }


        [HttpPost]
        //[AuthorizeRoles("Salesman")]
        public async Task<IActionResult> CreateBill([FromBody] CreateBillRequest request)
        {    
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var bills = await _billService.CreateBillAsync(request);
                if (bills.Any())
                {
                    return CreatedAtAction(nameof(GetBill), new { id = bills.First().Id }, bills.First());
                }

                return BadRequest("No bills were created.");
            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (Exception ex)
            {
                // ex Logging
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBill(int id)
        {
            try
            {
                var bill = await _billService.GetBillAsync(id);
                if (bill == null)
                {
                    return NotFound();
                }
                return Ok(bill);
            }
            catch (KeyNotFoundException knfException)
            {
                return NotFound(knfException.Message);
            }
            catch (Exception ex)
            {
                // ex Logging
                return StatusCode(500, ex.Message);
            }
        }

        //medatr
        [HttpPut("{billId}")]
        //[AuthorizeRoles("Salesman")]
        public async Task<ActionResult<Bill>> UpdateBill(int billId, UpdateBillRequest request)
        {
            var command = new UpdateBillCommand
            {
                BillId = billId,
                PaymentMethod = request.PaymentMethod,
                TotalPrice = request.TotalPrice,
                CreditCardId = request.CreditCardId
            };

            var updatedBill = await _mediator.Send(command);
            return Ok(updatedBill);
        }

        [HttpDelete("{billId}")]
        //[AuthorizeRoles("Salesman")]
        public async Task<IActionResult> SoftDeleteBill(int billId)
        {
            try
            {
                await _billService.SoftDeleteBillAsync(billId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Bill not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting bill with ID {BillId}", billId);
                return StatusCode(500, "An error occurred while deleting the bill.");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBillsForUser(int userId)
        {
            try
            {
                var bills = await _billRepository.GetBillsByUserIdAsync(userId);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bills for user with ID {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving the bills.");
            }
        }
    }
}
