using FluentValidation;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class TrackingIdValidator : AbstractValidator<string>
    {
        public TrackingIdValidator()
        {
            RuleFor(p => p).Matches("^[A-Z0-9]{9}$");
        }
    }
}
