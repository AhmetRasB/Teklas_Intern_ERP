using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public class MaterialCardRepository : BaseRepository<MaterialCard>, IRepository<MaterialCard>
    {
        public MaterialCardRepository(AppDbContext context) : base(context) { }
    }
} 