using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    /// <summary>
    /// Location Entity - Enterprise Resource Planning
    /// Represents warehouse locations/shelves in the system
    /// </summary>
    public class Location : AuditEntity
    {
        public Location()
        {
            StockEntries = new HashSet<StockEntry>();
        }

        /// <summary>
        /// Unique location code
        /// </summary>
        [Required]
        [StringLength(20)]
        public string LocationCode { get; set; } = string.Empty;

        /// <summary>
        /// Location name/description
        /// </summary>
        [Required]
        [StringLength(200)]
        public string LocationName { get; set; } = string.Empty;

        /// <summary>
        /// Warehouse foreign key
        /// </summary>
        public long WarehouseId { get; set; }

        /// <summary>
        /// Location type (Shelf, Rack, Zone, etc.)
        /// </summary>
        [StringLength(50)]
        public string? LocationType { get; set; }

        /// <summary>
        /// Aisle number
        /// </summary>
        [StringLength(10)]
        public string? Aisle { get; set; }

        /// <summary>
        /// Rack number
        /// </summary>
        [StringLength(10)]
        public string? Rack { get; set; }

        /// <summary>
        /// Level number
        /// </summary>
        [StringLength(10)]
        public string? Level { get; set; }

        /// <summary>
        /// Position number
        /// </summary>
        [StringLength(10)]
        public string? Position { get; set; }

        /// <summary>
        /// Location capacity (maximum quantity that can be stored)
        /// </summary>
        public decimal? Capacity { get; set; }

        /// <summary>
        /// Current occupied capacity
        /// </summary>
        public decimal? OccupiedCapacity { get; set; }

        /// <summary>
        /// Location dimensions (length x width x height)
        /// </summary>
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }

        /// <summary>
        /// Location weight capacity
        /// </summary>
        public decimal? WeightCapacity { get; set; }

        /// <summary>
        /// Temperature requirement (for temperature-controlled locations)
        /// </summary>
        public decimal? Temperature { get; set; }

        /// <summary>
        /// Humidity requirement (for humidity-controlled locations)
        /// </summary>
        public decimal? Humidity { get; set; }

        /// <summary>
        /// Location status (Active, Inactive, Maintenance, etc.)
        /// </summary>
        [StringLength(50)]
        public new string? Status { get; set; }

        /// <summary>
        /// Additional description
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Warehouse navigation property
        /// </summary>
        public virtual Warehouse Warehouse { get; set; } = null!;

        /// <summary>
        /// Stock entries for this location
        /// </summary>
        public virtual ICollection<StockEntry> StockEntries { get; set; }

        #endregion
    }
} 