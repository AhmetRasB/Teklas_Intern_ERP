import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import TableDataLayer from '../TableDataLayer';
import WarehouseModal from './WarehouseModal';
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

const COLUMN_OPTIONS = [
  { key: 'code', label: 'Kod' },
  { key: 'name', label: 'Ad' },
  { key: 'description', label: 'Açıklama' },
  { key: 'isActive', label: 'Aktif mi?' }
];

const TABLE_KEY = 'WarehouseTable';

const WarehouseTable = () => {
  const [data, setData] = useState([]);
  const [deletedData, setDeletedData] = useState([]);
  const [showDeleted, setShowDeleted] = useState(false);
  const [loading, setLoading] = useState(true);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedWarehouse, setSelectedWarehouse] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [categories, setCategories] = useState([]);
  const [columnSelectorOpen, setColumnSelectorOpen] = useState(false);
  const [selectedColumns, setSelectedColumns] = useState(COLUMN_OPTIONS.map(col => col.key));
  const [searchCol, setSearchCol] = useState('');
  const { user } = useAuth();
  const userId = user?.id || user?.userId;

  const navigate = useNavigate();

  const initialForm = {
    Code: '',
    Name: '',
    WarehouseType: '',
    Description: '',
    IsActive: true
  };

  const [form, setForm] = useState(initialForm);

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
      userId,
      tableKey: TABLE_KEY,
      columnsJson: JSON.stringify(columnsConfig)
    }).catch(() => {});
  }, [selectedColumns, userId]);

  useEffect(() => {
    fetchData();
  }, [page, pageSize, searchTerm, selectedCategory]);

  const fetchData = () => {
    setLoading(true);
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
      search: searchTerm
    });

    axios.get(`${BASE_URL}/api/warehouses?${params}`)
      .then(res => {
        setData(res.data.items || res.data);
        setTotalCount(res.data.totalCount || res.data.length);
      })
      .catch(err => {
        console.error('Error fetching warehouses:', err);
        setData([]);
      })
      .finally(() => setLoading(false));
  };

  const fetchDeletedData = async () => {
    try {
      const res = await axios.get(`${BASE_URL}/api/warehouses/deleted`);
      setDeletedData(res.data);
    } catch (err) {
      setDeletedData([]);
    }
  };

  const handleShowDeleted = () => {
    setShowDeleted(!showDeleted);
    if (!showDeleted) {
      fetchDeletedData();
    }
  };

  const handleRestore = async (item) => {
    try {
      await axios.put(`${BASE_URL}/api/warehouses/${item.id || item.Id}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({
        title: 'Başarılı!',
        text: 'Depo başarıyla geri alındı.',
        icon: 'success',
        confirmButtonText: 'Tamam'
      });
    } catch (err) {
      MySwal.fire({
        title: 'Hata!',
        text: 'Geri alma işlemi başarısız.',
        icon: 'error',
        confirmButtonText: 'Tamam'
      });
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type } = e.target;
    setForm(prev => ({ 
      ...prev, 
      [name]: type === 'checkbox' ? e.target.checked : value 
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError('');

    try {
      if (editMode) {
        await axios.put(`${BASE_URL}/api/warehouses/${form.id}`, form);
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Depo başarıyla güncellendi.',
          icon: 'success',
          confirmButtonText: 'Tamam'
        });
      } else {
        await axios.post(`${BASE_URL}/api/warehouses`, form);
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Depo başarıyla eklendi.',
          icon: 'success',
          confirmButtonText: 'Tamam'
        });
      }
      
      setShowModal(false);
      setEditMode(false);
      setForm(initialForm);
      fetchData();
    } catch (err) {
      console.error('Error submitting form:', err);
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
      text: `"${item.name || item.Name}" adlı depoyu silmek istediğinize emin misiniz?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/warehouses/${item.id || item.Id}`);
        fetchData();
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Depo başarıyla silindi.',
          icon: 'success',
          confirmButtonText: 'Tamam'
        });
      } catch (err) {
        MySwal.fire({
          title: 'Hata!',
          text: 'Silme işlemi başarısız.',
          icon: 'error',
          confirmButtonText: 'Tamam'
        });
      }
    }
  };

  const handleEdit = (row) => {
    setForm({
      id: row.id || row.Id,
      Code: row.code || row.Code || '',
      Name: row.name || row.Name || '',
      WarehouseType: row.warehouseType || row.WarehouseType || '',
      Description: row.description || row.Description || '',
      IsActive: row.isActive !== undefined ? row.isActive : (row.status === 'Active')
    });
    setEditMode(true);
    setShowModal(true);
  };

  const truncate = (str, n = 10) => {
    return str && str.length > n ? str.substring(0, n) + '...' : str;
  };

  const formatDateTime = (val) => {
    if (!val) return '';
    return new Date(val).toLocaleString('tr-TR');
  };

  const handleView = (row) => {
    setSelectedWarehouse(row);
    setShowDetail(true);
    setDetailFade(true);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
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
    { header: "#", accessor: "rowNumber" },
    ...COLUMN_OPTIONS.filter(col => selectedColumns.includes(col.key)).map(col => ({
      header: col.label,
      accessor: col.key
    }))
  ];
  // Sıra numarası ekle
  const dataWithRowNumber = data.map((item, idx) => ({
    ...item,
    rowNumber: idx + 1
  }));

  const modalStyle = {
    width: 'min(96vw, 1100px)',
    maxHeight: '96vh',
    overflowY: 'auto',
    background: '#fff',
    borderRadius: 16,
    boxShadow: '0 8px 32px rgba(0,0,0,0.15)',
    padding: 32,
    margin: '0 auto',
    display: 'flex',
    flexDirection: 'column',
    position: 'relative',
  };

  const gridStyle = {
    display: 'grid',
    gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
    gap: 24,
    width: '100%',
  };

  if (loading) return <div>Yükleniyor...</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Depolar</h5>
          <div className="d-flex gap-2 align-items-center">
            {/* Sütun Seç Butonu */}
            <button
              className="btn btn-outline-secondary"
              onClick={() => setColumnSelectorOpen(true)}
              title="Sütunları Seç"
            >
              <Icon icon="mdi:table-column" className="me-1" /> Sütun Seç
            </button>
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => {
              setShowModal(true);
            }}>
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
            <TableDataLayer
              data={showDeleted ? deletedData : dataWithRowNumber}
              columns={columns}
              onView={handleView}
              onEdit={handleEdit}
              onDelete={handleDeleteRow}
              actions={['view', 'edit', 'delete']}
              categories={categories}
              selectedCategory={selectedCategory}
              onCategoryChange={val => { setSelectedCategory(val); setPage(1); }}
              pageSize={pageSize}
              onPageSizeChange={val => { setPageSize(val); setPage(1); }}
              page={page}
              totalCount={totalCount}
              onPageChange={setPage}
            />
          </div>
        </div>
        <WarehouseModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Depo Düzenle' : 'Depo Ekle'}
          categories={categories}
        />
        {showDetail && (
          <div
            className={`modal fade ${detailFade ? 'show' : ''}`}
            style={{
              position: 'fixed',
              top: 0, left: 0, right: 0, bottom: 0,
              background: 'rgba(0,0,0,0.3)',
              zIndex: 1050,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center'
            }}
            onClick={() => { setShowDetail(false); setSelectedWarehouse(null); }}
          >
            <style>{`
              .modal.fade .modal-dialog { opacity: 0; transform: scale(0.96); transition: all 0.25s cubic-bezier(.4,0,.2,1); }
              .modal.fade.show .modal-dialog { opacity: 1; transform: scale(1); }
              .dark-bg {
                background: #23272f !important;
              }
            `}</style>
            <div
              className="modal-dialog"
              style={{
                maxWidth: 800,
                width: '90vw',
                margin: 0,
                borderRadius: 16,
                boxShadow: '0 8px 32px rgba(0,0,0,0.15)'
              }}
              onClick={e => e.stopPropagation()}
            >
              <div className="modal-content" style={{ borderRadius: 16, padding: '24px' }}>
                <div className="modal-header">
                  <h5 className="modal-title">Depo Detay</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedWarehouse(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedWarehouse && (
                      <div>
                        {[
                          ['Kod', selectedWarehouse.code],
                          ['Ad', selectedWarehouse.name],
                          ['Açıklama', selectedWarehouse.description],
                          ['Aktif mi?', selectedWarehouse.isActive ? 'Aktif' : 'Pasif'],
                          ['Oluşturulma', formatDateTime(selectedWarehouse.createdDate)],
                          ['Güncellenme', formatDateTime(selectedWarehouse.updatedDate)],
                          ['Oluşturan', selectedWarehouse.createdBy],
                          ['Güncelleyen', selectedWarehouse.updatedBy],
                        ].map(([label, value], i) => (
                          <div key={label} className="d-flex align-items-center px-4 py-3 mb-2" style={{background: 'transparent', borderRadius: 8}}>
                            <span className="fw-bold text-secondary-light flex-shrink-0" style={{minWidth: 140}}>{label}</span>
                            <span className="flex-grow-1 ms-3 px-3 py-2" style={{background: (typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'))) ? '#1B2431' : '#F5F6FA', borderRadius: 6, color: (typeof document !== 'undefined' && (document.body.classList.contains('dark') || document.documentElement.classList.contains('dark'))) ? '#f1f1f1' : '#23272f', minWidth: 0, wordBreak: 'break-all'}}>{value ?? ''}</span>
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                </div>
                <div className="modal-footer d-flex justify-content-end gap-2" style={{background: 'transparent', borderTop: 'none', boxShadow: 'none', padding: 0}}>
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedWarehouse)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedWarehouse)}>Sil</button>
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedWarehouse(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default WarehouseTable; 