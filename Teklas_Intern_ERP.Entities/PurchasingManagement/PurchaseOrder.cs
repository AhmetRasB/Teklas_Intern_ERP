using System;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.Entities.PurchasingManagement
{
    public sealed class PurchaseOrder : AuditEntity
    {
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;
        
        public DateTime OrderDate { get; set; }
        
        public DateTime? ExpectedDeliveryDate { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; } = "Draft"; // Draft, Confirmed, Received, Cancelled
        
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        
        [Required]
        public bool IsActive { get; set; } = true;
    }
} 