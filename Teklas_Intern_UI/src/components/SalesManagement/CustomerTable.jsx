import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import TableDataLayer from '../TableDataLayer';
import CustomerModal from './CustomerModal';

const BASE_URL = 'https://localhost:7178';

const MySwal = Swal.mixin({
  customClass: {
    confirmButton: 'btn btn-primary',
    cancelButton: 'btn btn-secondary me-2'
  },
  buttonsStyling: false
});

const CustomerTable = () => {
  const [data, setData] = useState([]);
  const [deletedData, setDeletedData] = useState([]);
  const [showDeleted, setShowDeleted] = useState(false);
  const [loading, setLoading] = useState(true);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [categories, setCategories] = useState([]);

  const navigate = useNavigate();

  const initialForm = {
    Name: '',
    Address: '',
    Phone: '',
    Email: '',
    TaxNumber: '',
    ContactPerson: ''
  };

  const [form, setForm] = useState(initialForm);

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

    axios.get(`${BASE_URL}/api/customers?${params}`)
      .then(res => {
        setData(res.data.items || res.data);
        setTotalCount(res.data.totalCount || res.data.length);
      })
      .catch(err => {
        console.error('Error fetching customers:', err);
        setData([]);
      })
      .finally(() => setLoading(false));
  };

  const fetchDeletedData = async () => {
    try {
      const res = await axios.get(`${BASE_URL}/api/customers/deleted`);
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
      await axios.put(`${BASE_URL}/api/customers/${item.id || item.Id}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({
        title: 'Başarılı!',
        text: 'Müşteri başarıyla geri alındı.',
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
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError('');

    try {
      if (editMode) {
        await axios.put(`${BASE_URL}/api/customers/${form.id}`, form);
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Müşteri başarıyla güncellendi.',
          icon: 'success',
          confirmButtonText: 'Tamam'
        });
      } else {
        await axios.post(`${BASE_URL}/api/customers`, form);
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Müşteri başarıyla eklendi.',
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
      text: `"${item.name || item.Name}" adlı müşteriyi silmek istediğinize emin misiniz?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/customers/${item.id || item.Id}`);
        fetchData();
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Müşteri başarıyla silindi.',
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
      Name: row.name || row.Name || '',
      Address: row.address || row.Address || '',
      Phone: row.phone || row.Phone || '',
      Email: row.email || row.Email || '',
      TaxNumber: row.taxNumber || row.TaxNumber || '',
      ContactPerson: row.contactPerson || row.ContactPerson || ''
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
    setSelectedCustomer(row);
    setShowDetail(true);
    setDetailFade(true);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  // Tablo kolon başlıkları
  const columns = [
    { header: "#", accessor: "rowNumber" },
    { header: "Ad", accessor: "name" },
    { header: "Adres", accessor: "address" },
    { header: "Telefon", accessor: "phone" },
    { header: "E-posta", accessor: "email" },
    { header: "Vergi No", accessor: "taxNumber" },
    { header: "İletişim Kişisi", accessor: "contactPerson" },
    { header: "Aktif mi?", accessor: "isActive" }
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
  if (error) return <div>{error}</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Müşteriler</h5>
          <div className="d-flex gap-2 align-items-center">
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => {
              setShowModal(true);
            }}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/customer-trash')}
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
        <CustomerModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Müşteri Düzenle' : 'Müşteri Ekle'}
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
            onClick={() => { setShowDetail(false); setSelectedCustomer(null); }}
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
                  <h5 className="modal-title">Müşteri Detay</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedCustomer(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedCustomer && (
                      <div>
                        {[
                          ['Ad', selectedCustomer.name],
                          ['Adres', selectedCustomer.address],
                          ['Telefon', selectedCustomer.phone],
                          ['E-posta', selectedCustomer.email],
                          ['Vergi Numarası', selectedCustomer.taxNumber],
                          ['İletişim Kişisi', selectedCustomer.contactPerson],
                          ['Aktif mi?', selectedCustomer.isActive ? 'Aktif' : 'Pasif'],
                          ['Oluşturulma', formatDateTime(selectedCustomer.createdDate)],
                          ['Güncellenme', formatDateTime(selectedCustomer.updatedDate)],
                          ['Oluşturan', selectedCustomer.createdBy],
                          ['Güncelleyen', selectedCustomer.updatedBy],
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
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedCustomer)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedCustomer)}>Sil</button>
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedCustomer(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default CustomerTable; 