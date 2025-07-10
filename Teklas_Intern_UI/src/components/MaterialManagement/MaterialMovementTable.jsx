import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MaterialMovementModal from './MaterialMovementModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  MaterialCardId: '',
  MovementType: '',
  Quantity: '',
  MovementDate: new Date().toISOString().slice(0, 16), // Current datetime in local format
  ReferenceNumber: '',
  ReferenceType: '',
  LocationFrom: '',
  LocationTo: '',
  Description: '',
  UnitPrice: '',
  TotalAmount: '',
  ResponsiblePerson: '',
  SupplierCustomer: '',
  BatchNumber: '',
  SerialNumber: '',
  ExpiryDate: '',
  StockBalance: '',
  IsActive: true,
};

const MySwal = withReactContent(Swal);

// SweetAlert2 dark mode custom CSS
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

const MaterialMovementTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedMovement, setSelectedMovement] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [page, setPage] = useState(1);
  const [materialCards, setMaterialCards] = useState([]);
  const [selectedMaterial, setSelectedMaterial] = useState('');
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

  const navigate = useNavigate();

  useEffect(() => {
    // Malzeme kartları listesini çek
    axios.get(`${BASE_URL}/api/materials`)
      .then(res => setMaterialCards(res.data))
      .catch(() => setMaterialCards([]));
  }, []);

  useEffect(() => {
    fetchData();
  }, [selectedMaterial]);

  const fetchData = () => {
    setLoading(true);
    let url = `${BASE_URL}/api/MaterialMovement`;
    if (selectedMaterial) url += `?materialCardId=${selectedMaterial}`;
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
    
    // Validate required fields before creating payload
    if (!form.MaterialCardId) {
      setFormError('Malzeme seçimi zorunludur!');
      setFormLoading(false);
      return;
    }
    
    if (!form.MovementType) {
      setFormError('Hareket tipi zorunludur!');
      setFormLoading(false);
      return;
    }
    
    if (!form.Quantity || Number(form.Quantity) <= 0) {
      setFormError('Geçerli bir miktar giriniz!');
      setFormLoading(false);
      return;
    }
    
    const now = new Date().toISOString();
    const payload = {
      MaterialCardId: Number(form.MaterialCardId),
      MovementType: form.MovementType,
      Quantity: Number(form.Quantity),
      MovementDate: form.MovementDate || now,
      ReferenceNumber: form.ReferenceNumber || null,
      ReferenceType: form.ReferenceType || null,
      LocationFrom: form.LocationFrom || null,
      LocationTo: form.LocationTo || null,
      Description: form.Description || null,
      UnitPrice: form.UnitPrice ? Number(form.UnitPrice) : null,
      TotalAmount: form.TotalAmount ? Number(form.TotalAmount) : null,
      ResponsiblePerson: form.ResponsiblePerson || null,
      SupplierCustomer: form.SupplierCustomer || null,
      BatchNumber: form.BatchNumber || null,
      SerialNumber: form.SerialNumber || null,
      ExpiryDate: form.ExpiryDate || null,
      StockBalance: form.StockBalance ? Number(form.StockBalance) : null,
      Status: form.IsActive ? "PENDING" : "CANCELLED",
    };

    try {
      if (editMode && form.id) {
        // GÜNCELLEME (PUT)
        await axios.put(`${BASE_URL}/api/MaterialMovement/${form.id}`, payload);
      } else {
        // YENİ EKLEME (POST)
        await axios.post(`${BASE_URL}/api/MaterialMovement`, payload);
      }
      setShowModal(false);
      setEditMode(false);
      setForm(initialForm); 
      fetchData();
      MySwal.fire({ title: 'Başarılı!', text: editMode ? 'Hareket güncellendi!' : 'Hareket eklendi!', icon: 'success' });
    } catch (err) {
      const errorMsg = err.response?.data?.details || err.response?.data?.error || (editMode ? 'Güncelleme başarısız!' : 'Ekleme başarısız!');
      setFormError(errorMsg);
      console.error('Submit error:', err.response?.data);
    } finally {
      setFormLoading(false);
    }
  };

  const handleDelete = async (item) => {
    // Dark mode algılama (ör: body'de dark class varsa)
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    // SweetAlert2 popup'ı açmadan önce stil ekle
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    const result = await MySwal.fire({
      title: 'Silme Onayı',
      text: 'Bu malzeme hareketini silmek istediğinize emin misiniz?',
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
        await axios.delete(`${BASE_URL}/api/MaterialMovement/${item.id || item.Id}`);
        fetchData();
        MySwal.fire({
          title: 'Silindi!',
          text: 'Malzeme hareketi başarıyla silindi.',
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
    const movementDate = row.movementDate ? new Date(row.movementDate).toISOString().slice(0, 16) : '';
    const expiryDate = row.expiryDate ? new Date(row.expiryDate).toISOString().slice(0, 10) : '';
    
    setForm({
      id: row.id || row.Id,
      MaterialCardId: row.materialCardId || '',
      MovementType: row.movementType || '',
      Quantity: row.quantity || '',
      MovementDate: movementDate,
      ReferenceNumber: row.referenceNumber || '',
      ReferenceType: row.referenceType || '',
      LocationFrom: row.locationFrom || '',
      LocationTo: row.locationTo || '',
      Description: row.description || '',
      UnitPrice: row.unitPrice || '',
      TotalAmount: row.totalAmount || '',
      ResponsiblePerson: row.responsiblePerson || '',
      SupplierCustomer: row.supplierCustomer || '',
      BatchNumber: row.batchNumber || '',
      SerialNumber: row.serialNumber || '',
      ExpiryDate: expiryDate,
      StockBalance: row.stockBalance || '',
      IsActive: row.isActive !== undefined ? row.isActive : true,
      CreateDate: row.createDate,
      CreateUserId: row.createUserId,
    });
    setEditMode(true);
    setShowModal(true);
  };

  const truncate = (str, n = 10) => {
    if (!str) return '';
    return str.length > n ? str.substr(0, n - 1) + '...' : str;
  };

  const formatDateTime = (val) => {
    if (!val) return '';
    try {
      return new Date(val).toLocaleString('tr-TR');
    } catch { return val; }
  };

  const handleView = (row) => {
    setSelectedMovement(row);
    setShowDetail(true);
    setDetailFade(true);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  const columns = [
    { header: '#', accessor: 'rowNumber' },
    { header: 'Malzeme', accessor: 'materialCardName', render: val => truncate(val, 15) },
    { header: 'Hareket Tipi', accessor: 'movementType' },
    { header: 'Miktar', accessor: 'quantity' },
    { header: 'Tarih', accessor: 'movementDate', render: val => formatDateTime(val) },
    { header: 'Referans No', accessor: 'referenceNumber', render: val => truncate(val, 12) },
    { header: 'Lokasyon (Çıkış)', accessor: 'locationFrom', render: val => truncate(val, 12) },
    { header: 'Lokasyon (Varış)', accessor: 'locationTo', render: val => truncate(val, 12) },
    { header: 'Açıklama', accessor: 'description', render: val => truncate(val, 20) },
  ];

  const dataWithRowNumber = data.map((item, idx) => ({ ...item, rowNumber: idx + 1 }));

  return (
    <div className="col-lg-12">
      <style>{swalDarkStyles}</style>
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Malzeme Hareketleri</h5>
          <div className="d-flex gap-2 align-items-center">
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => setShowModal(true)}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/material-movement-trash')}
            >
              <i className="ri-delete-bin-6-line" style={{ marginRight: 6 }} />
              Silinenleri Göster
            </button>
          </div>
        </div>
        <div className="card-body">
          <TableDataLayer
            data={dataWithRowNumber}
            columns={columns}
            showActions={true}
            onView={handleView}
            onEdit={handleEdit}
            onDelete={handleDeleteRow}
            loading={loading}
          />
        </div>
      </div>

      {/* Material Movement Modal */}
      <MaterialMovementModal
        open={showModal}
        onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
        onSubmit={handleSubmit}
        form={form}
        onChange={handleInputChange}
        loading={formLoading}
        error={formError}
        title={editMode ? 'Malzeme Hareketi Düzenle' : 'Yeni Malzeme Hareketi'}
        materialCards={materialCards}
      />

      {/* Detail Modal */}
      {showDetail && selectedMovement && (
        <MaterialMovementModal
          open={showDetail}
          onClose={() => { setShowDetail(false); setSelectedMovement(null); setDetailFade(false); }}
          form={{
            MaterialCardId: selectedMovement.materialCardId || '',
            MovementType: selectedMovement.movementType || '',
            Quantity: selectedMovement.quantity || '',
            MovementDate: selectedMovement.movementDate ? new Date(selectedMovement.movementDate).toISOString().slice(0, 16) : '',
            ReferenceNumber: selectedMovement.referenceNumber || '',
            ReferenceType: selectedMovement.referenceType || '',
            LocationFrom: selectedMovement.locationFrom || '',
            LocationTo: selectedMovement.locationTo || '',
            Description: selectedMovement.description || '',
            UnitPrice: selectedMovement.unitPrice || '',
            TotalAmount: selectedMovement.totalAmount || '',
            ResponsiblePerson: selectedMovement.responsiblePerson || '',
            SupplierCustomer: selectedMovement.supplierCustomer || '',
            BatchNumber: selectedMovement.batchNumber || '',
            SerialNumber: selectedMovement.serialNumber || '',
            ExpiryDate: selectedMovement.expiryDate ? new Date(selectedMovement.expiryDate).toISOString().slice(0, 10) : '',
            StockBalance: selectedMovement.stockBalance || '',
            IsActive: selectedMovement.isActive !== undefined ? selectedMovement.isActive : true,
          }}
          title="Malzeme Hareketi Detayları"
          materialCards={materialCards}
          isDetail={true}
        />
      )}
    </div>
  );
};

export default MaterialMovementTable; 