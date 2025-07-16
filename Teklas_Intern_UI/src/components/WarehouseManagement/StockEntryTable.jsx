import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Swal from 'sweetalert2';
import TableDataLayer from '../TableDataLayer';
import StockEntryModal from './StockEntryModal';
import { useNavigate } from 'react-router-dom';

const BASE_URL = 'https://localhost:7178';

const MySwal = Swal.mixin({
  customClass: {
    confirmButton: 'btn btn-primary',
    cancelButton: 'btn btn-secondary me-2'
  },
  buttonsStyling: false
});

const initialForm = {
  EntryNumber: '',
  EntryDate: '',
  WarehouseId: '',
  LocationId: '',
  MaterialId: '',
  EntryType: '',
  Quantity: '',
  UnitPrice: '',
  TotalValue: '',
  BatchNumber: '',
  SerialNumber: '',
  ExpiryDate: '',
  ProductionDate: '',
  QualityStatus: '',
  Notes: '',
  EntryReason: '',
  ResponsiblePerson: '',
  IsActive: true
};

const StockEntryTable = () => {
  const navigate = useNavigate();
  const [data, setData] = useState([]);
  const [deletedData, setDeletedData] = useState([]);
  const [showDeleted, setShowDeleted] = useState(false);
  const [loading, setLoading] = useState(true);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [selectedEntry, setSelectedEntry] = useState(null);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    setLoading(true);
    axios.get(`${BASE_URL}/api/stockentries`)
      .then(res => {
        setData(res.data.items || res.data);
      })
      .catch(() => setData([]))
      .finally(() => setLoading(false));
  };

  const fetchDeletedData = async () => {
    try {
      const res = await axios.get(`${BASE_URL}/api/stockentries/deleted`);
      setDeletedData(res.data);
    } catch {
      setDeletedData([]);
    }
  };

  const handleShowDeleted = () => {
    setShowDeleted(!showDeleted);
    if (!showDeleted) fetchDeletedData();
  };

  const handleRestore = async (item) => {
    try {
      await axios.put(`${BASE_URL}/api/stockentries/${item.id}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({ title: 'Başarılı!', text: 'Stok girişi başarıyla geri alındı.', icon: 'success', confirmButtonText: 'Tamam' });
    } catch {
      MySwal.fire({ title: 'Hata!', text: 'Geri alma işlemi başarısız.', icon: 'error', confirmButtonText: 'Tamam' });
    }
  };

  const handlePermanentDelete = async (item) => {
    const result = await MySwal.fire({
      title: 'Emin misiniz?',
      text: `"${item.entryNumber}" numaralı stok girişini kalıcı olarak silmek istediğinize emin misiniz?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });
    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/stockentries/${item.id}/permanent`);
        fetchDeletedData();
        MySwal.fire({ title: 'Başarılı!', text: 'Stok girişi kalıcı olarak silindi.', icon: 'success', confirmButtonText: 'Tamam' });
      } catch {
        MySwal.fire({ title: 'Hata!', text: 'Kalıcı silme işlemi başarısız.', icon: 'error', confirmButtonText: 'Tamam' });
      }
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'IsActive' ? value === 'true' : (type === 'checkbox' ? e.target.checked : value)
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError('');
    try {
      if (editMode) {
        await axios.put(`${BASE_URL}/api/stockentries/${form.id}`, form);
        MySwal.fire({ title: 'Başarılı!', text: 'Stok girişi başarıyla güncellendi.', icon: 'success', confirmButtonText: 'Tamam' });
      } else {
        await axios.post(`${BASE_URL}/api/stockentries`, form);
        MySwal.fire({ title: 'Başarılı!', text: 'Stok girişi başarıyla eklendi.', icon: 'success', confirmButtonText: 'Tamam' });
      }
      setShowModal(false);
      setEditMode(false);
      setForm(initialForm);
      fetchData();
    } catch (err) {
      if (err.response?.data?.errors) {
        const errorMessages = Object.values(err.response.data.errors).flat();
        setFormError(errorMessages.join(', '));
      } else if (err.response?.data?.message) {
        setFormError(err.response.data.message);
      } else {
        setFormError('Bir hata oluştu. Lütfen tekrar deneyin.');
      }
    } finally {
      setFormLoading(false);
    }
  };

  const handleDelete = async (item) => {
    const result = await MySwal.fire({
      title: 'Emin misiniz?',
      text: `"${item.entryNumber}" numaralı stok girişini silmek istediğinize emin misiniz?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });
    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/stockentries/${item.id}`);
        fetchData();
        MySwal.fire({ title: 'Başarılı!', text: 'Stok girişi başarıyla silindi.', icon: 'success', confirmButtonText: 'Tamam' });
      } catch {
        MySwal.fire({ title: 'Hata!', text: 'Silme işlemi başarısız.', icon: 'error', confirmButtonText: 'Tamam' });
      }
    }
  };

  const handleEdit = (row) => {
    setForm({
      id: row.id,
      EntryNumber: row.entryNumber || '',
      EntryDate: row.entryDate ? row.entryDate.substring(0, 10) : '',
      WarehouseId: row.warehouseId || '',
      LocationId: row.locationId || '',
      MaterialId: row.materialId || '',
      EntryType: row.entryType || '',
      Quantity: row.quantity || '',
      UnitPrice: row.unitPrice || '',
      TotalValue: row.totalValue || '',
      BatchNumber: row.batchNumber || '',
      SerialNumber: row.serialNumber || '',
      ExpiryDate: row.expiryDate ? row.expiryDate.substring(0, 10) : '',
      ProductionDate: row.productionDate ? row.productionDate.substring(0, 10) : '',
      QualityStatus: row.qualityStatus || '',
      Notes: row.notes || '',
      EntryReason: row.entryReason || '',
      ResponsiblePerson: row.responsiblePerson || '',
      IsActive: row.isActive !== undefined ? row.isActive : true
    });
    setEditMode(true);
    setShowModal(true);
  };

  const handleView = (row) => {
    // This function is not implemented in the original code,
    // but it's part of the new_code. It will be added later.
    console.log('View clicked for:', row);
    // Example: navigate(`/stock-entry/${row.id}`);
  };

  const columns = [
    { header: 'İşlem', accessor: 'actions' },
    { header: '#', accessor: 'rowNumber' },
    { header: 'Giriş No', accessor: 'entryNumber' },
    { header: 'Tarih', accessor: 'entryDate', render: val => val ? new Date(val).toLocaleDateString('tr-TR') : '' },
    { header: 'Depo', accessor: 'warehouseName' },
    { header: 'Lokasyon', accessor: 'locationName' },
    { header: 'Malzeme', accessor: 'materialName' },
    { header: 'Tip', accessor: 'entryType' },
    { header: 'Miktar', accessor: 'quantity' },
    { header: 'Birim Fiyat', accessor: 'unitPrice' },
    { header: 'Toplam', accessor: 'totalValue' },
    { header: 'Aktif mi?', accessor: 'isActive', render: val => val ? <span className="badge bg-success">Aktif</span> : <span className="badge bg-secondary">Pasif</span> },
  ];

  const dataWithRowNumber = data.map((item, idx) => ({ ...item, rowNumber: idx + 1 }));

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Stok Girişleri</h5>
          <div className="d-flex gap-2 align-items-center">
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => { setShowModal(true); setEditMode(false); setForm(initialForm); }}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/stock-entry-trash')}
            >
              <i className="ri-delete-bin-6-line" style={{ marginRight: 6 }} />
              Silinenleri Göster
            </button>
          </div>
        </div>
        <div className="card-body">
          <div className="table-responsive">
            <TableDataLayer columns={columns} data={dataWithRowNumber} loading={loading} actions={['view', 'edit', 'delete']} onView={handleView} onEdit={handleEdit} onDelete={handleDelete} />
          </div>
        </div>
        <StockEntryModal
          open={showModal}
          onClose={() => setShowModal(false)}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Stok Girişi Düzenle' : 'Stok Girişi Ekle'}
        />
      </div>
    </div>
  );
};

export default StockEntryTable; 