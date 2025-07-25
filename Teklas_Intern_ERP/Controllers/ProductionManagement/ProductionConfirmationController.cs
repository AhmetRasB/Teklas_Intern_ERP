using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement;

[ApiController]
[Route("api/production/confirmation")]
[ApiExplorerSettings(GroupName = "Production Management")]
public class ProductionConfirmationController : ControllerBase
{
    private readonly IProductionConfirmationService _service;
    public ProductionConfirmationController(IProductionConfirmationService service)
    {
        _service = service;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProductionConfirmationDto>> Get(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductionConfirmationDto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductionConfirmationDto>> Create([FromBody] CreateProductionConfirmationDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.ConfirmationId }, result);
    }

    [HttpPut]
    public async Task<ActionResult<ProductionConfirmationDto>> Update([FromBody] UpdateProductionConfirmationDto dto)
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
    /// Restore a soft-deleted production confirmation
    /// </summary>
    /// <param name="id">Production Confirmation ID to restore</param>
    /// <returns>Success or error response</returns>
    [HttpPut("{id:long}/restore")]
    public async Task<IActionResult> Restore(long id)
    {
        try
        {
            var restored = await _service.RestoreAsync(id);
            if (!restored)
                return NotFound(new { error = "Production confirmation not found or already restored" });
            
            return Ok(new { message = "Production confirmation restored successfully", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Cannot restore production confirmation", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all deleted production confirmations
    /// </summary>
    /// <returns>List of deleted production confirmations</returns>
    [HttpGet("deleted")]
    public async Task<ActionResult<List<ProductionConfirmationDto>>> GetDeleted()
    {
        var result = await _service.GetDeletedAsync();
        return Ok(result);
    }

    /// <summary>
    /// Permanently delete a production confirmation
    /// </summary>
    /// <param name="id">Production Confirmation ID to permanently delete</param>
    /// <returns>Success or error response</returns>
    [HttpDelete("{id:long}/permanent")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        try
        {
            var deleted = await _service.PermanentDeleteAsync(id);
            if (!deleted)
                return NotFound(new { error = "Production confirmation not found" });
            
            return Ok(new { message = "Production confirmation permanently deleted", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Cannot permanently delete production confirmation", details = ex.Message });
        }
    }
} 