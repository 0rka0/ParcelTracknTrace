using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseValidator : AbstractValidator<BLWarehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(p => p.NextHops).NotNull();
        }
    }
}
