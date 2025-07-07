using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Controllers.MaterialManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Material Management")]
    public class MaterialCardController : ControllerBase
    {
        private readonly IMaterialCardService _service;
        private readonly IMapper _mapper;
        public MaterialCardController(IMaterialCardService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/MaterialCard
        [HttpGet]
        public async Task<ActionResult<List<MaterialCardDto>>> GetAll([FromQuery] bool deleted = false)
        {
            var entities = deleted ? await _service.GetDeletedAsync() : await _service.GetAllAsync();
            var dtos = _mapper.Map<List<MaterialCardDto>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialCardDto>> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<MaterialCardDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<MaterialCardDto>> Add([FromBody] MaterialCardDto dto)
        {
            var entity = _mapper.Map<Entities.MaterialManagement.MaterialCard>(dto);
            await _service.AddAsync(entity);
            var resultDto = _mapper.Map<MaterialCardDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaterialCardDto dto)
        {
            dto.Id = id;
            var entity = _mapper.Map<Entities.MaterialManagement.MaterialCard>(dto);
            var updated = await _service.UpdateAsync(entity);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var restored = await _service.RestoreAsync(id);
            if (!restored) return NotFound();
            return Ok();
        }

        [HttpDelete("permanent/{id}")]
        public async Task<IActionResult> PermanentDelete(int id)
        {
            var deleted = await _service.PermanentDeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
} 