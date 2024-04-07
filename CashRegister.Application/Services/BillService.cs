using AutoMapper;
using CashRegister.Application.Services.CurrencyManager;
using CashRegister.Application.Services.Dto;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Context;
using CashRegister.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Application.Services
{
    public class BillService
    {
        private readonly IBillRepository _billRepository;
        private readonly CashRegisterDBContext _context;
        private readonly ICreditCardService _creditCardService;
        private readonly ICurrencyManager _currencyManager;
        private readonly IMapper _mapper;
        public BillService(CashRegisterDBContext context, ICreditCardService creditCardService, IBillRepository billRepository, ICurrencyManager currencyManager, IMapper mapper)
        {
            _context = context;
            _creditCardService = creditCardService;
            _billRepository = billRepository;
            _currencyManager = currencyManager;
            _mapper = mapper;
        }

        public async Task<List<Bill>> CreateBillAsync(CreateBillRequest request)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
            {
                throw new ArgumentException("User not found.", nameof(request.UserId));
            }

            if (request is { PaymentMethod: "Card", CreditCardId:not null  ,CreditCardNumber: not null })
            {
                await _creditCardService.VerifyUserCreditCardAsync(request.UserId, request.CreditCardNumber, request.CreditCardId.Value);
            }

            var billNumberService = new BillNumberService();

            var billManager = new BillManager(request.UserId, request.PaymentMethod, request.CreditCardId, _currencyManager, request.Currency, billNumberService);

            foreach (var productDto in request.Products)
            {
                var product = await _context.Products.FindAsync(productDto.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {productDto.ProductId} not found.");
                }

                await billManager.AddProductToBill(product, productDto.Quantity);
            }

            var bills = billManager.FinishBilling();

            await _billRepository.AddBillsAsync(bills);

            return bills;
        }

        public async Task<BillDto> GetBillAsync(int id)
        {
            var deletedBillIds = await _billRepository.GetDeletedBillIdsAsync();

            var bill = await _billRepository.GetBillByIdAsync(id, deletedBillIds);

            if (bill == null)
            {
                throw new KeyNotFoundException("Bill not found.");
            }

            var billDTO = _mapper.Map<BillDto>(bill);

            return billDTO;
        }

        public async Task SoftDeleteBillAsync(int billId)
        {
            var bill = await _billRepository.GetBillByIdAsync(billId, new List<int>());
            if (bill == null)
            {
                throw new KeyNotFoundException($"Bill with ID {billId} not found.");
            }
            await _billRepository.SoftDeleteBillAsync(billId);
        }


    }

}
