import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MaterialCardModal from './MaterialCardModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  MaterialCode: '',
  MaterialName: '',
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

  useEffect(() => {
    // Kategori listesini çek
    axios.get(`${BASE_URL}/api/MaterialCategory`)
      .then(res => setCategories(res.data))
      .catch(() => setCategories([]));
  }, []);

  useEffect(() => {
    fetchData();
  }, [selectedCategory]);

  const fetchData = () => {
    setLoading(true);
    let url = `${BASE_URL}/api/MaterialCard`;
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
    const now = new Date().toISOString();
    const payload = {
      MaterialCode: form.MaterialCode,
      MaterialName: form.MaterialName,
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
        await axios.put(`${BASE_URL}/api/MaterialCard/${form.id}`, payload);
      } else {
        // YENİ EKLEME (POST)
        await axios.post(`${BASE_URL}/api/MaterialCard`, payload);
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
        await axios.delete(`${BASE_URL}/api/MaterialCard/${item.id || item.Id}`);
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
    setForm({
      MaterialCode: row.materialCode || row.MaterialCode || '',
      MaterialName: row.materialName || row.MaterialName || '',
      MaterialType: row.materialType || row.MaterialType || '',
      CategoryId: row.categoryId || row.CategoryId || '',
      UnitOfMeasure: row.unitOfMeasure || row.UnitOfMeasure || '',
      Barcode: row.barcode || row.Barcode || '',
      Description: row.description || row.Description || '',
      Brand: row.brand || row.Brand || '',
      Model: row.model || row.Model || '',
      PurchasePrice: row.purchasePrice || row.PurchasePrice || '',
      SalesPrice: row.salesPrice || row.SalesPrice || '',
      MinimumStockLevel: row.minimumStockLevel || row.MinimumStockLevel || '',
      MaximumStockLevel: row.maximumStockLevel || row.MaximumStockLevel || '',
      ReorderLevel: row.reorderLevel || row.ReorderLevel || '',
      ShelfLife: row.shelfLife || row.ShelfLife || '',
      Weight: row.weight || row.Weight || '',
      Volume: row.volume || row.Volume || '',
      Length: row.length || row.Length || '',
      Width: row.width || row.Width || '',
      Height: row.height || row.Height || '',
      Color: row.color || row.Color || '',
      OriginCountry: row.originCountry || row.OriginCountry || '',
      Manufacturer: row.manufacturer || row.Manufacturer || '',
      IsActive: row.isActive ?? row.IsActive ?? true,
      id: row.id || row.Id || '',
      CreatedBy: row.createdBy || row.CreatedBy || '',
      CreatedDate: row.createdDate || row.CreatedDate || '',
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
    { header: "Kod", accessor: "materialCode" },
    { header: "Ad", accessor: "materialName" },
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

  if (loading) return <div>Yükleniyor...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Malzeme Kartları</h5>
          <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={() => setShowModal(true)}>
            <i className="ri-add-line"></i> Yeni Ekle
          </button>
        </div>
        <div className="card-body">
          <div className="table-responsive">
            <TableDataLayer
              data={dataWithRowNumber}
              columns={columns}
              onView={handleView}
              onEdit={handleEdit}
              onDelete={handleDeleteRow}
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
                          ['Kod', selectedCard.materialCode],
                          ['Ad', selectedCard.materialName],
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
                          ['Oluşturulma', selectedCard.createdDate],
                          ['Güncellenme', selectedCard.updatedDate],
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
                <div className="modal-footer d-flex justify-content-end gap-2">
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedCard)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedCard)}>Sil</button>
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