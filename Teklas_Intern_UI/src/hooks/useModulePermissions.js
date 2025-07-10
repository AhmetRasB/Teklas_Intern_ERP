import { useState, useEffect } from 'react';

const useModulePermissions = () => {
  const [permissions, setPermissions] = useState({});
  const [loading, setLoading] = useState(true);
  const [isAdmin, setIsAdmin] = useState(false);

  useEffect(() => {
    const loadPermissions = () => {
      try {
        const userDataString = localStorage.getItem('userData');
        if (!userDataString) {
          setLoading(false);
          return;
        }

        const userData = JSON.parse(userDataString);
        const userPermissions = userData.permissions || {};

        // Check if user is admin
        const userRoles = userData.roles || userData.roleNames || [];
        const adminStatus = userRoles.some(role => 
          role.toLowerCase().includes('admin') || 
          role.toLowerCase() === 'admin'
        );
        
        setIsAdmin(adminStatus);
        setPermissions(userPermissions);
      } catch (error) {
        console.error('Error loading permissions:', error);
        setPermissions({});
        setIsAdmin(false);
      } finally {
        setLoading(false);
      }
    };

    loadPermissions();
  }, []);

  const hasPermission = (module, level = 'read') => {
    if (loading) return false;
    
    // Admin bypass: admins always have full access
    if (isAdmin) return true;
    
    const modulePermission = permissions[module];
    if (!modulePermission) return false;

    if (level === 'read') {
      return modulePermission.read || modulePermission.write;
    }
    if (level === 'write') {
      return modulePermission.write;
    }
    
    return false;
  };

  const canRead = (module) => {
    // Admin bypass: admins always have full access
    if (isAdmin) return true;
    return hasPermission(module, 'read');
  };
  
  const canWrite = (module) => {
    // Admin bypass: admins always have full access  
    if (isAdmin) return true;
    return hasPermission(module, 'write');
  };

  return {
    permissions,
    loading,
    isAdmin,
    hasPermission,
    canRead,
    canWrite
  };
};

export default useModulePermissions; 