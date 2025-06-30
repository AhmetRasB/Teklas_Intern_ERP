using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public class MaterialCardRepository
    {
        private readonly AppDbContext _context;
        public MaterialCardRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MaterialCard> GetAll() => _context.MaterialCards.ToList();
        public MaterialCard GetById(int id) => _context.MaterialCards.Find(id);
        public MaterialCard Add(MaterialCard card)
        {
            _context.MaterialCards.Add(card);
            _context.SaveChanges();
            return card;
        }
        public bool Update(MaterialCard card)
        {
            var existing = _context.MaterialCards.Find(card.Id);
            if (existing == null) return false;
            existing.Name = card.Name;
            existing.Code = card.Code;
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
    }
} 