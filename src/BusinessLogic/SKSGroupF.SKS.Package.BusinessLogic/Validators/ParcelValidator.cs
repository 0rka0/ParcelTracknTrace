using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class ParcelValidator : AbstractValidator<BLParcel>
    {
        public ParcelValidator()
        {
            RuleFor(p => p.Receipient)
                .SetValidator(new ReceipientValidator())
                .NotNull()
                .NotEqual(p => p.Sender);
        }
    }
}
