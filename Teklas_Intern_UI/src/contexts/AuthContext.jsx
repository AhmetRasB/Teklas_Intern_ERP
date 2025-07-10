import React, { createContext, useContext, useReducer, useEffect } from 'react';
import axios from 'axios';

const BASE_URL = 'https://localhost:7178';

// Initial state
const initialState = {
  user: null,
  token: localStorage.getItem('token'),
  isAuthenticated: false,
  loading: true,
  error: null
};

// Action types
const actionTypes = {
  LOGIN_START: 'LOGIN_START',
  LOGIN_SUCCESS: 'LOGIN_SUCCESS',
  LOGIN_FAILURE: 'LOGIN_FAILURE',
  LOGOUT: 'LOGOUT',
  LOAD_USER: 'LOAD_USER',
  CLEAR_ERROR: 'CLEAR_ERROR'
};

// Reducer
const authReducer = (state, action) => {
  switch (action.type) {
    case actionTypes.LOGIN_START:
      return {
        ...state,
        loading: true,
        error: null
      };
    case actionTypes.LOGIN_SUCCESS:
      return {
        ...state,
        isAuthenticated: true,
        user: action.payload.user,
        token: action.payload.token,
        loading: false,
        error: null
      };
    case actionTypes.LOGIN_FAILURE:
      return {
        ...state,
        isAuthenticated: false,
        user: null,
        token: null,
        loading: false,
        error: action.payload
      };
    case actionTypes.LOGOUT:
      return {
        ...state,
        isAuthenticated: false,
        user: null,
        token: null,
        loading: false,
        error: null
      };
    case actionTypes.LOAD_USER:
      return {
        ...state,
        isAuthenticated: true,
        user: action.payload,
        loading: false
      };
    case actionTypes.CLEAR_ERROR:
      return {
        ...state,
        error: null
      };
    default:
      return state;
  }
};

// Create context
const AuthContext = createContext();

// AuthProvider component
export const AuthProvider = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, initialState);

  // Setup axios interceptor for token
  useEffect(() => {
    if (state.token) {
      axios.defaults.headers.common['Authorization'] = `Bearer ${state.token}`;
      localStorage.setItem('token', state.token);
    } else {
      delete axios.defaults.headers.common['Authorization'];
      localStorage.removeItem('token');
    }
  }, [state.token]);

  // Load user on app start if token exists
  useEffect(() => {
    if (state.token && !state.user) {
      loadUser();
    } else if (!state.token) {
      dispatch({ type: actionTypes.LOGOUT });
    }
  }, []);

  // Login function
  const login = async (credentials) => {
    try {
      dispatch({ type: actionTypes.LOGIN_START });
      
      const response = await axios.post(`${BASE_URL}/api/auth/login`, {
        usernameOrEmail: credentials.username,
        password: credentials.password,
        rememberMe: credentials.rememberMe || false
      });
      
      if (response.data.success) {
        const { token, user } = response.data;
        
        dispatch({
          type: actionTypes.LOGIN_SUCCESS,
          payload: { token, user }
        });
        
        return { success: true };
      } else {
        throw new Error(response.data.error || 'Giriş başarısız');
      }
    } catch (error) {
      const errorMessage = error.response?.data?.error || error.message || 'Giriş başarısız';
      dispatch({
        type: actionTypes.LOGIN_FAILURE,
        payload: errorMessage
      });
      return { success: false, error: errorMessage };
    }
  };

  // Load user function
  const loadUser = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/auth/me`);
      dispatch({
        type: actionTypes.LOAD_USER,
        payload: response.data
      });
    } catch (error) {
      console.error('Error loading user:', error);
      dispatch({ type: actionTypes.LOGOUT });
    }
  };

  // Logout function
  const logout = async () => {
    try {
      await axios.post(`${BASE_URL}/api/auth/logout`);
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      dispatch({ type: actionTypes.LOGOUT });
    }
  };

  // Change password function
  const changePassword = async (currentPassword, newPassword) => {
    try {
      const response = await axios.post(`${BASE_URL}/api/auth/change-password`, {
        currentPassword,
        newPassword
      });
      return { success: true, message: response.data.message };
    } catch (error) {
      const errorMessage = error.response?.data?.error || 'Şifre değiştirme başarısız';
      return { success: false, error: errorMessage };
    }
  };

  // Clear error function
  const clearError = () => {
    dispatch({ type: actionTypes.CLEAR_ERROR });
  };

  // Check if user has role
  const hasRole = (role) => {
    return state.user?.roleNames?.includes(role) || false;
  };

  // Check if user is admin
  const isAdmin = () => {
    return hasRole('Admin') || hasRole('SuperAdmin');
  };

  const value = {
    ...state,
    login,
    logout,
    loadUser,
    changePassword,
    clearError,
    hasRole,
    isAdmin
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook to use auth context
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export default AuthContext; 