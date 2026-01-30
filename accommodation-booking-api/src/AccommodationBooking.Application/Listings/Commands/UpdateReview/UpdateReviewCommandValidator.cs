using FluentValidation;

namespace AccommodationBooking.Application.Listings.Commands.UpdateReview
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Ocena musi byc w przedziale od 1 do 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Tresc opinii jest wymagana.")
                .MaximumLength(1000).WithMessage("Tresc opinii nie moze byc dluzsza niz 1000 znaków.");
        }
    }
}
