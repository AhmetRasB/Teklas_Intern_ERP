using FluentValidation;
using Teklas_Intern_ERP.Entities.ProductionManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class WorkOrderValidator : AbstractValidator<WorkOrder>
    {
        public WorkOrderValidator()
        {
            RuleFor(x => x.WorkOrderNo)
                .NotEmpty().WithMessage("İş emri numarası boş olamaz.")
                .MaximumLength(30);

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Ürün seçilmelidir.");

            RuleFor(x => x.PlannedStartDate)
                .NotEmpty().WithMessage("Planlanan başlama tarihi boş olamaz.");

            RuleFor(x => x.PlannedEndDate)
                .NotEmpty().WithMessage("Planlanan bitiş tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.PlannedStartDate).WithMessage("Bitiş tarihi başlangıçtan önce olamaz.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalı.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Durum boş olamaz.")
                .MaximumLength(30);

            RuleFor(x => x.ResponsiblePerson)
                .NotEmpty().WithMessage("Sorumlu kişi boş olamaz.")
                .MaximumLength(100);
        }
    }
} 