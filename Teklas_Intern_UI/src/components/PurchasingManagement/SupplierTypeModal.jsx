import React from 'react';
import { Button, Form } from 'react-bootstrap';
import * as yup from 'yup';

const modalOverlayStyle = {
  position: 'fixed',
  top: 0,
  left: 0,
  right: 0,
  bottom: 0,
  background: 'rgba(0,0,0,0.3)',
  zIndex: 1050,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
};

const a4LandscapeStyle = {
  borderRadius: 16,
  boxShadow: '0 8px 32px rgba(0,0,0,0.15)',
  width: 'min(100vw, 1100px)',
  maxWidth: '100vw',
  minWidth: 'min(96vw, 1100px)',
  minHeight: '300px',
  maxHeight: '96vh',
  overflowX: 'hidden',
  overflowY: 'auto',
  padding: 32,
  display: 'flex',
  flexDirection: 'column',
  position: 'relative',
  boxSizing: 'border-box',
};

const closeBtnStyle = {
  position: 'absolute',
  top: 16,
  right: 24,
  fontSize: 24,
  background: 'none',
  border: 'none',
  cursor: 'pointer',
  zIndex: 2,
};

const gridStyle = {
  display: 'grid',
  gridTemplateColumns: '1fr',
  gap: 24,
  width: '100%',
  boxSizing: 'border-box',
};

const getModalBg = () => {
  if (typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'))) {
    return { background: '#273142', color: '#f1f1f1' };
  }
  return { background: '#fff', color: '#23272f' };
};

const getInputStyles = () => ({
  background: '#fff',
  color: '#23272f',
  borderColor: '#e0e0e0',
});

const validationSchema = yup.object().shape({
  name: yup.string().required('Tip adı zorunludur.').min(2, 'Tip adı en az 2 karakter olmalı.').max(50, 'Tip adı en fazla 50 karakter olabilir.'),
  description: yup.string().max(200, 'Açıklama en fazla 200 karakter olabilir.'),
});

const SupplierTypeModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Tedarikçi Tipi Ekle',
  isDetail = false,
}) => {
  const [validationErrors, setValidationErrors] = React.useState({});

  const handleSubmit = async (e) => {
    e.preventDefault();
    setValidationErrors({});
    try {
      await validationSchema.validate(form, { abortEarly: false });
      if (onSubmit) onSubmit(e);
    } catch (validationErr) {
      if (validationErr.inner) {
        const errors = {};
        validationErr.inner.forEach(err => {
          errors[err.path] = err.message;
        });
        setValidationErrors(errors);
      }
    }
  };

  if (!open) return null;
  const modalBg = getModalBg();
  const inputStyles = getInputStyles();
  const isDark = typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'));

  return (
    <div style={modalOverlayStyle}>
      <div style={{...a4LandscapeStyle, ...modalBg, background: isDark ? '#273142' : modalBg.background}} className="a4-landscape-modal">
        <button style={closeBtnStyle} onClick={onClose} aria-label="Kapat">×</button>
        <h4 style={{marginBottom: 20, textAlign: 'center', fontWeight: 600, fontSize: '24px', lineHeight: 1.1, color: modalBg.color}}>{title}</h4>
        <Form onSubmit={handleSubmit} style={{flex: 1, overflow: 'auto'}}>
          {error && <div className="alert alert-danger mb-4">{error}</div>}
          <div style={gridStyle}>
            <Form.Group className="mb-3">
              <Form.Label>Tip Adı *</Form.Label>
              <Form.Control type="text" name="name" value={form.name} onChange={onChange} required maxLength={50} isInvalid={!!validationErrors.name} style={inputStyles} readOnly={isDetail} />
              {validationErrors.name && <Form.Control.Feedback type="invalid">{validationErrors.name}</Form.Control.Feedback>}
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Açıklama</Form.Label>
              <Form.Control type="text" name="description" value={form.description} onChange={onChange} maxLength={200} style={inputStyles} readOnly={isDetail} />
              {validationErrors.description && <Form.Control.Feedback type="invalid">{validationErrors.description}</Form.Control.Feedback>}
            </Form.Group>
            <Form.Group className="mb-3" style={{marginTop: 32}}>
              <Form.Check type="checkbox" label="Aktif mi?" name="isActive" checked={form.isActive} onChange={onChange} id="isActiveCheck" disabled={isDetail} />
            </Form.Group>
          </div>
          <div style={{display: 'flex', justifyContent: 'flex-end', gap: 16, marginTop: 32}}>
            <Button variant="secondary" type="button" onClick={onClose} disabled={loading}>Kapat</Button>
            {!isDetail && <Button variant="primary" type="submit" disabled={loading}>{loading ? 'Kaydediliyor...' : 'Kaydet'}</Button>}
          </div>
        </Form>
      </div>
    </div>
  );
};

export default SupplierTypeModal; 