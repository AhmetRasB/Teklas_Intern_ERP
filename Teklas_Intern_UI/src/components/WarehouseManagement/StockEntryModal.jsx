import React, { useState, useEffect } from 'react';
import { Button, Form } from 'react-bootstrap';
import * as yup from 'yup';
import axios from 'axios';

const BASE_URL = 'https://localhost:7178';

const StockEntryModal = ({ open, onClose, onSubmit, form, onChange, loading, error, title = 'Stok Girişi Tanımla' }) => {
  const [validationErrors, setValidationErrors] = useState({});
  const [warehouses, setWarehouses] = useState([]);
  const [locations, setLocations] = useState([]);
  const [materials, setMaterials] = useState([]);

  useEffect(() => {
    if (open) {
      fetchWarehouses();
      fetchMaterials();
      if (form.WarehouseId) fetchLocations(form.WarehouseId);
    }
    // eslint-disable-next-line
  }, [open]);

  useEffect(() => {
    if (form.WarehouseId) fetchLocations(form.WarehouseId);
    // eslint-disable-next-line
  }, [form.WarehouseId]);

  const fetchWarehouses = async () => {
    try {
      const res = await axios.get(`${BASE_URL}/api/warehouses`);
      setWarehouses(res.data);
    } catch {}
  };
  const fetchLocations = async (warehouseId) => {
    try {
      const res = await axios.get(`${BASE_URL}/api/locations?warehouseId=${warehouseId}`);
      setLocations(res.data.items || res.data);
    } catch {}
  };
  const fetchMaterials = async () => {
    try {
      const res = await axios.get(`${BASE_URL}/api/materials`);
      setMaterials(res.data);
    } catch {}
  };

  const validationSchema = yup.object().shape({
    EntryNumber: yup.string().required('Giriş numarası zorunludur.').min(2).max(20),
    EntryDate: yup.date().required('Giriş tarihi zorunludur.'),
    WarehouseId: yup.string().required('Depo zorunludur.'),
    LocationId: yup.string().required('Lokasyon zorunludur.'),
    MaterialId: yup.string().required('Malzeme zorunludur.'),
    EntryType: yup.string().required('Giriş tipi zorunludur.').max(50),
    Quantity: yup.number().required('Miktar zorunludur.').min(0.01),
    UnitPrice: yup.number().nullable().min(0),
    TotalValue: yup.number().nullable().min(0),
    BatchNumber: yup.string().max(50),
    SerialNumber: yup.string().max(50),
    ExpiryDate: yup.date().nullable(),
    ProductionDate: yup.date().nullable(),
    QualityStatus: yup.string().max(50),
    Notes: yup.string().max(1000),
    EntryReason: yup.string().max(200),
    ResponsiblePerson: yup.string().max(100),
    IsActive: yup.boolean(),
  });

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
    width: 'min(100vw, 800px)',
    maxWidth: '100vw',
    minWidth: 'min(96vw, 800px)',
    minHeight: '400px',
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
  .stockentry-modal-grid { grid-template-columns: 1fr !important; }
}
`;

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

  return (
    <div style={modalOverlayStyle}>
      <style>{responsiveStyle}</style>
      <div style={{...a4LandscapeStyle, ...getModalBg()}} className="a4-landscape-modal">
        <button style={closeBtnStyle} onClick={onClose} aria-label="Kapat">×</button>
        <h4 style={{marginBottom: 20, textAlign: 'center', fontWeight: 600, fontSize: '24px', lineHeight: 1.1}}>{title}</h4>
        <Form onSubmit={handleSubmit} style={{flex: 1, overflow: 'auto'}}>
          {error && <div className="alert alert-danger mb-4">{error}</div>}
          <div style={gridStyle} className="stockentry-modal-grid">
            <div className="col-md-6">
              <Form.Group className="mb-3">
                <Form.Label>Giriş Numarası *</Form.Label>
                <Form.Control type="text" name="EntryNumber" value={form.EntryNumber || ''} onChange={onChange} isInvalid={!!validationErrors.EntryNumber} style={getInputStyles()} />
                {validationErrors.EntryNumber && <Form.Control.Feedback type="invalid">{validationErrors.EntryNumber}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Giriş Tarihi *</Form.Label>
                <Form.Control type="date" name="EntryDate" value={form.EntryDate ? form.EntryDate.substring(0,10) : ''} onChange={onChange} isInvalid={!!validationErrors.EntryDate} style={getInputStyles()} />
                {validationErrors.EntryDate && <Form.Control.Feedback type="invalid">{validationErrors.EntryDate}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Depo *</Form.Label>
                <Form.Select name="WarehouseId" value={form.WarehouseId || ''} onChange={onChange} isInvalid={!!validationErrors.WarehouseId} style={getInputStyles()}>
                  <option value="">Depo seçin</option>
                  {warehouses.map(w => <option key={w.id} value={w.id}>{w.name} ({w.code})</option>)}
                </Form.Select>
                {validationErrors.WarehouseId && <Form.Control.Feedback type="invalid">{validationErrors.WarehouseId}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Lokasyon *</Form.Label>
                <Form.Select name="LocationId" value={form.LocationId || ''} onChange={onChange} isInvalid={!!validationErrors.LocationId} style={getInputStyles()}>
                  <option value="">Lokasyon seçin</option>
                  {locations.map(l => <option key={l.id} value={l.id}>{l.name} ({l.code})</option>)}
                </Form.Select>
                {validationErrors.LocationId && <Form.Control.Feedback type="invalid">{validationErrors.LocationId}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Malzeme *</Form.Label>
                <Form.Select name="MaterialId" value={form.MaterialId || ''} onChange={onChange} isInvalid={!!validationErrors.MaterialId} style={getInputStyles()}>
                  <option value="">Malzeme seçin</option>
                  {materials.map(m => <option key={m.id} value={m.id}>{m.name} ({m.code})</option>)}
                </Form.Select>
                {validationErrors.MaterialId && <Form.Control.Feedback type="invalid">{validationErrors.MaterialId}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Giriş Tipi *</Form.Label>
                <Form.Control type="text" name="EntryType" value={form.EntryType || ''} onChange={onChange} isInvalid={!!validationErrors.EntryType} style={getInputStyles()} />
                {validationErrors.EntryType && <Form.Control.Feedback type="invalid">{validationErrors.EntryType}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Miktar *</Form.Label>
                <Form.Control type="number" name="Quantity" value={form.Quantity || ''} onChange={onChange} isInvalid={!!validationErrors.Quantity} style={getInputStyles()} />
                {validationErrors.Quantity && <Form.Control.Feedback type="invalid">{validationErrors.Quantity}</Form.Control.Feedback>}
              </Form.Group>
            </div>
            <div className="col-md-6">
              <Form.Group className="mb-3">
                <Form.Label>Birim Fiyat</Form.Label>
                <Form.Control type="number" name="UnitPrice" value={form.UnitPrice || ''} onChange={onChange} isInvalid={!!validationErrors.UnitPrice} style={getInputStyles()} />
                {validationErrors.UnitPrice && <Form.Control.Feedback type="invalid">{validationErrors.UnitPrice}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Toplam Değer</Form.Label>
                <Form.Control type="number" name="TotalValue" value={form.TotalValue || ''} onChange={onChange} isInvalid={!!validationErrors.TotalValue} style={getInputStyles()} />
                {validationErrors.TotalValue && <Form.Control.Feedback type="invalid">{validationErrors.TotalValue}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Parti No</Form.Label>
                <Form.Control type="text" name="BatchNumber" value={form.BatchNumber || ''} onChange={onChange} isInvalid={!!validationErrors.BatchNumber} style={getInputStyles()} />
                {validationErrors.BatchNumber && <Form.Control.Feedback type="invalid">{validationErrors.BatchNumber}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Seri No</Form.Label>
                <Form.Control type="text" name="SerialNumber" value={form.SerialNumber || ''} onChange={onChange} isInvalid={!!validationErrors.SerialNumber} style={getInputStyles()} />
                {validationErrors.SerialNumber && <Form.Control.Feedback type="invalid">{validationErrors.SerialNumber}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Son Kullanma Tarihi</Form.Label>
                <Form.Control type="date" name="ExpiryDate" value={form.ExpiryDate ? form.ExpiryDate.substring(0,10) : ''} onChange={onChange} isInvalid={!!validationErrors.ExpiryDate} style={getInputStyles()} />
                {validationErrors.ExpiryDate && <Form.Control.Feedback type="invalid">{validationErrors.ExpiryDate}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Üretim Tarihi</Form.Label>
                <Form.Control type="date" name="ProductionDate" value={form.ProductionDate ? form.ProductionDate.substring(0,10) : ''} onChange={onChange} isInvalid={!!validationErrors.ProductionDate} style={getInputStyles()} />
                {validationErrors.ProductionDate && <Form.Control.Feedback type="invalid">{validationErrors.ProductionDate}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Kalite Durumu</Form.Label>
                <Form.Control type="text" name="QualityStatus" value={form.QualityStatus || ''} onChange={onChange} isInvalid={!!validationErrors.QualityStatus} style={getInputStyles()} />
                {validationErrors.QualityStatus && <Form.Control.Feedback type="invalid">{validationErrors.QualityStatus}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Notlar</Form.Label>
                <Form.Control as="textarea" name="Notes" value={form.Notes || ''} onChange={onChange} isInvalid={!!validationErrors.Notes} style={getInputStyles()} />
                {validationErrors.Notes && <Form.Control.Feedback type="invalid">{validationErrors.Notes}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Giriş Nedeni</Form.Label>
                <Form.Control type="text" name="EntryReason" value={form.EntryReason || ''} onChange={onChange} isInvalid={!!validationErrors.EntryReason} style={getInputStyles()} />
                {validationErrors.EntryReason && <Form.Control.Feedback type="invalid">{validationErrors.EntryReason}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Sorumlu Kişi</Form.Label>
                <Form.Control type="text" name="ResponsiblePerson" value={form.ResponsiblePerson || ''} onChange={onChange} isInvalid={!!validationErrors.ResponsiblePerson} style={getInputStyles()} />
                {validationErrors.ResponsiblePerson && <Form.Control.Feedback type="invalid">{validationErrors.ResponsiblePerson}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Durum</Form.Label>
                <Form.Select name="IsActive" value={form.IsActive ? 'true' : 'false'} onChange={e => onChange({ target: { name: 'IsActive', value: e.target.value === 'true' } })} style={getInputStyles()}>
                  <option value="true">Aktif</option>
                  <option value="false">Pasif</option>
                </Form.Select>
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

export default StockEntryModal; 