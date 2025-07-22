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
  minHeight: '500px',
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
  gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
  gap: 24,
  width: '100%',
  boxSizing: 'border-box',
};

const responsiveStyle = `
@media (max-width: 1200px) {
  .customer-order-modal-grid { grid-template-columns: 1fr 1fr !important; }
}
@media (max-width: 800px) {
  .customer-order-modal-grid { grid-template-columns: 1fr !important; }
}
`;

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
  orderNumber: yup.string().required('Sipariş numarası zorunludur.').min(3, 'Sipariş numarası en az 3 karakter olmalı.').max(20, 'Sipariş numarası en fazla 20 karakter olabilir.'),
  customerId: yup.string().required('Müşteri seçimi zorunludur.'),
  orderDate: yup.date().required('Sipariş tarihi zorunludur.'),
  status: yup.string().required('Durum zorunludur.'),
  totalAmount: yup.number().typeError('Tutar sayı olmalı.').min(0, 'Tutar pozitif olmalı.'),
});

const CustomerOrderModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Sipariş Ekle',
  customers = [],
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
      <style>{responsiveStyle}{isDark ? `.a4-landscape-modal { background: #273142 !important; }` : ''}</style>
      <div style={{...a4LandscapeStyle, ...modalBg, background: isDark ? '#273142' : modalBg.background}} className="a4-landscape-modal">
        <button style={closeBtnStyle} onClick={onClose} aria-label="Kapat">×</button>
        <h4 style={{marginBottom: 20, textAlign: 'center', fontWeight: 600, fontSize: '24px', lineHeight: 1.1, color: modalBg.color}}>{title}</h4>
        <Form onSubmit={handleSubmit} style={{flex: 1, overflow: 'auto'}}>
          {error && <div className="alert alert-danger mb-4">{error}</div>}
          <div style={gridStyle} className="customer-order-modal-grid">
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Sipariş No *</Form.Label>
                <Form.Control type="text" name="orderNumber" value={form.orderNumber} onChange={onChange} required maxLength={30} isInvalid={!!validationErrors.orderNumber} style={inputStyles} readOnly={isDetail} />
                {validationErrors.orderNumber && <Form.Control.Feedback type="invalid">{validationErrors.orderNumber}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Müşteri *</Form.Label>
                <Form.Select name="customerId" value={form.customerId} onChange={onChange} required isInvalid={!!validationErrors.customerId} style={inputStyles} disabled={isDetail}>
                  <option value="">Seçiniz</option>
                  {customers.map(cus => <option key={cus.id} value={cus.id}>{cus.name}</option>)}
                </Form.Select>
                {validationErrors.customerId && <Form.Control.Feedback type="invalid">{validationErrors.customerId}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Sipariş Tarihi *</Form.Label>
                <Form.Control type="date" name="orderDate" value={form.orderDate} onChange={onChange} required isInvalid={!!validationErrors.orderDate} style={inputStyles} readOnly={isDetail} />
                {validationErrors.orderDate && <Form.Control.Feedback type="invalid">{validationErrors.orderDate}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Durum *</Form.Label>
                <Form.Control type="text" name="status" value={form.status} onChange={onChange} required maxLength={30} isInvalid={!!validationErrors.status} style={inputStyles} readOnly={isDetail} />
                {validationErrors.status && <Form.Control.Feedback type="invalid">{validationErrors.status}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Açıklama</Form.Label>
                <Form.Control type="text" name="description" value={form.description} onChange={onChange} maxLength={200} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Tutar *</Form.Label>
                <Form.Control type="number" name="totalAmount" value={form.totalAmount} onChange={onChange} min={0} required style={inputStyles} readOnly={isDetail} />
                {validationErrors.totalAmount && <Form.Control.Feedback type="invalid">{validationErrors.totalAmount}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3" style={{marginTop: 32}}>
                <Form.Check type="checkbox" label="Aktif mi?" name="isActive" checked={form.isActive} onChange={onChange} id="isActiveCheck" disabled={isDetail} />
              </Form.Group>
            </div>
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

export default CustomerOrderModal; 