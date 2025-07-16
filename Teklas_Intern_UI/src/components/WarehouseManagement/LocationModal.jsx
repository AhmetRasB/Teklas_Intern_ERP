import React, { useState, useEffect } from 'react';
import { Button, Form } from 'react-bootstrap';
import * as yup from 'yup';
import axios from 'axios';

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
  .location-modal-grid { grid-template-columns: 1fr !important; }
}
`;

// --- THEME SUPPORT ---
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

const validationSchema = yup.object().shape({
  Code: yup.string().required('Lokasyon kodu zorunludur.').min(2, 'Lokasyon kodu en az 2 karakter olmalı.').max(20, 'Lokasyon kodu en fazla 20 karakter olabilir.'),
  Name: yup.string().required('Lokasyon adı zorunludur.').min(2, 'Lokasyon adı en az 2 karakter olmalı.').max(200, 'Lokasyon adı en fazla 200 karakter olabilir.'),
  WarehouseId: yup.string().required('Depo seçimi zorunludur.'),
  LocationType: yup.string().required('Lokasyon tipi zorunludur.').max(50, 'Lokasyon tipi en fazla 50 karakter olabilir.'),
  Capacity: yup.number().typeError('Kapasite sayı olmalı.').min(0, 'Kapasite pozitif olmalı.').nullable(),
  Length: yup.number().typeError('Uzunluk sayı olmalı.').min(0, 'Uzunluk pozitif olmalı.').nullable(),
  Width: yup.number().typeError('Genişlik sayı olmalı.').min(0, 'Genişlik pozitif olmalı.').nullable(),
  Height: yup.number().typeError('Yükseklik sayı olmalı.').min(0, 'Yükseklik pozitif olmalı.').nullable(),
  WeightCapacity: yup.number().typeError('Ağırlık kapasitesi sayı olmalı.').min(0, 'Ağırlık kapasitesi pozitif olmalı.').nullable(),
  Temperature: yup.number().typeError('Sıcaklık sayı olmalı.').min(-50, 'Sıcaklık -50 ile 100 arasında olmalı.').max(100, 'Sıcaklık -50 ile 100 arasında olmalı.').nullable(),
  Humidity: yup.number().typeError('Nem sayı olmalı.').min(0, 'Nem 0 ile 100 arasında olmalı.').max(100, 'Nem 0 ile 100 arasında olmalı.').nullable(),
  Description: yup.string().max(1000, 'Açıklama en fazla 1000 karakter olabilir.'),
  IsActive: yup.boolean(),
});

const LocationModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Lokasyon Tanımla',
}) => {
  const [validationErrors, setValidationErrors] = useState({});
  const [warehouses, setWarehouses] = useState([]);

  useEffect(() => {
    if (open) {
      fetchWarehouses();
    }
  }, [open]);

  const fetchWarehouses = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/warehouses`);
      setWarehouses(response.data);
    } catch (err) {
      console.error('Error fetching warehouses:', err);
    }
  };

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
          <div style={gridStyle} className="location-modal-grid">
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Kod *</Form.Label>
                <Form.Control type="text" name="Code" value={form.Code || ''} onChange={onChange} required maxLength={20} isInvalid={!!validationErrors.Code} style={inputStyles} />
                {validationErrors.Code && <Form.Control.Feedback type="invalid">{validationErrors.Code}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Ad *</Form.Label>
                <Form.Control type="text" name="Name" value={form.Name || ''} onChange={onChange} required maxLength={200} isInvalid={!!validationErrors.Name} style={inputStyles} />
                {validationErrors.Name && <Form.Control.Feedback type="invalid">{validationErrors.Name}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Depo *</Form.Label>
                <Form.Select name="WarehouseId" value={form.WarehouseId || ''} onChange={onChange} required isInvalid={!!validationErrors.WarehouseId} style={inputStyles}>
                  <option value="">Depo seçin</option>
                  {warehouses.map(warehouse => (
                    <option key={warehouse.id} value={warehouse.id}>
                      {warehouse.name} ({warehouse.code})
                    </option>
                  ))}
                </Form.Select>
                {validationErrors.WarehouseId && <Form.Control.Feedback type="invalid">{validationErrors.WarehouseId}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Lokasyon Tipi *</Form.Label>
                <Form.Select name="LocationType" value={form.LocationType || ''} onChange={onChange} required isInvalid={!!validationErrors.LocationType} style={inputStyles}>
                  <option value="">Lokasyon tipi seçin</option>
                  <option value="Raf">Raf</option>
                  <option value="Koridor">Koridor</option>
                  <option value="Bölge">Bölge</option>
                  <option value="Hücre">Hücre</option>
                  <option value="Palet">Palet</option>
                  <option value="Konteyner">Konteyner</option>
                </Form.Select>
                {validationErrors.LocationType && <Form.Control.Feedback type="invalid">{validationErrors.LocationType}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Koridor</Form.Label>
                <Form.Control type="text" name="Aisle" value={form.Aisle || ''} onChange={onChange} maxLength={10} style={inputStyles} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Raf</Form.Label>
                <Form.Control type="text" name="Rack" value={form.Rack || ''} onChange={onChange} maxLength={10} style={inputStyles} />
              </Form.Group>
            </div>
            <div>
              <Form.Group className="mb-3">
                <Form.Label>Seviye</Form.Label>
                <Form.Control type="text" name="Level" value={form.Level || ''} onChange={onChange} maxLength={10} style={inputStyles} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Pozisyon</Form.Label>
                <Form.Control type="text" name="Position" value={form.Position || ''} onChange={onChange} maxLength={10} style={inputStyles} />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Kapasite</Form.Label>
                <Form.Control type="number" step="0.01" name="Capacity" value={form.Capacity || ''} onChange={onChange} isInvalid={!!validationErrors.Capacity} style={inputStyles} />
                {validationErrors.Capacity && <Form.Control.Feedback type="invalid">{validationErrors.Capacity}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Uzunluk (m)</Form.Label>
                <Form.Control type="number" step="0.01" name="Length" value={form.Length || ''} onChange={onChange} isInvalid={!!validationErrors.Length} style={inputStyles} />
                {validationErrors.Length && <Form.Control.Feedback type="invalid">{validationErrors.Length}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Genişlik (m)</Form.Label>
                <Form.Control type="number" step="0.01" name="Width" value={form.Width || ''} onChange={onChange} isInvalid={!!validationErrors.Width} style={inputStyles} />
                {validationErrors.Width && <Form.Control.Feedback type="invalid">{validationErrors.Width}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Yükseklik (m)</Form.Label>
                <Form.Control type="number" step="0.01" name="Height" value={form.Height || ''} onChange={onChange} isInvalid={!!validationErrors.Height} style={inputStyles} />
                {validationErrors.Height && <Form.Control.Feedback type="invalid">{validationErrors.Height}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Ağırlık Kapasitesi (kg)</Form.Label>
                <Form.Control type="number" step="0.01" name="WeightCapacity" value={form.WeightCapacity || ''} onChange={onChange} isInvalid={!!validationErrors.WeightCapacity} style={inputStyles} />
                {validationErrors.WeightCapacity && <Form.Control.Feedback type="invalid">{validationErrors.WeightCapacity}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Sıcaklık (°C)</Form.Label>
                <Form.Control type="number" step="0.1" name="Temperature" value={form.Temperature || ''} onChange={onChange} isInvalid={!!validationErrors.Temperature} style={inputStyles} />
                {validationErrors.Temperature && <Form.Control.Feedback type="invalid">{validationErrors.Temperature}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Nem (%)</Form.Label>
                <Form.Control type="number" step="0.1" name="Humidity" value={form.Humidity || ''} onChange={onChange} isInvalid={!!validationErrors.Humidity} style={inputStyles} />
                {validationErrors.Humidity && <Form.Control.Feedback type="invalid">{validationErrors.Humidity}</Form.Control.Feedback>}
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Durum</Form.Label>
                <Form.Select
                  name="IsActive"
                  value={form.IsActive ? 'true' : 'false'}
                  onChange={e => onChange({
                    target: {
                      name: 'IsActive',
                      value: e.target.value === 'true'
                    }
                  })}
                  required
                  isInvalid={!!validationErrors.IsActive}
                  style={inputStyles}
                >
                  <option value="true">Aktif</option>
                  <option value="false">Pasif</option>
                </Form.Select>
                {validationErrors.IsActive && <Form.Control.Feedback type="invalid">{validationErrors.IsActive}</Form.Control.Feedback>}
              </Form.Group>
            </div>
          </div>
          <Form.Group className="mb-3">
            <Form.Label>Açıklama</Form.Label>
            <Form.Control as="textarea" rows={3} name="Description" value={form.Description || ''} onChange={onChange} maxLength={1000} isInvalid={!!validationErrors.Description} style={inputStyles} />
            {validationErrors.Description && <Form.Control.Feedback type="invalid">{validationErrors.Description}</Form.Control.Feedback>}
          </Form.Group>
          <div className="d-flex justify-content-end mt-4">
            <Button variant="secondary" onClick={onClose} className="me-2">İptal</Button>
            <Button variant="primary" type="submit" disabled={loading}>{loading ? 'Kaydediliyor...' : 'Kaydet'}</Button>
          </div>
        </Form>
      </div>
    </div>
  );
};

export default LocationModal; 