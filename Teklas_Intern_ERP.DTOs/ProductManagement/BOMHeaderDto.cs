namespace Teklas_Intern_ERP.DTOs;

using FluentValidation;
using Teklas_Intern_ERP.DTOs;
using System.Text.Json.Serialization;

public enum StatusType
{
    Active = 1,
    Passive = 2,
    Blocked = 3,
    Frozen = 4
}

// Create DTO
public sealed class CreateBOMHeaderDto
{
    public long ParentMaterialCardId { get; set; }
    public string? Version { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal? StandardCost { get; set; }
    public string? Notes { get; set; }
}

// Update DTO
public sealed class UpdateBOMHeaderDto
{
    [JsonPropertyName("bomHeaderId")]
    public long BOMHeaderId { get; set; }
    public string? Version { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal? StandardCost { get; set; }
    public string? Notes { get; set; }
}

public sealed class CreateBOMHeaderDtoValidator : AbstractValidator<CreateBOMHeaderDto>
{
    public CreateBOMHeaderDtoValidator()
    {
        RuleFor(x => x.ParentMaterialCardId)
            .GreaterThan(0).WithMessage(Error.MaterialCardIdRequired);
        RuleFor(x => x.Version)
            .MaximumLength(20).WithMessage(Error.VersionMaxLength);
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage(Error.NotesMaxLength);
    }
}

public sealed class UpdateBOMHeaderDtoValidator : AbstractValidator<UpdateBOMHeaderDto>
{
    public UpdateBOMHeaderDtoValidator()
    {
        RuleFor(x => x.BOMHeaderId)
            .GreaterThan(0).WithMessage(Error.BOMHeaderIdRequired);
        RuleFor(x => x.Version)
            .MaximumLength(20).WithMessage(Error.VersionMaxLength);
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage(Error.NotesMaxLength);
    }
}

// Response DTO
public sealed class BOMHeaderDto
{
    public long BOMHeaderId { get; set; }
    public long ParentMaterialCardId { get; set; }
    public string? ParentMaterialCardName { get; set; }
    public string? Version { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal? StandardCost { get; set; }
    public string? Notes { get; set; }
    public List<BOMItemDto>? BOMItems { get; set; }
    
    // Audit fields
    public DateTime CreateDate { get; set; }
    public long CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public long? UpdateUserId { get; set; }
    public StatusType Status { get; set; }
    public bool IsDeleted { get; set; }
}