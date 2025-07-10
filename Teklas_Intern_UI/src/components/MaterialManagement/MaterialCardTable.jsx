import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MaterialCardModal from './MaterialCardModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';
import useModulePermissions from '../../hooks/useModulePermissions';

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  Code: '',
  Name: '',
  MaterialType: '',
  CategoryId: '',
  UnitOfMeasure: '',
  Barcode: '',
  Description: '',
  Brand: '',
  Model: '',
  PurchasePrice: '',
  SalesPrice: '',
  MinimumStockLevel: '',
  MaximumStockLevel: '',
  ReorderLevel: '',
  ShelfLife: '',
  Weight: '',
  Volume: '',
  Length: '',
  Width: '',
  Height: '',
  Color: '',
  OriginCountry: '',
  Manufacturer: '',
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

const MaterialCardTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedCard, setSelectedCard] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const [deleteError, setDeleteError] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [page, setPage] = useState(1);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState('');
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [showDeleted, setShowDeleted] = useState(false);
  const [deletedData, setDeletedData] = useState([]);
  const [restoreLoading, setRestoreLoading] = useState(false);

  const navigate = useNavigate();
  
  // Module permissions hook
  const { canWrite, isAdmin } = useModulePermissions();

  useEffect(() => {
    // Kategori listesini çek
    axios.get(`${BASE_URL}/api/categories`)
      .then(res => setCategories(res.data))
      .catch(() => setCategories([]));
  }, []);

  useEffect(() => {
    fetchData();
  }, [selectedCategory]);

  const fetchData = () => {
    setLoading(true);
    let url = `${BASE_URL}/api/materials`;
    if (selectedCategory) url += `?categoryId=${selectedCategory}`;
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
      const res = await axios.get(`${BASE_URL}/api/materials/deleted`);
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
      await axios.put(`${BASE_URL}/api/materials/${item.id || item.Id}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({ title: 'Geri Alındı!', text: 'Kart başarıyla geri alındı.', icon: 'success' });
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
    
    // Permission check: Prevent read-only users from creating/editing (Admin bypass included)
    if (!canWrite('materialManagement')) {
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
      Code: form.Code,
      Name: form.Name,
      MaterialType: form.MaterialType,
      CategoryId: Number(form.CategoryId) || 1,
      UnitOfMeasure: form.UnitOfMeasure,
      Barcode: form.Barcode,
      Description: form.Description,
      Brand: form.Brand,
      Model: form.Model,
      PurchasePrice: Number(form.PurchasePrice) || 0,
      SalesPrice: Number(form.SalesPrice) || 0,
      MinimumStockLevel: Number(form.MinimumStockLevel) || 0,
      MaximumStockLevel: Number(form.MaximumStockLevel) || 0,
      ReorderLevel: Number(form.ReorderLevel) || 0,
      ShelfLife: form.ShelfLife ? Number(form.ShelfLife) : null,
      Weight: form.Weight ? Number(form.Weight) : null,
      Volume: form.Volume ? Number(form.Volume) : null,
      Length: form.Length ? Number(form.Length) : null,
      Width: form.Width ? Number(form.Width) : null,
      Height: form.Height ? Number(form.Height) : null,
      Color: form.Color,
      OriginCountry: form.OriginCountry,
      Manufacturer: form.Manufacturer,
      IsActive: form.IsActive,
      CreatedDate: form.CreatedDate || now,
      UpdatedDate: now,
      CreatedBy: form.CreatedBy || 'admin',
      UpdatedBy: 'admin',
    };

    try {
      if (editMode && form.id) {
        // GÜNCELLEME (PUT)
        await axios.put(`${BASE_URL}/api/materials/${form.id}`, payload);
      } else {
        // YENİ EKLEME (POST)
        await axios.post(`${BASE_URL}/api/materials`, payload);
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
    // Permission check: Prevent read-only users from deleting
    if (!canWrite('materialManagement')) {
      MySwal.fire({
        title: 'Erişim Engellendi',
        text: 'Bu modül için yalnızca görüntüleme izniniz var. Silme işlemi yapamazsınız.',
        icon: 'warning',
        confirmButtonText: 'Tamam'
      });
      return;
    }

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
      text: 'Bu malzeme kartını silmek istediğinize emin misiniz?',
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
        await axios.delete(`${BASE_URL}/api/materials/${item.id || item.Id}`);
        fetchData();
        MySwal.fire({
          title: 'Silindi!',
          text: 'Malzeme kartı başarıyla silindi.',
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
    // Permission check: Prevent read-only users from editing
    if (!canWrite('materialManagement')) {
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
      Code: row.code || '',
      Name: row.name || '',
      MaterialType: row.materialType || '',
      CategoryId: row.categoryId || '',
      UnitOfMeasure: row.unitOfMeasure || '',
      Barcode: row.barcode || '',
      Description: row.description || '',
      Brand: row.brand || '',
      Model: row.model || '',
      PurchasePrice: row.purchasePrice || '',
      SalesPrice: row.salesPrice || '',
      MinimumStockLevel: row.minimumStockLevel || '',
      MaximumStockLevel: row.maximumStockLevel || '',
      ReorderLevel: row.reorderLevel || '',
      ShelfLife: row.shelfLife || '',
      Weight: row.weight || '',
      Volume: row.volume || '',
      Length: row.length || '',
      Width: row.width || '',
      Height: row.height || '',
      Color: row.color || '',
      OriginCountry: row.originCountry || '',
      Manufacturer: row.manufacturer || '',
      IsActive: row.isActive ?? true,
      id: row.id || '',
      CreatedBy: row.createdBy || '',
      CreatedDate: row.createdDate || '',
      UpdatedBy: 'admin',
      UpdatedDate: new Date().toISOString(),
    });
    setEditMode(true);
    setShowModal(true);
  };

  // Truncate fonksiyonu
  const truncate = (str, n = 10) => {
    if (!str) return '';
    return str.length > n ? str.slice(0, 7) + '...' : str;
  };

  // Yardımcı fonksiyon: ISO tarihleri okunabilir formata çevir
  const formatDateTime = (val) => {
    if (!val) return '';
    const d = new Date(val);
    if (isNaN(d)) return val;
    return d.toLocaleDateString('tr-TR') + ' ' + d.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
  };

  // Detay modalını aç
  const handleView = (row) => {
    setSelectedCard(row);
    setShowDetail(true);
    setTimeout(() => setDetailFade(true), 10);
  };

  // Silme işlemini başlat
  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  // Tablo kolon başlıkları
  const columns = [
    { header: "#", accessor: "rowNumber" },
    { header: "Kod", accessor: "code" },
    { header: "Ad", accessor: "name" },
    { header: "Tip", accessor: "materialType" },
    { header: "Birim", accessor: "unitOfMeasure" },
    { header: "Barkod", accessor: "barcode" },
    { header: "Marka", accessor: "brand" },
    { header: "Model", accessor: "model" },
    { header: "Alış Fiyatı", accessor: "purchasePrice" },
    { header: "Satış Fiyatı", accessor: "salesPrice" },
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
          <h5 className="card-title mb-0">Malzeme Kartları</h5>
          <div className="d-flex gap-2 align-items-center">
            {canWrite('materialManagement') && (
              <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => {
                // Double-check permission before opening modal
                if (!canWrite('materialManagement')) {
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
              onClick={() => navigate('/material-card-trash')}
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
              actions={canWrite('materialManagement') ? ['view', 'edit', 'delete'] : ['view']}
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
        <MaterialCardModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Malzeme Kartı Düzenle' : 'Malzeme Kartı Ekle'}
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
            onClick={() => { setShowDetail(false); setSelectedCard(null); }}
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
                  <h5 className="modal-title">Malzeme Kartı Detay</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedCard(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedCard && (
                      <div>
                        {[
                          ['Kod', selectedCard.code],
                          ['Ad', selectedCard.name],
                          ['Tip', selectedCard.materialType],
                          ['Kategori', selectedCard.categoryId],
                          ['Birim', selectedCard.unitOfMeasure],
                          ['Barkod', selectedCard.barcode],
                          ['Açıklama', selectedCard.description],
                          ['Marka', selectedCard.brand],
                          ['Model', selectedCard.model],
                          ['Alış Fiyatı', selectedCard.purchasePrice],
                          ['Satış Fiyatı', selectedCard.salesPrice],
                          ['Minimum Stok', selectedCard.minimumStockLevel],
                          ['Maksimum Stok', selectedCard.maximumStockLevel],
                          ['Yeniden Sipariş', selectedCard.reorderLevel],
                          ['Raf Ömrü', selectedCard.shelfLife],
                          ['Ağırlık', selectedCard.weight],
                          ['Hacim', selectedCard.volume],
                          ['Uzunluk', selectedCard.length],
                          ['Genişlik', selectedCard.width],
                          ['Yükseklik', selectedCard.height],
                          ['Renk', selectedCard.color],
                          ['Menşei Ülke', selectedCard.originCountry],
                          ['Üretici', selectedCard.manufacturer],
                          ['Aktif mi?', selectedCard.isActive ? 'Aktif' : 'Pasif'],
                          ['Oluşturulma', formatDateTime(selectedCard.createdDate)],
                          ['Güncellenme', formatDateTime(selectedCard.updatedDate)],
                          ['Oluşturan', selectedCard.createdBy],
                          ['Güncelleyen', selectedCard.updatedBy],
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
                  {canWrite('materialManagement') && (
                    <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedCard)}>Düzenle</button>
                  )}
                  {canWrite('materialManagement') && (
                    <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedCard)}>Sil</button>
                  )}
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedCard(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default MaterialCardTable; 