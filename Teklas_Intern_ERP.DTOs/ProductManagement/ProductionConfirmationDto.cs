namespace Teklas_Intern_ERP.DTOs;

using FluentValidation;

// Create DTO
public sealed class CreateProductionConfirmationDto
{
    public long WorkOrderId { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public decimal QuantityProduced { get; set; }
    public decimal? QuantityScrapped { get; set; }
    public decimal? LaborHoursUsed { get; set; }
    public string? PerformedBy { get; set; }
    public List<CreateMaterialConsumptionDto>? Consumptions { get; set; }
}

// Update DTO
public sealed class UpdateProductionConfirmationDto
{
    public long ConfirmationId { get; set; }
    public decimal? QuantityProduced { get; set; }
    public decimal? QuantityScrapped { get; set; }
    public decimal? LaborHoursUsed { get; set; }
    public string? PerformedBy { get; set; }
}

public sealed class CreateProductionConfirmationDtoValidator : AbstractValidator<CreateProductionConfirmationDto>
{
    public CreateProductionConfirmationDtoValidator()
    {
        RuleFor(x => x.WorkOrderId)
            .GreaterThan(0).WithMessage(Error.WorkOrderIdRequired);
        RuleFor(x => x.ConfirmationDate)
            .NotEmpty().WithMessage(Error.ConfirmationDateRequired);
        RuleFor(x => x.QuantityProduced)
            .GreaterThanOrEqualTo(0).WithMessage(Error.QuantityMustBeNonNegative);
        RuleFor(x => x.QuantityScrapped)
            .GreaterThanOrEqualTo(0).When(x => x.QuantityScrapped.HasValue).WithMessage(Error.QuantityMustBeNonNegative);
        RuleFor(x => x.LaborHoursUsed)
            .GreaterThanOrEqualTo(0).When(x => x.LaborHoursUsed.HasValue).WithMessage(Error.LaborHoursMustBeNonNegative);
        RuleFor(x => x.PerformedBy)
            .MaximumLength(100).WithMessage(Error.PerformedByMaxLength);
    }
}

public sealed class UpdateProductionConfirmationDtoValidator : AbstractValidator<UpdateProductionConfirmationDto>
{
    public UpdateProductionConfirmationDtoValidator()
    {
        RuleFor(x => x.ConfirmationId)
            .GreaterThan(0).WithMessage(Error.ConfirmationIdRequired);
        RuleFor(x => x.QuantityProduced)
            .GreaterThanOrEqualTo(0).When(x => x.QuantityProduced.HasValue).WithMessage(Error.QuantityMustBeNonNegative);
        RuleFor(x => x.QuantityScrapped)
            .GreaterThanOrEqualTo(0).When(x => x.QuantityScrapped.HasValue).WithMessage(Error.QuantityMustBeNonNegative);
        RuleFor(x => x.LaborHoursUsed)
            .GreaterThanOrEqualTo(0).When(x => x.LaborHoursUsed.HasValue).WithMessage(Error.LaborHoursMustBeNonNegative);
        RuleFor(x => x.PerformedBy)
            .MaximumLength(100).WithMessage(Error.PerformedByMaxLength);
    }
}

// Response DTO
public sealed class ProductionConfirmationDto
{
    public long ConfirmationId { get; set; }
    public long WorkOrderId { get; set; }
    public DateTime ConfirmationDate { get; set; }
    public decimal QuantityProduced { get; set; }
    public decimal? QuantityScrapped { get; set; }
    public decimal? LaborHoursUsed { get; set; }
    public string? PerformedBy { get; set; }
    public List<MaterialConsumptionDto>? Consumptions { get; set; }
}

public sealed class MaterialConsumptionDto
{
    public long ConsumptionId { get; set; }
    public long MaterialCardId { get; set; }
    public string? MaterialCardName { get; set; }
    public decimal QuantityUsed { get; set; }
    public string? BatchNumber { get; set; }
}

public sealed class CreateMaterialConsumptionDto
{
    public long MaterialCardId { get; set; }
    public decimal QuantityUsed { get; set; }
    public string? BatchNumber { get; set; }
}

public sealed class UpdateMaterialConsumptionDto
{
    public long ConsumptionId { get; set; }
    public decimal? QuantityUsed { get; set; }
    public string? BatchNumber { get; set; }
}