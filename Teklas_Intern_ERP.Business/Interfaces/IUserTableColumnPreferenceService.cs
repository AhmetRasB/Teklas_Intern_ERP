using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs.UserManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IUserTableColumnPreferenceService
    {
        Task<UserTableColumnPreferenceDto?> GetPreferenceAsync(int userId, string tableKey);
        Task<bool> SetPreferenceAsync(UserTableColumnPreferenceDto dto);
    }
} 