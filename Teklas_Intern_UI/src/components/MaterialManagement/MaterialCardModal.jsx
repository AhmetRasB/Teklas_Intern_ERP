import React from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const categories = [
  { value: 1, label: 'Hammadde' },
  { value: 2, label: 'Yarı Mamul' },
  { value: 3, label: 'Mamül' },
];

const modalStyle = {
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

const contentStyle = {
  background: '#fff',
  borderRadius: 12,
  boxShadow: '0 8px 32px rgba(0,0,0,0.15)',
  width: 600,
  maxWidth: '98vw',
  animation: 'modalFadeIn 0.35s cubic-bezier(.4,0,.2,1)',
  padding: 0,
};

const keyframes = `@keyframes modalFadeIn {0%{opacity:0;transform:scale(0.92);}100%{opacity:1;transform:scale(1);}}`;

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

const MaterialCardModal = ({
  open,
  onClose,
  onSubmit,
  form,
  onChange,
  loading,
  error,
  title = 'Malzeme Kartı Ekle',
}) => {
  return (
    <Modal show={open} onHide={onClose} centered>
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Form onSubmit={onSubmit}>
        <Modal.Body>
          {error && <div className="alert alert-danger mb-4">{error}</div>}
          <Form.Group className="mb-3">
            <Form.Label>Malzeme Kodu *</Form.Label>
            <Form.Control type="text" name="Code" value={form.Code} onChange={onChange} required maxLength={30} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Malzeme Adı *</Form.Label>
            <Form.Control type="text" name="Name" value={form.Name} onChange={onChange} required maxLength={100} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Malzeme Tipi *</Form.Label>
            <Form.Control type="text" name="MaterialType" value={form.MaterialType} onChange={onChange} required maxLength={50} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Kategori *</Form.Label>
            <Form.Select name="CategoryId" value={form.CategoryId} onChange={onChange} required>
              <option value="">Seçiniz</option>
              {categories.map(cat => <option key={cat.value} value={cat.value}>{cat.label}</option>)}
            </Form.Select>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Birim *</Form.Label>
            <Form.Control type="text" name="UnitOfMeasure" value={form.UnitOfMeasure} onChange={onChange} required maxLength={10} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Barkod</Form.Label>
            <Form.Control type="text" name="Barcode" value={form.Barcode} onChange={onChange} maxLength={50} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Açıklama</Form.Label>
            <Form.Control type="text" name="Description" value={form.Description} onChange={onChange} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Marka</Form.Label>
            <Form.Control type="text" name="Brand" value={form.Brand} onChange={onChange} maxLength={50} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Model</Form.Label>
            <Form.Control type="text" name="Model" value={form.Model} onChange={onChange} maxLength={50} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Alış Fiyatı *</Form.Label>
            <Form.Control type="number" name="PurchasePrice" value={form.PurchasePrice} onChange={onChange} min={0} required />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Satış Fiyatı *</Form.Label>
            <Form.Control type="number" name="SalesPrice" value={form.SalesPrice} onChange={onChange} min={0} required />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Minimum Stok Seviyesi</Form.Label>
            <Form.Control type="number" name="MinimumStockLevel" value={form.MinimumStockLevel} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Maksimum Stok Seviyesi</Form.Label>
            <Form.Control type="number" name="MaximumStockLevel" value={form.MaximumStockLevel} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Yeniden Sipariş Seviyesi</Form.Label>
            <Form.Control type="number" name="ReorderLevel" value={form.ReorderLevel} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Raf Ömrü (gün)</Form.Label>
            <Form.Control type="number" name="ShelfLife" value={form.ShelfLife} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Ağırlık (kg)</Form.Label>
            <Form.Control type="number" name="Weight" value={form.Weight} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Hacim (m³)</Form.Label>
            <Form.Control type="number" name="Volume" value={form.Volume} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Uzunluk (cm)</Form.Label>
            <Form.Control type="number" name="Length" value={form.Length} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Genişlik (cm)</Form.Label>
            <Form.Control type="number" name="Width" value={form.Width} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Yükseklik (cm)</Form.Label>
            <Form.Control type="number" name="Height" value={form.Height} onChange={onChange} min={0} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Renk</Form.Label>
            <Form.Control type="text" name="Color" value={form.Color} onChange={onChange} maxLength={30} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Menşei Ülke</Form.Label>
            <Form.Control type="text" name="OriginCountry" value={form.OriginCountry} onChange={onChange} maxLength={50} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Üretici</Form.Label>
            <Form.Control type="text" name="Manufacturer" value={form.Manufacturer} onChange={onChange} maxLength={100} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Check type="checkbox" label="Aktif mi?" name="IsActive" checked={form.IsActive} onChange={onChange} id="isActiveCheck" />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" type="button" onClick={onClose} disabled={loading}>Kapat</Button>
          <Button variant="primary" type="submit" disabled={loading}>{loading ? 'Kaydediliyor...' : 'Kaydet'}</Button>
        </Modal.Footer>
      </Form>
    </Modal>
  );
};

export default MaterialCardModal; 