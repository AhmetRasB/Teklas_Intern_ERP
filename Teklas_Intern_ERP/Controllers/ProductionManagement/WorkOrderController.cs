using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement;

[ApiController]
[Route("api/production/workorder")]
[ApiExplorerSettings(GroupName = "Production Management")]
public class WorkOrderController : ControllerBase
{
    private readonly IWorkOrderService _service;
    public WorkOrderController(IWorkOrderService service)
    {
        _service = service;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<WorkOrderDto>> Get(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkOrderDto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderDto>> Create([FromBody] CreateWorkOrderDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.WorkOrderId }, result);
    }

    [HttpPut]
    public async Task<ActionResult<WorkOrderDto>> Update([FromBody] UpdateWorkOrderDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> SoftDelete(long id)
    {
        var success = await _service.SoftDeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Restore a soft-deleted work order
    /// </summary>
    /// <param name="id">Work Order ID to restore</param>
    /// <returns>Success or error response</returns>
    [HttpPut("{id:long}/restore")]
    public async Task<IActionResult> Restore(long id)
    {
        try
        {
            var restored = await _service.RestoreAsync(id);
            if (!restored)
                return NotFound(new { error = "Work order not found or already restored" });
            
            return Ok(new { message = "Work order restored successfully", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Cannot restore work order", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all deleted work orders
    /// </summary>
    /// <returns>List of deleted work orders</returns>
    [HttpGet("deleted")]
    public async Task<ActionResult<List<WorkOrderDto>>> GetDeleted()
    {
        var result = await _service.GetDeletedAsync();
        return Ok(result);
    }

    /// <summary>
    /// Permanently delete a work order
    /// </summary>
    /// <param name="id">Work Order ID to permanently delete</param>
    /// <returns>Success or error response</returns>
    [HttpDelete("{id:long}/permanent")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        try
        {
            var deleted = await _service.PermanentDeleteAsync(id);
            if (!deleted)
                return NotFound(new { error = "Work order not found" });
            
            return Ok(new { message = "Work order permanently deleted", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Cannot permanently delete work order", details = ex.Message });
        }
    }
} 