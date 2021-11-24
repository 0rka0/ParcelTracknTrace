using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System.Diagnostics.CodeAnalysis;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseValidator : AbstractValidator<BLWarehouse>
    {
        [ExcludeFromCodeCoverage]
        public WarehouseValidator()
        {
            RuleFor(p => p.NextHops).NotNull();
        }
    }
}
