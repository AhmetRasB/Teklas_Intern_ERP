import React, { useEffect, useState } from 'react';
import axios from 'axios';
import WorkOrderModal from './WorkOrderModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
// import useModulePermissions from '../../hooks/useModulePermissions'; // Uncomment if you have permissions logic

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  bomHeaderId: '',
  materialCardId: '',
  plannedQuantity: 0,
  plannedStartDate: '',
  plannedEndDate: '',
};

const MySwal = withReactContent(Swal);

const swalDarkStyles = `
  .swal2-popup.swal2-dark {
    background: #23272f !important;
    color: #f1f1f1 !important;
  }
  .swal2-popup.swal2-dark .swal2-title,
  .swal2-popup.swal2-dark .swal2-html-container {
    color: #f1f1f1 !important;
  }
  .swal2-popup.swal2-dark .swal2-actions .btn {
    margin: 0 8px;
  }
`;

const COLUMN_OPTIONS = [
  { key: 'bomName', label: 'BOM Adı' },
  { key: 'materialCardName', label: 'Ürün' },
  { key: 'plannedQuantity', label: 'Planlanan Miktar' },
  { key: 'plannedStartDate', label: 'Başlangıç Tarihi' },
  { key: 'status', label: 'Durum' },
  { key: 'isActive', label: 'Aktif mi?' }
];

const TABLE_KEY = 'WorkOrderTable';

const WorkOrderTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedWorkOrder, setSelectedWorkOrder] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [showDeleted, setShowDeleted] = useState(false);
  const [deletedData, setDeletedData] = useState([]);
  const [restoreLoading, setRestoreLoading] = useState(false);
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
      userId,
      tableKey: TABLE_KEY,
      columnsJson: JSON.stringify(columnsConfig)
    }).catch(() => {});
  }, [selectedColumns, userId]);

  const navigate = useNavigate();
  // const { canWrite, isAdmin } = useModulePermissions(); // Uncomment if you have permissions logic

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    setLoading(true);
    axios.get(`${BASE_URL}/api/production/workorder`)
      .then(res => {
        console.log('Work Order Data received:', res.data); // Debug log
        setData(res.data);
        setTotalCount(res.data.length);
        setLoading(false);
      })
      .catch(err => {
        setError('Veriler alınamadı!');
        setLoading(false);
      });
  };

  const fetchDeletedData = async () => {
    setRestoreLoading(true);
    try {
      const res = await axios.get(`${BASE_URL}/api/production/workorder/deleted`);
      setDeletedData(res.data);
    } catch {
      setDeletedData([]);
    } finally {
      setRestoreLoading(false);
    }
  };

  const handleShowDeleted = () => {
    setShowDeleted(!showDeleted);
    if (!showDeleted) fetchDeletedData();
  };

  const handleRestore = async (item) => {
    setRestoreLoading(true);
    try {
      await axios.put(`${BASE_URL}/api/production/workorder/${item.workOrderId}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({ title: 'Geri Alındı!', text: 'İş emri başarıyla geri alındı.', icon: 'success' });
    } catch {
      MySwal.fire({ title: 'Hata', text: 'Geri alma başarısız!', icon: 'error' });
    } finally {
      setRestoreLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({
      ...form,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError(null);
    try {
      if (editMode && form.id) {
        const updatePayload = {
          workOrderId: parseInt(form.id),
          plannedQuantity: parseFloat(form.plannedQuantity) || 0,
          plannedStartDate: form.plannedStartDate,
          plannedEndDate: form.plannedEndDate || null,
        };
        console.log('Update payload:', updatePayload); // Debug log
        console.log('Form ID being sent:', form.id); // Debug log
        await axios.put(`${BASE_URL}/api/production/workorder`, updatePayload);
      } else {
        const createPayload = {
          bomHeaderId: parseInt(form.bomHeaderId),
          materialCardId: parseInt(form.materialCardId),
          plannedQuantity: parseFloat(form.plannedQuantity) || 0,
          plannedStartDate: form.plannedStartDate,
          plannedEndDate: form.plannedEndDate || null,
        };
        await axios.post(`${BASE_URL}/api/production/workorder`, createPayload);
      }
      setShowModal(false);
      setEditMode(false);
      setForm(initialForm);
      fetchData();
    } catch (err) {
      setFormError(editMode ? 'Güncelleme başarısız!' : 'Ekleme başarısız!');
    } finally {
      setFormLoading(false);
    }
  };

  const handleDelete = async (item) => {
    console.log('Delete item:', item); // Debug log
    console.log('Item workOrderId:', item.workOrderId); // Debug log
    
    // Dark mode algılama
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    const result = await MySwal.fire({
      title: 'Silme Onayı',
      text: 'Bu iş emri kaydını silmek istediğinize emin misiniz?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'Vazgeç',
      reverseButtons: true,
      customClass: {
        popup: isDark ? 'swal2-dark' : '',
        confirmButton: 'btn btn-danger mx-2',
        cancelButton: 'btn btn-secondary mx-2',
      },
      buttonsStyling: false,
    });
    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/production/workorder/${item.workOrderId}`);
        fetchData();
        MySwal.fire({
          title: 'Silindi!',
          text: 'İş emri başarıyla silindi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (err) {
        MySwal.fire({
          title: 'Hata',
          text: 'Silme işlemi başarısız!',
          icon: 'error',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      }
    }
  };

  const handlePermanentDelete = async (item) => {
    console.log('Permanent delete item:', item); // Debug log
    
    // Dark mode algılama
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    const result = await MySwal.fire({
      title: 'Kalıcı Silme Onayı',
      text: 'Bu iş emri kaydını kalıcı olarak silmek istediğinize emin misiniz? Bu işlem geri alınamaz!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Kalıcı Sil',
      cancelButtonText: 'Vazgeç',
      reverseButtons: true,
      customClass: {
        popup: isDark ? 'swal2-dark' : '',
        confirmButton: 'btn btn-danger mx-2',
        cancelButton: 'btn btn-secondary mx-2',
      },
      buttonsStyling: false,
    });
    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/production/workorder/${item.workOrderId}/permanent`);
        fetchData();
        MySwal.fire({
          title: 'Kalıcı Olarak Silindi!',
          text: 'İş emri kalıcı olarak silindi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (err) {
        MySwal.fire({
          title: 'Hata',
          text: 'Kalıcı silme işlemi başarısız!',
          icon: 'error',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      }
    }
  };

  const formatDateForInput = (dateValue) => {
    if (!dateValue) return '';
    try {
      const date = new Date(dateValue);
      if (isNaN(date.getTime())) return '';
      return date.toISOString().slice(0, 16);
    } catch (error) {
      console.warn('Invalid date value:', dateValue, error);
      return '';
    }
  };

  const handleEdit = (row) => {
    console.log('Edit row data:', row); // Debug log
    const formData = {
      ...row,
      bomHeaderId: row.bomHeaderId || '',
      materialCardId: row.materialCardId || '',
      plannedQuantity: row.plannedQuantity || 0,
      plannedStartDate: formatDateForInput(row.plannedStartDate),
      plannedEndDate: formatDateForInput(row.plannedEndDate),
      id: row.workOrderId || row.WorkOrderId || row.id || '',
    };
    console.log('Form data:', formData); // Debug log
    setForm(formData);
    setEditMode(true);
    setShowModal(true);
  };

  const truncate = (str, n = 10) => {
    if (!str) return '';
    return str.length > n ? str.slice(0, 7) + '...' : str;
  };

  const formatDateTime = (val) => {
    if (!val) return '';
    try {
      const d = new Date(val);
      if (isNaN(d.getTime())) return 'Geçersiz tarih';
      return d.toLocaleDateString('tr-TR') + ' ' + d.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
    } catch (error) {
      console.warn('Error formatting date:', val, error);
      return 'Geçersiz tarih';
    }
  };

  const getStatusDisplay = (status) => {
    switch (status) {
      case 1: return 'Aktif';
      case 2: return 'Pasif';
      case 3: return 'Bloklu';
      case 4: return 'Dondurulmuş';
      default: return 'Bilinmiyor';
    }
  };

  const handleView = (row) => {
    setSelectedWorkOrder(row);
    setShowDetail(true);
    setTimeout(() => setDetailFade(true), 10);
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
  const dataWithRowNumber = data.map((item, idx) => ({
    ...item,
    rowNumber: idx + 1,
    isActive: item.isDeleted === false || item.isDeleted === 0 ? 'Aktif' : 'Pasif',
    materialCardName: item.materialCardName || `Ürün ID: ${item.materialCardId}`,
    bomName: item.bomName || `BOM ID: ${item.bomHeaderId}`,
    status: getStatusDisplay(item.status),
    plannedStartDate: formatDateTime(item.plannedStartDate),
  }));

  if (loading) return <div>Yükleniyor...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">İş Emirleri</h5>
          <div className="d-flex gap-2 align-items-center">
            {/* Sütun Seç Butonu */}
            <button
              className="btn btn-outline-secondary"
              onClick={() => setColumnSelectorOpen(true)}
              title="Sütunları Seç"
            >
              <Icon icon="mdi:table-column" className="me-1" /> Sütun Seç
            </button>
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => setShowModal(true)}>
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
              onPermanentDelete={handlePermanentDelete}
              actions={showDeleted ? ['restore', 'permanentDelete'] : ['view', 'edit', 'delete']}
              pageSize={pageSize}
              onPageSizeChange={val => { setPageSize(val); setPage(1); }}
              page={page}
              totalCount={totalCount}
              onPageChange={setPage}
            />
          </div>
        </div>
        <WorkOrderModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'İş Emri Düzenle' : 'İş Emri Ekle'}
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
            onClick={() => { setShowDetail(false); setSelectedWorkOrder(null); }}
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
                  <h5 className="modal-title">İş Emri Detay</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedWorkOrder(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedWorkOrder && (
                      <div>
                        {[
                          ['BOM Adı', selectedWorkOrder.bomName || `BOM ID: ${selectedWorkOrder.bomHeaderId}`],
                          ['Ürün', selectedWorkOrder.materialCardName || `Ürün ID: ${selectedWorkOrder.materialCardId}`],
                          ['Planlanan Miktar', selectedWorkOrder.plannedQuantity],
                          ['Başlangıç Tarihi', formatDateTime(selectedWorkOrder.plannedStartDate)],
                          ['Bitiş Tarihi', selectedWorkOrder.plannedEndDate ? formatDateTime(selectedWorkOrder.plannedEndDate) : 'Belirtilmemiş'],
                          ['Durum', getStatusDisplay(selectedWorkOrder.status)],
                          ['Aktif mi?', selectedWorkOrder.isDeleted === false || selectedWorkOrder.isDeleted === 0 ? 'Aktif' : 'Pasif'],
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
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedWorkOrder)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedWorkOrder)}>Sil</button>
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedWorkOrder(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default WorkOrderTable; 