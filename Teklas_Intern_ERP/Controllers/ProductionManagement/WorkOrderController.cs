using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement
{
    /// <summary>
    /// Work Order Management Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class WorkOrderController : ControllerBase
    {
        private readonly WorkOrderService _workOrderService;
        private readonly ILogger<WorkOrderController> _logger;

        public WorkOrderController(
            WorkOrderService workOrderService,
            ILogger<WorkOrderController> logger)
        {
            _workOrderService = workOrderService;
            _logger = logger;
        }

        #region Basic CRUD Operations

        /// <summary>
        /// Get all work orders
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetAll()
        {
            try
            {
                var workOrders = await _workOrderService.GetAllAsync();
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all work orders");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOrderDto>> GetById(long id)
        {
            try
            {
                var workOrder = await _workOrderService.GetByIdAsync(id);
                if (workOrder == null)
                {
                    return NotFound($"Work order with ID {id} not found");
                }
                return Ok(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work order by ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work order by number
        /// </summary>
        [HttpGet("by-number/{workOrderNumber}")]
        public async Task<ActionResult<WorkOrderDto>> GetByNumber(string workOrderNumber)
        {
            try
            {
                var workOrder = await _workOrderService.GetByNumberAsync(workOrderNumber);
                if (workOrder == null)
                {
                    return NotFound($"Work order with number {workOrderNumber} not found");
                }
                return Ok(workOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work order by number {WorkOrderNumber}", workOrderNumber);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create new work order
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<WorkOrderDto>> Create([FromBody] WorkOrderDto workOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdWorkOrder = await _workOrderService.CreateAsync(workOrderDto);
                return CreatedAtAction(nameof(GetById), new { id = createdWorkOrder.Id }, createdWorkOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating work order");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update work order
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<WorkOrderDto>> Update(long id, [FromBody] WorkOrderDto workOrderDto)
        {
            try
            {
                if (id != workOrderDto.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedWorkOrder = await _workOrderService.UpdateAsync(workOrderDto);
                return Ok(updatedWorkOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete work order
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _workOrderService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound($"Work order with ID {id} not found");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Business Operations

        /// <summary>
        /// Release work order for production
        /// </summary>
        [HttpPost("{id}/release")]
        public async Task<ActionResult<WorkOrderDto>> Release(long id, [FromBody] ReleaseRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.ReleaseAsync(id, request.ReleasedByUserId);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error releasing work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Start work order production
        /// </summary>
        [HttpPost("{id}/start")]
        public async Task<ActionResult<WorkOrderDto>> Start(long id, [FromBody] StartRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.StartAsync(id, request.OperatorUserId);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Complete work order
        /// </summary>
        [HttpPost("{id}/complete")]
        public async Task<ActionResult<WorkOrderDto>> Complete(long id, [FromBody] CompleteRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.CompleteAsync(id, request.CompletedByUserId);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cancel work order
        /// </summary>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<WorkOrderDto>> Cancel(long id, [FromBody] CancelRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.CancelAsync(id, request.Reason, request.CancelledByUserId);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Put work order on hold
        /// </summary>
        [HttpPost("{id}/hold")]
        public async Task<ActionResult<WorkOrderDto>> PutOnHold(long id, [FromBody] HoldRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.PutOnHoldAsync(id, request.Reason, request.HeldByUserId);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error putting work order on hold with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Resume work order from hold
        /// </summary>
        [HttpPost("{id}/resume")]
        public async Task<ActionResult<WorkOrderDto>> Resume(long id, [FromBody] ResumeRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.ResumeAsync(id, request.ResumedByUserId);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resuming work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create work order from BOM
        /// </summary>
        [HttpPost("from-bom")]
        public async Task<ActionResult<WorkOrderDto>> CreateFromBOM([FromBody] CreateFromBOMRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.CreateFromBOMAsync(
                    request.BOMId, 
                    request.Quantity, 
                    request.PlannedStartDate, 
                    request.PlannedEndDate);
                return CreatedAtAction(nameof(GetById), new { id = workOrder.Id }, workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating work order from BOM");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Split work order
        /// </summary>
        [HttpPost("{id}/split")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> Split(long id, [FromBody] SplitRequest request)
        {
            try
            {
                var workOrders = await _workOrderService.SplitAsync(id, request.Quantities);
                return Ok(workOrders);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error splitting work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Merge work orders
        /// </summary>
        [HttpPost("merge")]
        public async Task<ActionResult<WorkOrderDto>> Merge([FromBody] MergeRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.MergeAsync(request.WorkOrderIds);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error merging work orders");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update work order progress
        /// </summary>
        [HttpPost("{id}/progress")]
        public async Task<ActionResult<WorkOrderDto>> UpdateProgress(long id, [FromBody] ProgressRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.UpdateProgressAsync(id, request.CompletedQuantity, request.ScrapQuantity);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating progress for work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Reschedule work order
        /// </summary>
        [HttpPost("{id}/reschedule")]
        public async Task<ActionResult<WorkOrderDto>> Reschedule(long id, [FromBody] RescheduleRequest request)
        {
            try
            {
                var workOrder = await _workOrderService.RescheduleAsync(id, request.NewStartDate, request.NewEndDate);
                return Ok(workOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rescheduling work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Validate work order
        /// </summary>
        [HttpGet("{id}/validate")]
        public async Task<ActionResult<WorkOrderValidationResult>> Validate(long id)
        {
            try
            {
                var result = await _workOrderService.ValidateAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Calculate work order cost
        /// </summary>
        [HttpGet("{id}/cost")]
        public async Task<ActionResult<WorkOrderCostResult>> CalculateCost(long id)
        {
            try
            {
                var result = await _workOrderService.CalculateCostAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cost for work order with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion

        #region Query Operations

        /// <summary>
        /// Get work orders by BOM
        /// </summary>
        [HttpGet("by-bom/{bomId}")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByBOM(long bomId)
        {
            try
            {
                var workOrders = await _workOrderService.GetByBOMAsync(bomId);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by BOM {BOMId}", bomId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work orders by product
        /// </summary>
        [HttpGet("by-product/{productId}")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByProduct(long productId)
        {
            try
            {
                var workOrders = await _workOrderService.GetByProductAsync(productId);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by product {ProductId}", productId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work orders by status
        /// </summary>
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByStatus(string status)
        {
            try
            {
                var workOrders = await _workOrderService.GetByStatusAsync(status);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by status {Status}", status);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work orders by priority
        /// </summary>
        [HttpGet("by-priority/{priority}")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByPriority(int priority)
        {
            try
            {
                var workOrders = await _workOrderService.GetByPriorityAsync(priority);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by priority {Priority}", priority);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work orders by date range
        /// </summary>
        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var workOrders = await _workOrderService.GetByDateRangeAsync(startDate, endDate);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by date range");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get overdue work orders
        /// </summary>
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetOverdue()
        {
            try
            {
                var workOrders = await _workOrderService.GetOverdueAsync();
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue work orders");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get work orders by work center
        /// </summary>
        [HttpGet("by-work-center/{workCenter}")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> GetByWorkCenter(string workCenter)
        {
            try
            {
                var workOrders = await _workOrderService.GetByWorkCenterAsync(workCenter);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting work orders by work center {WorkCenter}", workCenter);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Search work orders
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<WorkOrderDto>>> Search([FromQuery] string term)
        {
            try
            {
                var workOrders = await _workOrderService.SearchAsync(term);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching work orders");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get completion statistics
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<WorkOrderCompletionStatistics>> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var statistics = await _workOrderService.GetCompletionStatisticsAsync(startDate, endDate);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting completion statistics");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion
    }

    #region Request Models

    public sealed class ReleaseRequest
    {
        public long ReleasedByUserId { get; set; }
    }

    public sealed class StartRequest
    {
        public long OperatorUserId { get; set; }
    }

    public sealed class CompleteRequest
    {
        public long CompletedByUserId { get; set; }
    }

    public sealed class CancelRequest
    {
        public string Reason { get; set; } = string.Empty;
        public long CancelledByUserId { get; set; }
    }

    public sealed class HoldRequest
    {
        public string Reason { get; set; } = string.Empty;
        public long HeldByUserId { get; set; }
    }

    public sealed class ResumeRequest
    {
        public long ResumedByUserId { get; set; }
    }

    public sealed class CreateFromBOMRequest
    {
        public long BOMId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
    }

    public sealed class SplitRequest
    {
        public IEnumerable<decimal> Quantities { get; set; } = new List<decimal>();
    }

    public sealed class MergeRequest
    {
        public IEnumerable<long> WorkOrderIds { get; set; } = new List<long>();
    }

    public sealed class ProgressRequest
    {
        public decimal CompletedQuantity { get; set; }
        public decimal ScrapQuantity { get; set; }
    }

    public sealed class RescheduleRequest
    {
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
    }

    #endregion
} 