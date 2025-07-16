using System;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.Entities.SalesManagement
{
    /// <summary>
    /// Customer Order Entity - Enterprise Resource Planning
    /// Represents customer orders in the system
    /// </summary>
    public sealed class CustomerOrder : AuditEntity
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
        public string Status { get; set; } = "Draft"; // Draft, Confirmed, Shipped, Delivered, Cancelled
        
        public long CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        
        [Required]
        public bool IsActive { get; set; } = true;
    }
} 