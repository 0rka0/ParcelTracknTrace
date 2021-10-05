using FizzWare.NBuilder;
using FluentValidation;
using NUnit.Framework;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Validators;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    class ValidatorTests
    {
        [Test]
        public void ReceipientValidator_ReceivesValidData_IsValidIsTrue()
        {
            var receipient = Builder<BLReceipient>.CreateNew()
                .With(p => p.Country = "Österreich")
                .With(p => p.City = "Vienna")
                .With(p => p.Name = "David")
                .With(p => p.PostalCode = "A-0000")
                .With(p => p.Street = "Hauptstraße 1")
                .Build();

            IValidator<BLReceipient> validator = new ReceipientValidator();
            var result = validator.Validate(receipient);

            Assert.IsTrue(result.IsValid);
        }
    }
}
