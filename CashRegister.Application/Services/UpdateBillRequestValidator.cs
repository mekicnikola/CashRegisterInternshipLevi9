using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services
{
    public class UpdateBillRequestValidator : AbstractValidator<UpdateBillRequest>
    {
        public UpdateBillRequestValidator()
        {
            RuleFor(request => request.TotalPrice).NotEmpty().WithMessage("Total price is required.");
        }
    }
}
