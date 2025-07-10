using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement
{
    /// <summary>
    /// Production Confirmation Management Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Production Management")]
    [Produces("application/json")]
    [Authorize]
    public class ProductionConfirmationController : ControllerBase
    {
        private readonly ProductionConfirmationService _confirmationService;
        private readonly ILogger<ProductionConfirmationController> _logger;

        public ProductionConfirmationController(
            ProductionConfirmationService confirmationService,
            ILogger<ProductionConfirmationController> logger)
        {
            _confirmationService = confirmationService;
            _logger = logger;
        }

        #region Basic CRUD Operations

        /// <summary>
        /// Get all production confirmations
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetAll()
        {
            try
            {
                var confirmations = await _confirmationService.GetAllAsync();
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all production confirmations");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmation by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionConfirmationDto>> GetById(long id)
        {
            try
            {
                var confirmation = await _confirmationService.GetByIdAsync(id);
                if (confirmation == null)
                {
                    return NotFound($"Production confirmation with ID {id} not found");
                }
                return Ok(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production confirmation by ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmation by number
        /// </summary>
        [HttpGet("by-number/{confirmationNumber}")]
        public async Task<ActionResult<ProductionConfirmationDto>> GetByNumber(string confirmationNumber)
        {
            try
            {
                var confirmation = await _confirmationService.GetByNumberAsync(confirmationNumber);
                if (confirmation == null)
                {
                    return NotFound($"Production confirmation with number {confirmationNumber} not found");
                }
                return Ok(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production confirmation by number {ConfirmationNumber}", confirmationNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create new production confirmation
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductionConfirmationDto>> Create([FromBody] ProductionConfirmationDto confirmationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdConfirmation = await _confirmationService.CreateAsync(confirmationDto);
                return CreatedAtAction(nameof(GetById), new { id = createdConfirmation.Id }, createdConfirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating production confirmation");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update production confirmation
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductionConfirmationDto>> Update(long id, [FromBody] ProductionConfirmationDto confirmationDto)
        {
            try
            {
                if (id != confirmationDto.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedConfirmation = await _confirmationService.UpdateAsync(confirmationDto);
                return Ok(updatedConfirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating production confirmation with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete production confirmation
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _confirmationService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound($"Production confirmation with ID {id} not found");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting production confirmation with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Business Operations

        /// <summary>
        /// Confirm production activity
        /// </summary>
        [HttpPost("confirm-production")]
        public async Task<ActionResult<ProductionConfirmationDto>> ConfirmProduction([FromBody] ConfirmProductionRequest request)
        {
            try
            {
                var confirmation = await _confirmationService.ConfirmProductionAsync(
                    request.WorkOrderId,
                    request.ConfirmedQuantity,
                    request.ScrapQuantity,
                    request.OperatorUserId);
                return Ok(confirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming production");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Process material consumption
        /// </summary>
        [HttpPost("{id}/material-consumption")]
        public async Task<ActionResult<ProductionConfirmationDto>> ProcessMaterialConsumption(long id, [FromBody] MaterialConsumptionRequest request)
        {
            try
            {
                var confirmation = await _confirmationService.ProcessMaterialConsumptionAsync(id, request.Consumptions);
                return Ok(confirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing material consumption for confirmation {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Record quality check results
        /// </summary>
        [HttpPost("{id}/quality-check")]
        public async Task<ActionResult<ProductionConfirmationDto>> RecordQualityCheck(long id, [FromBody] QualityCheckDto qualityCheck)
        {
            try
            {
                var confirmation = await _confirmationService.RecordQualityCheckAsync(id, qualityCheck);
                return Ok(confirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording quality check for confirmation {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Post production confirmation
        /// </summary>
        [HttpPost("{id}/post")]
        public async Task<ActionResult<ProductionConfirmationDto>> Post(long id, [FromBody] PostRequest request)
        {
            try
            {
                var confirmation = await _confirmationService.PostAsync(id, request.PostedByUserId);
                return Ok(confirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error posting production confirmation {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Reverse production confirmation
        /// </summary>
        [HttpPost("{id}/reverse")]
        public async Task<ActionResult<ProductionConfirmationDto>> Reverse(long id, [FromBody] ReverseRequest request)
        {
            try
            {
                var confirmation = await _confirmationService.ReverseAsync(id, request.Reason, request.ReversedByUserId);
                return Ok(confirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reversing production confirmation {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Calculate labor hours
        /// </summary>
        [HttpGet("{id}/labor-hours")]
        public async Task<ActionResult<LaborHoursResult>> CalculateLaborHours(long id)
        {
            try
            {
                var result = await _confirmationService.CalculateLaborHoursAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating labor hours for confirmation {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Validate production confirmation
        /// </summary>
        [HttpGet("{id}/validate")]
        public async Task<ActionResult<ProductionConfirmationValidationResult>> Validate(long id)
        {
            try
            {
                var result = await _confirmationService.ValidateAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating production confirmation {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Auto-confirm based on work order progress
        /// </summary>
        [HttpPost("auto-confirm/{workOrderId}")]
        public async Task<ActionResult<ProductionConfirmationDto>> AutoConfirm(long workOrderId)
        {
            try
            {
                var confirmation = await _confirmationService.AutoConfirmAsync(workOrderId);
                return Ok(confirmation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error auto-confirming for work order {WorkOrderId}", workOrderId);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Get production confirmations by work order
        /// </summary>
        [HttpGet("by-work-order/{workOrderId}")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetByWorkOrder(long workOrderId)
        {
            try
            {
                var confirmations = await _confirmationService.GetByWorkOrderAsync(workOrderId);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by work order {WorkOrderId}", workOrderId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmations by operator
        /// </summary>
        [HttpGet("by-operator/{operatorUserId}")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetByOperator(long operatorUserId)
        {
            try
            {
                var confirmations = await _confirmationService.GetByOperatorAsync(operatorUserId);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by operator {OperatorId}", operatorUserId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmations by date range
        /// </summary>
        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var confirmations = await _confirmationService.GetByDateRangeAsync(startDate, endDate);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by date range");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmations by shift
        /// </summary>
        [HttpGet("by-shift/{shift}")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetByShift(string shift)
        {
            try
            {
                var confirmations = await _confirmationService.GetByShiftAsync(shift);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by shift {Shift}", shift);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmations by work center
        /// </summary>
        [HttpGet("by-work-center/{workCenter}")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetByWorkCenter(string workCenter)
        {
            try
            {
                var confirmations = await _confirmationService.GetByWorkCenterAsync(workCenter);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by work center {WorkCenter}", workCenter);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production confirmations by status
        /// </summary>
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetByStatus(string status)
        {
            try
            {
                var confirmations = await _confirmationService.GetByStatusAsync(status);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting confirmations by status {Status}", status);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get pending confirmations
        /// </summary>
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> GetPending()
        {
            try
            {
                var confirmations = await _confirmationService.GetPendingAsync();
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending confirmations");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Search production confirmations
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductionConfirmationDto>>> Search([FromQuery] string term)
        {
            try
            {
                var confirmations = await _confirmationService.SearchAsync(term);
                return Ok(confirmations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching confirmations");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Reporting Operations

        /// <summary>
        /// Get production summary
        /// </summary>
        [HttpGet("reports/production-summary")]
        public async Task<ActionResult<ProductionSummaryDto>> GetProductionSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var summary = await _confirmationService.GetProductionSummaryAsync(startDate, endDate);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production summary");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get operator efficiency report
        /// </summary>
        [HttpGet("reports/operator-efficiency")]
        public async Task<ActionResult<IEnumerable<OperatorEfficiencyDto>>> GetOperatorEfficiency([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var efficiency = await _confirmationService.GetOperatorEfficiencyAsync(startDate, endDate);
                return Ok(efficiency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting operator efficiency");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get scrap analysis report
        /// </summary>
        [HttpGet("reports/scrap-analysis")]
        public async Task<ActionResult<IEnumerable<ScrapAnalysisDto>>> GetScrapAnalysis([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var scrapAnalysis = await _confirmationService.GetScrapAnalysisAsync(startDate, endDate);
                return Ok(scrapAnalysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scrap analysis");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get production trend data
        /// </summary>
        [HttpGet("reports/production-trend")]
        public async Task<ActionResult<IEnumerable<ProductionTrendDto>>> GetProductionTrend([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string groupBy = "DAY")
        {
            try
            {
                var trends = await _confirmationService.GetProductionTrendAsync(startDate, endDate, groupBy);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting production trend");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work center utilization
        /// </summary>
        [HttpGet("reports/work-center-utilization")]
        public async Task<ActionResult<IEnumerable<WorkCenterUtilizationDto>>> GetWorkCenterUtilization([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var utilization = await _confirmationService.GetWorkCenterUtilizationAsync(startDate, endDate);
                return Ok(utilization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work center utilization");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion
    }

    #region Request Models

    public sealed class ConfirmProductionRequest
    {
        public long WorkOrderId { get; set; }
        public decimal ConfirmedQuantity { get; set; }
        public decimal ScrapQuantity { get; set; }
        public long OperatorUserId { get; set; }
    }

    public sealed class MaterialConsumptionRequest
    {
        public IEnumerable<MaterialConsumptionDto> Consumptions { get; set; } = new List<MaterialConsumptionDto>();
    }

    public sealed class PostRequest
    {
        public long PostedByUserId { get; set; }
    }

    public sealed class ReverseRequest
    {
        public string Reason { get; set; } = string.Empty;
        public long ReversedByUserId { get; set; }
    }

    #endregion
} 