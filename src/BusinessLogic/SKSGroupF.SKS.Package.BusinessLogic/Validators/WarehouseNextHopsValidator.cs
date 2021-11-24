using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System.Diagnostics.CodeAnalysis;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    [ExcludeFromCodeCoverage]
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
