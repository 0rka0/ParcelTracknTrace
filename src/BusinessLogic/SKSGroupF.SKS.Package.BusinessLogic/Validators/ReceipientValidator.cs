using FluentValidation;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class ReceipientValidator : AbstractValidator<BLReceipient>
    {
        public ReceipientValidator()
        {
            //RuleFor(p => p.Country).Must(x => x.Equals("Austria") || x.Equals("Österreich"));
            RuleFor(p => p.Name).Matches(@"^[A-Z][A-Za-zßÄÖÜäöü\-\s]*$").When(x => x.Country.Equals("Austria") || x.Country.Equals("Österreich"));
            RuleFor(p => p.Street).Matches(@"^([A-Za-zßäöüÄÖÜ]+([\s][[A-Za-zßäöüÄÖÜ\S])?)+([\s][0-9]+[A-Za-z]?)(([\/][^\/])[0-9]*)*$").When(x => x.Country.Equals("Austria") || x.Country.Equals("Österreich"));
            RuleFor(p => p.PostalCode).Matches("^A-[0-9]{4}$").When(x => x.Country.Equals("Austria") || x.Country.Equals("Österreich"));
            RuleFor(p => p.City).Matches(@"^[A-Z][A-Za-zßÄÖÜäöü\-\s]*$").When(x => x.Country.Equals("Austria") || x.Country.Equals("Österreich"));
        }
    }
}
