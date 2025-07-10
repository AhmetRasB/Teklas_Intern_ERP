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
    /// Work Order Service - Monolithic Architecture
    /// </summary>
    public sealed class WorkOrderService
    {
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IBillOfMaterialRepository _bomRepository;
        private readonly IMaterialCardRepository _materialCardRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkOrderService> _logger;

        public WorkOrderService(
            IWorkOrderRepository workOrderRepository,
            IBillOfMaterialRepository bomRepository,
            IMaterialCardRepository materialCardRepository,
            IMapper mapper,
            ILogger<WorkOrderService> logger)
        {
            _workOrderRepository = workOrderRepository;
            _bomRepository = bomRepository;
            _materialCardRepository = materialCardRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Basic CRUD Operations

        public async Task<WorkOrderDto?> GetByIdAsync(long id)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                return workOrder != null ? _mapper.Map<WorkOrderDto>(workOrder) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work order by ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto?> GetByNumberAsync(string workOrderNumber)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByNumberAsync(workOrderNumber);
                return workOrder != null ? _mapper.Map<WorkOrderDto>(workOrder) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work order by number {WorkOrderNumber}", workOrderNumber);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetAllAsync()
        {
            try
            {
                var workOrders = await _workOrderRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all work orders");
                throw;
            }
        }

        public async Task<WorkOrderDto> CreateAsync(WorkOrderDto workOrderDto)
        {
            try
            {
                // Basic validation for monolithic approach
                if (workOrderDto == null)
                    throw new ArgumentNullException(nameof(workOrderDto));

                if (string.IsNullOrWhiteSpace(workOrderDto.Description))
                    throw new ArgumentException("Description is required", nameof(workOrderDto));

                // Check if work order number exists
                if (!string.IsNullOrEmpty(workOrderDto.WorkOrderNumber) && 
                    await _workOrderRepository.ExistsAsync(workOrderDto.WorkOrderNumber))
                {
                    throw new InvalidOperationException($"Work order with number {workOrderDto.WorkOrderNumber} already exists");
                }

                // Validate BOM exists and is approved
                var bom = await _bomRepository.GetByIdAsync(workOrderDto.BillOfMaterialId);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {workOrderDto.BillOfMaterialId} not found");
                }

                if (bom.ApprovalStatus != "APPROVED")
                {
                    throw new InvalidOperationException("Cannot create work order from non-approved BOM");
                }

                // Validate product material card
                var productCard = await _materialCardRepository.GetByIdAsync(workOrderDto.ProductMaterialCardId);
                if (productCard == null)
                {
                    throw new InvalidOperationException($"Product material card with ID {workOrderDto.ProductMaterialCardId} not found");
                }

                // Map and create
                var workOrderEntity = _mapper.Map<WorkOrder>(workOrderDto);
                workOrderEntity.WorkOrderNumber = string.IsNullOrEmpty(workOrderDto.WorkOrderNumber) ? 
                    await _workOrderRepository.GetNextWorkOrderNumberAsync() : workOrderDto.WorkOrderNumber;
                workOrderEntity.Status = "CREATED";

                var createdWorkOrder = await _workOrderRepository.AddAsync(workOrderEntity);
                
                _logger.LogInformation("Work order created successfully with ID {Id} and number {WorkOrderNumber}", 
                    createdWorkOrder.Id, createdWorkOrder.WorkOrderNumber);

                return _mapper.Map<WorkOrderDto>(createdWorkOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating work order");
                throw;
            }
        }

        public async Task<WorkOrderDto> UpdateAsync(WorkOrderDto workOrderDto)
        {
            try
            {
                // Validate DTO
                if (string.IsNullOrWhiteSpace(workOrderDto.Description))
                {
                    throw new ArgumentException("Description is required", nameof(workOrderDto));
                }

                // Check if work order exists
                var existingWorkOrder = await _workOrderRepository.GetByIdAsync(workOrderDto.Id);
                if (existingWorkOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {workOrderDto.Id} not found");
                }

                // Check if work order can be modified
                if (existingWorkOrder.Status == "COMPLETED" || existingWorkOrder.Status == "CANCELLED")
                {
                    throw new InvalidOperationException($"Cannot modify {existingWorkOrder.Status.ToLower()} work order");
                }

                // Map and update
                var workOrderEntity = _mapper.Map<WorkOrder>(workOrderDto);
                await _workOrderRepository.UpdateAsync(workOrderEntity);

                _logger.LogInformation("Work order updated successfully with ID {Id}", workOrderDto.Id);

                return await GetByIdAsync(workOrderDto.Id) ?? workOrderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating work order with ID {Id}", workOrderDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    return false;
                }

                // Check if work order can be deleted
                if (workOrder.Status == "IN_PROGRESS" || workOrder.Status == "COMPLETED")
                {
                    throw new InvalidOperationException($"Cannot delete {workOrder.Status.ToLower()} work order");
                }

                await _workOrderRepository.DeleteAsync(id);
                
                _logger.LogInformation("Work order deleted successfully with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting work order with ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region Business Operations

        public async Task<WorkOrderDto> ReleaseAsync(long id, long releasedByUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "CREATED")
                {
                    throw new InvalidOperationException($"Cannot release work order with status {workOrder.Status}");
                }

                // Validate work order can be released
                var validationResult = await ValidateForReleaseAsync(workOrder);
                if (!validationResult.IsValid)
                {
                    throw new InvalidOperationException($"Cannot release work order: {string.Join(", ", validationResult.Errors)}");
                }

                workOrder.Status = "RELEASED";
                workOrder.ReleasedByUserId = releasedByUserId;
                workOrder.ReleasedDate = DateTime.UtcNow;

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order released successfully with ID {Id} by user {UserId}", id, releasedByUserId);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error releasing work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> StartAsync(long id, long operatorUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "RELEASED")
                {
                    throw new InvalidOperationException($"Cannot start work order with status {workOrder.Status}");
                }

                workOrder.Status = "IN_PROGRESS";
                workOrder.ActualStartDate = DateTime.UtcNow;
                workOrder.SupervisorUserId = operatorUserId;

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order started successfully with ID {Id} by user {UserId}", id, operatorUserId);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> CompleteAsync(long id, long completedByUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "IN_PROGRESS")
                {
                    throw new InvalidOperationException($"Cannot complete work order with status {workOrder.Status}");
                }

                // Check if all required quantity is produced
                if (workOrder.CompletedQuantity + workOrder.ScrapQuantity < workOrder.PlannedQuantity)
                {
                    throw new InvalidOperationException("Cannot complete work order before all planned quantity is produced or scrapped");
                }

                workOrder.Status = "COMPLETED";
                workOrder.ActualEndDate = DateTime.UtcNow;
                workOrder.CompletionPercentage = 100;

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order completed successfully with ID {Id} by user {UserId}", id, completedByUserId);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> CancelAsync(long id, string reason, long cancelledByUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status == "COMPLETED" || workOrder.Status == "CANCELLED")
                {
                    throw new InvalidOperationException($"Cannot cancel work order with status {workOrder.Status}");
                }

                workOrder.Status = "CANCELLED";
                workOrder.Description = $"{workOrder.Description}\nCancellation Reason: {reason}";

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order cancelled with ID {Id} by user {UserId}. Reason: {Reason}", 
                    id, cancelledByUserId, reason);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> PutOnHoldAsync(long id, string reason, long heldByUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "RELEASED" && workOrder.Status != "IN_PROGRESS")
                {
                    throw new InvalidOperationException($"Cannot hold work order with status {workOrder.Status}");
                }

                workOrder.Status = "ON_HOLD";
                workOrder.Description = $"{workOrder.Description}\nHold Reason: {reason}";

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order put on hold with ID {Id} by user {UserId}. Reason: {Reason}", 
                    id, heldByUserId, reason);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error putting work order on hold with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> ResumeAsync(long id, long resumedByUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "ON_HOLD")
                {
                    throw new InvalidOperationException($"Cannot resume work order with status {workOrder.Status}");
                }

                // Determine previous status
                var newStatus = workOrder.ActualStartDate.HasValue ? "IN_PROGRESS" : "RELEASED";
                workOrder.Status = newStatus;

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order resumed with ID {Id} by user {UserId}", id, resumedByUserId);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resuming work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> CloseAsync(long id, long closedByUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "COMPLETED")
                {
                    throw new InvalidOperationException("Only completed work orders can be closed");
                }

                // Additional closing logic can be added here (stock movements, costing, etc.)

                _logger.LogInformation("Work order closed with ID {Id} by user {UserId}", id, closedByUserId);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> CreateFromBOMAsync(long bomId, decimal quantity, DateTime plannedStartDate, DateTime plannedEndDate)
        {
            try
            {
                var bom = await _bomRepository.GetByIdAsync(bomId);
                if (bom == null)
                {
                    throw new InvalidOperationException($"BOM with ID {bomId} not found");
                }

                if (bom.ApprovalStatus != "APPROVED")
                {
                    throw new InvalidOperationException("Cannot create work order from non-approved BOM");
                }

                var workOrderDto = new WorkOrderDto
                {
                    BillOfMaterialId = bomId,
                    ProductMaterialCardId = bom.ProductMaterialCardId,
                    PlannedQuantity = quantity,
                    PlannedStartDate = plannedStartDate,
                    PlannedEndDate = plannedEndDate,
                    WorkOrderType = "PRODUCTION",
                    Priority = 3,
                    RequiresQualityCheck = true
                };

                return await CreateAsync(workOrderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating work order from BOM {BomId}", bomId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> SplitAsync(long id, IEnumerable<decimal> quantities)
        {
            try
            {
                var originalWorkOrder = await _workOrderRepository.GetByIdAsync(id);
                if (originalWorkOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (originalWorkOrder.Status != "CREATED")
                {
                    throw new InvalidOperationException("Can only split work orders in CREATED status");
                }

                var splitQuantities = quantities.ToList();
                var totalSplitQuantity = splitQuantities.Sum();

                if (totalSplitQuantity != originalWorkOrder.PlannedQuantity)
                {
                    throw new InvalidOperationException("Total split quantities must equal original planned quantity");
                }

                var splitWorkOrders = new List<WorkOrderDto>();

                // Cancel original work order
                await CancelAsync(id, "Split into multiple work orders", originalWorkOrder.CreateUserId);

                // Create new work orders
                for (int i = 0; i < splitQuantities.Count; i++)
                {
                    var splitWorkOrderDto = _mapper.Map<WorkOrderDto>(originalWorkOrder);
                    splitWorkOrderDto.Id = 0; // New entity
                    splitWorkOrderDto.WorkOrderNumber = string.Empty; // Will be auto-generated
                    splitWorkOrderDto.PlannedQuantity = splitQuantities[i];
                    splitWorkOrderDto.Description = $"{originalWorkOrder.Description} (Split {i + 1}/{splitQuantities.Count})";

                    var createdSplitWorkOrder = await CreateAsync(splitWorkOrderDto);
                    splitWorkOrders.Add(createdSplitWorkOrder);
                }

                _logger.LogInformation("Work order split successfully. Original ID {Id} split into {Count} orders", 
                    id, splitQuantities.Count);

                return splitWorkOrders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error splitting work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> MergeAsync(IEnumerable<long> workOrderIds)
        {
            try
            {
                var workOrderIdList = workOrderIds.ToList();
                var workOrders = new List<WorkOrder>();

                foreach (var id in workOrderIdList)
                {
                    var workOrder = await _workOrderRepository.GetByIdAsync(id);
                    if (workOrder == null)
                    {
                        throw new InvalidOperationException($"Work order with ID {id} not found");
                    }

                    if (workOrder.Status != "CREATED")
                    {
                        throw new InvalidOperationException($"Can only merge work orders in CREATED status. Work order {id} is {workOrder.Status}");
                    }

                    workOrders.Add(workOrder);
                }

                // Validate all work orders have same BOM and product
                var firstWorkOrder = workOrders.First();
                if (workOrders.Any(wo => wo.BillOfMaterialId != firstWorkOrder.BillOfMaterialId ||
                                        wo.ProductMaterialCardId != firstWorkOrder.ProductMaterialCardId))
                {
                    throw new InvalidOperationException("Can only merge work orders with same BOM and product");
                }

                // Create merged work order
                var mergedWorkOrderDto = _mapper.Map<WorkOrderDto>(firstWorkOrder);
                mergedWorkOrderDto.Id = 0; // New entity
                mergedWorkOrderDto.WorkOrderNumber = string.Empty; // Will be auto-generated
                mergedWorkOrderDto.PlannedQuantity = workOrders.Sum(wo => wo.PlannedQuantity);
                mergedWorkOrderDto.Description = $"Merged from work orders: {string.Join(", ", workOrderIdList)}";

                var createdMergedWorkOrder = await CreateAsync(mergedWorkOrderDto);

                // Cancel original work orders
                foreach (var id in workOrderIdList)
                {
                    await CancelAsync(id, $"Merged into work order {createdMergedWorkOrder.WorkOrderNumber}", firstWorkOrder.CreateUserId);
                }

                _logger.LogInformation("Work orders merged successfully. {Count} orders merged into ID {Id}", 
                    workOrderIdList.Count, createdMergedWorkOrder.Id);

                return createdMergedWorkOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error merging work orders");
                throw;
            }
        }

        public async Task<WorkOrderDto> UpdateProgressAsync(long id, decimal completedQuantity, decimal scrapQuantity)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status != "IN_PROGRESS")
                {
                    throw new InvalidOperationException("Can only update progress for work orders in progress");
                }

                if (completedQuantity + scrapQuantity > workOrder.PlannedQuantity)
                {
                    throw new InvalidOperationException("Total completed and scrap quantity cannot exceed planned quantity");
                }

                workOrder.CompletedQuantity = completedQuantity;
                workOrder.ScrapQuantity = scrapQuantity;
                workOrder.CompletionPercentage = (completedQuantity + scrapQuantity) / workOrder.PlannedQuantity * 100;

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order progress updated. ID {Id}, Completed: {Completed}, Scrap: {Scrap}", 
                    id, completedQuantity, scrapQuantity);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating progress for work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderDto> RescheduleAsync(long id, DateTime newStartDate, DateTime newEndDate)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                if (workOrder.Status == "COMPLETED" || workOrder.Status == "CANCELLED")
                {
                    throw new InvalidOperationException($"Cannot reschedule {workOrder.Status.ToLower()} work order");
                }

                if (newEndDate <= newStartDate)
                {
                    throw new InvalidOperationException("End date must be after start date");
                }

                workOrder.PlannedStartDate = newStartDate;
                workOrder.PlannedEndDate = newEndDate;

                await _workOrderRepository.UpdateAsync(workOrder);

                _logger.LogInformation("Work order rescheduled. ID {Id}, New dates: {StartDate} - {EndDate}", 
                    id, newStartDate, newEndDate);

                return _mapper.Map<WorkOrderDto>(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rescheduling work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderValidationResult> ValidateAsync(long id)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    return new WorkOrderValidationResult
                    {
                        IsValid = false,
                        Errors = new List<string> { "Work order not found" }
                    };
                }

                var errors = new List<string>();
                var warnings = new List<string>();

                // Validate BOM
                var bom = await _bomRepository.GetByIdAsync(workOrder.BillOfMaterialId);
                if (bom == null)
                {
                    errors.Add("Associated BOM not found");
                }
                else if (bom.ApprovalStatus != "APPROVED")
                {
                    errors.Add("Associated BOM is not approved");
                }

                // Validate dates
                if (workOrder.PlannedEndDate <= workOrder.PlannedStartDate)
                {
                    errors.Add("Planned end date must be after start date");
                }

                if (workOrder.DueDate.HasValue && workOrder.DueDate.Value < workOrder.PlannedStartDate)
                {
                    warnings.Add("Due date is before planned start date");
                }

                // Validate quantities
                if (workOrder.CompletedQuantity + workOrder.ScrapQuantity > workOrder.PlannedQuantity)
                {
                    errors.Add("Total completed and scrap quantity exceeds planned quantity");
                }

                return new WorkOrderValidationResult
                {
                    IsValid = !errors.Any(),
                    Errors = errors,
                    Warnings = warnings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating work order with ID {Id}", id);
                throw;
            }
        }

        public async Task<WorkOrderCostResult> CalculateCostAsync(long id)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(id);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {id} not found");
                }

                decimal materialCost = 0;
                decimal laborCost = 0;

                // Get BOM and calculate material costs
                var bom = await _bomRepository.GetWithItemsAsync(workOrder.BillOfMaterialId);
                if (bom != null)
                {
                    foreach (var item in bom.BOMItems)
                    {
                        var materialCard = await _materialCardRepository.GetByIdAsync(item.MaterialCardId);
                        if (materialCard != null)
                        {
                            var requiredQuantity = item.Quantity * workOrder.PlannedQuantity;
                            materialCost += requiredQuantity * (materialCard.SalesPrice ?? 0);
                        }
                    }
                }

                // Simplified labor cost calculation
                var estimatedHours = (workOrder.PlannedEndDate - workOrder.PlannedStartDate).TotalHours;
                laborCost = (decimal)(estimatedHours * 50); // 50 TRY per hour average

                var overheadCost = (materialCost + laborCost) * 0.15m; // 15% overhead
                var totalCost = materialCost + laborCost + overheadCost;

                return new WorkOrderCostResult
                {
                    MaterialCost = materialCost,
                    LaborCost = laborCost,
                    OverheadCost = overheadCost,
                    TotalCost = totalCost,
                    Currency = "TRY",
                    CalculationDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cost for work order with ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region Query Operations

        public async Task<IEnumerable<WorkOrderDto>> GetByBOMAsync(long bomId)
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allWorkOrders = await _workOrderRepository.GetAllAsync();
                var bomWorkOrders = allWorkOrders.Where(w => w.BillOfMaterialId == bomId).ToList();
                return _mapper.Map<IEnumerable<WorkOrderDto>>(bomWorkOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by BOM {BomId}", bomId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetByProductAsync(long productMaterialCardId)
        {
            try
            {
                var workOrders = await _workOrderRepository.GetByProductMaterialCardAsync(productMaterialCardId);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by product {ProductId}", productMaterialCardId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetByStatusAsync(string status)
        {
            try
            {
                var workOrders = await _workOrderRepository.GetByStatusAsync(status);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by status {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetByPriorityAsync(int priority)
        {
            try
            {
                var workOrders = await _workOrderRepository.GetByPriorityAsync(priority);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by priority {Priority}", priority);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allWorkOrders = await _workOrderRepository.GetAllAsync();
                var dateRangeWorkOrders = allWorkOrders.Where(w => 
                    w.PlannedStartDate >= startDate && w.PlannedStartDate <= endDate).ToList();
                return _mapper.Map<IEnumerable<WorkOrderDto>>(dateRangeWorkOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by date range");
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetOverdueAsync()
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allWorkOrders = await _workOrderRepository.GetAllAsync();
                var overdueWorkOrders = allWorkOrders.Where(w => 
                    w.PlannedEndDate < DateTime.Now && 
                    (w.Status == "RELEASED" || w.Status == "IN_PROGRESS")).ToList();
                return _mapper.Map<IEnumerable<WorkOrderDto>>(overdueWorkOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue work orders");
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetByWorkCenterAsync(string workCenter)
        {
            try
            {
                var workOrders = await _workOrderRepository.GetByWorkCenterAsync(workCenter);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by work center {WorkCenter}", workCenter);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetByShiftAsync(string shift)
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allWorkOrders = await _workOrderRepository.GetAllAsync();
                var shiftWorkOrders = allWorkOrders.Where(w => w.Shift == shift).ToList();
                return _mapper.Map<IEnumerable<WorkOrderDto>>(shiftWorkOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by shift {Shift}", shift);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetBySupervisorAsync(long supervisorUserId)
        {
            try
            {
                var workOrders = await _workOrderRepository.GetBySupervisorAsync(supervisorUserId);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by supervisor {SupervisorId}", supervisorUserId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> GetBySourceTypeAsync(string sourceType)
        {
            try
            {
                var workOrders = await _workOrderRepository.GetBySourceTypeAsync(sourceType);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by source type {SourceType}", sourceType);
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrderDto>> SearchAsync(string searchTerm)
        {
            try
            {
                var workOrders = await _workOrderRepository.SearchAsync(searchTerm);
                return _mapper.Map<IEnumerable<WorkOrderDto>>(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching work orders with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<WorkOrderCompletionStatistics> GetCompletionStatisticsAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Simplified for monolithic - get all and calculate
                var allWorkOrders = await _workOrderRepository.GetAllAsync();
                var filteredWorkOrders = allWorkOrders.AsEnumerable();
                
                if (startDate.HasValue)
                    filteredWorkOrders = filteredWorkOrders.Where(w => w.CreateDate >= startDate.Value);
                if (endDate.HasValue)
                    filteredWorkOrders = filteredWorkOrders.Where(w => w.CreateDate <= endDate.Value);

                var workOrdersList = filteredWorkOrders.ToList();

                return new WorkOrderCompletionStatistics
                {
                    TotalWorkOrders = workOrdersList.Count,
                    CompletedWorkOrders = workOrdersList.Count(w => w.Status == "COMPLETED"),
                    InProgressWorkOrders = workOrdersList.Count(w => w.Status == "IN_PROGRESS"),
                    PendingWorkOrders = workOrdersList.Count(w => w.Status == "CREATED" || w.Status == "RELEASED"),
                    CancelledWorkOrders = workOrdersList.Count(w => w.Status == "CANCELLED"),
                    OverdueWorkOrders = workOrdersList.Count(w => w.PlannedEndDate < DateTime.Now && w.Status != "COMPLETED"),
                    AverageCompletionTime = 0, // Would require complex calculation
                    OnTimeDeliveryRate = 0, // Would require complex calculation
                    AverageLeadTime = 0 // Would require complex calculation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting completion statistics");
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string workOrderNumber)
        {
            try
            {
                return await _workOrderRepository.ExistsAsync(workOrderNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking work order existence for number {WorkOrderNumber}", workOrderNumber);
                throw;
            }
        }

        #endregion

        #region Private Methods

        private async Task<WorkOrderValidationResult> ValidateForReleaseAsync(WorkOrder workOrder)
        {
            var errors = new List<string>();

            // Check BOM is approved
            var bom = await _bomRepository.GetByIdAsync(workOrder.BillOfMaterialId);
            if (bom == null || bom.ApprovalStatus != "APPROVED")
            {
                errors.Add("Associated BOM must be approved");
            }

            // Check planned dates are valid
            if (workOrder.PlannedStartDate >= workOrder.PlannedEndDate)
            {
                errors.Add("Planned end date must be after start date");
            }

            // Check planned quantity is positive
            if (workOrder.PlannedQuantity <= 0)
            {
                errors.Add("Planned quantity must be positive");
            }

            return new WorkOrderValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        #endregion
    }
} 