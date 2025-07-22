import React, { useEffect, useState } from 'react';
import axios from 'axios';
import PurchaseOrderModal from './PurchaseOrderModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { useNavigate } from 'react-router-dom';
import useModulePermissions from '../../hooks/useModulePermissions';

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  orderNumber: '',
  supplierId: '',
  orderDate: '',
  status: '',
  description: '',
  totalAmount: '',
  isActive: true,
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

const PurchaseOrderTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const [deleteError, setDeleteError] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [page, setPage] = useState(1);
  const [suppliers, setSuppliers] = useState([]);
  const [selectedSupplier, setSelectedSupplier] = useState('');
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [showDeleted, setShowDeleted] = useState(false);
  const [deletedData, setDeletedData] = useState([]);
  const [restoreLoading, setRestoreLoading] = useState(false);

  const navigate = useNavigate();
  const { canWrite, isAdmin } = useModulePermissions();

  useEffect(() => {
    axios.get(`${BASE_URL}/api/supplier`)
      .then(res => setSuppliers(res.data))
      .catch(() => setSuppliers([]));
  }, []);

  useEffect(() => {
    fetchData();
  }, [selectedSupplier]);

  const fetchData = () => {
    setLoading(true);
    let url = `${BASE_URL}/api/purchaseorder`;
    if (selectedSupplier) url += `?supplierId=${selectedSupplier}`;
    axios.get(url)
      .then(res => {
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
      const res = await axios.get(`${BASE_URL}/api/purchaseorder/deleted`);
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
      await axios.post(`${BASE_URL}/api/purchaseorder/${item.id || item.Id}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({ title: 'Geri Alındı!', text: 'Sipariş başarıyla geri alındı.', icon: 'success' });
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
    if (!canWrite('purchasingManagement')) {
      MySwal.fire({
        title: 'Erişim Engellendi',
        text: 'Bu modül için yalnızca görüntüleme izniniz var. Düzenleme yapamaz veya yeni kayıt ekleyemezsiniz.',
        icon: 'warning',
        confirmButtonText: 'Tamam'
      });
      return;
    }
    setFormLoading(true);
    setFormError(null);
    const now = new Date().toISOString();
    const payload = {
      orderNumber: form.orderNumber,
      supplierId: Number(form.supplierId) || 1,
      orderDate: form.orderDate,
      status: form.status,
      description: form.description,
      totalAmount: Number(form.totalAmount) || 0,
      isActive: form.isActive,
      createdDate: form.createdDate || now,
      updatedDate: now,
      createdBy: form.createdBy || 'admin',
      updatedBy: 'admin',
    };
    try {
      if (editMode && form.id) {
        await axios.put(`${BASE_URL}/api/purchaseorder/${form.id}`, payload);
      } else {
        await axios.post(`${BASE_URL}/api/purchaseorder`, payload);
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
    if (!canWrite('purchasingManagement')) {
      MySwal.fire({
        title: 'Erişim Engellendi',
        text: 'Bu modül için yalnızca görüntüleme izniniz var. Silme işlemi yapamazsınız.',
        icon: 'warning',
        confirmButtonText: 'Tamam'
      });
      return;
    }
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    const result = await MySwal.fire({
      title: 'Silme Onayı',
      text: 'Bu satın alma siparişini silmek istediğinize emin misiniz?',
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
        await axios.delete(`${BASE_URL}/api/purchaseorder/${item.id || item.Id}`);
        fetchData();
        MySwal.fire({
          title: 'Silindi!',
          text: 'Satın alma siparişi başarıyla silindi.',
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
    if (!canWrite('purchasingManagement')) {
      MySwal.fire({
        title: 'Erişim Engellendi',
        text: 'Bu modül için yalnızca görüntüleme izniniz var. Düzenleme yapamazsınız.',
        icon: 'warning',
        confirmButtonText: 'Tamam'
      });
      return;
    }
    setForm({
      ...row,
      orderNumber: row.orderNumber || '',
      supplierId: row.supplierId || '',
      orderDate: row.orderDate || '',
      status: row.status || '',
      description: row.description || '',
      totalAmount: row.totalAmount || '',
      isActive: row.isActive ?? true,
      id: row.id || '',
      createdBy: row.createdBy || '',
      createdDate: row.createdDate || '',
      updatedBy: 'admin',
      updatedDate: new Date().toISOString(),
    });
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

  const handleView = (row) => {
    setSelectedOrder(row);
    setShowDetail(true);
    setTimeout(() => setDetailFade(true), 10);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  const columns = [
    { header: '#', accessor: 'rowNumber' },
    { header: 'Sipariş No', accessor: 'orderNumber' },
    { header: 'Tedarikçi', accessor: 'supplierName' },
    { header: 'Tarih', accessor: 'orderDate', render: formatDateTime },
    { header: 'Durum', accessor: 'status' },
    { header: 'Tutar', accessor: 'totalAmount' },
    { header: 'Aktif mi?', accessor: 'isActive' }
  ];
  const dataWithRowNumber = data.map((item, idx) => ({
    ...item,
    rowNumber: idx + 1
  }));

  if (loading) return <div>Yükleniyor...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Satın Alma Siparişleri</h5>
          <div className="d-flex gap-2 align-items-center">
            {canWrite('purchasingManagement') && (
              <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => {
                if (!canWrite('purchasingManagement')) {
                  MySwal.fire({
                    title: 'Erişim Engellendi',
                    text: 'Bu modül için yalnızca görüntüleme izniniz var. Yeni kayıt ekleyemezsiniz.',
                    icon: 'warning',
                    confirmButtonText: 'Tamam'
                  });
                  return;
                }
                setShowModal(true);
              }}>
                <i className="ri-add-line"></i> Yeni Ekle
              </button>
            )}
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/purchase-order-trash')}
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
              actions={canWrite('purchasingManagement') ? ['view', 'edit', 'delete'] : ['view']}
              categories={suppliers}
              selectedCategory={selectedSupplier}
              onCategoryChange={val => { setSelectedSupplier(val); setPage(1); }}
              pageSize={pageSize}
              onPageSizeChange={val => { setPageSize(val); setPage(1); }}
              page={page}
              totalCount={totalCount}
              onPageChange={setPage}
            />
          </div>
        </div>
        <PurchaseOrderModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Sipariş Düzenle' : 'Sipariş Ekle'}
          suppliers={suppliers}
        />
      </div>
    </div>
  );
};

export default PurchaseOrderTable; 