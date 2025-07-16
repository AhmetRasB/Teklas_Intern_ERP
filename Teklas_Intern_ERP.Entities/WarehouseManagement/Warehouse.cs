using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    /// <summary>
    /// Warehouse Entity - Enterprise Resource Planning
    /// Represents warehouse/warehouse information in the system
    /// </summary>
    public class Warehouse : AuditEntity
    {
        public Warehouse()
        {
            Locations = new HashSet<Location>();
            StockEntries = new HashSet<StockEntry>();
        }

        /// <summary>
        /// Unique warehouse code
        /// </summary>
        [Required]
        [StringLength(20)]
        public string WarehouseCode { get; set; } = string.Empty;

        /// <summary>
        /// Warehouse name/description
        /// </summary>
        [Required]
        [StringLength(200)]
        public string WarehouseName { get; set; } = string.Empty;

        /// <summary>
        /// Warehouse type (Main, Branch, External, etc.)
        /// </summary>
        [StringLength(50)]
        public string? WarehouseType { get; set; }

        /// <summary>
        /// Warehouse address
        /// </summary>
        [StringLength(500)]
        public string? Address { get; set; }

        /// <summary>
        /// Warehouse city
        /// </summary>
        [StringLength(100)]
        public string? City { get; set; }

        /// <summary>
        /// Warehouse country
        /// </summary>
        [StringLength(100)]
        public string? Country { get; set; }

        /// <summary>
        /// Warehouse postal code
        /// </summary>
        [StringLength(20)]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Warehouse phone number
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Warehouse email
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Warehouse manager name
        /// </summary>
        [StringLength(100)]
        public string? ManagerName { get; set; }

        /// <summary>
        /// Warehouse manager phone
        /// </summary>
        [StringLength(20)]
        public string? ManagerPhone { get; set; }

        /// <summary>
        /// Warehouse manager email
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? ManagerEmail { get; set; }

        /// <summary>
        /// Warehouse capacity (total area in square meters)
        /// </summary>
        public decimal? Capacity { get; set; }

        /// <summary>
        /// Warehouse temperature (for temperature-controlled warehouses)
        /// </summary>
        public decimal? Temperature { get; set; }

        /// <summary>
        /// Warehouse humidity (for humidity-controlled warehouses)
        /// </summary>
        public decimal? Humidity { get; set; }

        /// <summary>
        /// Additional description
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Warehouse status (Active, Inactive, Maintenance, etc.)
        /// </summary>
        [StringLength(50)]
        public new string? Status { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Locations in this warehouse
        /// </summary>
        public virtual ICollection<Location> Locations { get; set; }

        /// <summary>
        /// Stock entries for this warehouse
        /// </summary>
        public virtual ICollection<StockEntry> StockEntries { get; set; }

        #endregion
    }
} 