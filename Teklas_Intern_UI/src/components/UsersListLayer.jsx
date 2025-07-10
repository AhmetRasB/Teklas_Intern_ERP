import React, { useState, useEffect } from 'react';
import { Icon } from '@iconify/react/dist/iconify.js';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const UsersListLayer = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [pageSize, setPageSize] = useState(10);
  const [selectedUsers, setSelectedUsers] = useState([]);
  const [editingUser, setEditingUser] = useState(null);
  const [showUserModal, setShowUserModal] = useState(false);
  const [roles, setRoles] = useState([]);

  const { isAdmin, user: currentUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    fetchUsers();
    fetchRoles();
  }, []);

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${BASE_URL}/api/users`);
      console.log('Users fetched:', response.data);
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
      // Backend returns { success: true, data: [...roles...], pagination: {...} }
      const roles = response.data?.data || response.data || [];
      setRoles(Array.isArray(roles) ? roles : []);
    } catch (error) {
      console.error('Error fetching roles:', error);
      setRoles([]);
    }
  };

  const handleDelete = async (userId) => {
    // Prevent admin from deleting themselves
    if (userId === currentUser?.id) {
      MySwal.fire({
        title: 'Hata',
        text: 'Kendi hesabınızı silemezsiniz.',
        icon: 'error'
      });
      return;
    }

    const result = await MySwal.fire({
      title: 'Kullanıcıyı Sil',
      text: 'Bu kullanıcıyı silmek istediğinizden emin misiniz?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/users/${userId}`);
        await fetchUsers();
        MySwal.fire({
          title: 'Başarılı',
          text: 'Kullanıcı başarıyla silindi.',
          icon: 'success',
          timer: 1500
        });
      } catch (error) {
        console.error('Error deleting user:', error);
        MySwal.fire({
          title: 'Hata',
          text: 'Kullanıcı silinirken bir hata oluştu.',
          icon: 'error'
        });
      }
    }
  };

  const handleEdit = (user) => {
    navigate('/view-profile', { state: { editUser: user, isAdmin: true } });
  };

  const handleView = (user) => {
    navigate('/view-profile', { state: { viewUser: user, isAdmin: true } });
  };

  const handleStatusToggle = async (userId, currentStatus) => {
    // Prevent admin from deactivating themselves
    if (userId === currentUser?.id && currentStatus === 1) {
      MySwal.fire({
        title: 'Hata',
        text: 'Kendi hesabınızı deaktif edemezsiniz.',
        icon: 'error'
      });
      return;
    }

    const newStatus = currentStatus === 1 ? 0 : 1;
    const statusText = newStatus === 1 ? 'aktif' : 'pasif';

    const result = await MySwal.fire({
      title: 'Durum Değiştir',
      text: `Bu kullanıcıyı ${statusText} yapmak istediğinizden emin misiniz?`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: `Evet, ${statusText.charAt(0).toUpperCase() + statusText.slice(1)} Yap`,
      cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
      try {
        await axios.put(`${BASE_URL}/api/users/${userId}/status`, { status: newStatus });
        await fetchUsers();
        MySwal.fire({
          title: 'Başarılı',
          text: `Kullanıcı durumu ${statusText} olarak güncellendi.`,
          icon: 'success',
          timer: 1500
        });
      } catch (error) {
        console.error('Error updating user status:', error);
        MySwal.fire({
          title: 'Hata',
          text: 'Kullanıcı durumu güncellenirken bir hata oluştu.',
          icon: 'error'
        });
      }
    }
  };

  const handleBulkDelete = async () => {
    if (selectedUsers.length === 0) {
      MySwal.fire({
        title: 'Uyarı',
        text: 'Lütfen silmek istediğiniz kullanıcıları seçin.',
        icon: 'warning'
      });
      return;
    }

    // Check if trying to delete current user
    if (selectedUsers.includes(currentUser?.id)) {
      MySwal.fire({
        title: 'Hata',
        text: 'Kendi hesabınızı silemezsiniz.',
        icon: 'error'
      });
      return;
    }

    const result = await MySwal.fire({
      title: 'Toplu Silme',
      text: `${selectedUsers.length} kullanıcıyı silmek istediğinizden emin misiniz?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/users/bulk`, {
          data: { userIds: selectedUsers }
        });
        setSelectedUsers([]);
        await fetchUsers();
        MySwal.fire({
          title: 'Başarılı',
          text: 'Seçilen kullanıcılar başarıyla silindi.',
          icon: 'success',
          timer: 1500
        });
      } catch (error) {
        console.error('Error bulk deleting users:', error);
        MySwal.fire({
          title: 'Hata',
          text: 'Kullanıcılar silinirken bir hata oluştu.',
          icon: 'error'
        });
      }
    }
  };

  // Calculate filtered users - ensure users is always an array
  const filteredUsers = (users || []).filter(user => {
    const matchesSearch = 
      user.username?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.firstName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.lastName?.toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesStatus = statusFilter === '' || 
      (statusFilter === 'Active' && user.status === 1) ||
      (statusFilter === 'Inactive' && user.status !== 1);
    
    return matchesSearch && matchesStatus;
  });

  const paginatedUsers = filteredUsers.slice(0, pageSize);

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

  const formatDate = (dateString) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('tr-TR');
  };

  const getUserRoles = (roleNames) => {
    if (!roleNames || roleNames.length === 0) return 'Rol Atanmamış';
    return roleNames.join(', ');
  };

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
            <option value={100}>100</option>
                    </select>
          
          <form className="navbar-search" onSubmit={(e) => e.preventDefault()}>
                        <input
                            type="text"
                            className="bg-base h-40-px w-auto"
                            name="search"
              placeholder="Ara..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
                        />
                        <Icon icon="ion:search-outline" className="icon" />
                    </form>
          
          <select 
            className="form-select form-select-sm w-auto ps-12 py-6 radius-12 h-40-px"
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
          >
            <option value="">Tüm Durumlar</option>
            <option value="Active">Aktif</option>
            <option value="Inactive">Pasif</option>
                    </select>

          {selectedUsers.length > 0 && (
            <button
              type="button"
              className="btn btn-danger text-sm btn-sm px-12 py-12 radius-8 d-flex align-items-center gap-2"
              onClick={handleBulkDelete}
            >
              <Icon icon="fluent:delete-24-regular" className="icon text-xl line-height-1" />
              Seçilenleri Sil ({selectedUsers.length})
            </button>
          )}
                </div>
        
        <div className="d-flex gap-2">
                <Link
                    to="/add-user"
                    className="btn btn-primary text-sm btn-sm px-12 py-12 radius-8 d-flex align-items-center gap-2"
                >
            <Icon icon="ic:baseline-plus" className="icon text-xl line-height-1" />
            Yeni Kullanıcı
          </Link>
          
          <Link
            to="/assign-role"
            className="btn btn-info text-sm btn-sm px-12 py-12 radius-8 d-flex align-items-center gap-2"
          >
            <Icon icon="mdi:account-key" className="icon text-xl line-height-1" />
            Rol Ata
                </Link>
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
                <th scope="col">Kayıt Tarihi</th>
                <th scope="col">Kullanıcı</th>
                <th scope="col">E-posta</th>
                <th scope="col">Telefon</th>
                <th scope="col">Roller</th>
                <th scope="col" className="text-center">Durum</th>
                <th scope="col" className="text-center">İşlemler</th>
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
                  <td>{formatDate(user.createDate)}</td>
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
                                <td>
                                    <span className="text-md mb-0 fw-normal text-secondary-light">
                      {user.email}
                                    </span>
                    {user.isEmailConfirmed && (
                      <Icon icon="mdi:check-circle" className="text-success ms-2" title="E-posta Doğrulanmış" />
                    )}
                                </td>
                  <td>{user.phoneNumber || '-'}</td>
                  <td>
                    <span className="text-sm text-secondary-light">
                      {getUserRoles(user.roleNames)}
                                    </span>
                                </td>
                                <td className="text-center">
                                        <button
                                            type="button"
                      className={`border px-16 py-4 radius-4 fw-medium text-sm ${
                        user.status === 1
                          ? 'bg-success-focus text-success-600 border-success-main'
                          : 'bg-neutral-200 text-neutral-600 border-neutral-400'
                      }`}
                      onClick={() => handleStatusToggle(user.id, user.status)}
                      disabled={!isAdmin()}
                    >
                      {user.status === 1 ? 'Aktif' : 'Pasif'}
                                        </button>
                                </td>
                                <td className="text-center">
                                    <div className="d-flex align-items-center gap-10 justify-content-center">
                                        <button
                                            type="button"
                                            className="bg-info-focus bg-hover-info-200 text-info-600 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle"
                        onClick={() => handleView(user)}
                        title="Görüntüle"
                                        >
                        <Icon icon="majesticons:eye-line" className="icon text-xl" />
                                        </button>
                      
                      {isAdmin() && (
                        <>
                                        <button
                                            type="button"
                                            className="bg-success-focus text-success-600 bg-hover-success-200 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle"
                            onClick={() => handleEdit(user)}
                            title="Düzenle"
                                        >
                                            <Icon icon="lucide:edit" className="menu-icon" />
                                        </button>
                          
                                        <button
                                            type="button"
                            className="bg-danger-focus bg-hover-danger-200 text-danger-600 fw-medium w-40-px h-40-px d-flex justify-content-center align-items-center rounded-circle"
                            onClick={() => handleDelete(user.id)}
                            disabled={user.id === currentUser?.id}
                            title={user.id === currentUser?.id ? 'Kendi hesabınızı silemezsiniz' : 'Sil'}
                                        >
                            <Icon icon="fluent:delete-24-regular" className="menu-icon" />
                                        </button>
                        </>
                      )}
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
                {searchTerm || statusFilter ? 'Arama kriterlerinize uygun kullanıcı bulunamadı.' : 'Henüz kullanıcı bulunmuyor.'}
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

export default UsersListLayer;