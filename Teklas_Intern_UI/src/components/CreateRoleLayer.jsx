import React, { useState } from 'react';
import { Icon } from '@iconify/react/dist/iconify.js';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const CreateRoleLayer = () => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    isActive: true
  });
  const [loading, setLoading] = useState(false);

  const { isAdmin } = useAuth();
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const validateForm = () => {
    // Check required fields
    if (!formData.name.trim()) {
      MySwal.fire({
        title: 'Hata',
        text: 'Rol adı gereklidir.',
        icon: 'error'
      });
      return false;
    }

    // Validate role name format
    const roleNameRegex = /^[a-zA-Z0-9\s_-]+$/;
    if (!roleNameRegex.test(formData.name)) {
      MySwal.fire({
        title: 'Hata',
        text: 'Rol adı sadece harf, rakam, boşluk, alt çizgi ve tire içerebilir.',
        icon: 'error'
      });
      return false;
    }

    // Check length
    if (formData.name.length < 2) {
      MySwal.fire({
        title: 'Hata',
        text: 'Rol adı en az 2 karakter olmalıdır.',
        icon: 'error'
      });
      return false;
    }

    if (formData.name.length > 50) {
      MySwal.fire({
        title: 'Hata',
        text: 'Rol adı 50 karakterden uzun olamaz.',
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
      const response = await axios.post(`${BASE_URL}/api/roles`, {
        name: formData.name.trim(),
        description: formData.description.trim() || null,
        isActive: formData.isActive
      });

      if (response.data.success) {
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Rol başarıyla oluşturuldu.',
          icon: 'success',
          timer: 1500,
          showConfirmButton: false
        });

        navigate('/assign-role');
      } else {
        throw new Error(response.data.message || 'Rol oluşturulamadı');
      }
    } catch (error) {
      console.error('Error creating role:', error);
      const errorMessage = error.response?.data?.error || 
                          error.response?.data?.message || 
                          error.message || 
                          'Rol oluşturulurken bir hata oluştu.';
      
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
      name: '',
      description: '',
      isActive: true
    });
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
          <div className="col-xxl-6 col-xl-8 col-lg-10">
            <div className="card border">
              <div className="card-header bg-base py-16 px-24">
                <h6 className="text-lg mb-0">Yeni Rol Oluştur</h6>
              </div>
              <div className="card-body">
                <form onSubmit={handleSubmit}>
                  <div className="row gy-3">
                    {/* Role Name */}
                    <div className="col-12">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Rol Adı <span className="text-danger-600">*</span>
                      </label>
                      <input
                        type="text"
                        name="name"
                        className="form-control radius-8"
                        placeholder="Rol adını girin (örn: Manager, Operator, etc.)"
                        value={formData.name}
                        onChange={handleInputChange}
                        required
                      />
                    </div>

                    {/* Role Description */}
                    <div className="col-12">
                      <label className="form-label fw-semibold text-primary-light text-sm mb-8">
                        Açıklama
                      </label>
                      <textarea
                        name="description"
                        className="form-control radius-8"
                        rows="3"
                        placeholder="Rol hakkında açıklama girin (isteğe bağlı)"
                        value={formData.description}
                        onChange={handleInputChange}
                      />
                    </div>

                    {/* Active Status */}
                    <div className="col-12">
                      <div className="form-check">
                        <input
                          className="form-check-input"
                          type="checkbox"
                          name="isActive"
                          id="isActive"
                          checked={formData.isActive}
                          onChange={handleInputChange}
                        />
                        <label className="form-check-label fw-semibold text-primary-light text-sm" htmlFor="isActive">
                          Aktif Rol
                        </label>
                      </div>
                      <small className="text-secondary-light">
                        Aktif olmayan roller kullanıcılara atanamaz.
                      </small>
                    </div>
                  </div>

                  {/* Action Buttons */}
                  <div className="d-flex align-items-center justify-content-center gap-3 mt-24">
                    <button
                      type="button"
                      className="btn rounded-pill btn-danger-100 text-danger-600 radius-8 px-20 py-11 fw-medium"
                      onClick={() => navigate('/assign-role')}
                    >
                      <Icon icon="mdi:close" className="me-2" />
                      İptal
                    </button>
                    <button
                      type="button"
                      className="btn rounded-pill btn-neutral-100 text-neutral-600 radius-8 px-20 py-11 fw-medium"
                      onClick={handleReset}
                    >
                      <Icon icon="mdi:refresh" className="me-2" />
                      Temizle
                    </button>
                    <button
                      type="submit"
                      className="btn rounded-pill btn-primary-100 text-primary-600 radius-8 px-20 py-11 fw-medium"
                      disabled={loading}
                    >
                      {loading ? (
                        <>
                          <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                          Oluşturuluyor...
                        </>
                      ) : (
                        <>
                          <Icon icon="mdi:plus" className="me-2" />
                          Rol Oluştur
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

export default CreateRoleLayer; 