namespace Teklas_Intern_ERP.DTOs;

using FluentValidation;
using Teklas_Intern_ERP.DTOs;
using System.Text.Json.Serialization;

// Create DTO
public sealed class CreateWorkOrderDto
{
    public long BOMHeaderId { get; set; }
    public long MaterialCardId { get; set; }
    public decimal PlannedQuantity { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
}

// Update DTO
public sealed class UpdateWorkOrderDto
{
    [JsonPropertyName("workOrderId")]
    public long WorkOrderId { get; set; }
    public decimal? PlannedQuantity { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
}

// Response DTO
public sealed class WorkOrderDto
{
    [JsonPropertyName("workOrderId")]
    public long WorkOrderId { get; set; }
    public long BOMHeaderId { get; set; }
    public string? BOMName { get; set; }
    public long MaterialCardId { get; set; }
    public string? MaterialCardName { get; set; }
    public decimal PlannedQuantity { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public List<WorkOrderOperationDto>? Operations { get; set; }
    
    // Audit fields
    public DateTime CreateDate { get; set; }
    public long CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public long? UpdateUserId { get; set; }
    public StatusType Status { get; set; }
    public bool IsDeleted { get; set; }
}

public sealed class CreateWorkOrderDtoValidator : AbstractValidator<CreateWorkOrderDto>
{
    public CreateWorkOrderDtoValidator()
    {
        RuleFor(x => x.BOMHeaderId)
            .GreaterThan(0).WithMessage(Error.BOMHeaderIdRequired);
        RuleFor(x => x.MaterialCardId)
            .GreaterThan(0).WithMessage(Error.MaterialCardIdRequired);
        RuleFor(x => x.PlannedQuantity)
            .GreaterThan(0).WithMessage(Error.QuantityMustBePositive);
        RuleFor(x => x.PlannedStartDate)
            .NotEmpty().WithMessage(Error.PlannedStartDateRequired);
    }
}

public sealed class UpdateWorkOrderDtoValidator : AbstractValidator<UpdateWorkOrderDto>
{
    public UpdateWorkOrderDtoValidator()
    {
        RuleFor(x => x.WorkOrderId)
            .GreaterThan(0).WithMessage(Error.WorkOrderIdRequired);
        RuleFor(x => x.PlannedQuantity)
            .GreaterThan(0).When(x => x.PlannedQuantity.HasValue).WithMessage(Error.QuantityMustBePositive);
    }
}