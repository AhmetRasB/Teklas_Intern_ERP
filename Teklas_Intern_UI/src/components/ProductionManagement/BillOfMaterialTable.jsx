import React, { useEffect, useState } from 'react';
import axios from 'axios';
import BillOfMaterialModal from './BillOfMaterialModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';
// import useModulePermissions from '../../hooks/useModulePermissions'; // Uncomment if you have permissions logic

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  version: '',
  notes: '',
  parentMaterialCardId: '',
  validFrom: '',
  validTo: '',
  standardCost: 0,
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

const BillOfMaterialTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedBOM, setSelectedBOM] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [showDeleted, setShowDeleted] = useState(false);
  const [deletedData, setDeletedData] = useState([]);
  const [restoreLoading, setRestoreLoading] = useState(false);

  const navigate = useNavigate();
  // const { canWrite, isAdmin } = useModulePermissions(); // Uncomment if you have permissions logic

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    setLoading(true);
    axios.get(`${BASE_URL}/api/production/bom`)
      .then(res => {
        console.log('BOM Data received:', res.data); // Debug log
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
      const res = await axios.get(`${BASE_URL}/api/production/bom/deleted`);
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
      await axios.put(`${BASE_URL}/api/production/bom/${item.bomHeaderId}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({ title: 'Geri Alındı!', text: 'BOM başarıyla geri alındı.', icon: 'success' });
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
          bomHeaderId: parseInt(form.id),
          version: form.version,
          notes: form.notes,
          parentMaterialCardId: parseInt(form.parentMaterialCardId),
          validFrom: form.validFrom,
          validTo: form.validTo || null,
          standardCost: parseFloat(form.standardCost) || 0,
        };
        console.log('Update payload:', updatePayload); // Debug log
        console.log('Form ID being sent:', form.id); // Debug log
        await axios.put(`${BASE_URL}/api/production/bom`, updatePayload);
      } else {
        const createPayload = {
          version: form.version,
          notes: form.notes,
          parentMaterialCardId: parseInt(form.parentMaterialCardId),
          validFrom: form.validFrom,
          validTo: form.validTo || null,
          standardCost: parseFloat(form.standardCost) || 0,
        };
        await axios.post(`${BASE_URL}/api/production/bom`, createPayload);
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
    console.log('Item bomHeaderId:', item.bomHeaderId); // Debug log
    
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
      text: 'Bu BOM kaydını silmek istediğinize emin misiniz?',
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
        await axios.delete(`${BASE_URL}/api/production/bom/${item.bomHeaderId}`);
        fetchData();
        MySwal.fire({
          title: 'Silindi!',
          text: 'BOM başarıyla silindi.',
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

  const handleEdit = (row) => {
    console.log('Edit row data:', row); // Debug log
    const formData = {
      ...row,
      version: row.version || '',
      notes: row.notes || '',
      parentMaterialCardId: row.parentMaterialCardId || '',
      validFrom: row.validFrom ? new Date(row.validFrom).toISOString().slice(0, 16) : '',
      validTo: row.validTo ? new Date(row.validTo).toISOString().slice(0, 16) : '',
      standardCost: row.standardCost || 0,
      id: row.bomHeaderId || row.BOMHeaderId || row.id || '',
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
    const d = new Date(val);
    if (isNaN(d)) return val;
    return d.toLocaleDateString('tr-TR') + ' ' + d.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
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
    setSelectedBOM(row);
    setShowDetail(true);
    setTimeout(() => setDetailFade(true), 10);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  const columns = [
    { header: '#', accessor: 'rowNumber' },
    { header: 'BOM Kodu', accessor: 'version' },
    { header: 'Açıklama', accessor: 'notes' },
    { header: 'Ürün', accessor: 'parentMaterialCardName' },
    { header: 'Durum', accessor: 'status' },
    { header: 'Aktif mi?', accessor: 'isActive' },
  ];
  const dataWithRowNumber = data.map((item, idx) => ({
    ...item,
    rowNumber: idx + 1,
    isActive: item.isDeleted === false || item.isDeleted === 0 ? 'Aktif' : 'Pasif',
    parentMaterialCardName: item.parentMaterialCardName || `Ürün ID: ${item.parentMaterialCardId}`,
    status: getStatusDisplay(item.status),
  }));

  if (loading) return <div>Yükleniyor...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Ürün Ağaçları (BOM)</h5>
          <div className="d-flex gap-2 align-items-center">
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => setShowModal(true)}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/bill-of-material-trash')}
            >
              <i className="ri-delete-bin-6-line" style={{ marginRight: 6 }} />
              Silinenleri Göster
            </button>
          </div>
        </div>
        <div className="card-body">
          <div className="table-responsive">
            <TableDataLayer
              data={showDeleted ? deletedData : dataWithRowNumber}
              columns={columns}
              onView={handleView}
              onEdit={handleEdit}
              onDelete={handleDeleteRow}
              actions={['view', 'edit', 'delete']}
              pageSize={pageSize}
              onPageSizeChange={val => { setPageSize(val); setPage(1); }}
              page={page}
              totalCount={totalCount}
              onPageChange={setPage}
            />
          </div>
        </div>
        <BillOfMaterialModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'BOM Düzenle' : 'BOM Ekle'}
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
            onClick={() => { setShowDetail(false); setSelectedBOM(null); }}
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
                  <h5 className="modal-title">BOM Detay</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedBOM(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedBOM && (
                      <div>
                        {[
                          ['BOM Kodu', selectedBOM.version],
                          ['Açıklama', selectedBOM.notes],
                          ['Ürün', selectedBOM.parentMaterialCardName || `ID: ${selectedBOM.parentMaterialCardId}`],
                          ['Durum', getStatusDisplay(selectedBOM.status)],
                          ['Aktif mi?', selectedBOM.isDeleted === false || selectedBOM.isDeleted === 0 ? 'Aktif' : 'Pasif'],
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
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedBOM)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedBOM)}>Sil</button>
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedBOM(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default BillOfMaterialTable; 