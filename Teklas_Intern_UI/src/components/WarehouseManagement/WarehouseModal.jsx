import React from 'react';
import { Button, Form } from 'react-bootstrap';
import * as yup from 'yup';

const BASE_URL = 'https://localhost:7178';

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
  width: 'min(100vw, 700px)',
  maxWidth: '100vw',
  minWidth: 'min(96vw, 700px)',
  minHeight: '350px',
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
  gridTemplateColumns: '1fr 1fr',
  gap: 24,
  width: '100%',
  boxSizing: 'border-box',
};

const responsiveStyle = `
@media (max-width: 900px) {
  .warehouse-modal-grid { grid-template-columns: 1fr !important; }
}
`;

// --- THEME SUPPORT (from MaterialCardModal.jsx) ---
const getModalBg = () => {
  if (typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'))) {
    return { background: '#1B2431', color: '#f1f1f1' };
  }
  return { background: '#fff', color: '#23272f' };
};

const getInputStyles = () => {
  if (typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'))) {
    return {
      background: '#1B2431',
      color: '#f1f1f1',
      borderColor: '#23272f',
    };
  }
  return {};
};
// ---------------------------------------------------

const validationSchema = yup.object().shape({
  Code: yup.string().required('Depo kodu zorunludur.').max(50, 'Depo kodu en fazla 50 karakter olabilir.'),
  Name: yup.string().required('Depo adı zorunludur.').max(100, 'Depo adı en fazla 100 karakter olabilir.'),
  Description: yup.string().max(500, 'Açıklama en fazla 500 karakter olabilir.'),
  Status: yup.number().oneOf([0, 1], 'Durum 0 (Pasif) veya 1 (Aktif) olmalı.'),
});

const WarehouseModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Depo Tanımla',
  fetchWarehouses,
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
  
  // Modal ve inputlar için tema renklerini uygula
  const modalBg = getModalBg();
  const inputStyles = getInputStyles();

  return (
    <div style={modalOverlayStyle}>
      <style>{responsiveStyle}</style>
      <div style={{...a4LandscapeStyle, ...modalBg}} className="a4-landscape-modal">
        <button style={closeBtnStyle} onClick={onClose} aria-label="Kapat">×</button>
        <h4 style={{marginBottom: 20, textAlign: 'center', fontWeight: 600, fontSize: '24px', lineHeight: 1.1}}>{title}</h4>
        <Form onSubmit={handleSubmit} style={{flex: 1, overflow: 'auto'}}>
          {error && <div className="alert alert-danger mb-4">{error}</div>}
          <div style={gridStyle} className="warehouse-modal-grid">
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Kod *</Form.Label>
                <Form.Control type="text" name="Code" value={form.Code || ''} onChange={onChange} required maxLength={50} isInvalid={!!validationErrors.Code} style={inputStyles} />
                {validationErrors.Code && <Form.Control.Feedback type="invalid">{validationErrors.Code}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Ad *</Form.Label>
                <Form.Control type="text" name="Name" value={form.Name || ''} onChange={onChange} required maxLength={100} isInvalid={!!validationErrors.Name} style={inputStyles} />
                {validationErrors.Name && <Form.Control.Feedback type="invalid">{validationErrors.Name}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Açıklama</Form.Label>
                <Form.Control type="text" name="Description" value={form.Description || ''} onChange={onChange} maxLength={500} isInvalid={!!validationErrors.Description} style={inputStyles} />
                {validationErrors.Description && <Form.Control.Feedback type="invalid">{validationErrors.Description}</Form.Control.Feedback>}
              </Form.Group>
            </div>
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Durum</Form.Label>
                <Form.Select name="Status" value={form.Status || 1} onChange={onChange} required isInvalid={!!validationErrors.Status} style={inputStyles}>
                  <option value={1}>Aktif</option>
                  <option value={0}>Pasif</option>
                </Form.Select>
                {validationErrors.Status && <Form.Control.Feedback type="invalid">{validationErrors.Status}</Form.Control.Feedback>}
              </Form.Group>
            </div>
          </div>
          <div className="d-flex justify-content-end mt-4">
            <Button variant="secondary" onClick={onClose} className="me-2">İptal</Button>
            <Button variant="primary" type="submit" disabled={loading}>{loading ? 'Kaydediliyor...' : 'Kaydet'}</Button>
          </div>
        </Form>
      </div>
    </div>
  );
};

export default WarehouseModal; 