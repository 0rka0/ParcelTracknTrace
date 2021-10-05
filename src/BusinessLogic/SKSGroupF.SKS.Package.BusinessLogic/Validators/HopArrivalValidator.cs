using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class HopArrivalValidator : AbstractValidator<BLHopArrival>
    {
        public HopArrivalValidator()
        {
            RuleFor(p => p.Code).Matches(@"^[A-Z]{4}\\d{1,4}$");
        }
    }
}
