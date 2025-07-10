using AutoMapper;
using Microsoft.Extensions.Logging;
using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Business.ProductionManagement
{
    /// <summary>
    /// Bill of Material Service - Monolithic Architecture
    /// </summary>
    public sealed class BillOfMaterialService
    {
        private readonly IBillOfMaterialRepository _bomRepository;
        private readonly IMaterialCardRepository _materialCardRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BillOfMaterialService> _logger;

        public BillOfMaterialService(
            IBillOfMaterialRepository bomRepository,
            IMaterialCardRepository materialCardRepository,
            IMapper mapper,
            ILogger<BillOfMaterialService> logger)
        {
            _bomRepository = bomRepository;
            _materialCardRepository = materialCardRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Basic CRUD Operations

        public async Task<BillOfMaterialDto?> GetByIdAsync(long id)
        {
            try
            {
                var bom = await _bomRepository.GetWithItemsAsync(id);
                return bom != null ? _mapper.Map<BillOfMaterialDto>(bom) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BOM by ID {Id}", id);
                throw;
            }
        }

        public async Task<BillOfMaterialDto?> GetByCodeAsync(string bomCode)
        {
            try
            {
                var bom = await _bomRepository.GetByCodeAsync(bomCode);
                return bom != null ? _mapper.Map<BillOfMaterialDto>(bom) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BOM by code {BomCode}", bomCode);
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetAllAsync()
        {
            try
            {
                var boms = await _bomRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(boms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all BOMs");
                throw;
            }
        }

        public async Task<BillOfMaterialDto> CreateAsync(BillOfMaterialDto bomDto)
        {
            try
            {
                // Validate DTO
                if (string.IsNullOrEmpty(bomDto.BOMCode))
                {
                    throw new InvalidOperationException("BOM code cannot be empty");
                }

                if (await _bomRepository.ExistsAsync(bomDto.BOMCode))
                {
                    throw new InvalidOperationException($"BOM with code {bomDto.BOMCode} already exists");
                }

                // Validate product material card exists
                var productCard = await _materialCardRepository.GetByIdAsync(bomDto.ProductMaterialCardId);
                if (productCard == null)
                {
                    throw new InvalidOperationException($"Product material card with ID {bomDto.ProductMaterialCardId} not found");
                }

                // Validate BOM items
                await ValidateBOMItemsAsync(bomDto.BOMItems);

                // Map and create
                var bomEntity = _mapper.Map<BillOfMaterial>(bomDto);
                // Generate BOM code if empty - simplified for monolithic
                if (string.IsNullOrEmpty(bomDto.BOMCode))
                {
                    bomEntity.BOMCode = $"BOM{DateTime.Now:yyyyMMddHHmm}";
                }
                else
                {
                    bomEntity.BOMCode = bomDto.BOMCode;
                }

                var createdBom = await _bomRepository.AddAsync(bomEntity);
                
                _logger.LogInformation("BOM created successfully with ID {Id} and code {BomCode}", 
                    createdBom.Id, createdBom.BOMCode);

                return _mapper.Map<BillOfMaterialDto>(createdBom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating BOM with code {BomCode}", bomDto.BOMCode);
                throw;
            }
        }

        public async Task<BillOfMaterialDto> UpdateAsync(BillOfMaterialDto bomDto)
        {
            try
            {
                // Validate DTO
                if (string.IsNullOrEmpty(bomDto.BOMCode))
                {
                    throw new InvalidOperationException("BOM code cannot be empty");
                }

                // Check if BOM exists
                var existingBom = await _bomRepository.GetByIdAsync(bomDto.Id);
                if (existingBom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {bomDto.Id} not found");
                }

                // Check if approved BOM is being modified
                if (existingBom.ApprovalStatus == "APPROVED")
                {
                    throw new InvalidOperationException("Cannot modify approved BOM");
                }

                // Validate BOM items
                await ValidateBOMItemsAsync(bomDto.BOMItems);

                // Map and update
                var bomEntity = _mapper.Map<BillOfMaterial>(bomDto);
                await _bomRepository.UpdateAsync(bomEntity);

                _logger.LogInformation("BOM updated successfully with ID {Id}", bomDto.Id);

                return await GetByIdAsync(bomDto.Id) ?? bomDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating BOM with ID {Id}", bomDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var bom = await _bomRepository.GetByIdAsync(id);
                if (bom == null)
                {
                    return false;
                }

                // Check if BOM has work orders
                var bomWithWorkOrders = await _bomRepository.GetWithWorkOrdersAsync(id);
                if (bomWithWorkOrders?.WorkOrders.Any() == true)
                {
                    throw new InvalidOperationException("Cannot delete BOM that has associated work orders");
                }

                await _bomRepository.DeleteAsync(id);
                
                _logger.LogInformation("BOM deleted successfully with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting BOM with ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region Business Operations

        public async Task<BillOfMaterialDto> ApproveAsync(long id, long approvedByUserId)
        {
            try
            {
                var bom = await _bomRepository.GetWithItemsAsync(id);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {id} not found");
                }

                if (bom.ApprovalStatus == "APPROVED")
                {
                    throw new InvalidOperationException("BOM is already approved");
                }

                if (!bom.BOMItems.Any())
                {
                    throw new InvalidOperationException("Cannot approve BOM without items");
                }

                bom.ApprovalStatus = "APPROVED";
                bom.ApprovedByUserId = approvedByUserId;
                bom.ApprovalDate = DateTime.UtcNow;

                await _bomRepository.UpdateAsync(bom);

                _logger.LogInformation("BOM approved successfully with ID {Id} by user {UserId}", id, approvedByUserId);

                return _mapper.Map<BillOfMaterialDto>(bom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving BOM with ID {Id}", id);
                throw;
            }
        }

        public async Task<BillOfMaterialDto> RejectAsync(long id, string reason)
        {
            try
            {
                var bom = await _bomRepository.GetByIdAsync(id);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {id} not found");
                }

                bom.ApprovalStatus = "DRAFT";
                bom.Description = $"{bom.Description}\nRejection Reason: {reason}";

                await _bomRepository.UpdateAsync(bom);

                _logger.LogInformation("BOM rejected with ID {Id}. Reason: {Reason}", id, reason);

                return _mapper.Map<BillOfMaterialDto>(bom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting BOM with ID {Id}", id);
                throw;
            }
        }

        public async Task<BillOfMaterialDto> CreateNewVersionAsync(long originalBomId, string newVersion)
        {
            try
            {
                var originalBom = await _bomRepository.GetWithItemsAsync(originalBomId);
                if (originalBom == null)
                {
                    throw new InvalidOperationException($"Original BOM with ID {originalBomId} not found");
                }

                // Create new BOM with incremented version
                var newBomDto = _mapper.Map<BillOfMaterialDto>(originalBom);
                newBomDto.Id = 0; // New entity
                newBomDto.Version = newVersion;
                newBomDto.ApprovalStatus = "DRAFT";
                newBomDto.ApprovedByUserId = null;
                newBomDto.ApprovalDate = null;

                // Clear IDs for items
                foreach (var item in newBomDto.BOMItems)
                {
                    item.Id = 0;
                }

                return await CreateAsync(newBomDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new version of BOM with ID {Id}", originalBomId);
                throw;
            }
        }

        public async Task<BOMValidationResult> ValidateAsync(long id)
        {
            try
            {
                var bom = await _bomRepository.GetWithItemsAsync(id);
                if (bom == null)
                {
                    return new BOMValidationResult
                    {
                        IsValid = false,
                        Errors = new List<string> { "BOM not found" }
                    };
                }

                var errors = new List<string>();
                var warnings = new List<string>();

                // Basic validations
                if (!bom.BOMItems.Any())
                {
                    errors.Add("BOM must have at least one item");
                }

                // Check for duplicate line numbers
                var duplicateLines = bom.BOMItems
                    .GroupBy(x => x.LineNumber)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);

                if (duplicateLines.Any())
                {
                    errors.Add($"Duplicate line numbers found: {string.Join(", ", duplicateLines)}");
                }

                // Check material card availability
                foreach (var item in bom.BOMItems)
                {
                    var materialCard = await _materialCardRepository.GetByIdAsync(item.MaterialCardId);
                    if (materialCard == null)
                    {
                        errors.Add($"Material card not found for line {item.LineNumber}");
                    }
                    else if (materialCard.Status.ToString() != "Active")
                    {
                        warnings.Add($"Material card on line {item.LineNumber} is not active");
                    }
                }

                // Check effective dates
                if (bom.EffectiveFrom.HasValue && bom.EffectiveTo.HasValue && 
                    bom.EffectiveTo.Value <= bom.EffectiveFrom.Value)
                {
                    errors.Add("Effective To date must be after Effective From date");
                }

                return new BOMValidationResult
                {
                    IsValid = !errors.Any(),
                    Errors = errors,
                    Warnings = warnings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating BOM with ID {Id}", id);
                throw;
            }
        }

        public async Task<BOMCostResult> CalculateCostAsync(long id, decimal baseQuantity = 1)
        {
            try
            {
                var bom = await _bomRepository.GetWithItemsAsync(id);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {id} not found");
                }

                // Simplified cost calculation for monolithic architecture
                decimal totalCost = 0;

                foreach (var item in bom.BOMItems)
                {
                    var materialCard = await _materialCardRepository.GetByIdAsync(item.MaterialCardId);
                    if (materialCard != null)
                    {
                        var effectiveQuantity = item.Quantity * (1 + item.ScrapFactor / 100) * baseQuantity;
                        var itemCost = effectiveQuantity * (materialCard.SalesPrice ?? 0);
                        totalCost += itemCost;
                    }
                }

                return new BOMCostResult
                {
                    MaterialCost = totalCost,
                    LaborCost = 0, // Would be calculated based on routing
                    OverheadCost = totalCost * 0.15m, // 15% overhead
                    TotalCost = totalCost * 1.15m,
                    Currency = "TRY",
                    CalculationDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cost for BOM with ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BOMExplosionDto>> ExplodeAsync(long id, decimal quantity, int level = 0)
        {
            try
            {
                var bom = await _bomRepository.GetWithItemsAsync(id);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {id} not found");
                }

                var explosion = new List<BOMExplosionDto>();

                foreach (var item in bom.BOMItems.OrderBy(x => x.LineNumber))
                {
                    var materialCard = await _materialCardRepository.GetByIdAsync(item.MaterialCardId);
                    if (materialCard != null)
                    {
                        var requiredQuantity = item.Quantity * quantity * (1 + item.ScrapFactor / 100);

                        var explosionItem = new BOMExplosionDto
                        {
                            Level = level,
                            MaterialCardId = item.MaterialCardId,
                            MaterialCardCode = materialCard.CardCode,
                            MaterialCardName = materialCard.CardName,
                            RequiredQuantity = requiredQuantity,
                            Unit = item.Unit,
                            ComponentType = item.ComponentType,
                            UnitCost = materialCard.SalesPrice ?? 0,
                            TotalCost = requiredQuantity * (materialCard.SalesPrice ?? 0)
                        };

                        explosion.Add(explosionItem);

                        // Simplified: No multi-level explosion for monolithic architecture
                    }
                }

                return explosion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exploding BOM with ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region Query Operations

        public async Task<IEnumerable<BillOfMaterialDto>> GetByProductMaterialCardAsync(long productMaterialCardId)
        {
            try
            {
                var boms = await _bomRepository.GetByProductMaterialCardAsync(productMaterialCardId);
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(boms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BOMs by product {ProductId}", productMaterialCardId);
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetActiveBOMsAsync()
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allBoms = await _bomRepository.GetAllAsync();
                var activeBoms = allBoms.Where(b => b.IsActive).ToList();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(activeBoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active BOMs");
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetApprovedBOMsAsync()
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allBoms = await _bomRepository.GetAllAsync();
                var approvedBoms = allBoms.Where(b => b.ApprovalStatus == "APPROVED").ToList();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(approvedBoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting approved BOMs");
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetByTypeAsync(string bomType)
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allBoms = await _bomRepository.GetAllAsync();
                var typedBoms = allBoms.Where(b => b.BOMType == bomType).ToList();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(typedBoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BOMs by type {BomType}", bomType);
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetEffectiveBOMsAsync(DateTime effectiveDate)
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allBoms = await _bomRepository.GetAllAsync();
                var effectiveBoms = allBoms.Where(b => 
                    (!b.EffectiveFrom.HasValue || b.EffectiveFrom.Value <= effectiveDate) &&
                    (!b.EffectiveTo.HasValue || b.EffectiveTo.Value >= effectiveDate)).ToList();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(effectiveBoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting effective BOMs for date {Date}", effectiveDate);
                throw;
            }
        }

        public async Task<BillOfMaterialDto?> GetWithItemsAsync(long id)
        {
            try
            {
                var bom = await _bomRepository.GetWithItemsAsync(id);
                return bom != null ? _mapper.Map<BillOfMaterialDto>(bom) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BOM with items by ID {Id}", id);
                throw;
            }
        }

        public async Task<BillOfMaterialDto> ApproveBOMAsync(long id, long approvedByUserId)
        {
            return await ApproveAsync(id, approvedByUserId);
        }

        public async Task<BillOfMaterialDto> RejectBOMAsync(long id, string rejectionReason)
        {
            return await RejectAsync(id, rejectionReason);
        }

        public async Task<BillOfMaterialDto> ActivateBOMAsync(long id)
        {
            try
            {
                var bom = await _bomRepository.GetByIdAsync(id);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {id} not found");
                }

                if (bom.ApprovalStatus != "APPROVED")
                {
                    throw new InvalidOperationException("Only approved BOMs can be activated");
                }

                bom.IsActive = true;
                await _bomRepository.UpdateAsync(bom);

                _logger.LogInformation("BOM activated with ID {Id}", id);

                return _mapper.Map<BillOfMaterialDto>(bom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating BOM with ID {Id}", id);
                throw;
            }
        }

        public async Task<BillOfMaterialDto> DeactivateBOMAsync(long id)
        {
            try
            {
                var bom = await _bomRepository.GetByIdAsync(id);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {id} not found");
                }

                bom.IsActive = false;
                await _bomRepository.UpdateAsync(bom);

                _logger.LogInformation("BOM deactivated with ID {Id}", id);

                return _mapper.Map<BillOfMaterialDto>(bom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating BOM with ID {Id}", id);
                throw;
            }
        }

        public async Task<BillOfMaterialDto> CopyBOMAsync(long sourceBomId, string newBomCode, string newBomName)
        {
            try
            {
                var sourceBom = await _bomRepository.GetWithItemsAsync(sourceBomId);
                if (sourceBom == null)
                {
                    throw new InvalidOperationException($"Source BOM with ID {sourceBomId} not found");
                }

                // Check if new BOM code exists
                if (await _bomRepository.ExistsAsync(newBomCode))
                {
                    throw new InvalidOperationException($"BOM with code {newBomCode} already exists");
                }

                // Create new BOM
                var newBomDto = _mapper.Map<BillOfMaterialDto>(sourceBom);
                newBomDto.Id = 0; // New entity
                newBomDto.BOMCode = newBomCode;
                newBomDto.BOMName = newBomName;
                newBomDto.ApprovalStatus = "DRAFT";
                newBomDto.ApprovedByUserId = null;
                newBomDto.ApprovalDate = null;
                newBomDto.IsActive = false;

                // Clear IDs for items
                foreach (var item in newBomDto.BOMItems)
                {
                    item.Id = 0;
                }

                return await CreateAsync(newBomDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error copying BOM with ID {Id}", sourceBomId);
                throw;
            }
        }

        public async Task<BOMValidationResult> ValidateBOMAsync(long id)
        {
            return await ValidateAsync(id);
        }

        public async Task<BOMCostResult> CalculateBOMCostAsync(long id)
        {
            try
            {
                var costResult = await CalculateCostAsync(id);
                return new BOMCostResult
                {
                    MaterialCost = costResult.TotalMaterialCost,
                    LaborCost = 0, // Would be calculated based on routing
                    OverheadCost = costResult.TotalMaterialCost * 0.15m, // 15% overhead
                    TotalCost = costResult.TotalMaterialCost * 1.15m,
                    Currency = "TRY",
                    CalculationDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating BOM cost for ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BOMExplosionDto>> GetBOMExplosionAsync(long id, decimal quantity = 1)
        {
            try
            {
                var explosion = await ExplodeAsync(id, quantity);
                return explosion.Select(e => new BOMExplosionDto
                {
                    MaterialCardId = e.MaterialCardId,
                    MaterialCardCode = e.MaterialCardCode,
                    MaterialCardName = e.MaterialCardName,
                    RequiredQuantity = e.RequiredQuantity,
                    Unit = e.Unit,
                    Level = e.Level,
                    ComponentType = e.ComponentType,
                    UnitCost = 0, // Would be filled from material card
                    TotalCost = 0 // Would be calculated
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting BOM explosion for ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetExpiredBOMsAsync()
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allBoms = await _bomRepository.GetAllAsync();
                var expiredBoms = allBoms.Where(b => 
                    b.EffectiveTo.HasValue && b.EffectiveTo.Value < DateTime.Now).ToList();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(expiredBoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expired BOMs");
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetExpiringSoonAsync(int daysAhead = 30)
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allBoms = await _bomRepository.GetAllAsync();
                var cutoffDate = DateTime.Now.AddDays(daysAhead);
                var expiringSoonBoms = allBoms.Where(b => 
                    b.EffectiveTo.HasValue && 
                    b.EffectiveTo.Value >= DateTime.Now && 
                    b.EffectiveTo.Value <= cutoffDate).ToList();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(expiringSoonBoms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expiring BOMs");
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> GetPendingApprovalAsync()
        {
            try
            {
                var boms = await _bomRepository.GetPendingApprovalAsync();
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(boms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending approval BOMs");
                throw;
            }
        }

        public async Task<IEnumerable<BillOfMaterialDto>> SearchAsync(string searchTerm)
        {
            try
            {
                var boms = await _bomRepository.SearchAsync(searchTerm);
                return _mapper.Map<IEnumerable<BillOfMaterialDto>>(boms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching BOMs with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string bomCode)
        {
            try
            {
                return await _bomRepository.ExistsAsync(bomCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking BOM existence for code {BomCode}", bomCode);
                throw;
            }
        }

        #endregion

        #region Private Methods

        private async Task ValidateBOMItemsAsync(IEnumerable<BillOfMaterialItemDto> items)
        {
            foreach (var item in items)
            {
                if (item.LineNumber <= 0)
                {
                    throw new InvalidOperationException("Line number must be positive");
                }

                if (item.Quantity <= 0)
                {
                    throw new InvalidOperationException($"Quantity must be positive for line {item.LineNumber}");
                }

                if (item.ScrapFactor < 0 || item.ScrapFactor > 100)
                {
                    throw new InvalidOperationException($"Scrap factor must be between 0 and 100 for line {item.LineNumber}");
                }

                if (string.IsNullOrEmpty(item.Unit))
                {
                    throw new InvalidOperationException($"Unit cannot be empty for line {item.LineNumber}");
                }

                if (string.IsNullOrEmpty(item.ComponentType))
                {
                    throw new InvalidOperationException($"Component type cannot be empty for line {item.LineNumber}");
                }

                if (item.MaterialCardId <= 0)
                {
                    throw new InvalidOperationException($"Material card ID must be positive for line {item.LineNumber}");
                }

                // Check if material card exists
                var materialCard = await _materialCardRepository.GetByIdAsync(item.MaterialCardId);
                if (materialCard == null)
                {
                    throw new InvalidOperationException($"Material card with ID {item.MaterialCardId} not found for line {item.LineNumber}");
                }
            }

            // Check for duplicate line numbers
            var duplicateLines = items
                .GroupBy(x => x.LineNumber)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (duplicateLines.Any())
            {
                throw new InvalidOperationException($"Duplicate line numbers found: {string.Join(", ", duplicateLines)}");
            }
        }

        #endregion

        public async Task<IEnumerable<BOMCostTrendDto>> GetCostTrend(long bomId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var bom = await _bomRepository.GetByIdAsync(bomId);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {bomId} not found");
                }

                // Simplified cost trend - return current cost only for monolithic architecture
                var currentCost = await CalculateCostAsync(bomId);
                
                return new List<BOMCostTrendDto>
                {
                    new BOMCostTrendDto
                    {
                        Date = DateTime.Now,
                        TotalCost = currentCost.TotalCost,
                        MaterialCost = currentCost.MaterialCost,
                        LaborCost = currentCost.LaborCost,
                        OverheadCost = currentCost.OverheadCost
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cost trend for BOM {BomId}", bomId);
                throw;
            }
        }
    }
} 