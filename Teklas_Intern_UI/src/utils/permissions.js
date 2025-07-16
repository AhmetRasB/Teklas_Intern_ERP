// Permission constants
export const PERMISSIONS = {
  // User Management
  USERS_VIEW: 'users.view',
  USERS_CREATE: 'users.create',
  USERS_EDIT: 'users.edit',
  USERS_DELETE: 'users.delete',
  USERS_ACTIVATE: 'users.activate',
  USERS_ASSIGN_ROLES: 'users.assign_roles',
  
  // Role Management
  ROLES_VIEW: 'roles.view',
  ROLES_CREATE: 'roles.create',
  ROLES_EDIT: 'roles.edit',
  ROLES_DELETE: 'roles.delete',
  ROLES_ASSIGN: 'roles.assign',
  
  // Material Management
  MATERIALS_VIEW: 'materials.view',
  MATERIALS_CREATE: 'materials.create',
  MATERIALS_EDIT: 'materials.edit',
  MATERIALS_DELETE: 'materials.delete',
  MATERIALS_RESTORE: 'materials.restore',
  MATERIALS_PERMANENT_DELETE: 'materials.permanent_delete',
  
  // Material Categories
  CATEGORIES_VIEW: 'categories.view',
  CATEGORIES_CREATE: 'categories.create',
  CATEGORIES_EDIT: 'categories.edit',
  CATEGORIES_DELETE: 'categories.delete',
  CATEGORIES_RESTORE: 'categories.restore',
  CATEGORIES_PERMANENT_DELETE: 'categories.permanent_delete',
  
  // Material Movements
  MOVEMENTS_VIEW: 'movements.view',
  MOVEMENTS_CREATE: 'movements.create',
  MOVEMENTS_EDIT: 'movements.edit',
  MOVEMENTS_DELETE: 'movements.delete',
  MOVEMENTS_RESTORE: 'movements.restore',
  MOVEMENTS_PERMANENT_DELETE: 'movements.permanent_delete',
  MOVEMENTS_CONFIRM: 'movements.confirm',
  MOVEMENTS_CANCEL: 'movements.cancel',
  
  // Warehouse Management
  WAREHOUSE_VIEW: 'warehouse.view',
  WAREHOUSE_CREATE: 'warehouse.create',
  WAREHOUSE_EDIT: 'warehouse.edit',
  WAREHOUSE_DELETE: 'warehouse.delete',
  WAREHOUSE_RESTORE: 'warehouse.restore',
  WAREHOUSE_PERMANENT_DELETE: 'warehouse.permanent_delete',
  
  // Purchasing Management
  PURCHASING_VIEW: 'purchasing.view',
  PURCHASING_CREATE: 'purchasing.create',
  PURCHASING_EDIT: 'purchasing.edit',
  PURCHASING_DELETE: 'purchasing.delete',
  PURCHASING_RESTORE: 'purchasing.restore',
  PURCHASING_PERMANENT_DELETE: 'purchasing.permanent_delete',
  
  // Sales Management
  SALES_VIEW: 'sales.view',
  SALES_CREATE: 'sales.create',
  SALES_EDIT: 'sales.edit',
  SALES_DELETE: 'sales.delete',
  SALES_RESTORE: 'sales.restore',
  SALES_PERMANENT_DELETE: 'sales.permanent_delete',
  
  // Production Management
  PRODUCTION_VIEW: 'production.view',
  PRODUCTION_CREATE: 'production.create',
  PRODUCTION_EDIT: 'production.edit',
  PRODUCTION_DELETE: 'production.delete',
  PRODUCTION_CONFIRM: 'production.confirm',
  PRODUCTION_CANCEL: 'production.cancel',
  
  // Work Orders
  WORK_ORDERS_VIEW: 'work_orders.view',
  WORK_ORDERS_CREATE: 'work_orders.create',
  WORK_ORDERS_EDIT: 'work_orders.edit',
  WORK_ORDERS_DELETE: 'work_orders.delete',
  WORK_ORDERS_RELEASE: 'work_orders.release',
  WORK_ORDERS_START: 'work_orders.start',
  WORK_ORDERS_COMPLETE: 'work_orders.complete',
  WORK_ORDERS_CANCEL: 'work_orders.cancel',
  
  // Reports
  REPORTS_VIEW: 'reports.view',
  REPORTS_EXPORT: 'reports.export',
  REPORTS_ADVANCED: 'reports.advanced',
  
  // System Settings
  SETTINGS_VIEW: 'settings.view',
  SETTINGS_EDIT: 'settings.edit',
  SYSTEM_LOGS: 'system.logs',
  SYSTEM_BACKUP: 'system.backup'
};

