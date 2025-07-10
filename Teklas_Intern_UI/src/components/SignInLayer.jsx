import { Icon } from "@iconify/react/dist/iconify.js";
import React, { useState, useEffect } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const MySwal = withReactContent(Swal);

const SignInLayer = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: ''
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const { login, error, clearError, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Redirect if already authenticated
  useEffect(() => {
    if (isAuthenticated) {
      const from = location.state?.from?.pathname || '/';
      navigate(from, { replace: true });
    }
  }, [isAuthenticated, navigate, location]);

  // Clear errors when component mounts
  useEffect(() => {
    clearError();
  }, [clearError]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    // Clear error when user starts typing
    if (error) {
      clearError();
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.username || !formData.password) {
      MySwal.fire({
        title: 'Hata',
        text: 'Lütfen tüm alanları doldurun.',
        icon: 'error',
        confirmButtonText: 'Tamam'
      });
      return;
    }

    setIsLoading(true);
    
    const result = await login({
      username: formData.username,
      password: formData.password
    });

    setIsLoading(false);

    if (result.success) {
      MySwal.fire({
        title: 'Başarılı!',
        text: 'Giriş başarılı. Yönlendiriliyorsunuz...',
        icon: 'success',
        timer: 1500,
        showConfirmButton: false
      });
      
      // Navigate will be handled by useEffect above
    } else {
      MySwal.fire({
        title: 'Giriş Başarısız',
        text: result.error || 'Kullanıcı adı veya şifre hatalı.',
        icon: 'error',
        confirmButtonText: 'Tamam'
      });
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <section className='auth bg-base d-flex flex-wrap'>
      <div className='auth-left d-lg-block d-none'>
        <div className='d-flex align-items-center flex-column h-100 justify-content-center'>
          <img src='assets/images/auth/auth-img.png' alt='Auth' />
        </div>
      </div>
      <div className='auth-right py-32 px-24 d-flex flex-column justify-content-center'>
        <div className='max-w-464-px mx-auto w-100'>
          <div>
            <Link to='/' className='mb-40 max-w-290-px'>
              <img src='assets/images/logo.png' alt='Logo' />
            </Link>
            <h4 className='mb-12'>Admin Paneli Girişi</h4>
            <p className='mb-32 text-secondary-light text-lg'>
              Hoş geldiniz! Lütfen bilgilerinizi girin
            </p>
          </div>
          
          <form onSubmit={handleSubmit}>
            {/* Username Field */}
            <div className='icon-field mb-16'>
              <span className='icon top-50 translate-middle-y'>
                <Icon icon='mage:user' />
              </span>
              <input
                type='text'
                name='username'
                value={formData.username}
                onChange={handleInputChange}
                className='form-control h-56-px bg-neutral-50 radius-12'
                placeholder='Kullanıcı Adı'
                disabled={isLoading}
                autoComplete='username'
                required
              />
            </div>

            {/* Password Field */}
            <div className='position-relative mb-20'>
              <div className='icon-field'>
                <span className='icon top-50 translate-middle-y'>
                  <Icon icon='solar:lock-password-outline' />
                </span>
                <input
                  type={showPassword ? 'text' : 'password'}
                  name='password'
                  value={formData.password}
                  onChange={handleInputChange}
                  className='form-control h-56-px bg-neutral-50 radius-12'
                  placeholder='Şifre'
                  disabled={isLoading}
                  autoComplete='current-password'
                  required
                />
              </div>
              <span
                className={`toggle-password ${showPassword ? 'ri-eye-off-line' : 'ri-eye-line'} cursor-pointer position-absolute end-0 top-50 translate-middle-y me-16 text-secondary-light`}
                onClick={togglePasswordVisibility}
                role="button"
                tabIndex={0}
                onKeyDown={(e) => e.key === 'Enter' && togglePasswordVisibility()}
              />
            </div>

            {/* Remember Me */}
            <div className='mb-20'>
              <div className='form-check style-check d-flex align-items-center'>
                <input
                  className='form-check-input border border-neutral-300'
                  type='checkbox'
                  id='remember'
                />
                <label className='form-check-label' htmlFor='remember'>
                  Beni Hatırla
                </label>
              </div>
            </div>

            {/* Submit Button */}
            <button
              type='submit'
              className='btn btn-primary text-sm btn-sm px-12 py-16 w-100 radius-12 mt-32'
              disabled={isLoading}
            >
              {isLoading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Giriş Yapılıyor...
                </>
              ) : (
                'Giriş Yap'
              )}
            </button>
          </form>

          {/* Footer */}
          <div className='mt-32 text-center text-sm'>
            <p className='mb-0 text-muted'>
              Teklas ERP Admin Paneli
            </p>
          </div>
        </div>
      </div>
    </section>
  );
};

export default SignInLayer;
