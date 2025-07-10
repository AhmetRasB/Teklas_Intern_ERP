import React, { useState, useEffect } from 'react';
import { Icon } from '@iconify/react/dist/iconify.js';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const AssignRoleLayer = () => {
  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [roleFilter, setRoleFilter] = useState('');
  const [pageSize, setPageSize] = useState(10);
  const [selectedUsers, setSelectedUsers] = useState([]);
  const [assigningRoles, setAssigningRoles] = useState({});

  const { user: currentUser, isAdmin } = useAuth();

  useEffect(() => {
    if (isAdmin()) {
      fetchUsers();
      fetchRoles();
    } else {
      setLoading(false);
    }
  }, [isAdmin]);

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${BASE_URL}/api/users`);
      console.log('Users fetched for role assignment:', response.data);
      // Backend returns { success: true, data: [...users...], pagination: {...} }
      const users = response.data?.data || response.data || [];
      setUsers(Array.isArray(users) ? users : []);
    } catch (error) {
      console.error('Error fetching users:', error);
      MySwal.fire({
        title: 'Hata',
        text: 'Kullanıcılar yüklenirken bir hata oluştu.',
        icon: 'error'
      });
      setUsers([]);
    } finally {
      setLoading(false);
    }
  };

  const fetchRoles = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/roles`);
      console.log('Roles fetched:', response.data);
      // Backend returns { success: true, data: [...roles...], pagination: {...} }
      const roles = response.data?.data || response.data || [];
      setRoles(Array.isArray(roles) ? roles : []);
    } catch (error) {
      console.error('Error fetching roles:', error);
      MySwal.fire({
        title: 'Hata',
        text: 'Roller yüklenirken bir hata oluştu.',
        icon: 'error'
      });
      setRoles([]);
    }
  };

  const handleAssignRole = async (userId, roleName) => {
    // Prevent modifying current user's roles in certain cases
    if (userId === currentUser?.id && roleName === 'Admin') {
      MySwal.fire({
        title: 'Uyarı',
        text: 'Kendi admin rolünüzü değiştiremezsiniz.',
        icon: 'warning'
      });
      return;
    }

    setAssigningRoles(prev => ({ ...prev, [userId]: true }));

    try {
      const user = users.find(u => u.id === userId);
      const currentRoles = user?.roleNames || [];
      
      let newRoles;
      if (currentRoles.includes(roleName)) {
        // Remove role
        newRoles = currentRoles.filter(role => role !== roleName);
        
        // Prevent removing the last admin role if it's the current user
        if (userId === currentUser?.id && roleName === 'Admin' && !newRoles.some(role => role === 'Admin' || role === 'SuperAdmin')) {
          MySwal.fire({
            title: 'Hata',
            text: 'Son admin rolünüzü kaldıramazsınız.',
            icon: 'error'
          });
          return;
        }
      } else {
        // Add role
        newRoles = [...currentRoles, roleName];
      }

      await axios.post(`${BASE_URL}/api/users/${userId}/assign-roles`, {
        roleNames: newRoles
      });

      await fetchUsers(); // Refresh user list
      
      MySwal.fire({
        title: 'Başarılı',
        text: `Rol ${currentRoles.includes(roleName) ? 'kaldırıldı' : 'atandı'}.`,
        icon: 'success',
        timer: 1500
      });
    } catch (error) {
      console.error('Error assigning role:', error);
      MySwal.fire({
        title: 'Hata',
        text: 'Rol ataması sırasında bir hata oluştu.',
        icon: 'error'
      });
    } finally {
      setAssigningRoles(prev => ({ ...prev, [userId]: false }));
    }
  };

  // Calculate filtered users - ensure users is always an array
  const filteredUsers = (users || []).filter(user => {
    const matchesSearch = 
      user.username?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.firstName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.lastName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email?.toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesRole = roleFilter === '' || 
      (user.roleNames && user.roleNames.includes(roleFilter));
    
    return matchesSearch && matchesRole;
  });

  const paginatedUsers = filteredUsers.slice(0, pageSize);

  const handleBulkRoleAssignment = async () => {
    if (selectedUsers.length === 0) {
      MySwal.fire({
        title: 'Uyarı',
        text: 'Lütfen kullanıcı seçin.',
        icon: 'warning'
      });
      return;
    }

    const { value: selectedRole } = await MySwal.fire({
      title: 'Toplu Rol Ataması',
      input: 'select',
      inputOptions: roles.reduce((options, role) => {
        options[role.name] = role.name;
        return options;
      }, {}),
      inputPlaceholder: 'Bir rol seçin',
      showCancelButton: true,
      confirmButtonText: 'Ata',
      cancelButtonText: 'İptal',
      inputValidator: (value) => {
        if (!value) {
          return 'Lütfen bir rol seçin';
        }
      }
    });

    if (selectedRole) {
      try {
        const promises = selectedUsers.map(userId => {
          const user = users.find(u => u.id === userId);
          const currentRoles = user?.roleNames || [];
          const newRoles = currentRoles.includes(selectedRole) ? currentRoles : [...currentRoles, selectedRole];
          
          return axios.post(`${BASE_URL}/api/users/${userId}/assign-roles`, {
            roleNames: newRoles
          });
        });

        await Promise.all(promises);
        await fetchUsers();
        setSelectedUsers([]);

        MySwal.fire({
          title: 'Başarılı',
          text: `${selectedUsers.length} kullanıcıya ${selectedRole} rolü atandı.`,
          icon: 'success',
          timer: 1500
        });
      } catch (error) {
        console.error('Error in bulk role assignment:', error);
        MySwal.fire({
          title: 'Hata',
          text: 'Toplu rol ataması sırasında bir hata oluştu.',
          icon: 'error'
        });
      }
    }
  };

  const handleSelectAll = (e) => {
    if (e.target.checked) {
      const allUserIds = filteredUsers.map(user => user.id);
      setSelectedUsers(allUserIds);
    } else {
      setSelectedUsers([]);
    }
  };

  const handleSelectUser = (userId) => {
    setSelectedUsers(prev => 
      prev.includes(userId) 
        ? prev.filter(id => id !== userId)
        : [...prev, userId]
    );
  };

  const getUserRoles = (roleNames) => {
    if (!roleNames || roleNames.length === 0) return 'Rol Atanmamış';
    return roleNames.join(', ');
  };

  if (!isAdmin()) {
    return (
      <div className="text-center py-5">
        <Icon icon="mdi:shield-alert" className="text-warning mb-3" style={{ fontSize: '3rem' }} />
        <h4 className="text-warning">Erişim Engellendi</h4>
        <p className="text-secondary-light">Bu sayfaya erişim yetkiniz bulunmuyor.</p>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '400px' }}>
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

    return (
        <div className="card h-100 p-0 radius-12">
            <div className="card-header border-bottom bg-base py-16 px-24 d-flex align-items-center flex-wrap gap-3 justify-content-between">
                <div className="d-flex align-items-center flex-wrap gap-3">
          <span className="text-md fw-medium text-secondary-light mb-0">Göster</span>
                    <select
                        className="form-select form-select-sm w-auto ps-12 py-6 radius-12 h-40-px"
            value={pageSize}
            onChange={(e) => setPageSize(Number(e.target.value))}
          >
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={25}>25</option>
            <option value={50}>50</option>
                    </select>
          
          <form className="navbar-search" onSubmit={(e) => e.preventDefault()}>
                        <input
                            type="text"
                            className="bg-base h-40-px w-auto"
                            name="search"
              placeholder="Kullanıcı ara..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
                        />
                        <Icon icon="ion:search-outline" className="icon" />
                    </form>
          
                    <select
                        className="form-select form-select-sm w-auto ps-12 py-6 radius-12 h-40-px"
            value={roleFilter}
            onChange={(e) => setRoleFilter(e.target.value)}
          >
            <option value="">Tüm Roller</option>
            {roles.map(role => (
              <option key={role.id} value={role.name}>{role.name}</option>
            ))}
                    </select>

          {selectedUsers.length > 0 && (
            <button
              type="button"
              className="btn btn-info text-sm btn-sm px-12 py-12 radius-8 d-flex align-items-center gap-2"
              onClick={handleBulkRoleAssignment}
            >
              <Icon icon="mdi:account-key" className="icon text-xl line-height-1" />
              Toplu Rol Ata ({selectedUsers.length})
            </button>
          )}
          
          <button
            type="button"
            className="btn btn-success text-sm btn-sm px-12 py-12 radius-8 d-flex align-items-center gap-2"
            onClick={() => window.location.href = '/create-role'}
          >
            <Icon icon="mdi:plus" className="icon text-xl line-height-1" />
            Yeni Rol Oluştur
          </button>
                </div>
            </div>
      
            <div className="card-body p-24">
                <div className="table-responsive scroll-sm">
                    <table className="table bordered-table sm-table mb-0">
                        <thead>
                            <tr>
                                <th scope="col">
                                    <div className="d-flex align-items-center gap-10">
                                        <div className="form-check style-check d-flex align-items-center">
                                            <input
                                                className="form-check-input radius-4 border input-form-dark"
                                                type="checkbox"
                                                id="selectAll"
                        checked={selectedUsers.length === filteredUsers.length && filteredUsers.length > 0}
                        onChange={handleSelectAll}
                                            />
                                        </div>
                                        S.L
                                    </div>
                                </th>
                <th scope="col">Kullanıcı</th>
                <th scope="col" className="text-center">Mevcut Roller</th>
                <th scope="col" className="text-center">Rol Ataması</th>
                            </tr>
                        </thead>
                        <tbody>
              {paginatedUsers.map((user, index) => (
                <tr key={user.id}>
                                <td>
                                    <div className="d-flex align-items-center gap-10">
                                        <div className="form-check style-check d-flex align-items-center">
                                            <input
                                                className="form-check-input radius-4 border border-neutral-400"
                                                type="checkbox"
                          checked={selectedUsers.includes(user.id)}
                          onChange={() => handleSelectUser(user.id)}
                                            />
                                        </div>
                      {String(index + 1).padStart(2, '0')}
                                    </div>
                                </td>
                                <td>
                                    <div className="d-flex align-items-center">
                      <div className="w-40-px h-40-px rounded-circle flex-shrink-0 me-12 overflow-hidden bg-primary-50 d-flex align-items-center justify-content-center">
                        {user.profilePicture ? (
                          <img
                            src={user.profilePicture}
                            alt={user.fullName || user.username}
                            className="w-100 h-100 object-fit-cover"
                          />
                        ) : (
                          <Icon icon="mdi:account" className="text-primary text-xl" />
                        )}
                                    </div>
                                        <div className="flex-grow-1">
                        <span className="text-md mb-0 fw-medium text-secondary-light">
                          {user.fullName || `${user.firstName} ${user.lastName}` || user.username}
                                            </span>
                        <div className="text-sm text-secondary-light">@{user.username}</div>
                                        </div>
                                    </div>
                                </td>
                                <td className="text-center">
                    <div className="d-flex flex-wrap gap-1 justify-content-center">
                      {user.roleNames && user.roleNames.length > 0 ? (
                        user.roleNames.map((role, roleIndex) => (
                          <span 
                            key={roleIndex} 
                            className="badge bg-primary-50 text-primary-600 border border-primary-600 px-8 py-4 text-xs"
                          >
                            {role}
                                            </span>
                        ))
                      ) : (
                        <span className="text-secondary-light text-sm">Rol Atanmamış</span>
                      )}
                                    </div>
                                </td>
                                <td className="text-center">
                                    <div className="dropdown">
                                        <button
                        className="btn btn-outline-primary-600 px-18 py-11 dropdown-toggle"
                                            type="button"
                                            data-bs-toggle="dropdown"
                                            aria-expanded="false"
                        disabled={assigningRoles[user.id]}
                      >
                        {assigningRoles[user.id] ? (
                          <>
                            <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                            İşleniyor...
                          </>
                        ) : (
                          'Rol Ata/Kaldır'
                        )}
                                        </button>
                      <ul className="dropdown-menu">
                        {roles.map((role) => {
                          const hasRole = user.roleNames?.includes(role.name);
                          const isCurrentUserAdmin = user.id === currentUser?.id && role.name === 'Admin';
                          
                          return (
                            <li key={role.id}>
                                        <button
                                className={`dropdown-item px-16 py-8 rounded text-secondary-light bg-hover-neutral-200 text-hover-neutral-900 d-flex align-items-center gap-2 ${
                                  hasRole ? 'bg-primary-50 text-primary-600' : ''
                                }`}
                                onClick={() => handleAssignRole(user.id, role.name)}
                                disabled={isCurrentUserAdmin}
                                title={isCurrentUserAdmin ? 'Kendi admin rolünüzü değiştiremezsiniz' : ''}
                              >
                                <Icon 
                                  icon={hasRole ? 'mdi:check-circle' : 'mdi:circle-outline'} 
                                  className={hasRole ? 'text-success' : 'text-secondary'} 
                                />
                                {role.name}
                                        </button>
                                            </li>
                          );
                        })}
                        
                        {user.roleNames && user.roleNames.length > 0 && (
                          <>
                            <li><hr className="dropdown-divider" /></li>
                            <li>
                                        <button
                                className="dropdown-item px-16 py-8 rounded text-danger bg-hover-danger-100 d-flex align-items-center gap-2"
                                onClick={async () => {
                                  if (user.id === currentUser?.id) {
                                    MySwal.fire({
                                      title: 'Hata',
                                      text: 'Kendi rollerinizi kaldıramazsınız.',
                                      icon: 'error'
                                    });
                                    return;
                                  }
                                  
                                  const result = await MySwal.fire({
                                    title: 'Tüm Rolleri Kaldır',
                                    text: 'Bu kullanıcının tüm rollerini kaldırmak istediğinizden emin misiniz?',
                                    icon: 'warning',
                                    showCancelButton: true,
                                    confirmButtonText: 'Evet, Kaldır',
                                    cancelButtonText: 'İptal'
                                  });

                                  if (result.isConfirmed) {
                                    try {
                                      await axios.post(`${BASE_URL}/api/users/${user.id}/assign-roles`, {
                                        roleNames: []
                                      });
                                      await fetchUsers();
                                      MySwal.fire({
                                        title: 'Başarılı',
                                        text: 'Tüm roller kaldırıldı.',
                                        icon: 'success',
                                        timer: 1500
                                      });
                                    } catch (error) {
                                      MySwal.fire({
                                        title: 'Hata',
                                        text: 'Roller kaldırılırken bir hata oluştu.',
                                        icon: 'error'
                                      });
                                    }
                                  }
                                }}
                                disabled={user.id === currentUser?.id}
                              >
                                <Icon icon="mdi:delete-circle" className="text-danger" />
                                Tüm Rolleri Kaldır
                                        </button>
                                            </li>
                          </>
                        )}
                                        </ul>
                                    </div>
                                </td>
                            </tr>
              ))}
                        </tbody>
                    </table>
          
          {filteredUsers.length === 0 && (
            <div className="text-center py-5">
              <Icon icon="mdi:account-search" className="text-secondary mb-3" style={{ fontSize: '3rem' }} />
              <p className="text-secondary-light">
                {searchTerm || roleFilter ? 'Arama kriterlerinize uygun kullanıcı bulunamadı.' : 'Henüz kullanıcı bulunmuyor.'}
              </p>
            </div>
          )}
        </div>

        {filteredUsers.length > pageSize && (
          <div className="d-flex align-items-center justify-content-between flex-wrap gap-2 mt-24">
            <span className="text-md text-secondary-light fw-medium">
              Toplam {filteredUsers.length} kullanıcıdan {Math.min(pageSize, filteredUsers.length)} tanesi gösteriliyor
            </span>
            <button
              type="button"
              className="btn btn-outline-primary btn-sm"
              onClick={() => setPageSize(prev => prev + 10)}
            >
              Daha Fazla Göster
            </button>
          </div>
        )}
      </div>
    </div>
    );
};

export default AssignRoleLayer;