import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Swal from 'sweetalert2';
import TableDataLayer from '../TableDataLayer';
import StockEntryModal from './StockEntryModal';
import { useNavigate } from 'react-router-dom';
import { Icon } from '@iconify/react';
import { useAuth } from '../../contexts/AuthContext';

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

const COLUMN_OPTIONS = [
  { key: 'entryNumber', label: 'Giriş No' },
  { key: 'entryDate', label: 'Tarih' },
  { key: 'warehouseName', label: 'Depo' },
  { key: 'locationName', label: 'Lokasyon' },
  { key: 'materialName', label: 'Malzeme' },
  { key: 'entryType', label: 'Tip' },
  { key: 'quantity', label: 'Miktar' },
  { key: 'unitPrice', label: 'Birim Fiyat' },
  { key: 'totalValue', label: 'Toplam' },
  { key: 'isActive', label: 'Aktif mi?' }
];

const TABLE_KEY = 'StockEntryTable';

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
  const [columnSelectorOpen, setColumnSelectorOpen] = useState(false);
  const [selectedColumns, setSelectedColumns] = useState(COLUMN_OPTIONS.map(col => col.key));
  const [searchCol, setSearchCol] = useState('');
  const { user } = useAuth();
  const userId = user?.id || user?.userId;

  // Sütun tercihlerini yükle
  useEffect(() => {
    if (!userId) return;
    axios.get(`${BASE_URL}/api/user-table-column-preferences`, {
      params: { userId, tableKey: TABLE_KEY }
    })
      .then(res => {
        if (res.data && res.data.columnsJson) {
          const config = JSON.parse(res.data.columnsJson);
          if (Array.isArray(config.columns)) {
            setSelectedColumns(config.columns.filter(c => c.visible).map(c => c.key));
          }
        }
      })
      .catch(() => {
        setSelectedColumns(COLUMN_OPTIONS.map(col => col.key));
      });
  }, [userId]);

  // Sütun seçimi değişince kaydet
  useEffect(() => {
    if (!userId) return;
    const columnsConfig = { columns: COLUMN_OPTIONS.map(col => ({ key: col.key, visible: selectedColumns.includes(col.key) })) };
    axios.post(`${BASE_URL}/api/user-table-column-preferences`, {
      tableKey: TABLE_KEY,
      columnsJson: JSON.stringify(columnsConfig)
    }).catch(() => {});
  }, [selectedColumns, userId]);

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

  // Sütun seçici modalı
  const handleColumnToggle = (key) => {
    setSelectedColumns(cols =>
      cols.includes(key) ? cols.filter(c => c !== key) : [...cols, key]
    );
  };
  const handleSelectAllColumns = () => {
    if (selectedColumns.length === COLUMN_OPTIONS.length) {
      setSelectedColumns([]);
    } else {
      setSelectedColumns(COLUMN_OPTIONS.map(col => col.key));
    }
  };
  const filteredColumnOptions = COLUMN_OPTIONS.filter(col =>
    col.label.toLowerCase().includes(searchCol.toLowerCase())
  );

  // Tablo kolon başlıkları (seçime göre)
  const columns = [
    ...COLUMN_OPTIONS.filter(col => selectedColumns.includes(col.key)).map(col => ({
      header: col.label,
      accessor: col.key
    }))
  ];

  const dataWithRowNumber = data.map((item, idx) => ({ ...item, rowNumber: idx + 1 }));

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Stok Girişleri</h5>
          <div className="d-flex gap-2 align-items-center">
            {/* Sütun Seç Butonu */}
            <button
              className="btn btn-outline-secondary"
              onClick={() => setColumnSelectorOpen(true)}
              title="Sütunları Seç"
            >
              <Icon icon="mdi:table-column" className="me-1" /> Sütun Seç
            </button>
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => { setShowModal(true); setEditMode(false); setForm(initialForm); }}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => handleShowDeleted()}
            >
              <i className="ri-delete-bin-6-line" style={{ marginRight: 6 }} />
              Silinenleri Göster
            </button>
          </div>
        </div>
        {/* Sütun Seçici Modal */}
        {columnSelectorOpen && (
          <div className="modal fade show" style={{ display: 'block', background: 'rgba(0,0,0,0.2)' }}>
            <div className="modal-dialog" style={{ maxWidth: 340 }}>
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title">Sütunları Seç</h5>
                  <button type="button" className="btn-close" onClick={() => setColumnSelectorOpen(false)}></button>
                </div>
                <div className="modal-body">
                  <input
                    type="text"
                    className="form-control mb-2"
                    placeholder="Ara..."
                    value={searchCol}
                    onChange={e => setSearchCol(e.target.value)}
                  />
                  <div className="form-check mb-2">
                    <input
                      type="checkbox"
                      className="form-check-input"
                      id="selectAllCols"
                      checked={selectedColumns.length === COLUMN_OPTIONS.length}
                      onChange={handleSelectAllColumns}
                    />
                    <label className="form-check-label" htmlFor="selectAllCols">Tümünü Seç</label>
                  </div>
                  <div style={{ maxHeight: 200, overflowY: 'auto' }}>
                    {filteredColumnOptions.map(col => (
                      <div className="form-check" key={col.key}>
                        <input
                          type="checkbox"
                          className="form-check-input"
                          id={col.key}
                          checked={selectedColumns.includes(col.key)}
                          onChange={() => handleColumnToggle(col.key)}
                        />
                        <label className="form-check-label" htmlFor={col.key}>{col.label}</label>
                      </div>
                    ))}
                  </div>
                </div>
                <div className="modal-footer">
                  <button className="btn btn-primary" onClick={() => setColumnSelectorOpen(false)}>Tamam</button>
                  <button className="btn btn-secondary" onClick={() => setColumnSelectorOpen(false)}>İptal</button>
                </div>
              </div>
            </div>
          </div>
        )}
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