// Role-based permissions mapping
export const ROLE_PERMISSIONS = {
  Admin: [
    // Full access to everything
    ...Object.values(PERMISSIONS)
  ],
  
  SuperAdmin: [
    // Same as Admin but with additional system permissions
    ...Object.values(PERMISSIONS)
  ],
  
  Manager: [
    // View all, edit most, no user management
    PERMISSIONS.MATERIALS_VIEW,
    PERMISSIONS.MATERIALS_CREATE,
    PERMISSIONS.MATERIALS_EDIT,
    PERMISSIONS.MATERIALS_DELETE,
    PERMISSIONS.MATERIALS_RESTORE,
    
    PERMISSIONS.CATEGORIES_VIEW,
    PERMISSIONS.CATEGORIES_CREATE,
    PERMISSIONS.CATEGORIES_EDIT,
    PERMISSIONS.CATEGORIES_DELETE,
    PERMISSIONS.CATEGORIES_RESTORE,
    
    PERMISSIONS.MOVEMENTS_VIEW,
    PERMISSIONS.MOVEMENTS_CREATE,
    PERMISSIONS.MOVEMENTS_EDIT,
    PERMISSIONS.MOVEMENTS_DELETE,
    PERMISSIONS.MOVEMENTS_RESTORE,
    PERMISSIONS.MOVEMENTS_CONFIRM,
    PERMISSIONS.MOVEMENTS_CANCEL,
    
    PERMISSIONS.WAREHOUSE_VIEW,
    PERMISSIONS.WAREHOUSE_CREATE,
    PERMISSIONS.WAREHOUSE_EDIT,
    PERMISSIONS.WAREHOUSE_DELETE,
    PERMISSIONS.WAREHOUSE_RESTORE,
    
    PERMISSIONS.PURCHASING_VIEW,
    PERMISSIONS.PURCHASING_CREATE,
    PERMISSIONS.PURCHASING_EDIT,
    PERMISSIONS.PURCHASING_DELETE,
    PERMISSIONS.PURCHASING_RESTORE,
    
    PERMISSIONS.SALES_VIEW,
    PERMISSIONS.SALES_CREATE,
    PERMISSIONS.SALES_EDIT,
    PERMISSIONS.SALES_DELETE,
    PERMISSIONS.SALES_RESTORE,
    
    PERMISSIONS.PRODUCTION_VIEW,
    PERMISSIONS.PRODUCTION_CREATE,
    PERMISSIONS.PRODUCTION_EDIT,
    PERMISSIONS.PRODUCTION_DELETE,
    PERMISSIONS.PRODUCTION_CONFIRM,
    PERMISSIONS.PRODUCTION_CANCEL,
    
    PERMISSIONS.WORK_ORDERS_VIEW,
    PERMISSIONS.WORK_ORDERS_CREATE,
    PERMISSIONS.WORK_ORDERS_EDIT,
    PERMISSIONS.WORK_ORDERS_DELETE,
    PERMISSIONS.WORK_ORDERS_RELEASE,
    PERMISSIONS.WORK_ORDERS_START,
    PERMISSIONS.WORK_ORDERS_COMPLETE,
    PERMISSIONS.WORK_ORDERS_CANCEL,
    
    PERMISSIONS.REPORTS_VIEW,
    PERMISSIONS.REPORTS_EXPORT,
    PERMISSIONS.REPORTS_ADVANCED,
    
    PERMISSIONS.SETTINGS_VIEW,
    PERMISSIONS.SETTINGS_EDIT
  ],
  
  Supervisor: [
    // View all, edit some, no deletion
    PERMISSIONS.MATERIALS_VIEW,
    PERMISSIONS.MATERIALS_CREATE,
    PERMISSIONS.MATERIALS_EDIT,
    
    PERMISSIONS.CATEGORIES_VIEW,
    PERMISSIONS.CATEGORIES_CREATE,
    PERMISSIONS.CATEGORIES_EDIT,
    
    PERMISSIONS.MOVEMENTS_VIEW,
    PERMISSIONS.MOVEMENTS_CREATE,
    PERMISSIONS.MOVEMENTS_EDIT,
    PERMISSIONS.MOVEMENTS_CONFIRM,
    PERMISSIONS.MOVEMENTS_CANCEL,
    
    PERMISSIONS.WAREHOUSE_VIEW,
    PERMISSIONS.WAREHOUSE_CREATE,
    PERMISSIONS.WAREHOUSE_EDIT,
    
    PERMISSIONS.PURCHASING_VIEW,
    PERMISSIONS.PURCHASING_CREATE,
    PERMISSIONS.PURCHASING_EDIT,
    
    PERMISSIONS.SALES_VIEW,
    PERMISSIONS.SALES_CREATE,
    PERMISSIONS.SALES_EDIT,
    
    PERMISSIONS.PRODUCTION_VIEW,
    PERMISSIONS.PRODUCTION_CREATE,
    PERMISSIONS.PRODUCTION_EDIT,
    PERMISSIONS.PRODUCTION_CONFIRM,
    PERMISSIONS.PRODUCTION_CANCEL,
    
    PERMISSIONS.WORK_ORDERS_VIEW,
    PERMISSIONS.WORK_ORDERS_CREATE,
    PERMISSIONS.WORK_ORDERS_EDIT,
    PERMISSIONS.WORK_ORDERS_RELEASE,
    PERMISSIONS.WORK_ORDERS_START,
    PERMISSIONS.WORK_ORDERS_COMPLETE,
    
    PERMISSIONS.REPORTS_VIEW,
    PERMISSIONS.REPORTS_EXPORT,
    
    PERMISSIONS.SETTINGS_VIEW
  ],
  
  ProductionOperator: [
    // View most, edit limited, no deletion
    PERMISSIONS.MATERIALS_VIEW,
    
    PERMISSIONS.CATEGORIES_VIEW,
    
    PERMISSIONS.MOVEMENTS_VIEW,
    PERMISSIONS.MOVEMENTS_CREATE,
    PERMISSIONS.MOVEMENTS_EDIT,
    PERMISSIONS.MOVEMENTS_CONFIRM,
    
    PERMISSIONS.PRODUCTION_VIEW,
    PERMISSIONS.PRODUCTION_CREATE,
    PERMISSIONS.PRODUCTION_EDIT,
    PERMISSIONS.PRODUCTION_CONFIRM,
    
    PERMISSIONS.WORK_ORDERS_VIEW,
    PERMISSIONS.WORK_ORDERS_START,
    PERMISSIONS.WORK_ORDERS_COMPLETE,
    
    PERMISSIONS.REPORTS_VIEW,
    
    PERMISSIONS.SETTINGS_VIEW
  ],
  
  QualityControl: [
    // View most, limited editing
    PERMISSIONS.MATERIALS_VIEW,
    
    PERMISSIONS.CATEGORIES_VIEW,
    
    PERMISSIONS.MOVEMENTS_VIEW,
    
    PERMISSIONS.PRODUCTION_VIEW,
    PERMISSIONS.PRODUCTION_CONFIRM,
    
    PERMISSIONS.WORK_ORDERS_VIEW,
    PERMISSIONS.WORK_ORDERS_COMPLETE,
    
    PERMISSIONS.REPORTS_VIEW,
    PERMISSIONS.REPORTS_EXPORT,
    
    PERMISSIONS.SETTINGS_VIEW
  ],
  
  Accountant: [
    // View only for most things, some reporting
    PERMISSIONS.MATERIALS_VIEW,
    PERMISSIONS.CATEGORIES_VIEW,
    PERMISSIONS.MOVEMENTS_VIEW,
    PERMISSIONS.WAREHOUSE_VIEW,
    PERMISSIONS.PURCHASING_VIEW,
    PERMISSIONS.SALES_VIEW,
    PERMISSIONS.PRODUCTION_VIEW,
    PERMISSIONS.WORK_ORDERS_VIEW,
    PERMISSIONS.REPORTS_VIEW,
    PERMISSIONS.REPORTS_EXPORT,
    PERMISSIONS.SETTINGS_VIEW
  ],
  
  WarehouseWorker: [
    // Material and movement focus
    PERMISSIONS.MATERIALS_VIEW,
    PERMISSIONS.MATERIALS_CREATE,
    PERMISSIONS.MATERIALS_EDIT,
    
    PERMISSIONS.CATEGORIES_VIEW,
    
    PERMISSIONS.MOVEMENTS_VIEW,
    PERMISSIONS.MOVEMENTS_CREATE,
    PERMISSIONS.MOVEMENTS_EDIT,
    PERMISSIONS.MOVEMENTS_CONFIRM,
    
    PERMISSIONS.WAREHOUSE_VIEW,
    PERMISSIONS.WAREHOUSE_CREATE,
    PERMISSIONS.WAREHOUSE_EDIT,
    
    PERMISSIONS.PURCHASING_VIEW,
    
    PERMISSIONS.SALES_VIEW,
    
    PERMISSIONS.PRODUCTION_VIEW,
    
    PERMISSIONS.WORK_ORDERS_VIEW,
    
    PERMISSIONS.REPORTS_VIEW,
    
    PERMISSIONS.SETTINGS_VIEW
  ],
  
  User: [
    // Basic view permissions
    PERMISSIONS.MATERIALS_VIEW,
    PERMISSIONS.CATEGORIES_VIEW,
    PERMISSIONS.MOVEMENTS_VIEW,
    PERMISSIONS.PRODUCTION_VIEW,
    PERMISSIONS.WORK_ORDERS_VIEW,
    PERMISSIONS.REPORTS_VIEW,
    PERMISSIONS.SETTINGS_VIEW
  ]
};

