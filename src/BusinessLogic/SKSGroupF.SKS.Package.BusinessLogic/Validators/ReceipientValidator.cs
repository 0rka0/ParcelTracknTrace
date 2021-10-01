using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class ReceipientValidator : AbstractValidator<BLReceipient>
    {
        public ReceipientValidator()
        {
            RuleFor(p => p.Name).Matches(@"^[A-Z][A-Za-z\-\s]*$");
            RuleFor(p => p.Country).Equal("Austria").Equal("Österreich");
            RuleFor(p => p.Street).Matches(@"^([A-Za-zßäöüÄÖÜ]+([\s][[A-Za-zßäöüÄÖÜ\S])?)+([\s][0-9]+[A-Za-z]?)(([\/][^\/])[0-9]*)*$");
            RuleFor(p => p.PostalCode).Matches("^A-[0-9]{4}$");
            RuleFor(p => p.City).Matches(@"^[A-Z][A-Za-z\-\s]*$");
        }
    }
}
