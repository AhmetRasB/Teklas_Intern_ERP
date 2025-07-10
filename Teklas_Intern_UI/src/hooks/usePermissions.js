import { useAuth } from '../contexts/AuthContext';
import { 
  hasPermission, 
  hasAnyPermission, 
  hasAllPermissions, 
  isAdmin,
  getUserPermissions,
  canViewModule,
  canEditModule,
  canDeleteModule,
  canCreateModule
} from '../utils/permissions';

export const usePermissions = () => {
  const { user } = useAuth();
  
  // Get user roles
  const userRoles = user?.roleNames || [];
  
  return {
    // Basic permission checks
    hasPermission: (permission) => hasPermission(userRoles, permission),
    hasAnyPermission: (permissions) => hasAnyPermission(userRoles, permissions),
    hasAllPermissions: (permissions) => hasAllPermissions(userRoles, permissions),
    isAdmin: () => isAdmin(userRoles),
    
    // Get all user permissions
    getUserPermissions: () => getUserPermissions(userRoles),
    
    // Module-specific permissions
    canViewModule: (module) => canViewModule(userRoles, module),
    canEditModule: (module) => canEditModule(userRoles, module),
    canDeleteModule: (module) => canDeleteModule(userRoles, module),
    canCreateModule: (module) => canCreateModule(userRoles, module),
    
    // Role checks
    hasRole: (role) => userRoles.includes(role),
    hasAnyRole: (roles) => roles.some(role => userRoles.includes(role)),
    hasAllRoles: (roles) => roles.every(role => userRoles.includes(role)),
    
    // Get user roles
    userRoles,
    
    // Check if user can perform specific actions
    canManageUsers: () => hasPermission(userRoles, 'users.edit'),
    canManageRoles: () => hasPermission(userRoles, 'roles.edit'),
    canViewReports: () => hasPermission(userRoles, 'reports.view'),
    canExportReports: () => hasPermission(userRoles, 'reports.export'),
    canAccessSettings: () => hasPermission(userRoles, 'settings.view'),
    canEditSettings: () => hasPermission(userRoles, 'settings.edit'),
    
    // Material management specific
    canManageMaterials: () => hasPermission(userRoles, 'materials.edit'),
    canDeleteMaterials: () => hasPermission(userRoles, 'materials.delete'),
    canRestoreMaterials: () => hasPermission(userRoles, 'materials.restore'),
    
    // Production management specific
    canManageProduction: () => hasPermission(userRoles, 'production.edit'),
    canConfirmProduction: () => hasPermission(userRoles, 'production.confirm'),
    canCancelProduction: () => hasPermission(userRoles, 'production.cancel'),
    
    // Work order specific
    canManageWorkOrders: () => hasPermission(userRoles, 'work_orders.edit'),
    canReleaseWorkOrders: () => hasPermission(userRoles, 'work_orders.release'),
    canStartWorkOrders: () => hasPermission(userRoles, 'work_orders.start'),
    canCompleteWorkOrders: () => hasPermission(userRoles, 'work_orders.complete'),
    
    // Movement specific
    canManageMovements: () => hasPermission(userRoles, 'movements.edit'),
    canConfirmMovements: () => hasPermission(userRoles, 'movements.confirm'),
    canCancelMovements: () => hasPermission(userRoles, 'movements.cancel'),
    
    // Quick checks for common scenarios
    isViewOnlyUser: () => {
      const editPermissions = [
        'materials.edit',
        'categories.edit',
        'movements.edit',
        'production.edit',
        'work_orders.edit',
        'users.edit',
        'roles.edit'
      ];
      return !hasAnyPermission(userRoles, editPermissions);
    },
    
    // Check if user has any modification permissions
    canModifyAnyData: () => {
      const modifyPermissions = [
        'materials.create', 'materials.edit', 'materials.delete',
        'categories.create', 'categories.edit', 'categories.delete',
        'movements.create', 'movements.edit', 'movements.delete',
        'production.create', 'production.edit', 'production.delete',
        'work_orders.create', 'work_orders.edit', 'work_orders.delete',
        'users.create', 'users.edit', 'users.delete',
        'roles.create', 'roles.edit', 'roles.delete'
      ];
      return hasAnyPermission(userRoles, modifyPermissions);
    }
  };
};

export default usePermissions; 