using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
