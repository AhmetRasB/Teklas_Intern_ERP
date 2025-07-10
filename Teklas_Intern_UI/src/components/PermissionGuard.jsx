import React from 'react';
import { useAuth } from '../contexts/AuthContext';
import { hasPermission, hasAnyPermission, hasAllPermissions, isAdmin } from '../utils/permissions';

const PermissionGuard = ({ 
  children, 
  permission, 
  permissions, 
  requireAll = false, 
  adminOnly = false,
  roles = null,
  fallback = null 
}) => {
  const { user } = useAuth();
  
  // Get user roles
  const userRoles = user?.roleNames || [];
  
  // Check admin access
  if (adminOnly && !isAdmin(userRoles)) {
    return fallback;
  }
  
  // Check specific roles
  if (roles && roles.length > 0) {
    const hasRequiredRole = roles.some(role => userRoles.includes(role));
    if (!hasRequiredRole) {
      return fallback;
    }
  }
  
  // Check single permission
  if (permission && !hasPermission(userRoles, permission)) {
    return fallback;
  }
  
  // Check multiple permissions
  if (permissions && permissions.length > 0) {
    const hasRequiredPermissions = requireAll 
      ? hasAllPermissions(userRoles, permissions)
      : hasAnyPermission(userRoles, permissions);
    
    if (!hasRequiredPermissions) {
      return fallback;
    }
  }
  
  return children;
};

export default PermissionGuard; 