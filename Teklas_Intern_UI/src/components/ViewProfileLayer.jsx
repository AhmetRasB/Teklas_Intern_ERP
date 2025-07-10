import React, { useState, useEffect } from 'react';
import { Icon } from '@iconify/react/dist/iconify.js';
import { useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const ViewProfileLayer = () => {
  const [user, setUser] = useState(null);
  const [editedUser, setEditedUser] = useState({});
  const [roles, setRoles] = useState([]);
  const [selectedRoles, setSelectedRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [activeTab, setActiveTab] = useState('edit-profile');
  const [passwordData, setPasswordData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [passwordVisible, setPasswordVisible] = useState({
    current: false,
    new: false,
    confirm: false
  });
  const [modulePermissions, setModulePermissions] = useState({
    materialManagement: false,
    productionManagement: false,
    purchasingManagement: false,
    salesManagement: false,
    warehouseManagement: false,
    reportManagement: false,
    userManagement: false
  });

  const { user: currentUser, isAdmin, changePassword } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Determine context: admin editing user, admin viewing user, or user viewing own profile
  const editUser = location.state?.editUser;
  const viewUser = location.state?.viewUser;
  const isAdminContext = location.state?.isAdmin;
  const isEditing = Boolean(editUser);
  const isViewing = Boolean(viewUser) && !isEditing;
  const isOwnProfile = !editUser && !viewUser;

  useEffect(() => {
    let targetUser = null;
    
    if (editUser) {
      setUser(editUser);
      setEditedUser(editUser);
      setSelectedRoles(editUser.roleNames || []);
      targetUser = editUser;
    } else if (viewUser) {
      setUser(viewUser);
      setEditedUser(viewUser);
      targetUser = viewUser;
    } else {
      setUser(currentUser);
      setEditedUser(currentUser || {});
      targetUser = currentUser;
    }
    
    // Load module permissions for the target user from database
    if (targetUser?.id) {
      loadUserPermissions(targetUser.id);
    }
    
    if (isAdmin()) {
      fetchRoles();
    }
    
    setLoading(false);
  }, [editUser, viewUser, currentUser, isAdmin]);

  const fetchRoles = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/roles`);
      console.log('Roles fetched:', response.data);
      // Backend returns { success: true, data: [...roles...], pagination: {...} }
      const roles = response.data?.data || response.data || [];
      setRoles(Array.isArray(roles) ? roles : []);
    } catch (error) {
      console.error('Error fetching roles:', error);
      setRoles([]);
    }
  };

  const loadUserPermissions = async (userId) => {
    try {
      // Load permissions from localStorage (original localStorage-based system)
      const userDataString = localStorage.getItem('userData');
      if (!userDataString) return;

      const userData = JSON.parse(userDataString);
      const permissions = userData.permissions || {};

      // Convert localStorage format to frontend format
      const frontendPermissions = {
        materialManagement: permissions.materialManagement?.write || false,
        productionManagement: permissions.productionManagement?.write || false,
        purchasingManagement: permissions.purchasingManagement?.write || false,
        salesManagement: permissions.salesManagement?.write || false,
        warehouseManagement: permissions.warehouseManagement?.write || false,
        reportManagement: permissions.reportManagement?.write || false,
        userManagement: permissions.userManagement?.write || false
      };

      // If user is admin, give them full access to everything
      const userRoles = userData.roles || userData.roleNames || [];
      const isAdmin = userRoles.some(role => 
        role.toLowerCase().includes('admin') || 
        role.toLowerCase() === 'admin'
      );

      if (isAdmin) {
        // Admin gets full access to all modules
        Object.keys(frontendPermissions).forEach(key => {
          frontendPermissions[key] = true;
        });
      }

      setModulePermissions(frontendPermissions);
    } catch (error) {
      console.error('Error loading user permissions:', error);
      // Keep default permissions on error
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditedUser(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handlePasswordChange = (e) => {
    const { name, value } = e.target;
    setPasswordData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const togglePasswordVisibility = (field) => {
    setPasswordVisible(prev => ({
      ...prev,
      [field]: !prev[field]
    }));
  };

  const handleSaveProfile = async () => {
    if (!editedUser.firstName || !editedUser.lastName || !editedUser.email) {
      MySwal.fire({
        title: 'Hata',
        text: 'Lütfen tüm zorunlu alanları doldurun.',
        icon: 'error'
      });
      return;
    }

    setSaving(true);
    try {
      const endpoint = isAdminContext && editUser 
        ? `${BASE_URL}/api/users/${editUser.id}`
        : `${BASE_URL}/api/users/${currentUser.id}`;

      const payload = {
        ...editedUser,
        roleNames: isAdminContext && selectedRoles.length > 0 ? selectedRoles : undefined
      };

      await axios.put(endpoint, payload);

      MySwal.fire({
        title: 'Başarılı',
        text: 'Profil başarıyla güncellendi.',
        icon: 'success',
        timer: 1500
      });

      if (isAdminContext && editUser) {
        navigate('/users-list');
      } else {
        // Refresh current user data
        window.location.reload();
      }
    } catch (error) {
      console.error('Error updating profile:', error);
      MySwal.fire({
        title: 'Hata',
        text: 'Profil güncellenirken bir hata oluştu.',
        icon: 'error'
      });
    } finally {
      setSaving(false);
    }
  };

  const handleChangePassword = async () => {
    if (!passwordData.currentPassword || !passwordData.newPassword || !passwordData.confirmPassword) {
      MySwal.fire({
        title: 'Hata',
        text: 'Lütfen tüm şifre alanlarını doldurun.',
        icon: 'error'
      });
      return;
    }

    if (passwordData.newPassword !== passwordData.confirmPassword) {
      MySwal.fire({
        title: 'Hata',
        text: 'Yeni şifreler eşleşmiyor.',
        icon: 'error'
      });
      return;
    }

    if (passwordData.newPassword.length < 6) {
      MySwal.fire({
        title: 'Hata',
        text: 'Yeni şifre en az 6 karakter olmalıdır.',
        icon: 'error'
      });
      return;
    }

    setSaving(true);
    try {
      const result = await changePassword(passwordData.currentPassword, passwordData.newPassword);
      
      if (result.success) {
        MySwal.fire({
          title: 'Başarılı',
          text: 'Şifre başarıyla değiştirildi.',
          icon: 'success',
          timer: 1500
        });
        setPasswordData({
          currentPassword: '',
          newPassword: '',
          confirmPassword: ''
        });
      } else {
        MySwal.fire({
          title: 'Hata',
          text: result.error || 'Şifre değiştirilirken bir hata oluştu.',
          icon: 'error'
        });
      }
    } catch (error) {
      console.error('Error changing password:', error);
      MySwal.fire({
        title: 'Hata',
        text: 'Şifre değiştirilirken bir hata oluştu.',
        icon: 'error'
      });
    } finally {
      setSaving(false);
    }
  };

  const handleRoleSelect = (roleName) => {
    setSelectedRoles([roleName]); // Single role selection
  };



  const formatDate = (dateString) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('tr-TR');
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

  if (!user) {
    return (
      <div className="text-center py-5">
        <Icon icon="mdi:account-question" className="text-secondary mb-3" style={{ fontSize: '3rem' }} />
        <p className="text-secondary-light">Kullanıcı bilgileri bulunamadı.</p>
      </div>
    );
  }

    return (
        <div className="row gy-4">
      {/* Profile Info Sidebar */}
            <div className="col-lg-4">
                <div className="user-grid-card position-relative border radius-16 overflow-hidden bg-base h-100">
                    <img
                        src="assets/images/user-grid/user-grid-bg1.png"
            alt="Background"
                        className="w-100 object-fit-cover"
                    />
          <div className="pb-24 ms-16 mb-24 me-16 mt--100">
                        <div className="text-center border border-top-0 border-start-0 border-end-0">
              <div className="w-200-px h-200-px rounded-circle mx-auto overflow-hidden bg-primary-50 d-flex align-items-center justify-content-center border border-white border-width-2-px">
                {user.profilePicture ? (
                  <img
                    src={user.profilePicture}
                    alt={user.fullName || user.username}
                    className="w-100 h-100 object-fit-cover"
                  />
                ) : (
                  <Icon icon="mdi:account" className="text-primary" style={{ fontSize: '4rem' }} />
                )}
              </div>
              <h6 className="mb-0 mt-16">{user.fullName || `${user.firstName} ${user.lastName}` || user.username}</h6>
              <span className="text-secondary-light mb-16">{user.email}</span>
              {user.roleNames && user.roleNames.length > 0 && (
                <div className="d-flex flex-wrap gap-1 justify-content-center mt-2">
                  {user.roleNames.map((role, index) => (
                    <span key={index} className="badge bg-primary-50 text-primary-600 border border-primary-600 px-8 py-4 text-xs">
                      {role}
                    </span>
                  ))}
                </div>
              )}
                        </div>
            
                        <div className="mt-24">
              <h6 className="text-xl mb-16">Kişisel Bilgiler</h6>
                            <ul>
                                <li className="d-flex align-items-center gap-1 mb-12">
                                    <span className="w-30 text-md fw-semibold text-primary-light">
                    Kullanıcı Adı
                                    </span>
                                    <span className="w-70 text-secondary-light fw-medium">
                    : {user.username}
                                    </span>
                                </li>
                                <li className="d-flex align-items-center gap-1 mb-12">
                                    <span className="w-30 text-md fw-semibold text-primary-light">
                    E-posta
                                    </span>
                                    <span className="w-70 text-secondary-light fw-medium">
                    : {user.email}
                    {user.isEmailConfirmed && (
                      <Icon icon="mdi:check-circle" className="text-success ms-2" title="Doğrulanmış" />
                    )}
                                    </span>
                                </li>
                                <li className="d-flex align-items-center gap-1 mb-12">
                                    <span className="w-30 text-md fw-semibold text-primary-light">
                    Telefon
                                    </span>
                                    <span className="w-70 text-secondary-light fw-medium">
                    : {user.phoneNumber || '-'}
                                    </span>
                                </li>
                                <li className="d-flex align-items-center gap-1 mb-12">
                                    <span className="w-30 text-md fw-semibold text-primary-light">
                    Kayıt Tarihi
                                    </span>
                                    <span className="w-70 text-secondary-light fw-medium">
                    : {formatDate(user.createDate)}
                                    </span>
                                </li>
                                <li className="d-flex align-items-center gap-1 mb-12">
                                    <span className="w-30 text-md fw-semibold text-primary-light">
                    Son Giriş
                                    </span>
                                    <span className="w-70 text-secondary-light fw-medium">
                    : {formatDate(user.lastLoginDate)}
                                    </span>
                                </li>
                                <li className="d-flex align-items-center gap-1">
                                    <span className="w-30 text-md fw-semibold text-primary-light">
                    Durum
                                    </span>
                                    <span className="w-70 text-secondary-light fw-medium">
                    : <span className={`badge ${user.status === 1 ? 'bg-success-focus text-success-600' : 'bg-neutral-200 text-neutral-600'}`}>
                      {user.status === 1 ? 'Aktif' : 'Pasif'}
                    </span>
                                    </span>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>

      {/* Profile Edit Section */}
            <div className="col-lg-8">
                <div className="card h-100">
                    <div className="card-body p-24">
            {/* Navigation Tabs */}
            <ul className="nav border-gradient-tab nav-pills mb-20 d-inline-flex" role="tablist">
                            <li className="nav-item" role="presentation">
                                <button
                  className={`nav-link d-flex align-items-center px-24 ${activeTab === 'edit-profile' ? 'active' : ''}`}
                  onClick={() => setActiveTab('edit-profile')}
                                    type="button"
                  disabled={isViewing}
                                >
                  {isViewing ? 'Profil Bilgileri' : 'Profili Düzenle'}
                                </button>
                            </li>
              {(isOwnProfile || (isAdminContext && editUser)) && (
                            <li className="nav-item" role="presentation">
                                <button
                    className={`nav-link d-flex align-items-center px-24 ${activeTab === 'change-password' ? 'active' : ''}`}
                    onClick={() => setActiveTab('change-password')}
                                    type="button"
                                >
                    Şifre Değiştir
                                </button>
                            </li>
              )}
                            <li className="nav-item" role="presentation">
                                <button
                  className={`nav-link d-flex align-items-center px-24 ${activeTab === 'module-permissions' ? 'active' : ''}`}
                  onClick={() => setActiveTab('module-permissions')}
                                    type="button"
                                >
                  Modül Erişim İzinleri
                                </button>
                            </li>
                        </ul>

            {/* Tab Content */}
            <div className="tab-content">
              {/* Edit Profile Tab */}
              {activeTab === 'edit-profile' && (
                <div className="tab-pane fade show active">
                  <form onSubmit={(e) => { e.preventDefault(); handleSaveProfile(); }}>
                    <div className="row gy-3">
                      <div className="col-sm-6">
                        <label className="form-label text-secondary-light fw-semibold">
                          Ad <span className="text-danger">*</span>
                        </label>
                                            <input
                          type="text"
                          name="firstName"
                          className="form-control radius-8"
                          placeholder="Adınızı girin"
                          value={editedUser.firstName || ''}
                          onChange={handleInputChange}
                          disabled={isViewing}
                          required
                        />
                      </div>
                      
                      <div className="col-sm-6">
                        <label className="form-label text-secondary-light fw-semibold">
                          Soyad <span className="text-danger">*</span>
                                            </label>
                        <input
                          type="text"
                          name="lastName"
                          className="form-control radius-8"
                          placeholder="Soyadınızı girin"
                          value={editedUser.lastName || ''}
                          onChange={handleInputChange}
                          disabled={isViewing}
                          required
                                            />
                                        </div>
                      
                                        <div className="col-sm-6">
                        <label className="form-label text-secondary-light fw-semibold">
                          Kullanıcı Adı
                                                </label>
                                                <input
                                                    type="text"
                          name="username"
                                                    className="form-control radius-8"
                          value={editedUser.username || ''}
                          disabled
                                                />
                                            </div>
                      
                                        <div className="col-sm-6">
                        <label className="form-label text-secondary-light fw-semibold">
                          E-posta <span className="text-danger">*</span>
                                                </label>
                                                <input
                                                    type="email"
                          name="email"
                                                    className="form-control radius-8"
                          placeholder="E-posta adresinizi girin"
                          value={editedUser.email || ''}
                          onChange={handleInputChange}
                          disabled={isViewing}
                          required
                                                />
                                            </div>
                      
                                        <div className="col-sm-6">
                        <label className="form-label text-secondary-light fw-semibold">
                          Telefon Numarası
                                                </label>
                                                <input
                          type="tel"
                          name="phoneNumber"
                                                    className="form-control radius-8"
                          placeholder="Telefon numaranızı girin"
                          value={editedUser.phoneNumber || ''}
                          onChange={handleInputChange}
                          disabled={isViewing}
                                                />
                                            </div>

                      {/* Role Assignment for Admin */}
                      {isAdminContext && editUser && (
                        <div className="col-12">
                          <label className="form-label text-secondary-light fw-semibold">
                            Rol <span className="text-danger">*</span>
                          </label>
                          <div className="d-flex flex-wrap gap-3">
                            {(roles || []).map((role) => (
                              <div key={role.id} className="form-check">
                                <input
                                  className="form-check-input"
                                  type="radio"
                                  name="userRole"
                                  id={`role-${role.id}`}
                                  checked={selectedRoles.includes(role.name)}
                                  onChange={() => handleRoleSelect(role.name)}
                                  disabled={isViewing}
                                />
                                <label className="form-check-label fw-medium" htmlFor={`role-${role.id}`}>
                                  {role.name}
                                </label>
                                        </div>
                            ))}
                            {(!roles || roles.length === 0) && (
                              <p className="text-secondary-light text-sm">Sistemde henüz rol tanımlanmamış.</p>
                            )}
                                            </div>
                                        </div>
                      )}
                    </div>

                    {!isViewing && (
                      <div className="d-flex align-items-center justify-content-center gap-3 mt-24">
                        <button
                          type="button"
                          className="btn rounded-pill btn-danger-100 text-danger-600 radius-8 px-20 py-11 fw-medium"
                          onClick={() => isAdminContext ? navigate('/users-list') : navigate('/')}
                        >
                          <Icon icon="mdi:close" className="me-2" />
                          İptal
                        </button>
                        <button
                          type="submit"
                          className="btn rounded-pill btn-primary-100 text-primary-600 radius-8 px-20 py-11 fw-medium"
                          disabled={saving}
                        >
                          {saving ? (
                            <>
                              <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                              Kaydediliyor...
                            </>
                          ) : (
                            <>
                              <Icon icon="mdi:content-save" className="me-2" />
                              Değişiklikleri Kaydet
                            </>
                          )}
                        </button>
                      </div>
                    )}
                  </form>
                </div>
              )}

              {/* Change Password Tab */}
              {activeTab === 'change-password' && (isOwnProfile || (isAdminContext && editUser)) && (
                <div className="tab-pane fade show active">
                  <form onSubmit={(e) => { e.preventDefault(); handleChangePassword(); }}>
                    <div className="row gy-3">
                      <div className="col-12">
                        <label className="form-label text-secondary-light fw-semibold">
                          Mevcut Şifre <span className="text-danger">*</span>
                                                </label>
                        <div className="position-relative">
                          <input
                            type={passwordVisible.current ? 'text' : 'password'}
                            name="currentPassword"
                            className="form-control radius-8"
                            placeholder="Mevcut şifrenizi girin"
                            value={passwordData.currentPassword}
                            onChange={handlePasswordChange}
                            required
                          />
                          <span
                            className="position-absolute end-0 top-50 translate-middle-y me-16 cursor-pointer"
                            onClick={() => togglePasswordVisibility('current')}
                          >
                            <Icon icon={passwordVisible.current ? 'mdi:eye-off' : 'mdi:eye'} />
                          </span>
                                            </div>
                                        </div>
                      
                      <div className="col-12">
                        <label className="form-label text-secondary-light fw-semibold">
                          Yeni Şifre <span className="text-danger">*</span>
                                                </label>
                        <div className="position-relative">
                          <input
                            type={passwordVisible.new ? 'text' : 'password'}
                            name="newPassword"
                            className="form-control radius-8"
                            placeholder="Yeni şifrenizi girin"
                            value={passwordData.newPassword}
                            onChange={handlePasswordChange}
                            required
                          />
                          <span
                            className="position-absolute end-0 top-50 translate-middle-y me-16 cursor-pointer"
                            onClick={() => togglePasswordVisibility('new')}
                          >
                            <Icon icon={passwordVisible.new ? 'mdi:eye-off' : 'mdi:eye'} />
                          </span>
                                            </div>
                                        </div>
                      
                      <div className="col-12">
                        <label className="form-label text-secondary-light fw-semibold">
                          Yeni Şifre Tekrar <span className="text-danger">*</span>
                                                </label>
                        <div className="position-relative">
                          <input
                            type={passwordVisible.confirm ? 'text' : 'password'}
                            name="confirmPassword"
                                                    className="form-control radius-8"
                            placeholder="Yeni şifrenizi tekrar girin"
                            value={passwordData.confirmPassword}
                            onChange={handlePasswordChange}
                            required
                          />
                          <span
                            className="position-absolute end-0 top-50 translate-middle-y me-16 cursor-pointer"
                            onClick={() => togglePasswordVisibility('confirm')}
                          >
                            <Icon icon={passwordVisible.confirm ? 'mdi:eye-off' : 'mdi:eye'} />
                          </span>
                                            </div>
                                        </div>
                                    </div>

                    <div className="d-flex align-items-center justify-content-center gap-3 mt-24">
                                        <button
                                            type="button"
                        className="btn rounded-pill btn-danger-100 text-danger-600 radius-8 px-20 py-11 fw-medium"
                        onClick={() => {
                          setPasswordData({
                            currentPassword: '',
                            newPassword: '',
                            confirmPassword: ''
                          });
                        }}
                      >
                        <Icon icon="mdi:close" className="me-2" />
                        Temizle
                                        </button>
                                        <button
                        type="submit"
                        className="btn rounded-pill btn-primary-100 text-primary-600 radius-8 px-20 py-11 fw-medium"
                        disabled={saving}
                      >
                        {saving ? (
                          <>
                            <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                            Değiştiriliyor...
                          </>
                        ) : (
                          <>
                            <Icon icon="mdi:lock-reset" className="me-2" />
                            Şifreyi Değiştir
                          </>
                        )}
                                        </button>
                                    </div>
                                </form>
                </div>
              )}

              {/* Module Permissions Tab */}
              {activeTab === 'module-permissions' && (
                <div className="tab-pane fade show active">
                  <div className="module-permissions">
                    <h6 className="text-md mb-16">Modül Erişim İzinleri</h6>
                    <p className="text-secondary-light mb-24">
                      Hangi modüllerde tam erişim (CRUD) yetkisine sahip olacağınızı belirleyin. Kapalı modüllerde sadece görüntüleme iznini olacaktır.
                    </p>
                    
                    <div className="row gy-3">
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-primary-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:package-variant-closed" className="text-primary-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Malzeme Yönetimi</h6>
                              <p className="text-secondary-light text-sm mb-0">Malzeme kartları, kategoriler ve hareketler</p>
                            </div>
                          </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="materialManagement"
                                id="materialManagement_full"
                                checked={modulePermissions.materialManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, materialManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="materialManagement_full">
                                Tam Erişim (CRUD)
                                    </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                className="form-check-input"
                                type="radio"
                                name="materialManagement"
                                id="materialManagement_read"
                                checked={!modulePermissions.materialManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, materialManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="materialManagement_read">
                                Sadece Görüntüleme
                              </label>
                            </div>
                          </div>
                                    </div>
                                </div>

                      
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-warning-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:factory" className="text-warning-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Üretim Yönetimi</h6>
                              <p className="text-secondary-light text-sm mb-0">İş emirleri, BOM ve üretim onayları</p>
                            </div>
                          </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="productionManagement"
                                id="productionManagement_full"
                                checked={modulePermissions.productionManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, productionManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="productionManagement_full">
                                Tam Erişim (CRUD)
                                    </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                className="form-check-input"
                                type="radio"
                                name="productionManagement"
                                id="productionManagement_read"
                                checked={!modulePermissions.productionManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, productionManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="productionManagement_read">
                                Sadece Görüntüleme
                              </label>
                            </div>
                          </div>
                                    </div>
                                </div>
                      
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-info-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:cart-plus" className="text-info-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Satınalma Yönetimi</h6>
                              <p className="text-secondary-light text-sm mb-0">Satın alma talepleri, siparişler ve tedarikçiler</p>
                            </div>
                          </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="purchasingManagement"
                                id="purchasingManagement_full"
                                checked={modulePermissions.purchasingManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, purchasingManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="purchasingManagement_full">
                                Tam Erişim (CRUD)
                              </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                            className="form-check-input"
                                type="radio"
                                name="purchasingManagement"
                                id="purchasingManagement_read"
                                checked={!modulePermissions.purchasingManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, purchasingManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="purchasingManagement_read">
                                Sadece Görüntüleme
                              </label>
                            </div>
                          </div>
                        </div>
                      </div>
                      
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-success-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:cart-arrow-up" className="text-success-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Satış Yönetimi</h6>
                              <p className="text-secondary-light text-sm mb-0">Satış siparişleri, teklifler ve müşteriler</p>
                                    </div>
                                </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="salesManagement"
                                id="salesManagement_full"
                                checked={modulePermissions.salesManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, salesManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="salesManagement_full">
                                Tam Erişim (CRUD)
                              </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                            className="form-check-input"
                                type="radio"
                                name="salesManagement"
                                id="salesManagement_read"
                                checked={!modulePermissions.salesManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, salesManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="salesManagement_read">
                                Sadece Görüntüleme
                              </label>
                            </div>
                          </div>
                        </div>
                      </div>
                      
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-info-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:warehouse" className="text-info-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Depo/Stok Yönetimi</h6>
                              <p className="text-secondary-light text-sm mb-0">Depo işlemleri, stok hareketleri ve envanter</p>
                                    </div>
                                </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="warehouseManagement"
                                id="warehouseManagement_full"
                                checked={modulePermissions.warehouseManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, warehouseManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="warehouseManagement_full">
                                Tam Erişim (CRUD)
                              </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                            className="form-check-input"
                                type="radio"
                                name="warehouseManagement"
                                id="warehouseManagement_read"
                                checked={!modulePermissions.warehouseManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, warehouseManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="warehouseManagement_read">
                                Sadece Görüntüleme
                              </label>
                            </div>
                          </div>
                        </div>
                      </div>
                      
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-danger-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:chart-line" className="text-danger-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Raporlar</h6>
                              <p className="text-secondary-light text-sm mb-0">Analiz raporları ve dashboard</p>
                                    </div>
                                </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="reportManagement"
                                id="reportManagement_full"
                                checked={modulePermissions.reportManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, reportManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="reportManagement_full">
                                Tam Erişim (CRUD)
                              </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                            className="form-check-input"
                                type="radio"
                                name="reportManagement"
                                id="reportManagement_read"
                                checked={!modulePermissions.reportManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, reportManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="reportManagement_read">
                                Sadece Görüntüleme
                              </label>
                            </div>
                          </div>
                        </div>
                      </div>
                      
                      <div className="col-12">
                        <div className="py-16 px-24 border radius-8">
                          <div className="d-flex align-items-center gap-3 mb-16">
                            <div className="w-40-px h-40-px bg-success-50 rounded-circle d-flex align-items-center justify-content-center">
                              <Icon icon="mdi:account-group" className="text-success-600" />
                            </div>
                            <div>
                              <h6 className="text-md mb-0">Kullanıcı Yönetimi</h6>
                              <p className="text-secondary-light text-sm mb-0">Kullanıcılar, roller ve yetkilendirme</p>
                                    </div>
                                </div>
                          <div className="d-flex align-items-center gap-3">
                            <div className="form-check checked-success d-flex align-items-center gap-2">
                              <input
                                className="form-check-input"
                                type="radio"
                                name="userManagement"
                                id="userManagement_full"
                                checked={modulePermissions.userManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, userManagement: true}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="userManagement_full">
                                Tam Erişim (CRUD)
                              </label>
                            </div>
                            <div className="form-check checked-warning d-flex align-items-center gap-2">
                                        <input
                                            className="form-check-input"
                                type="radio"
                                name="userManagement"
                                id="userManagement_read"
                                checked={!modulePermissions.userManagement}
                                onChange={() => setModulePermissions(prev => ({...prev, userManagement: false}))}
                              />
                              <label className="form-check-label line-height-1 fw-medium text-secondary-light" htmlFor="userManagement_read">
                                Sadece Görüntüleme
                              </label>
                                    </div>
                                </div>
                            </div>
                      </div>
                    </div>
                    
                    <div className="d-flex align-items-center justify-content-center gap-3 mt-24">
                      <button
                        type="button"
                        className="btn rounded-pill btn-danger-100 text-danger-600 radius-8 px-20 py-11 fw-medium"
                        onClick={() => {
                          setModulePermissions({
                            materialManagement: false,
                            productionManagement: false,
                            purchasingManagement: false,
                            salesManagement: false,
                            warehouseManagement: false,
                            reportManagement: false,
                            userManagement: false
                          });
                        }}
                      >
                        <Icon icon="mdi:close" className="me-2" />
                        Tüm İzinleri Kaldır
                      </button>
                      <button
                        type="button"
                        className="btn rounded-pill btn-primary-100 text-primary-600 radius-8 px-20 py-11 fw-medium"
                        onClick={async () => {
                          try {
                            setSaving(true);
                            
                            // Get current userData from localStorage
                            const userDataString = localStorage.getItem('userData');
                            const currentUserData = userDataString ? JSON.parse(userDataString) : {};
                            
                            // Save permissions to localStorage (original localStorage-based system)
                            const updatedUserData = {
                              ...currentUserData,
                              permissions: {
                                materialManagement: { 
                                  read: true, 
                                  write: modulePermissions.materialManagement 
                                },
                                productionManagement: { 
                                  read: true, 
                                  write: modulePermissions.productionManagement 
                                },
                                purchasingManagement: { 
                                  read: true, 
                                  write: modulePermissions.purchasingManagement 
                                },
                                salesManagement: { 
                                  read: true, 
                                  write: modulePermissions.salesManagement 
                                },
                                warehouseManagement: { 
                                  read: true, 
                                  write: modulePermissions.warehouseManagement 
                                },
                                reportManagement: { 
                                  read: true, 
                                  write: modulePermissions.reportManagement 
                                },
                                userManagement: { 
                                  read: true, 
                                  write: modulePermissions.userManagement 
                                }
                              }
                            };
                            
                            // Update localStorage
                            localStorage.setItem('userData', JSON.stringify(updatedUserData));
                            
                            MySwal.fire({
                              title: 'Başarılı',
                              text: 'Modül erişim izinleri kaydedildi.',
                              icon: 'success',
                              timer: 1500,
                              showConfirmButton: false
                            });
                            
                            // No page refresh - permissions will be updated automatically through the hook
                            
                          } catch (error) {
                            console.error('Error saving module permissions:', error);
                            MySwal.fire({
                              title: 'Hata',
                              text: 'Modül izinleri kaydedilirken bir hata oluştu: ' + error.message,
                              icon: 'error'
                            });
                          } finally {
                            setSaving(false);
                          }
                        }}
                        disabled={saving}
                      >
                        {saving ? (
                          <>
                            <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                            Kaydediliyor...
                          </>
                        ) : (
                          <>
                            <Icon icon="mdi:content-save" className="me-2" />
                            İzinleri Kaydet
                          </>
                        )}
                      </button>
                        </div>
                    </div>
                </div>
              )}
                </div>
            </div>
        </div>
      </div>
    </div>
    );
};

export default ViewProfileLayer;