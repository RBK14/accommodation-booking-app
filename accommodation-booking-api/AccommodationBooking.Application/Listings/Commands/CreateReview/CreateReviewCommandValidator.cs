using FluentValidation;

namespace AccommodationBooking.Application.Listings.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Ocena musi być w przedziale od 1 do 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Treść opinii jest wymagana.")
                .MaximumLength(1000).WithMessage("Treść opinii nie może być dłuższa niż 1000 znaków.");
        }
    }
}
