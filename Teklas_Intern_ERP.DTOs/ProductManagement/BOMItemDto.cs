namespace Teklas_Intern_ERP.DTOs;

using FluentValidation;
using Teklas_Intern_ERP.DTOs;

// Create DTO
public sealed class CreateBOMItemDto
{
    public long BOMHeaderId { get; set; }
    public long ComponentMaterialCardId { get; set; }
    public decimal Quantity { get; set; }
    public decimal? ScrapRate { get; set; }
}

// Update DTO
public sealed class UpdateBOMItemDto
{
    public long BOMItemId { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? ScrapRate { get; set; }
}

// Response DTO
public sealed class BOMItemDto
{
    public long BOMItemId { get; set; }
    public long BOMHeaderId { get; set; }
    public long ComponentMaterialCardId { get; set; }
    public string? ComponentMaterialCardName { get; set; }
    public decimal Quantity { get; set; }
    public decimal? ScrapRate { get; set; }
}

public sealed class CreateBOMItemDtoValidator : AbstractValidator<CreateBOMItemDto>
{
    public CreateBOMItemDtoValidator()
    {
        RuleFor(x => x.BOMHeaderId)
            .GreaterThan(0).WithMessage(Error.BOMHeaderIdRequired);
        RuleFor(x => x.ComponentMaterialCardId)
            .GreaterThan(0).WithMessage(Error.MaterialCardIdRequired);
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(Error.QuantityMustBePositive);
        RuleFor(x => x.ScrapRate)
            .GreaterThanOrEqualTo(0).When(x => x.ScrapRate.HasValue).WithMessage(Error.ScrapRateMustBeNonNegative);
    }
}

public sealed class UpdateBOMItemDtoValidator : AbstractValidator<UpdateBOMItemDto>
{
    public UpdateBOMItemDtoValidator()
    {
        RuleFor(x => x.BOMItemId)
            .GreaterThan(0).WithMessage(Error.BOMItemIdRequired);
        RuleFor(x => x.Quantity)
            .GreaterThan(0).When(x => x.Quantity.HasValue).WithMessage(Error.QuantityMustBePositive);
        RuleFor(x => x.ScrapRate)
            .GreaterThanOrEqualTo(0).When(x => x.ScrapRate.HasValue).WithMessage(Error.ScrapRateMustBeNonNegative);
    }
}