using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the review text is between 10 and 5000 characters.
/// </summary>
internal sealed class TextLengthValidator
{
    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.Text.Length >= 10 && model.Root.Text.Length <= 5000;

    public string Reason => "Review text must be between 10 and 5000 characters.";
}
