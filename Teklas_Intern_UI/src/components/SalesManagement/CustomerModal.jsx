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
  .customer-modal-grid { grid-template-columns: 1fr !important; }
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
  Name: yup.string().required('Müşteri adı zorunludur.').max(100, 'Müşteri adı en fazla 100 karakter olabilir.'),
  Address: yup.string().max(500, 'Adres en fazla 500 karakter olabilir.'),
  Phone: yup.string().max(20, 'Telefon en fazla 20 karakter olabilir.'),
  Email: yup.string().email('Geçerli bir e-posta adresi giriniz.').max(100, 'E-posta en fazla 100 karakter olabilir.'),
  TaxNumber: yup.string().max(20, 'Vergi numarası en fazla 20 karakter olabilir.'),
  ContactPerson: yup.string().max(100, 'İletişim kişisi en fazla 100 karakter olabilir.'),
});

const CustomerModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Müşteri Tanımla',
  fetchCustomers,
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
          <div style={gridStyle} className="customer-modal-grid">
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Ad *</Form.Label>
                <Form.Control type="text" name="Name" value={form.Name || ''} onChange={onChange} required maxLength={100} isInvalid={!!validationErrors.Name} style={inputStyles} />
                {validationErrors.Name && <Form.Control.Feedback type="invalid">{validationErrors.Name}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Adres</Form.Label>
                <Form.Control type="text" name="Address" value={form.Address || ''} onChange={onChange} maxLength={500} isInvalid={!!validationErrors.Address} style={inputStyles} />
                {validationErrors.Address && <Form.Control.Feedback type="invalid">{validationErrors.Address}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Telefon</Form.Label>
                <Form.Control type="text" name="Phone" value={form.Phone || ''} onChange={onChange} maxLength={20} isInvalid={!!validationErrors.Phone} style={inputStyles} />
                {validationErrors.Phone && <Form.Control.Feedback type="invalid">{validationErrors.Phone}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>E-posta</Form.Label>
                <Form.Control type="email" name="Email" value={form.Email || ''} onChange={onChange} maxLength={100} isInvalid={!!validationErrors.Email} style={inputStyles} />
                {validationErrors.Email && <Form.Control.Feedback type="invalid">{validationErrors.Email}</Form.Control.Feedback>}
              </Form.Group>
            </div>
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Vergi Numarası</Form.Label>
                <Form.Control type="text" name="TaxNumber" value={form.TaxNumber || ''} onChange={onChange} maxLength={20} isInvalid={!!validationErrors.TaxNumber} style={inputStyles} />
                {validationErrors.TaxNumber && <Form.Control.Feedback type="invalid">{validationErrors.TaxNumber}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>İletişim Kişisi</Form.Label>
                <Form.Control type="text" name="ContactPerson" value={form.ContactPerson || ''} onChange={onChange} maxLength={100} isInvalid={!!validationErrors.ContactPerson} style={inputStyles} />
                {validationErrors.ContactPerson && <Form.Control.Feedback type="invalid">{validationErrors.ContactPerson}</Form.Control.Feedback>}
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

export default CustomerModal; 