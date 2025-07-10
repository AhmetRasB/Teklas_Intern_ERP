import React, { useState, useEffect } from 'react';
import { Icon } from '@iconify/react/dist/iconify.js';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const AddUserLayer = () => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: '',
    phoneNumber: ''
  });
  const [selectedRole, setSelectedRole] = useState('');
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(false);
  const [profileImage, setProfileImage] = useState(null);
  const [imagePreviewUrl, setImagePreviewUrl] = useState('');
  const [passwordVisible, setPasswordVisible] = useState({
    password: false,
    confirm: false
  });

  const { isAdmin } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAdmin()) {
      fetchRoles();
    }
  }, [isAdmin]);

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

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      // Validate file type
      if (!file.type.startsWith('image/')) {
        MySwal.fire({
          title: 'Hata',
          text: 'Lütfen geçerli bir görsel dosyası seçin.',
          icon: 'error'
        });
        return;
      }

      // Validate file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        MySwal.fire({
          title: 'Hata',
          text: 'Dosya boyutu 5MB\'dan küçük olmalıdır.',
          icon: 'error'
        });
        return;
      }

      setProfileImage(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreviewUrl(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleRoleSelect = (roleName) => {
    setSelectedRole(roleName);
  };

  const togglePasswordVisibility = (field) => {
    setPasswordVisible(prev => ({
      ...prev,
      [field]: !prev[field]
    }));
  };

  const validateForm = () => {
    // Check required fields
    if (!formData.username || !formData.email || !formData.password || !formData.firstName || !formData.lastName) {
      MySwal.fire({
        title: 'Hata',
        text: 'Lütfen tüm zorunlu alanları doldurun.',
        icon: 'error'
      });
      return false;
    }

    // Validate email format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      MySwal.fire({
        title: 'Hata',
        text: 'Lütfen geçerli bir e-posta adresi girin.',
        icon: 'error'
      });
      return false;
    }

    // Validate username format
    const usernameRegex = /^[a-zA-Z0-9_]+$/;
    if (!usernameRegex.test(formData.username)) {
      MySwal.fire({
        title: 'Hata',
        text: 'Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.',
        icon: 'error'
      });
      return false;
    }

    // Validate password
    if (formData.password.length < 6) {
      MySwal.fire({
        title: 'Hata',
        text: 'Şifre en az 6 karakter olmalıdır.',
        icon: 'error'
      });
      return false;
    }

    // Validate password complexity
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/;
    if (!passwordRegex.test(formData.password)) {
      MySwal.fire({
        title: 'Hata',
        text: 'Şifre en az bir küçük harf, bir büyük harf ve bir rakam içermelidir.',
        icon: 'error'
      });
      return false;
    }

    // Validate password confirmation
    if (formData.password !== formData.confirmPassword) {
      MySwal.fire({
        title: 'Hata',
        text: 'Şifreler eşleşmiyor.',
        icon: 'error'
      });
      return false;
    }

    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!validateForm()) return;

    setLoading(true);
    try {
      // Create user payload
      const userPayload = {
        username: formData.username,
        email: formData.email,
        password: formData.password,
        confirmPassword: formData.confirmPassword,
        firstName: formData.firstName,
        lastName: formData.lastName,
        phoneNumber: formData.phoneNumber || null
      };

      // Create user via registration endpoint
      const response = await axios.post(`${BASE_URL}/api/auth/register`, userPayload);
      
      if (response.data.success) {
        const newUserId = response.data.user.id;

        // Assign role if one is selected
        if (selectedRole) {
          try {
            await axios.post(`${BASE_URL}/api/users/${newUserId}/assign-roles`, {
              roleNames: [selectedRole]
            });
          } catch (roleError) {
            console.warn('Failed to assign role:', roleError);
            // Don't fail the whole operation if role assignment fails
          }
        }

        // Upload profile image if provided
        if (profileImage) {
          try {
            const formData = new FormData();
            formData.append('profileImage', profileImage);
            await axios.post(`${BASE_URL}/api/users/${newUserId}/profile-image`, formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } catch (imageError) {
            console.warn('Failed to upload profile image:', imageError);
            // Don't fail the whole operation if image upload fails
          }
        }

        MySwal.fire({
          title: 'Başarılı!',
          text: 'Kullanıcı başarıyla oluşturuldu.',
          icon: 'success',
          timer: 1500,
          showConfirmButton: false
        });

        navigate('/users-list');
      } else {
        throw new Error(response.data.message || 'Kullanıcı oluşturulamadı');
      }
    } catch (error) {
      console.error('Error creating user:', error);
      const errorMessage = error.response?.data?.error || 
                          error.response?.data?.message || 
                          error.message || 
                          'Kullanıcı oluşturulurken bir hata oluştu.';
      
      MySwal.fire({
        title: 'Hata',
        text: errorMessage,
        icon: 'error'
      });
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setFormData({
      username: '',
      email: '',
      password: '',
      confirmPassword: '',
      firstName: '',
      lastName: '',
      phoneNumber: ''
    });
    setSelectedRole('');
    setProfileImage(null);
    setImagePreviewUrl('');
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

  return (
    <div className="card h-100 p-0 radius-12">
      <div className="card-body p-24">
        <div className="row justify-content-center">
          <div className="col-xxl-8 col-xl-10 col-lg-12">
            <div className="card border">
              <div className="card-header bg-base py-16 px-24">
                <h6 className="text-lg mb-0">Yeni Kullanıcı Ekle</h6>
              </div>
              <div className="card-body">
                {/* Profile Image Upload */}
                <h6 className="text-md text-primary-light mb-16">Profil Fotoğrafı</h6>
                <div className="mb-24 mt-16">
                  <div className="avatar-upload">
                    <div className="avatar-edit position-absolute bottom-0 end-0 me-24 mt-16 z-1 cursor-pointer">
                      <input
                        type="file"
                        id="imageUpload"
                        accept=".png, .jpg, .jpeg"
                        hidden
                        onChange={handleImageChange}
                      />
                      <label
                        htmlFor="imageUpload"
                        className="w-32-px h-32-px d-flex justify-content-center align-items-center bg-primary-50 text-primary-600 border border-primary-600 bg-hover-primary-100 text-lg rounded-circle cursor-pointer"
                      >
                        <Icon icon="solar:camera-outline" className="icon" />
                      </label>
                    </div>
                    <div className="avatar-preview">
                      <div
                        id="imagePreview"
                        className="w-100-px h-100-px rounded-circle bg-neutral-200 d-flex align-items-center justify-content-center overflow-hidden"
                        style={{
                          backgroundImage: imagePreviewUrl ? `url(${imagePreviewUrl})` : '',
                          backgroundSize: 'cover',
                          backgroundPosition: 'center'
                        }}
                      >
                        {!imagePreviewUrl && (
                          <Icon icon="mdi:account-plus" className="text-neutral-400 text-3xl" />
                        )}
                      </div>
                    </div>
                  </div>
                </div>

                <form onSubmit={handleSubmit}>
                  <div className="row gy-3">
                    {/* Username */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Kullanıcı Adı <span className="text-danger-600">*</span>
                      </label>
                      <input
                        type="text"
                        name="username"
                        className="form-control radius-8"
                        placeholder="Kullanıcı adını girin"
                        value={formData.username}
                        onChange={handleInputChange}
                        required
                      />
                    </div>

                    {/* Email */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        E-posta <span className="text-danger-600">*</span>
                      </label>
                      <input
                        type="email"
                        name="email"
                        className="form-control radius-8"
                        placeholder="E-posta adresini girin"
                        value={formData.email}
                        onChange={handleInputChange}
                        required
                      />
                    </div>

                    {/* First Name */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Ad <span className="text-danger-600">*</span>
                      </label>
                      <input
                        type="text"
                        name="firstName"
                        className="form-control radius-8"
                        placeholder="Adını girin"
                        value={formData.firstName}
                        onChange={handleInputChange}
                        required
                      />
                    </div>

                    {/* Last Name */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Soyad <span className="text-danger-600">*</span>
                      </label>
                      <input
                        type="text"
                        name="lastName"
                        className="form-control radius-8"
                        placeholder="Soyadını girin"
                        value={formData.lastName}
                        onChange={handleInputChange}
                        required
                      />
                    </div>

                    {/* Phone Number */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Telefon Numarası
                      </label>
                      <input
                        type="tel"
                        name="phoneNumber"
                        className="form-control radius-8"
                        placeholder="Telefon numarasını girin"
                        value={formData.phoneNumber}
                        onChange={handleInputChange}
                      />
                    </div>

                    {/* Password */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Şifre <span className="text-danger-600">*</span>
                      </label>
                      <div className="position-relative">
                        <input
                          type={passwordVisible.password ? 'text' : 'password'}
                          name="password"
                          className="form-control radius-8"
                          placeholder="Şifreyi girin"
                          value={formData.password}
                          onChange={handleInputChange}
                          required
                        />
                        <span
                          className="position-absolute end-0 top-50 translate-middle-y me-16 cursor-pointer"
                          onClick={() => togglePasswordVisibility('password')}
                        >
                          <Icon icon={passwordVisible.password ? 'mdi:eye-off' : 'mdi:eye'} />
                        </span>
                      </div>
                    </div>

                    {/* Confirm Password */}
                    <div className="col-md-6">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Şifre Tekrar <span className="text-danger-600">*</span>
                      </label>
                      <div className="position-relative">
                        <input
                          type={passwordVisible.confirm ? 'text' : 'password'}
                          name="confirmPassword"
                          className="form-control radius-8"
                          placeholder="Şifreyi tekrar girin"
                          value={formData.confirmPassword}
                          onChange={handleInputChange}
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

                    {/* Role Assignment */}
                    <div className="col-12">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Roller
                      </label>
                      <div className="d-flex flex-wrap gap-3">
                        {(roles || []).map((role) => (
                          <div key={role.id} className="form-check">
                            <input
                              className="form-check-input"
                              type="radio"
                              name="userRole"
                              id={`role-${role.id}`}
                              value={role.name}
                              checked={selectedRole === role.name}
                              onChange={() => handleRoleSelect(role.name)}
                            />
                            <label className="form-check-label" htmlFor={`role-${role.id}`}>
                              {role.name}
                            </label>
                          </div>
                        ))}
                      </div>
                      {(!roles || roles.length === 0) && (
                        <p className="text-secondary-light text-sm">Sistemde henüz rol tanımlanmamış.</p>
                      )}
                    </div>
                  </div>

                  {/* Action Buttons */}
                  <div className="d-flex align-items-center justify-content-center gap-3 mt-24">
                    <button
                      type="button"
                      className="border border-danger-600 bg-hover-danger-200 text-danger-600 text-md px-24 py-11 radius-8"
                      onClick={() => navigate('/users-list')}
                    >
                      İptal
                    </button>
                    <button
                      type="button"
                      className="border border-neutral-600 bg-hover-neutral-200 text-neutral-600 text-md px-24 py-11 radius-8"
                      onClick={handleReset}
                    >
                      Temizle
                    </button>
                    <button
                      type="submit"
                      className="btn btn-primary border border-primary-600 text-md px-24 py-12 radius-8"
                      disabled={loading}
                    >
                      {loading ? (
                        <>
                          <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                          Oluşturuluyor...
                        </>
                      ) : (
                        <>
                          <Icon icon="mdi:account-plus" className="me-2" />
                          Kullanıcı Oluştur
                        </>
                      )}
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddUserLayer;