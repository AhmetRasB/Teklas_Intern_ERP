using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;
using AutoMapper;
using Teklas_Intern_ERP.DataAccess.DTOs;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement   
{
    public class MaterialCardRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public MaterialCardRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<MaterialCard> GetAll() => _context.MaterialCards.ToList();
        public MaterialCard GetById(int id) => _context.MaterialCards.Find(id);
        public MaterialCard Add(MaterialCard card)
        {
            if (card.CreatedDate == default)
                card.CreatedDate = DateTime.Now;
            _context.MaterialCards.Add(card);
            _context.SaveChanges();
            return card;
        }
        public bool Update(MaterialCard card)
        {
            if (card == null) return false;
            var existing = _context.MaterialCards.Find(card.Id);
            if (existing == null) return false;

            _mapper.Map(card, existing);
            existing.UpdatedDate = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var card = _context.MaterialCards.Find(id);
            if (card == null) return false;
            _context.MaterialCards.Remove(card);
            _context.SaveChanges();
            return true;
        }
        public MaterialCardDto GetByIdDto(int id)
        {
            var entity = _context.MaterialCards.Find(id);
            return _mapper.Map<MaterialCardDto>(entity);
        }
        public MaterialCardDto AddDto(MaterialCardDto dto)
        {
            var entity = _mapper.Map<MaterialCard>(dto);
            _context.MaterialCards.Add(entity);
            _context.SaveChanges();
            return _mapper.Map<MaterialCardDto>(entity);
        }
        public async Task<List<MaterialCard>> GetAllAsync() => await _context.MaterialCards.ToListAsync();
        public async Task<MaterialCard> GetByIdAsync(int id) => await _context.MaterialCards.FindAsync(id);
        public async Task<MaterialCard> AddAsync(MaterialCard card)
        {
            if (card.CreatedDate == default)
                card.CreatedDate = DateTime.Now;
            _context.MaterialCards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }
        public async Task<bool> UpdateAsync(MaterialCard card)
        {
            if (card == null) return false;
            var existing = await _context.MaterialCards.FindAsync(card.Id);
            if (existing == null) return false;
            _mapper.Map(card, existing);
            existing.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var card = await _context.MaterialCards.FindAsync(id);
            if (card == null) return false;
            _context.MaterialCards.Remove(card);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<(List<MaterialCard> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.MaterialCards.OrderBy(x => x.Id);
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }
    }
} 