using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
