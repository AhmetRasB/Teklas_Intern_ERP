import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const ProtectedRoute = ({ children, requireAuth = true, requiredRole = null, adminOnly = false }) => {
  const { isAuthenticated, user, loading, isAdmin } = useAuth();
  const location = useLocation();

  // Show loading spinner while checking authentication
  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center min-vh-100">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  // Redirect to login if authentication is required but user is not authenticated
  if (requireAuth && !isAuthenticated) {
    return <Navigate to="/sign-in" state={{ from: location }} replace />;
  }

  // Redirect authenticated users away from auth pages
  if (!requireAuth && isAuthenticated) {
    const from = location.state?.from?.pathname || '/';
    return <Navigate to={from} replace />;
  }

  // Check admin role requirement
  if (adminOnly && !isAdmin()) {
    return (
      <div className="d-flex justify-content-center align-items-center min-vh-100">
        <div className="text-center">
          <h3 className="text-danger">Access Denied</h3>
          <p className="text-muted">Bu sayfaya erişim yetkiniz bulunmuyor.</p>
          <button 
            className="btn btn-primary" 
            onClick={() => window.history.back()}
          >
            Geri Dön
          </button>
        </div>
      </div>
    );
  }

  // Check specific role requirement
  if (requiredRole && !user?.roleNames?.includes(requiredRole)) {
    return (
      <div className="d-flex justify-content-center align-items-center min-vh-100">
        <div className="text-center">
          <h3 className="text-danger">Access Denied</h3>
          <p className="text-muted">Bu sayfaya erişim için gerekli role sahip değilsiniz.</p>
          <button 
            className="btn btn-primary" 
            onClick={() => window.history.back()}
          >
            Geri Dön
          </button>
        </div>
      </div>
    );
  }

  return children;
};

export default ProtectedRoute; 