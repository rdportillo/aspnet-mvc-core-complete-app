using FluentValidation;

namespace Dev.Business.Models.Validations
{
    public class AddressValidation : AbstractValidator<Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.Address1)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 200).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");

            RuleFor(a => a.Address2)
                .Length(2, 200).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");

            RuleFor(a => a.ZipCode)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(8).WithMessage("The {PropertyName} field have {MaxLength} characters");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 100).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");

            RuleFor(a => a.Province)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 50).WithMessage("The {PropertyName} field have between {MinLength} and {MaxLength} characters");
        }
    }
}
