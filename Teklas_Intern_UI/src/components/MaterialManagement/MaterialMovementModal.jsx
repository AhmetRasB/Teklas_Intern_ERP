import React from 'react';
import { Button, Form } from 'react-bootstrap';
import * as yup from 'yup';

// Material Cards will be passed as prop

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
  .material-movement-modal-grid { grid-template-columns: 1fr 1fr !important; }
}
@media (max-width: 800px) {
  .material-movement-modal-grid { grid-template-columns: 1fr !important; }
}
`;

const getModalBg = () => {
  if (typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'))) {
    return { background: '#273142', color: '#f1f1f1' };
  }
  return { background: '#fff', color: '#23272f' };
};

// Inputlar her zaman beyaz, yazı rengi koyu, border açık gri
const getInputStyles = () => ({
  background: '#fff',
  color: '#23272f',
  borderColor: '#e0e0e0',
});

const validationSchema = yup.object().shape({
  MaterialCardId: yup.string().required('Malzeme seçimi zorunludur.'),
  MovementType: yup.string().required('Hareket tipi zorunludur.'),
  Quantity: yup.number().typeError('Miktar sayı olmalı.').required('Miktar zorunludur.').notOneOf([0], 'Miktar sıfır olamaz.'),
  MovementDate: yup.date().required('Hareket tarihi zorunludur.').max(new Date(Date.now() + 24*60*60*1000), 'Hareket tarihi en fazla 1 gün ileri olabilir.'),
  ReferenceNumber: yup.string().max(100, 'Referans numarası en fazla 100 karakter olabilir.'),
  ReferenceType: yup.string().max(50, 'Referans tipi en fazla 50 karakter olabilir.'),
  LocationFrom: yup.string().max(100, 'Çıkış lokasyonu en fazla 100 karakter olabilir.'),
  LocationTo: yup.string().max(100, 'Varış lokasyonu en fazla 100 karakter olabilir.'),
  Description: yup.string().max(500, 'Açıklama en fazla 500 karakter olabilir.'),
  UnitPrice: yup.number().typeError('Birim fiyat sayı olmalı.').min(0, 'Birim fiyat pozitif olmalı.').nullable(),
  TotalAmount: yup.number().typeError('Toplam tutar sayı olmalı.').min(0, 'Toplam tutar pozitif olmalı.').nullable(),
  ResponsiblePerson: yup.string().max(100, 'Sorumlu kişi en fazla 100 karakter olabilir.'),
  SupplierCustomer: yup.string().max(100, 'Tedarikçi/Müşteri en fazla 100 karakter olabilir.'),
  BatchNumber: yup.string().max(50, 'Parti numarası en fazla 50 karakter olabilir.'),
  SerialNumber: yup.string().max(50, 'Seri numarası en fazla 50 karakter olabilir.'),
  ExpiryDate: yup.date().min(new Date(), 'Son kullanma tarihi gelecek tarih olmalı.').nullable(),
  StockBalance: yup.number().typeError('Stok bakiyesi sayı olmalı.').min(0, 'Stok bakiyesi pozitif olmalı.').nullable(),
});

const MaterialMovementModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Malzeme Hareketi Ekle',
  materialCards = [],
  isDetail = false,
}) => {
  const [validationErrors, setValidationErrors] = React.useState({});

  const handleSubmit = async (e) => {
    e.preventDefault();
    setValidationErrors({});
    
    // Comprehensive frontend validation
    const errors = {};
    const now = new Date();
    const tomorrow = new Date(now.getTime() + 24 * 60 * 60 * 1000);
    
    // MaterialCardId validation
    if (!form.MaterialCardId) {
      errors.MaterialCardId = 'Malzeme seçimi zorunludur';
    } else if (!materialCards.find(m => m.id == form.MaterialCardId)) {
      errors.MaterialCardId = 'Geçersiz malzeme seçimi';
    }
    
    // MovementType validation
    if (!form.MovementType) {
      errors.MovementType = 'Hareket tipi zorunludur';
    }
    
    // Quantity validation
    if (!form.Quantity || parseFloat(form.Quantity) === 0) {
      errors.Quantity = 'Miktar sıfır olamaz';
    } else if (parseFloat(form.Quantity) < 0) {
      errors.Quantity = 'Miktar pozitif olmalı';
    }
    
    // MovementDate validation
    if (!form.MovementDate) {
      errors.MovementDate = 'Hareket tarihi zorunludur';
    } else {
      const movementDate = new Date(form.MovementDate);
      if (movementDate > tomorrow) {
        errors.MovementDate = 'Hareket tarihi en fazla 1 gün ileride olabilir';
      }
    }
    
    // ExpiryDate validation (if provided)
    if (form.ExpiryDate) {
      const expiryDate = new Date(form.ExpiryDate);
      if (expiryDate <= now) {
        errors.ExpiryDate = 'Son kullanma tarihi gelecek tarih olmalı';
      }
    }
    
    // Length validations
    if (form.ReferenceNumber && form.ReferenceNumber.length > 100) {
      errors.ReferenceNumber = 'Referans numarası en fazla 100 karakter olabilir';
    }
    if (form.LocationFrom && form.LocationFrom.length > 100) {
      errors.LocationFrom = 'Çıkış lokasyonu en fazla 100 karakter olabilir';
    }
    if (form.LocationTo && form.LocationTo.length > 100) {
      errors.LocationTo = 'Varış lokasyonu en fazla 100 karakter olabilir';
    }
    if (form.Description && form.Description.length > 500) {
      errors.Description = 'Açıklama en fazla 500 karakter olabilir';
    }
    if (form.ResponsiblePerson && form.ResponsiblePerson.length > 100) {
      errors.ResponsiblePerson = 'Sorumlu kişi en fazla 100 karakter olabilir';
    }
    if (form.SupplierCustomer && form.SupplierCustomer.length > 100) {
      errors.SupplierCustomer = 'Tedarikçi/Müşteri en fazla 100 karakter olabilir';
    }
    if (form.BatchNumber && form.BatchNumber.length > 50) {
      errors.BatchNumber = 'Parti numarası en fazla 50 karakter olabilir';
    }
    if (form.SerialNumber && form.SerialNumber.length > 50) {
      errors.SerialNumber = 'Seri numarası en fazla 50 karakter olabilir';
    }
    
    // Price validations
    if (form.UnitPrice && parseFloat(form.UnitPrice) < 0) {
      errors.UnitPrice = 'Birim fiyat pozitif olmalı';
    }
    if (form.TotalAmount && parseFloat(form.TotalAmount) < 0) {
      errors.TotalAmount = 'Toplam tutar pozitif olmalı';
    }
    if (form.StockBalance && parseFloat(form.StockBalance) < 0) {
      errors.StockBalance = 'Stok bakiyesi pozitif olmalı';
    }
    
    // If validation errors exist, show them
    if (Object.keys(errors).length > 0) {
      setValidationErrors(errors);
      return;
    }
    
    // Proceed with original validation and submit
    try {
      await validationSchema.validate(form, { abortEarly: false });
      if (onSubmit) onSubmit(e);
    } catch (validationErr) {
      if (validationErr.inner) {
        const yupErrors = {};
        validationErr.inner.forEach(err => {
          yupErrors[err.path] = err.message;
        });
        setValidationErrors(yupErrors);
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
          <div style={gridStyle} className="material-movement-modal-grid">
            {/* Sütun 1 */}
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Malzeme *</Form.Label>
                <Form.Select name="MaterialCardId" value={form.MaterialCardId} onChange={onChange} required isInvalid={!!validationErrors.MaterialCardId} style={inputStyles} disabled={isDetail}>
                  <option value="">Seçiniz</option>
                  {materialCards.map(mat => <option key={mat.id} value={mat.id}>{mat.code} - {mat.name}</option>)}
                </Form.Select>
                {validationErrors.MaterialCardId && <Form.Control.Feedback type="invalid">{validationErrors.MaterialCardId}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Hareket Tipi *</Form.Label>
                <Form.Select name="MovementType" value={form.MovementType} onChange={onChange} required isInvalid={!!validationErrors.MovementType} style={inputStyles} disabled={isDetail}>
                  <option value="">Seçiniz</option>
                  <option value="IN">Giriş (IN)</option>
                  <option value="OUT">Çıkış (OUT)</option>
                  <option value="TRANSFER">Transfer</option>
                  <option value="ADJUSTMENT">Ayarlama</option>
                </Form.Select>
                {validationErrors.MovementType && <Form.Control.Feedback type="invalid">{validationErrors.MovementType}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Miktar *</Form.Label>
                <Form.Control type="number" name="Quantity" value={form.Quantity} onChange={onChange} required min={0.01} step={0.01} isInvalid={!!validationErrors.Quantity} style={inputStyles} readOnly={isDetail} />
                {validationErrors.Quantity && <Form.Control.Feedback type="invalid">{validationErrors.Quantity}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Hareket Tarihi *</Form.Label>
                <Form.Control type="datetime-local" name="MovementDate" value={form.MovementDate} onChange={onChange} required isInvalid={!!validationErrors.MovementDate} style={inputStyles} readOnly={isDetail} />
                {validationErrors.MovementDate && <Form.Control.Feedback type="invalid">{validationErrors.MovementDate}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Referans Numarası</Form.Label>
                <Form.Control type="text" name="ReferenceNumber" value={form.ReferenceNumber} onChange={onChange} maxLength={100} isInvalid={!!validationErrors.ReferenceNumber} style={inputStyles} readOnly={isDetail} />
                {validationErrors.ReferenceNumber && <Form.Control.Feedback type="invalid">{validationErrors.ReferenceNumber}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Referans Tipi</Form.Label>
                <Form.Select name="ReferenceType" value={form.ReferenceType} onChange={onChange} style={inputStyles} disabled={isDetail}>
                  <option value="">Seçiniz</option>
                  <option value="PURCHASE_ORDER">Satın Alma Siparişi</option>
                  <option value="SALES_ORDER">Satış Siparişi</option>
                  <option value="WORK_ORDER">İş Emri</option>
                  <option value="PRODUCTION_ORDER">Üretim Emri</option>
                  <option value="INVENTORY_ADJUSTMENT">Stok Ayarlama</option>
                  <option value="TRANSFER">Transfer</option>
                  <option value="OTHER">Diğer</option>
                </Form.Select>
                {validationErrors.ReferenceType && <Form.Control.Feedback type="invalid">{validationErrors.ReferenceType}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Açıklama</Form.Label>
                <Form.Control as="textarea" rows={3} name="Description" value={form.Description} onChange={onChange} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
            </div>
            {/* Sütun 2 */}
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Çıkış Lokasyonu</Form.Label>
                <Form.Control type="text" name="LocationFrom" value={form.LocationFrom} onChange={onChange} maxLength={100} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Varış Lokasyonu</Form.Label>
                <Form.Control type="text" name="LocationTo" value={form.LocationTo} onChange={onChange} maxLength={100} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Birim Fiyat</Form.Label>
                <Form.Control type="number" name="UnitPrice" value={form.UnitPrice} onChange={onChange} min={0} step={0.01} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Toplam Tutar</Form.Label>
                <Form.Control type="number" name="TotalAmount" value={form.TotalAmount} onChange={onChange} min={0} step={0.01} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Sorumlu Kişi</Form.Label>
                <Form.Control type="text" name="ResponsiblePerson" value={form.ResponsiblePerson} onChange={onChange} maxLength={100} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Tedarikçi/Müşteri</Form.Label>
                <Form.Control type="text" name="SupplierCustomer" value={form.SupplierCustomer} onChange={onChange} maxLength={100} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Stok Bakiyesi</Form.Label>
                <Form.Control type="number" name="StockBalance" value={form.StockBalance} onChange={onChange} min={0} step={0.01} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
            </div>
            {/* Sütun 3 */}
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Parti Numarası</Form.Label>
                <Form.Control type="text" name="BatchNumber" value={form.BatchNumber} onChange={onChange} maxLength={50} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Seri Numarası</Form.Label>
                <Form.Control type="text" name="SerialNumber" value={form.SerialNumber} onChange={onChange} maxLength={50} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Son Kullanma Tarihi</Form.Label>
                <Form.Control type="date" name="ExpiryDate" value={form.ExpiryDate} onChange={onChange} style={inputStyles} readOnly={isDetail} />
              </Form.Group>
              <Form.Group className="mb-3" style={{marginTop: 120}}>
                <Form.Check type="checkbox" label="Aktif mi?" name="IsActive" checked={form.IsActive} onChange={onChange} id="isActiveCheck" disabled={isDetail} />
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

export default MaterialMovementModal; 