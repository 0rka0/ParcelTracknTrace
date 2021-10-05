using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseNextHopsValidator : AbstractValidator<BLWarehouseNextHops>
    {
        public WarehouseNextHopsValidator()
        {
            RuleFor(p => p.Hop)
                .SetValidator(new HopValidator())
                .NotNull();
        }
    }
}
