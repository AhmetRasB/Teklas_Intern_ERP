import React from 'react';
import { Button, Form } from 'react-bootstrap';

const WorkOrderModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'İş Emri Ekle',
  isDetail = false,
}) => {
  if (!open) return null;
  return (
    <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, background: 'rgba(0,0,0,0.3)', zIndex: 1050, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <div style={{ borderRadius: 16, boxShadow: '0 8px 32px rgba(0,0,0,0.15)', width: 'min(100vw, 700px)', maxWidth: '100vw', minWidth: 'min(96vw, 700px)', minHeight: '300px', maxHeight: '96vh', overflowX: 'hidden', overflowY: 'auto', padding: 32, display: 'flex', flexDirection: 'column', position: 'relative', boxSizing: 'border-box', background: '#fff' }}>
        <button style={{ position: 'absolute', top: 16, right: 24, fontSize: 24, background: 'none', border: 'none', cursor: 'pointer', zIndex: 2 }} onClick={onClose} aria-label="Kapat">×</button>
        <h4 style={{ marginBottom: 20, textAlign: 'center', fontWeight: 600, fontSize: '24px', lineHeight: 1.1, color: '#23272f' }}>{title}</h4>
        <Form onSubmit={onSubmit} style={{ flex: 1, overflow: 'auto' }}>
          {error && <div className="alert alert-danger mb-4">{error}</div>}
          <Form.Group className="mb-3">
            <Form.Label>BOM ID *</Form.Label>
            <Form.Control type="number" name="bomHeaderId" value={form.bomHeaderId} onChange={onChange} required readOnly={isDetail} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Ürün ID *</Form.Label>
            <Form.Control type="number" name="materialCardId" value={form.materialCardId} onChange={onChange} required readOnly={isDetail} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Planlanan Miktar *</Form.Label>
            <Form.Control type="number" step="0.01" name="plannedQuantity" value={form.plannedQuantity} onChange={onChange} required readOnly={isDetail} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Planlanan Başlangıç Tarihi *</Form.Label>
            <Form.Control 
              type="datetime-local" 
              name="plannedStartDate" 
              value={form.plannedStartDate || ''} 
              onChange={onChange} 
              required 
              readOnly={isDetail} 
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Planlanan Bitiş Tarihi</Form.Label>
            <Form.Control 
              type="datetime-local" 
              name="plannedEndDate" 
              value={form.plannedEndDate || ''} 
              onChange={onChange} 
              readOnly={isDetail} 
            />
          </Form.Group>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 16, marginTop: 32 }}>
            <Button variant="secondary" type="button" onClick={onClose} disabled={loading}>Kapat</Button>
            {!isDetail && <Button variant="primary" type="submit" disabled={loading}>{loading ? 'Kaydediliyor...' : 'Kaydet'}</Button>}
          </div>
        </Form>
      </div>
    </div>
  );
};

export default WorkOrderModal; 