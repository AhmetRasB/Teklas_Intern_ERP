namespace Teklas_Intern_ERP.DTOs;

using FluentValidation;

// Create DTO
public sealed class CreateWorkOrderOperationDto
{
    public long WorkOrderId { get; set; }
    public string OperationName { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public decimal PlannedHours { get; set; }
    public string? Resource { get; set; }
}

// Update DTO
public sealed class UpdateWorkOrderOperationDto
{
    public long OperationId { get; set; }
    public string? OperationName { get; set; }
    public int? Sequence { get; set; }
    public decimal? PlannedHours { get; set; }
    public string? Resource { get; set; }
}

public sealed class CreateWorkOrderOperationDtoValidator : AbstractValidator<CreateWorkOrderOperationDto>
{
    public CreateWorkOrderOperationDtoValidator()
    {
        RuleFor(x => x.WorkOrderId)
            .GreaterThan(0).WithMessage(Error.WorkOrderIdRequired);
        RuleFor(x => x.OperationName)
            .NotEmpty().WithMessage(Error.OperationNameRequired)
            .MaximumLength(100).WithMessage(Error.OperationNameMaxLength);
        RuleFor(x => x.Sequence)
            .GreaterThan(0).WithMessage(Error.SequenceMustBePositive);
        RuleFor(x => x.PlannedHours)
            .GreaterThanOrEqualTo(0).WithMessage(Error.PlannedHoursMustBeNonNegative);
        RuleFor(x => x.Resource)
            .MaximumLength(100).WithMessage(Error.ResourceMaxLength);
    }
}

public sealed class UpdateWorkOrderOperationDtoValidator : AbstractValidator<UpdateWorkOrderOperationDto>
{
    public UpdateWorkOrderOperationDtoValidator()
    {
        RuleFor(x => x.OperationId)
            .GreaterThan(0).WithMessage(Error.OperationIdRequired);
        RuleFor(x => x.OperationName)
            .MaximumLength(100).When(x => x.OperationName != null).WithMessage(Error.OperationNameMaxLength);
        RuleFor(x => x.Sequence)
            .GreaterThan(0).When(x => x.Sequence.HasValue).WithMessage(Error.SequenceMustBePositive);
        RuleFor(x => x.PlannedHours)
            .GreaterThanOrEqualTo(0).When(x => x.PlannedHours.HasValue).WithMessage(Error.PlannedHoursMustBeNonNegative);
        RuleFor(x => x.Resource)
            .MaximumLength(100).When(x => x.Resource != null).WithMessage(Error.ResourceMaxLength);
    }
}

// Response DTO
public sealed class WorkOrderOperationDto
{
    public long OperationId { get; set; }
    public long WorkOrderId { get; set; }
    public string OperationName { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public decimal PlannedHours { get; set; }
    public string? Resource { get; set; }
}