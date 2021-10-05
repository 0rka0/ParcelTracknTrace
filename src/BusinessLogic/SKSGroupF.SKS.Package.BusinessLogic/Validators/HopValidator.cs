using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class HopValidator : AbstractValidator<BLHop>
    {
        public HopValidator()
        {
            RuleFor(p => p.LocationCoordinates).NotNull();
        }
    }
}
