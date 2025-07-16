import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ProductionConfirmationModal from './ProductionConfirmationModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  workOrderId: '',
  confirmationDate: '',
  quantityProduced: '',
  quantityScrapped: '',
  laborHoursUsed: '',
  performedBy: '',
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

const ProductionConfirmationTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedConfirmation, setSelectedConfirmation] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    setLoading(true);
    axios.get(`${BASE_URL}/api/production/confirmation`)
      .then(res => {
        console.log('Production Confirmation Data received:', res.data); // Debug log
        setData(res.data);
        setTotalCount(res.data.length);
        setLoading(false);
      })
      .catch(err => {
        setError('Veriler yüklenemedi!');
        setLoading(false);
      });
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
          confirmationId: parseInt(form.id),
          quantityProduced: parseFloat(form.quantityProduced) || 0,
          quantityScrapped: form.quantityScrapped ? parseFloat(form.quantityScrapped) : null,
          laborHoursUsed: form.laborHoursUsed ? parseFloat(form.laborHoursUsed) : null,
          performedBy: form.performedBy,
        };
        console.log('Update payload:', updatePayload); // Debug log
        console.log('Form ID being sent:', form.id); // Debug log
        await axios.put(`${BASE_URL}/api/production/confirmation`, updatePayload);
      } else {
        const createPayload = {
          workOrderId: parseInt(form.workOrderId),
          confirmationDate: form.confirmationDate,
          quantityProduced: parseFloat(form.quantityProduced) || 0,
          quantityScrapped: form.quantityScrapped ? parseFloat(form.quantityScrapped) : null,
          laborHoursUsed: form.laborHoursUsed ? parseFloat(form.laborHoursUsed) : null,
          performedBy: form.performedBy,
        };
        await axios.post(`${BASE_URL}/api/production/confirmation`, createPayload);
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
    console.log('Item confirmationId:', item.confirmationId); // Debug log
    
    // Dark mode detection
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    const result = await MySwal.fire({
      title: 'Silme Onayı',
      text: 'Bu üretim teyidini silmek istediğinizden emin misiniz?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal',
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
        await axios.delete(`${BASE_URL}/api/production/confirmation/${item.confirmationId}`);
        fetchData();
        MySwal.fire({
          title: 'Silindi!',
          text: 'Üretim teyidi başarıyla silindi.',
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
      workOrderId: row.workOrderId || '',
      confirmationDate: row.confirmationDate ? new Date(row.confirmationDate).toISOString().slice(0, 16) : '',
      quantityProduced: row.quantityProduced || '',
      quantityScrapped: row.quantityScrapped || '',
      laborHoursUsed: row.laborHoursUsed || '',
      performedBy: row.performedBy || '',
      id: row.confirmationId || row.ConfirmationId || row.id || '',
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
    setSelectedConfirmation(row);
    setShowDetail(true);
    setTimeout(() => setDetailFade(true), 10);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  const columns = [
    { header: '#', accessor: 'rowNumber' },
    { header: 'Teyit ID', accessor: 'confirmationId' },
    { header: 'İş Emri ID', accessor: 'workOrderId' },
    { header: 'Teyit Tarihi', accessor: 'confirmationDate', render: val => formatDateTime(val) },
    { header: 'Üretilen Miktar', accessor: 'quantityProduced' },
    { header: 'Yapan Kişi', accessor: 'performedBy' },
  ];
  
  const dataWithRowNumber = data.map((item, idx) => ({
    ...item,
    rowNumber: idx + 1,
    confirmationId: item.confirmationId || 'N/A',
    workOrderId: item.workOrderId || 'N/A',
    confirmationDate: item.confirmationDate || 'Tarih mevcut değil',
    quantityProduced: item.quantityProduced || 'N/A',
    performedBy: item.performedBy || 'Belirtilmemiş',
  }));

  if (loading) return <div>Yükleniyor...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Üretim Teyitleri</h5>
          <div className="d-flex gap-2 align-items-center">
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => setShowModal(true)}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/production-confirmation-trash')}
            >
              <i className="ri-delete-bin-6-line" style={{ marginRight: 6 }} />
              Silinenleri Göster
            </button>
          </div>
        </div>
        <div className="card-body">
          <div className="table-responsive">
            <TableDataLayer
              data={dataWithRowNumber}
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
        <ProductionConfirmationModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Üretim Teyidini Düzenle' : 'Yeni Üretim Teyidi Ekle'}
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
            onClick={() => { setShowDetail(false); setSelectedConfirmation(null); }}
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
                  <h5 className="modal-title">Üretim Teyidi Detayları</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedConfirmation(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedConfirmation && (
                      <div>
                        {[
                          ['Teyit ID', selectedConfirmation.confirmationId],
                          ['İş Emri ID', selectedConfirmation.workOrderId],
                          ['Teyit Tarihi', formatDateTime(selectedConfirmation.confirmationDate)],
                          ['Üretilen Miktar', selectedConfirmation.quantityProduced],
                          ['Fire Miktarı', selectedConfirmation.quantityScrapped || 'N/A'],
                          ['Kullanılan İş Saati', selectedConfirmation.laborHoursUsed || 'N/A'],
                          ['Yapan Kişi', selectedConfirmation.performedBy || 'Belirtilmemiş'],
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
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedConfirmation)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedConfirmation)}>Sil</button>
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedConfirmation(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ProductionConfirmationTable; 