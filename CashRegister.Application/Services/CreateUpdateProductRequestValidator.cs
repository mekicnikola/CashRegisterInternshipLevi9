using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services
{
    public class CreateUpdateProductRequestValidator : AbstractValidator<CreateUpdateProductRequest>
    {
        public CreateUpdateProductRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(request => request.Price).NotEmpty().WithMessage("Price is required.");
        }
    }
}
