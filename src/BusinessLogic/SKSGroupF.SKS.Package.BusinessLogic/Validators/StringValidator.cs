﻿using FluentValidation;

namespace SKSGroupF.SKS.Package.BusinessLogic.Validators
{
    public class StringValidator : AbstractValidator<string>
    {
        public StringValidator(bool val)
        {
            if (val)
                RuleFor(p => p).Matches("^[A-Z0-9]{9}$");
            else
                RuleFor(p => p).Matches(@"^[A-Z]{4}\\d{1,4}$");
        }
    }
}