// Utility functions
export const hasPermission = (userRoles, requiredPermission) => {
  if (!userRoles || !Array.isArray(userRoles)) {
    return false;
  }
  
  return userRoles.some(role => {
    const rolePermissions = ROLE_PERMISSIONS[role] || [];
    return rolePermissions.includes(requiredPermission);
  });
};

export const hasAnyPermission = (userRoles, requiredPermissions) => {
  if (!userRoles || !Array.isArray(userRoles) || !requiredPermissions || !Array.isArray(requiredPermissions)) {
    return false;
  }
  
  return requiredPermissions.some(permission => hasPermission(userRoles, permission));
};

export const hasAllPermissions = (userRoles, requiredPermissions) => {
  if (!userRoles || !Array.isArray(userRoles) || !requiredPermissions || !Array.isArray(requiredPermissions)) {
    return false;
  }
  
  return requiredPermissions.every(permission => hasPermission(userRoles, permission));
};

export const isAdmin = (userRoles) => {
  if (!userRoles || !Array.isArray(userRoles)) {
    return false;
  }
  
  return userRoles.some(role => role === 'Admin' || role === 'SuperAdmin');
};

export const getUserPermissions = (userRoles) => {
  if (!userRoles || !Array.isArray(userRoles)) {
    return [];
  }
  
  const permissions = new Set();
  userRoles.forEach(role => {
    const rolePermissions = ROLE_PERMISSIONS[role] || [];
    rolePermissions.forEach(permission => permissions.add(permission));
  });
  
  return Array.from(permissions);
};

