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
    /// Production Confirmation Service - Monolithic Architecture
    /// </summary>
    public sealed class ProductionConfirmationService
    {
        private readonly IProductionConfirmationRepository _confirmationRepository;
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IBillOfMaterialRepository _bomRepository; // Add missing repository
        private readonly IMaterialCardRepository _materialCardRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductionConfirmationService> _logger;

        public ProductionConfirmationService(
            IProductionConfirmationRepository confirmationRepository,
            IWorkOrderRepository workOrderRepository,
            IBillOfMaterialRepository bomRepository, // Add to constructor
            IMaterialCardRepository materialCardRepository,
            IMapper mapper,
            ILogger<ProductionConfirmationService> logger)
        {
            _confirmationRepository = confirmationRepository;
            _workOrderRepository = workOrderRepository;
            _bomRepository = bomRepository; // Initialize
            _materialCardRepository = materialCardRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region Basic CRUD Operations

        public async Task<ProductionConfirmationDto?> GetByIdAsync(long id)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                return confirmation != null ? _mapper.Map<ProductionConfirmationDto>(confirmation) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production confirmation by ID {Id}", id);
                throw;
            }
        }

        public async Task<ProductionConfirmationDto?> GetByNumberAsync(string confirmationNumber)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByNumberAsync(confirmationNumber);
                return confirmation != null ? _mapper.Map<ProductionConfirmationDto>(confirmation) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production confirmation by number {ConfirmationNumber}", confirmationNumber);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetAllAsync()
        {
            try
            {
                var confirmations = await _confirmationRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all production confirmations");
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> CreateAsync(ProductionConfirmationDto confirmationDto)
        {
            try
            {
                // Validate DTO
                if (confirmationDto.ConfirmedQuantity < 0)
                {
                    throw new InvalidOperationException("Confirmed quantity cannot be negative");
                }
                if (confirmationDto.ScrapQuantity < 0)
                {
                    throw new InvalidOperationException("Scrap quantity cannot be negative");
                }

                // Check if confirmation number exists
                if (!string.IsNullOrEmpty(confirmationDto.ConfirmationNumber) && 
                    await _confirmationRepository.ExistsAsync(confirmationDto.ConfirmationNumber))
                {
                    throw new InvalidOperationException($"Production confirmation with number {confirmationDto.ConfirmationNumber} already exists");
                }

                // Validate work order exists and is in progress
                var workOrder = await _workOrderRepository.GetByIdAsync(confirmationDto.WorkOrderId);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {confirmationDto.WorkOrderId} not found");
                }

                if (workOrder.Status != "IN_PROGRESS" && workOrder.Status != "RELEASED")
                {
                    throw new InvalidOperationException($"Cannot create confirmation for work order with status {workOrder.Status}");
                }

                // Map and create
                var confirmationEntity = _mapper.Map<ProductionConfirmation>(confirmationDto);
                confirmationEntity.ConfirmationNumber = string.IsNullOrEmpty(confirmationDto.ConfirmationNumber) ? 
                    await _confirmationRepository.GetNextConfirmationNumberAsync() : confirmationDto.ConfirmationNumber;
                confirmationEntity.Status = "PENDING";

                var createdConfirmation = await _confirmationRepository.AddAsync(confirmationEntity);
                
                _logger.LogInformation("Production confirmation created successfully with ID {Id} and number {ConfirmationNumber}", 
                    createdConfirmation.Id, createdConfirmation.ConfirmationNumber);

                return _mapper.Map<ProductionConfirmationDto>(createdConfirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating production confirmation");
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> UpdateAsync(ProductionConfirmationDto confirmationDto)
        {
            try
            {
                // Validate DTO
                if (confirmationDto.ConfirmedQuantity < 0)
                {
                    throw new InvalidOperationException("Confirmed quantity cannot be negative");
                }
                if (confirmationDto.ScrapQuantity < 0)
                {
                    throw new InvalidOperationException("Scrap quantity cannot be negative");
                }

                // Check if confirmation exists
                var existingConfirmation = await _confirmationRepository.GetByIdAsync(confirmationDto.Id);
                if (existingConfirmation == null)
                {
                    throw new InvalidOperationException($"Production confirmation with ID {confirmationDto.Id} not found");
                }

                // Check if confirmation can be modified
                if (existingConfirmation.Status == "POSTED")
                {
                    throw new InvalidOperationException("Cannot modify posted production confirmation");
                }

                // Map and update
                var confirmationEntity = _mapper.Map<ProductionConfirmation>(confirmationDto);
                await _confirmationRepository.UpdateAsync(confirmationEntity);

                _logger.LogInformation("Production confirmation updated successfully with ID {Id}", confirmationDto.Id);

                return await GetByIdAsync(confirmationDto.Id) ?? confirmationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating production confirmation with ID {Id}", confirmationDto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    return false;
                }

                // Check if confirmation can be deleted
                if (confirmation.Status == "POSTED")
                {
                    throw new InvalidOperationException("Cannot delete posted production confirmation");
                }

                await _confirmationRepository.DeleteAsync(id);
                
                _logger.LogInformation("Production confirmation deleted successfully with ID {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting production confirmation with ID {Id}", id);
                throw;
            }
        }

        #endregion

        #region Business Operations

        public async Task<ProductionConfirmationDto> ConfirmProductionAsync(long workOrderId, decimal confirmedQuantity, decimal scrapQuantity, long operatorUserId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {workOrderId} not found");
                }

                if (workOrder.Status != "IN_PROGRESS")
                {
                    throw new InvalidOperationException($"Cannot confirm production for work order with status {workOrder.Status}");
                }

                // Validate quantities
                var totalConfirmedSoFar = workOrder.CompletedQuantity + workOrder.ScrapQuantity;
                if (totalConfirmedSoFar + confirmedQuantity + scrapQuantity > workOrder.PlannedQuantity)
                {
                    throw new InvalidOperationException("Total confirmed quantity would exceed planned quantity");
                }

                var confirmationDto = new ProductionConfirmationDto
                {
                    WorkOrderId = workOrderId,
                    ConfirmedQuantity = confirmedQuantity,
                    ScrapQuantity = scrapQuantity,
                    OperatorUserId = operatorUserId,
                    ConfirmationDate = DateTime.UtcNow,
                    ActivityType = "PRODUCTION",
                    Status = "PENDING"
                };

                var createdConfirmation = await CreateAsync(confirmationDto);

                // Update work order progress
                await UpdateWorkOrderProgressAsync(workOrderId, confirmedQuantity, scrapQuantity);

                _logger.LogInformation("Production confirmed for work order {WorkOrderId}. Quantity: {Quantity}, Scrap: {Scrap}", 
                    workOrderId, confirmedQuantity, scrapQuantity);

                return createdConfirmation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming production for work order {WorkOrderId}", workOrderId);
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> ProcessMaterialConsumptionAsync(long id, IEnumerable<MaterialConsumptionDto> consumptions)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    throw new InvalidOperationException($"Production confirmation with ID {id} not found");
                }

                // Simplified for monolithic - just mark as material consumed
                confirmation.MaterialConsumed = true;
                // Remove UpdatedAt reference as it might not exist in AuditEntity

                await _confirmationRepository.UpdateAsync(confirmation);
                
                _logger.LogInformation("Material consumption processed for confirmation {Id}", id);
                
                return _mapper.Map<ProductionConfirmationDto>(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing material consumption for confirmation {Id}", id);
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> RecordQualityCheckAsync(long id, QualityCheckDto qualityCheck)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    throw new InvalidOperationException($"Production confirmation with ID {id} not found");
                }

                // Simplified quality check recording
                confirmation.QualityStatus = qualityCheck.Status;
                confirmation.QualityNotes = qualityCheck.Notes;
                confirmation.QualityCheckResult = qualityCheck.Result;
                confirmation.RequiresQualityCheck = false; // Mark as checked
                // Remove UpdatedAt reference

                await _confirmationRepository.UpdateAsync(confirmation);
                
                _logger.LogInformation("Quality check recorded for confirmation {Id}", id);
                
                return _mapper.Map<ProductionConfirmationDto>(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording quality check for confirmation {Id}", id);
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> PostAsync(long id, long postedByUserId)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    throw new InvalidOperationException($"Production confirmation with ID {id} not found");
                }

                if (confirmation.Status == "POSTED")
                {
                    throw new InvalidOperationException("Production confirmation is already posted");
                }

                // Validate confirmation can be posted
                var validationResult = await ValidateForPostingAsync(confirmation);
                if (!validationResult.IsValid)
                {
                    throw new InvalidOperationException($"Cannot post confirmation: {string.Join(", ", validationResult.Errors)}");
                }

                confirmation.Status = "POSTED";
                confirmation.PostedByUserId = postedByUserId;
                confirmation.PostedDate = DateTime.UtcNow;

                await _confirmationRepository.UpdateAsync(confirmation);

                // Process stock movements, cost postings, etc. (simplified)
                await ProcessPostingEffectsAsync(confirmation);

                _logger.LogInformation("Production confirmation posted with ID {Id} by user {UserId}", id, postedByUserId);

                return _mapper.Map<ProductionConfirmationDto>(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting production confirmation with ID {Id}", id);
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> ReverseAsync(long id, string reason, long reversedByUserId)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    throw new InvalidOperationException($"Production confirmation with ID {id} not found");
                }

                if (confirmation.Status != "POSTED")
                {
                    throw new InvalidOperationException("Only posted confirmations can be reversed");
                }

                confirmation.Status = "REVERSED";
                confirmation.ReversalReason = reason;

                await _confirmationRepository.UpdateAsync(confirmation);

                // Reverse stock movements, cost postings, etc. (simplified)
                await ProcessReversalEffectsAsync(confirmation);

                _logger.LogInformation("Production confirmation reversed with ID {Id} by user {UserId}. Reason: {Reason}", 
                    id, reversedByUserId, reason);

                return _mapper.Map<ProductionConfirmationDto>(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reversing production confirmation with ID {Id}", id);
                throw;
            }
        }

        public async Task<LaborHoursResult> CalculateLaborHoursAsync(long id)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    throw new InvalidOperationException($"Production confirmation with ID {id} not found");
                }

                var result = new LaborHoursResult
                {
                    ConfirmationId = id,
                    Details = new List<LaborHoursDetailDto>()
                };

                // Calculate based on start and end times
                if (confirmation.StartTime.HasValue && confirmation.EndTime.HasValue)
                {
                    var totalTime = confirmation.EndTime.Value - confirmation.StartTime.Value;
                    result.TotalHours = (decimal)totalTime.TotalHours;

                    // Simple calculation - in real system would be more complex
                    result.SetupHours = confirmation.SetupTime ?? 0;
                    result.RunHours = confirmation.RunTime ?? 0;
                    result.WaitHours = confirmation.WaitTime ?? 0;
                    result.BreakHours = result.TotalHours - result.SetupHours - result.RunHours - result.WaitHours;

                    if (result.BreakHours < 0) result.BreakHours = 0;

                    result.Details.Add(new LaborHoursDetailDto
                    {
                        Activity = "Total Production Time",
                        StartTime = confirmation.StartTime.Value,
                        EndTime = confirmation.EndTime.Value,
                        Hours = result.TotalHours
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating labor hours for confirmation {Id}", id);
                throw;
            }
        }

        public async Task<ProductionConfirmationValidationResult> ValidateAsync(long id)
        {
            try
            {
                var confirmation = await _confirmationRepository.GetByIdAsync(id);
                if (confirmation == null)
                {
                    return new ProductionConfirmationValidationResult
                    {
                        IsValid = false,
                        Errors = new List<string> { "Production confirmation not found" }
                    };
                }

                var errors = new List<string>();
                var warnings = new List<string>();

                // Validate work order
                var workOrder = await _workOrderRepository.GetByIdAsync(confirmation.WorkOrderId);
                if (workOrder == null)
                {
                    errors.Add("Associated work order not found");
                }

                // Validate quantities
                if (confirmation.ConfirmedQuantity < 0)
                {
                    errors.Add("Confirmed quantity cannot be negative");
                }

                if (confirmation.ScrapQuantity < 0)
                {
                    errors.Add("Scrap quantity cannot be negative");
                }

                // Validate times
                if (confirmation.StartTime.HasValue && confirmation.EndTime.HasValue &&
                    confirmation.EndTime.Value <= confirmation.StartTime.Value)
                {
                    errors.Add("End time must be after start time");
                }

                // Check if quality check is required but not completed
                if (confirmation.RequiresQualityCheck && string.IsNullOrEmpty(confirmation.QualityCheckResult))
                {
                    warnings.Add("Quality check is required but not completed");
                }

                return new ProductionConfirmationValidationResult
                {
                    IsValid = !errors.Any(),
                    Errors = errors,
                    Warnings = warnings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating production confirmation with ID {Id}", id);
                throw;
            }
        }

        public async Task<ProductionConfirmationDto> AutoConfirmAsync(long workOrderId)
        {
            try
            {
                var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
                if (workOrder == null)
                {
                    throw new InvalidOperationException($"Work order with ID {workOrderId} not found");
                }

                if (workOrder.Status != "IN_PROGRESS")
                {
                    throw new InvalidOperationException($"Cannot auto-confirm for work order with status {workOrder.Status}");
                }

                // Calculate remaining quantity to confirm
                var remainingQuantity = workOrder.PlannedQuantity - workOrder.CompletedQuantity - workOrder.ScrapQuantity;

                if (remainingQuantity <= 0)
                {
                    throw new InvalidOperationException("No remaining quantity to confirm");
                }

                var confirmationDto = new ProductionConfirmationDto
                {
                    WorkOrderId = workOrderId,
                    ConfirmedQuantity = remainingQuantity,
                    ScrapQuantity = 0,
                    OperatorUserId = workOrder.SupervisorUserId ?? workOrder.CreateUserId,
                    ConfirmationDate = DateTime.UtcNow,
                    ActivityType = "AUTO_CONFIRMATION",
                    Status = "PENDING"
                };

                var createdConfirmation = await CreateAsync(confirmationDto);

                _logger.LogInformation("Auto-confirmation created for work order {WorkOrderId}. Quantity: {Quantity}", 
                    workOrderId, remainingQuantity);

                return createdConfirmation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating auto-confirmation for work order {WorkOrderId}", workOrderId);
                throw;
            }
        }

        #endregion

        #region Query Operations

        public async Task<IEnumerable<ProductionConfirmationDto>> GetByWorkOrderAsync(long workOrderId)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByWorkOrderAsync(workOrderId);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by work order {WorkOrderId}", workOrderId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetByOperatorAsync(long operatorUserId)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByOperatorAsync(operatorUserId);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by operator {OperatorId}", operatorUserId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByDateRangeAsync(startDate, endDate);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by date range {StartDate} - {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetByShiftAsync(string shift)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByShiftAsync(shift);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by shift {Shift}", shift);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetByWorkCenterAsync(string workCenter)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByWorkCenterAsync(workCenter);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by work center {WorkCenter}", workCenter);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetByStatusAsync(string status)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByStatusAsync(status);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by status {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> GetPendingAsync()
        {
            try
            {
                // Simplified for monolithic - get all and filter
                var allConfirmations = await _confirmationRepository.GetAllAsync();
                var pendingConfirmations = allConfirmations.Where(c => c.Status == "DRAFT" || c.Status == "PENDING").ToList();
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(pendingConfirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending confirmations");
                throw;
            }
        }

        public async Task<IEnumerable<ProductionConfirmationDto>> SearchAsync(string searchTerm)
        {
            try
            {
                var confirmations = await _confirmationRepository.SearchAsync(searchTerm);
                return _mapper.Map<IEnumerable<ProductionConfirmationDto>>(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching confirmations with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string confirmationNumber)
        {
            try
            {
                return await _confirmationRepository.ExistsAsync(confirmationNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking confirmation existence for number {ConfirmationNumber}", confirmationNumber);
                throw;
            }
        }

        #endregion

        #region Reporting Operations

        public async Task<ProductionSummaryDto> GetProductionSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Simplified for monolithic
                var allConfirmations = await _confirmationRepository.GetAllAsync();
                
                // Filter by date range if provided
                if (startDate.HasValue)
                    allConfirmations = allConfirmations.Where(c => c.ConfirmationDate >= startDate.Value).ToList();
                if (endDate.HasValue)
                    allConfirmations = allConfirmations.Where(c => c.ConfirmationDate <= endDate.Value).ToList();

                var confirmationsList = allConfirmations.ToList();

                return new ProductionSummaryDto
                {
                    TotalQuantityProduced = confirmationsList.Sum(c => c.ConfirmedQuantity),
                    TotalScrapQuantity = confirmationsList.Sum(c => c.ScrapQuantity),
                    TotalReworkQuantity = confirmationsList.Sum(c => c.ReworkQuantity),
                    TotalConfirmations = confirmationsList.Count,
                    AverageEfficiency = 85.0m, // Simplified calculation
                    QualityRate = 95.0m, // Simplified calculation
                    ReportDate = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production summary");
                throw;
            }
        }

        public async Task<IEnumerable<OperatorEfficiencyDto>> GetOperatorEfficiencyAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Use default dates if not provided
                var start = startDate ?? DateTime.Now.AddDays(-30);
                var end = endDate ?? DateTime.Now;
                
                // Simplified for monolithic
                var allConfirmations = await _confirmationRepository.GetAllAsync();
                var filteredConfirmations = allConfirmations
                    .Where(c => c.ConfirmationDate >= start && c.ConfirmationDate <= end)
                    .ToList();

                var operatorGroups = filteredConfirmations
                    .Where(c => c.OperatorUserId.HasValue)
                    .GroupBy(c => c.OperatorUserId.Value);

                var efficiencyResults = new List<OperatorEfficiencyDto>();

                foreach (var group in operatorGroups)
                {
                    var confirmations = group.ToList();
                    efficiencyResults.Add(new OperatorEfficiencyDto
                    {
                        OperatorUserId = group.Key,
                        OperatorName = $"Operator {group.Key}", // Would fetch from user service
                        TotalQuantityProduced = confirmations.Sum(c => c.ConfirmedQuantity),
                        TotalScrapQuantity = confirmations.Sum(c => c.ScrapQuantity),
                        EfficiencyPercentage = 85.0m, // Simplified calculation
                        QualityRate = 95.0m, // Simplified calculation
                        TotalQuantity = confirmations.Sum(c => c.ConfirmedQuantity),
                        TotalTime = 0, // Simplified
                        AverageEfficiency = 85.0m,
                        ConfirmationCount = confirmations.Count
                    });
                }

                return efficiencyResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting operator efficiency");
                throw;
            }
        }

        public async Task<IEnumerable<ScrapAnalysisDto>> GetScrapAnalysisAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Use default dates if not provided
                var start = startDate ?? DateTime.Now.AddDays(-30);
                var end = endDate ?? DateTime.Now;
                
                // Simplified for monolithic
                var allConfirmations = await _confirmationRepository.GetAllAsync();
                var scrapConfirmations = allConfirmations
                    .Where(c => c.ConfirmationDate >= start && c.ConfirmationDate <= end && c.ScrapQuantity > 0)
                    .ToList();

                var scrapGroups = scrapConfirmations.GroupBy(c => c.WorkCenter ?? "Unknown");

                var scrapResults = new List<ScrapAnalysisDto>();

                foreach (var group in scrapGroups)
                {
                    var confirmations = group.ToList();
                    scrapResults.Add(new ScrapAnalysisDto
                    {
                        WorkCenter = group.Key,
                        TotalScrapQuantity = confirmations.Sum(c => c.ScrapQuantity),
                        ScrapRate = 5.0m, // Simplified calculation
                        MainScrapReason = "Quality Issues" // Simplified
                    });
                }

                return scrapResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scrap analysis");
                throw;
            }
        }

        public async Task<IEnumerable<ProductionTrendDto>> GetProductionTrendAsync(DateTime startDate, DateTime endDate, string groupBy = "DAY")
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByDateRangeAsync(startDate, endDate);
                
                var groupedData = groupBy.ToUpper() switch
                {
                    "HOUR" => confirmations.GroupBy(c => new DateTime(c.ConfirmationDate.Year, c.ConfirmationDate.Month, c.ConfirmationDate.Day, c.ConfirmationDate.Hour, 0, 0)),
                    "DAY" => confirmations.GroupBy(c => c.ConfirmationDate.Date),
                    "WEEK" => confirmations.GroupBy(c => c.ConfirmationDate.Date.AddDays(-(int)c.ConfirmationDate.DayOfWeek)),
                    "MONTH" => confirmations.GroupBy(c => new DateTime(c.ConfirmationDate.Year, c.ConfirmationDate.Month, 1)),
                    _ => confirmations.GroupBy(c => c.ConfirmationDate.Date)
                };

                var trends = groupedData.Select(g => new ProductionTrendDto
                {
                    Date = g.Key,
                    Period = g.Key.ToString("yyyy-MM-dd"), // Convert DateTime to string
                    ProducedQuantity = g.Sum(c => c.ConfirmedQuantity),
                    TotalQuantity = g.Sum(c => c.ConfirmedQuantity),
                    ScrapQuantity = g.Sum(c => c.ScrapQuantity),
                    EfficiencyPercentage = g.Any() ? g.Sum(c => c.ConfirmedQuantity) / (g.Sum(c => c.ConfirmedQuantity) + g.Sum(c => c.ScrapQuantity)) * 100 : 0,
                    CompletedWorkOrders = g.Select(c => c.WorkOrderId).Distinct().Count()
                }).OrderBy(t => t.Date).ToList();

                return trends;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production trend");
                throw;
            }
        }

        public async Task<IEnumerable<WorkCenterUtilizationDto>> GetWorkCenterUtilizationAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var confirmations = await _confirmationRepository.GetByDateRangeAsync(
                    startDate ?? DateTime.Today.AddDays(-30), 
                    endDate ?? DateTime.Today);

                var utilizationData = confirmations
                    .Where(c => !string.IsNullOrEmpty(c.WorkCenter))
                    .GroupBy(c => c.WorkCenter)
                    .Select(g => new WorkCenterUtilizationDto
                    {
                        WorkCenter = g.Key,
                        TotalHours = g.Sum(c => (decimal)(c.EndTime?.Subtract(c.StartTime ?? DateTime.Now).TotalHours ?? 0)),
                        ProductiveHours = g.Sum(c => c.RunTime ?? 0),
                        WaitHours = g.Sum(c => c.WaitTime ?? 0),
                        BreakHours = g.Sum(c => (decimal)(c.EndTime?.Subtract(c.StartTime ?? DateTime.Now).TotalHours ?? 0)) - g.Sum(c => (c.RunTime ?? 0) + (c.WaitTime ?? 0)),
                        WorkOrdersCompleted = g.Select(c => c.WorkOrderId).Distinct().Count()
                    })
                    .ToList();

                // Calculate utilization percentage
                foreach (var item in utilizationData)
                {
                    item.UtilizationPercentage = item.TotalHours > 0 ? (item.ProductiveHours / item.TotalHours) * 100 : 0;
                    if (item.BreakHours < 0) item.BreakHours = 0;
                }

                return utilizationData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work center utilization");
                throw;
            }
        }

        #endregion

        #region Private Methods

        private async Task UpdateWorkOrderProgressAsync(long workOrderId, decimal additionalCompleted, decimal additionalScrap)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
            if (workOrder != null)
            {
                workOrder.CompletedQuantity += additionalCompleted;
                workOrder.ScrapQuantity += additionalScrap;
                workOrder.CompletionPercentage = (workOrder.CompletedQuantity + workOrder.ScrapQuantity) / workOrder.PlannedQuantity * 100;

                // Check if work order is completed
                if (workOrder.CompletedQuantity + workOrder.ScrapQuantity >= workOrder.PlannedQuantity)
                {
                    workOrder.Status = "COMPLETED";
                    workOrder.ActualEndDate = DateTime.UtcNow;
                }

                await _workOrderRepository.UpdateAsync(workOrder);
            }
        }

        private async Task<ProductionConfirmationValidationResult> ValidateForPostingAsync(ProductionConfirmation confirmation)
        {
            var errors = new List<string>();

            // Basic validations
            if (confirmation.ConfirmedQuantity <= 0 && confirmation.ScrapQuantity <= 0)
            {
                errors.Add("Either confirmed quantity or scrap quantity must be positive");
            }

            // Check if quality check is required and completed
            if (confirmation.RequiresQualityCheck && string.IsNullOrEmpty(confirmation.QualityCheckResult))
            {
                errors.Add("Quality check is required but not completed");
            }

            // Check work order status
            var workOrder = await _workOrderRepository.GetByIdAsync(confirmation.WorkOrderId);
            if (workOrder == null)
            {
                errors.Add("Associated work order not found");
            }
            else if (workOrder.Status != "IN_PROGRESS")
            {
                errors.Add($"Cannot post confirmation for work order with status {workOrder.Status}");
            }

            return new ProductionConfirmationValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        private async Task ProcessPostingEffectsAsync(ProductionConfirmation confirmation)
        {
            // This would include:
            // 1. Stock movements (increase finished goods, decrease raw materials)
            // 2. Cost postings
            // 3. Integration with other modules
            // Simplified for now

            _logger.LogInformation("Processing posting effects for confirmation {Id}", confirmation.Id);
            await Task.CompletedTask; // Placeholder
        }

        private async Task ProcessReversalEffectsAsync(ProductionConfirmation confirmation)
        {
            // This would reverse all posting effects
            // Simplified for now

            _logger.LogInformation("Processing reversal effects for confirmation {Id}", confirmation.Id);
            await Task.CompletedTask; // Placeholder
        }

        #endregion
    }
} 