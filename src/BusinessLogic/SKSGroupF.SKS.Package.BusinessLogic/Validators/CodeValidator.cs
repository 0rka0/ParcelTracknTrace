using FluentValidation;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class CodeValidator : AbstractValidator<string>
    {
        public CodeValidator()
        {
            RuleFor(p => p).Matches(@"^[A-Z]{4}\\d{1,4}$");
        }
    }
}