export const canViewModule = (userRoles, module) => {
  const modulePermissions = {
    'materials': PERMISSIONS.MATERIALS_VIEW,
    'categories': PERMISSIONS.CATEGORIES_VIEW,
    'movements': PERMISSIONS.MOVEMENTS_VIEW,
    'production': PERMISSIONS.PRODUCTION_VIEW,
    'work_orders': PERMISSIONS.WORK_ORDERS_VIEW,
    'users': PERMISSIONS.USERS_VIEW,
    'roles': PERMISSIONS.ROLES_VIEW,
    'reports': PERMISSIONS.REPORTS_VIEW,
    'settings': PERMISSIONS.SETTINGS_VIEW
  };
  
  return hasPermission(userRoles, modulePermissions[module]);
};

export const canEditModule = (userRoles, module) => {
  const modulePermissions = {
    'materials': PERMISSIONS.MATERIALS_EDIT,
    'categories': PERMISSIONS.CATEGORIES_EDIT,
    'movements': PERMISSIONS.MOVEMENTS_EDIT,
    'production': PERMISSIONS.PRODUCTION_EDIT,
    'work_orders': PERMISSIONS.WORK_ORDERS_EDIT,
    'users': PERMISSIONS.USERS_EDIT,
    'roles': PERMISSIONS.ROLES_EDIT,
    'settings': PERMISSIONS.SETTINGS_EDIT
  };
  
  return hasPermission(userRoles, modulePermissions[module]);
};

export const canDeleteModule = (userRoles, module) => {
  const modulePermissions = {
    'materials': PERMISSIONS.MATERIALS_DELETE,
    'categories': PERMISSIONS.CATEGORIES_DELETE,
    'movements': PERMISSIONS.MOVEMENTS_DELETE,
    'production': PERMISSIONS.PRODUCTION_DELETE,
    'work_orders': PERMISSIONS.WORK_ORDERS_DELETE,
    'users': PERMISSIONS.USERS_DELETE,
    'roles': PERMISSIONS.ROLES_DELETE
  };
  
  return hasPermission(userRoles, modulePermissions[module]);
};

export const canCreateModule = (userRoles, module) => {
  const modulePermissions = {
    'materials': PERMISSIONS.MATERIALS_CREATE,
    'categories': PERMISSIONS.CATEGORIES_CREATE,
    'movements': PERMISSIONS.MOVEMENTS_CREATE,
    'production': PERMISSIONS.PRODUCTION_CREATE,
    'work_orders': PERMISSIONS.WORK_ORDERS_CREATE,
    'users': PERMISSIONS.USERS_CREATE,
    'roles': PERMISSIONS.ROLES_CREATE
  };
  
  return hasPermission(userRoles, modulePermissions[module]);
};

const permissions = {
  PERMISSIONS,
  ROLE_PERMISSIONS,
  hasPermission,
  hasAnyPermission,
  hasAllPermissions,
  isAdmin,
  getUserPermissions,
  canViewModule,
  canEditModule,
  canDeleteModule,
  canCreateModule
};

export default permissions; 