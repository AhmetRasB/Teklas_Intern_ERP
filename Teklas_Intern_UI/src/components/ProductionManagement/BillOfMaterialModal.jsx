import React from 'react';
import { Button, Form } from 'react-bootstrap';

const BillOfMaterialModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'BOM Ekle',
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
            <Form.Label>BOM Kodu (Version) *</Form.Label>
            <Form.Control type="text" name="version" value={form.version} onChange={onChange} required readOnly={isDetail} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Açıklama (Notes)</Form.Label>
            <Form.Control as="textarea" rows={3} name="notes" value={form.notes} onChange={onChange} readOnly={isDetail} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Ürün (Parent Material Card ID) *</Form.Label>
            <Form.Control type="number" name="parentMaterialCardId" value={form.parentMaterialCardId} onChange={onChange} required readOnly={isDetail} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Geçerlilik Başlangıcı *</Form.Label>
            <Form.Control 
              type="datetime-local" 
              name="validFrom" 
              value={form.validFrom ? new Date(form.validFrom).toISOString().slice(0, 16) : ''} 
              onChange={onChange} 
              required 
              readOnly={isDetail} 
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Geçerlilik Sonu</Form.Label>
            <Form.Control 
              type="datetime-local" 
              name="validTo" 
              value={form.validTo ? new Date(form.validTo).toISOString().slice(0, 16) : ''} 
              onChange={onChange} 
              readOnly={isDetail} 
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Standart Maliyet</Form.Label>
            <Form.Control type="number" step="0.01" name="standardCost" value={form.standardCost} onChange={onChange} readOnly={isDetail} />
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

export default BillOfMaterialModal; 