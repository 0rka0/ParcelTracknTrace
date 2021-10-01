using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
