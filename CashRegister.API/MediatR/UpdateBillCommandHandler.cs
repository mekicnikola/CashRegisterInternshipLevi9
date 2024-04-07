using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services.MediatR
{
    public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand, Bill>
    {
        private readonly IBillRepository _billRepository;

        public UpdateBillCommandHandler(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public async Task<Bill> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            var deletedBillIds = await _billRepository.GetDeletedBillIdsAsync();
            var bill = await _billRepository.GetBillByIdAsync(request.BillId, deletedBillIds);

            if (bill == null)
            {
                throw new KeyNotFoundException("Bill not found.");
            }

            bill.PaymentMethod = request.PaymentMethod;
            bill.TotalPrice = request.TotalPrice;
            bill.CreditCardId = request.CreditCardId;

            await _billRepository.UpdateBillAsync(bill);

            return bill;
        }
    }
}
