import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MaterialCardModal from './MaterialCardModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import PaginationLayer from '../PaginationLayer';
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
  const [pageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);

  useEffect(() => {
    fetchData();
  }, [page]);

  const fetchData = () => {
    setLoading(true);
    axios.get(`${BASE_URL}/api/MaterialCard/paged?page=${page}&pageSize=${pageSize}`)
      .then(res => {
        setData(res.data.items);
        setTotalCount(res.data.totalCount);
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
      CreatedDate: now,
      UpdatedDate: null,
      CreatedBy: 'admin',
      UpdatedBy: '',
    };
    try {
      await axios.post(BASE_URL + '/api/MaterialCard', payload);
      setShowModal(false);
      setForm(initialForm);
      fetchData();
    } catch (err) {
      setFormError('Ekleme başarısız!');
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

  const startEdit = (item) => {
    setForm({
      MaterialCode: item.materialCode || item.MaterialCode || '',
      MaterialName: item.materialName || item.MaterialName || '',
      MaterialType: item.materialType || item.MaterialType || '',
      CategoryId: item.categoryId || item.CategoryId || '',
      UnitOfMeasure: item.unitOfMeasure || item.UnitOfMeasure || '',
      Barcode: item.barcode || item.Barcode || '',
      Description: item.description || item.Description || '',
      Brand: item.brand || item.Brand || '',
      Model: item.model || item.Model || '',
      PurchasePrice: item.purchasePrice || item.PurchasePrice || '',
      SalesPrice: item.salesPrice || item.SalesPrice || '',
      MinimumStockLevel: item.minimumStockLevel || item.MinimumStockLevel || '',
      MaximumStockLevel: item.maximumStockLevel || item.MaximumStockLevel || '',
      ReorderLevel: item.reorderLevel || item.ReorderLevel || '',
      ShelfLife: item.shelfLife || item.ShelfLife || '',
      Weight: item.weight || item.Weight || '',
      Volume: item.volume || item.Volume || '',
      Length: item.length || item.Length || '',
      Width: item.width || item.Width || '',
      Height: item.height || item.Height || '',
      Color: item.color || item.Color || '',
      OriginCountry: item.originCountry || item.OriginCountry || '',
      Manufacturer: item.manufacturer || item.Manufacturer || '',
      IsActive: item.isActive ?? item.IsActive ?? true,
      id: item.id || item.Id || '',
      CreatedBy: item.createdBy || item.CreatedBy || '',
      CreatedDate: item.createdDate || item.CreatedDate || '',
      UpdatedBy: 'admin',
      UpdatedDate: new Date().toISOString(),
    });
    setEditMode(true);
    setShowModal(true);
  };

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError(null);
    const payload = {
      Id: form.id || form.Id,
      MaterialCode: form.MaterialCode,
      MaterialName: form.MaterialName,
      MaterialType: form.MaterialType,
      CategoryId: Number(form.CategoryId) || 1,
      UnitOfMeasure: form.UnitOfMeasure,
      IsActive: form.IsActive,
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
      CreatedBy: form.CreatedBy,
      CreatedDate: form.CreatedDate,
      UpdatedBy: form.UpdatedBy,
      UpdatedDate: form.UpdatedDate,
    };
    try {
      await axios.put(`${BASE_URL}/api/MaterialCard/${payload.Id}`, payload);
      setShowModal(false);
      setEditMode(false);
      setForm(initialForm);
      fetchData();
    } catch (err) {
      setFormError('Güncelleme başarısız!');
    } finally {
      setFormLoading(false);
    }
  };

  // Truncate fonksiyonu
  const truncate = (str, n = 10) => {
    if (!str) return '';
    return str.length > n ? str.slice(0, 7) + '...' : str;
  };

  // Detay modalını açarken animasyonu başlat
  const openDetail = (item) => {
    setSelectedCard(item);
    setShowDetail(true);
    setTimeout(() => setDetailFade(true), 10);
  };
  // Detay modalını kapatırken animasyonu başlat
  const closeDetail = () => {
    setDetailFade(false);
    setTimeout(() => {
      setShowDetail(false);
      setSelectedCard(null);
    }, 250); // animasyon süresiyle uyumlu
  };

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
            <table className="table striped-table mb-0">
              <thead>
                <tr>
                  <th>#</th>
                  <th>Kod</th>
                  <th>Ad</th>
                  <th>Tip</th>
                  <th>Birim</th>
                  <th>Barkod</th>
                  <th>Marka</th>
                  <th>Model</th>
                  <th>Alış Fiyatı</th>
                  <th>Satış Fiyatı</th>
                  <th>Aktif mi?</th>
                  <th>İşlem</th>
                </tr>
              </thead>
              <tbody>
                {data.map((item, idx) => (
                  <tr key={item.id || item.Id}>
                    <td>{idx + 1}</td>
                    <td>{truncate(item.materialCode)}</td>
                    <td>{truncate(item.materialName)}</td>
                    <td>{truncate(item.materialType)}</td>
                    <td>{truncate(item.unitOfMeasure)}</td>
                    <td>{truncate(item.barcode)}</td>
                    <td>{truncate(item.brand)}</td>
                    <td>{truncate(item.model)}</td>
                    <td>{item.purchasePrice}</td>
                    <td>{item.salesPrice}</td>
                    <td>
                      {item.isActive ? (
                        <span className="bg-success-focus text-success-main px-32 py-4 rounded-pill fw-medium text-sm">Aktif</span>
                      ) : (
                        <span className="bg-lilac-100 text-lilac-600 px-32 py-4 rounded-pill fw-medium text-sm">Pasif</span>
                      )}
                    </td>
                    <td className="d-flex gap-2">
                      <button className="btn btn-sm rounded-pill btn-neutral-100 text-primary-light px-16 py-6" title="Detay" onClick={() => openDetail(item)}>
                        <i className="ri-eye-line"></i>
                      </button>
                      <button className="btn btn-sm rounded-pill btn-warning-100 text-warning-600 px-16 py-6" title="Düzenle" onClick={() => startEdit(item)}>
                        <i className="ri-edit-line"></i>
                      </button>
                      <button className="btn btn-sm rounded-pill btn-danger-100 text-danger-600 px-16 py-6" title="Sil" onClick={() => handleDelete(item)}>
                        <i className="ri-delete-bin-line"></i>
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <div className="d-flex justify-content-center mt-4">
            <nav>
              <ul className="pagination d-flex flex-wrap align-items-center gap-2 justify-content-center">
                <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                  <button className="page-link bg-primary-50 text-secondary-light fw-medium radius-8 border-0 py-10 d-flex align-items-center justify-content-center h-48-px w-48-px"
                    onClick={() => setPage(1)} disabled={page === 1}>
                    <Icon icon="ep:d-arrow-left" className="text-xl" />
                  </button>
                </li>
                <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                  <button className="page-link bg-primary-50 text-secondary-light fw-medium radius-8 border-0 py-10 d-flex align-items-center justify-content-center h-48-px w-48-px"
                    onClick={() => setPage(page - 1)} disabled={page === 1}>
                    <Icon icon="iconamoon:arrow-left-2-light" className="text-xxl" />
                  </button>
                </li>
                {Array.from({ length: Math.ceil(totalCount / pageSize) }, (_, i) => i + 1).map(num => (
                  <li key={num} className={`page-item ${page === num ? 'active' : ''}`}>
                    <button className={`page-link bg-primary-50 text-secondary-light fw-medium radius-8 border-0 py-10 d-flex align-items-center justify-content-center h-48-px w-48-px ${page === num ? 'bg-primary-600 text-white' : ''}`}
                      onClick={() => setPage(num)}>{num}</button>
                  </li>
                ))}
                <li className={`page-item ${page === Math.ceil(totalCount / pageSize) ? 'disabled' : ''}`}>
                  <button className="page-link bg-primary-50 text-secondary-light fw-medium radius-8 border-0 py-10 d-flex align-items-center justify-content-center h-48-px w-48-px"
                    onClick={() => setPage(page + 1)} disabled={page === Math.ceil(totalCount / pageSize)}>
                    <Icon icon="iconamoon:arrow-right-2-light" className="text-xxl" />
                  </button>
                </li>
                <li className={`page-item ${page === Math.ceil(totalCount / pageSize) ? 'disabled' : ''}`}>
                  <button className="page-link bg-primary-50 text-secondary-light fw-medium radius-8 border-0 py-10 d-flex align-items-center justify-content-center h-48-px w-48-px"
                    onClick={() => setPage(Math.ceil(totalCount / pageSize))} disabled={page === Math.ceil(totalCount / pageSize)}>
                    <Icon icon="ep:d-arrow-right" className="text-xl" />
                  </button>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </div>
      <MaterialCardModal
        open={showModal}
        onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
        onSubmit={editMode ? handleEditSubmit : handleSubmit}
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
          onClick={closeDetail}
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
                <button type="button" className="btn-close" onClick={closeDetail}></button>
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
                <button className="btn btn-soft-primary" onClick={() => startEdit(selectedCard)}>Düzenle</button>
                <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedCard)}>Sil</button>
                <button className="btn btn-secondary" onClick={closeDetail}>Kapat</button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default MaterialCardTable; 