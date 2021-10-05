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
            RuleFor(p => p.Sender)
                .SetValidator(new ReceipientValidator())
                .NotNull()
                .NotEqual(p => p.Receipient);
            RuleFor(p => p.TrackingId).Matches("^[A-Z0-9]{9}$");
            RuleFor(p => p.Weight).GreaterThanOrEqualTo(0.0f);
            RuleFor(p => p.State).NotNull();
            RuleFor(p => p.VisitedHops).NotNull();
            RuleFor(p => p.FutureHops).NotNull();
        }
    }
}
