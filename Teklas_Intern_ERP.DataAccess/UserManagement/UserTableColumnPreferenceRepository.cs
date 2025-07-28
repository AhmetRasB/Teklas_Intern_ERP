using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.UserManagement;

namespace Teklas_Intern_ERP.DataAccess.UserManagement
{
    public class UserTableColumnPreferenceRepository
    {
        private readonly AppDbContext _context;
        public UserTableColumnPreferenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserTableColumnPreference?> GetByUserAndTableAsync(int userId, string tableKey)
        {
            return await _context.Set<UserTableColumnPreference>()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.TableKey == tableKey);
        }

        public async Task<bool> SetPreferenceAsync(UserTableColumnPreference pref)
        {
            var existing = await _context.Set<UserTableColumnPreference>()
                .FirstOrDefaultAsync(x => x.UserId == pref.UserId && x.TableKey == pref.TableKey);
            if (existing != null)
            {
                existing.ColumnsJson = pref.ColumnsJson;
                existing.UpdatedAt = pref.UpdatedAt;
                _context.Update(existing);
            }
            else
            {
                await _context.AddAsync(pref);
            }
            return await _context.SaveChangesAsync() > 0;
        }
    }
} 