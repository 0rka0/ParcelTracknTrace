using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class ReceipientValidator : AbstractValidator<BLReceipient>
    {
        public ReceipientValidator()
        {
            RuleFor(p => p.Country).Equal("Austria").Equal("Österreich");
            RuleFor(p => p.Street).NotNull();
            RuleFor(p => p.PostalCode).NotNull();
            RuleFor(p => p.City).NotNull();
        }
    }
}
