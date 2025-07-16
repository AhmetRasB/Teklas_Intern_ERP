using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement;

[ApiController]
[Route("api/production/bom")]
[ApiExplorerSettings(GroupName = "Production Management")]
public class BillOfMaterialController : ControllerBase
{
    private readonly IBillOfMaterialService _service;
    public BillOfMaterialController(IBillOfMaterialService service)
    {
        _service = service;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<BOMHeaderDto>> Get(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<BOMHeaderDto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BOMHeaderDto>> Create([FromBody] CreateBOMHeaderDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.BOMHeaderId }, result);
    }

    [HttpPut]
    public async Task<ActionResult<BOMHeaderDto>> Update([FromBody] UpdateBOMHeaderDto dto)
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
    /// Restore a soft-deleted BOM
    /// </summary>
    /// <param name="id">BOM ID to restore</param>
    /// <returns>Success or error response</returns>
    [HttpPut("{id:long}/restore")]
    public async Task<IActionResult> Restore(long id)
    {
        try
        {
            var restored = await _service.RestoreAsync(id);
            if (!restored)
                return NotFound(new { error = "BOM not found or already restored" });
            
            return Ok(new { message = "BOM restored successfully", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Cannot restore BOM", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all deleted BOMs
    /// </summary>
    /// <returns>List of deleted BOMs</returns>
    [HttpGet("deleted")]
    public async Task<ActionResult<List<BOMHeaderDto>>> GetDeleted()
    {
        var result = await _service.GetDeletedAsync();
        return Ok(result);
    }

    /// <summary>
    /// Permanently delete a BOM
    /// </summary>
    /// <param name="id">BOM ID to permanently delete</param>
    /// <returns>Success or error response</returns>
    [HttpDelete("{id:long}/permanent")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        try
        {
            var deleted = await _service.PermanentDeleteAsync(id);
            if (!deleted)
                return NotFound(new { error = "BOM not found" });
            
            return Ok(new { message = "BOM permanently deleted", id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = "Cannot permanently delete BOM", details = ex.Message });
        }
    }
} 