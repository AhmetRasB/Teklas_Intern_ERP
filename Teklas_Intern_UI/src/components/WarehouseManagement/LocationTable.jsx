import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import TableDataLayer from '../TableDataLayer';
import LocationModal from './LocationModal';

const BASE_URL = 'https://localhost:7178';

const MySwal = Swal.mixin({
  customClass: {
    confirmButton: 'btn btn-primary',
    cancelButton: 'btn btn-secondary me-2'
  },
  buttonsStyling: false
});

const LocationTable = () => {
  const [data, setData] = useState([]);
  const [deletedData, setDeletedData] = useState([]);
  const [showDeleted, setShowDeleted] = useState(false);
  const [loading, setLoading] = useState(true);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [showDetail, setShowDetail] = useState(false);
  const [selectedLocation, setSelectedLocation] = useState(null);
  const [detailFade, setDetailFade] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [categories, setCategories] = useState([]);

  const navigate = useNavigate();

  const initialForm = {
    Code: '',
    Name: '',
    WarehouseId: '',
    LocationType: '',
    Aisle: '',
    Rack: '',
    Level: '',
    Position: '',
    Capacity: '',
    Length: '',
    Width: '',
    Height: '',
    WeightCapacity: '',
    Temperature: '',
    Humidity: '',
    Description: '',
    IsActive: true
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

    axios.get(`${BASE_URL}/api/locations?${params}`)
      .then(res => {
        setData(res.data.items || res.data);
        setTotalCount(res.data.totalCount || res.data.length);
      })
      .catch(err => {
        console.error('Error fetching locations:', err);
        setData([]);
      })
      .finally(() => setLoading(false));
  };

  const fetchDeletedData = async () => {
    try {
      const res = await axios.get(`${BASE_URL}/api/locations/deleted`);
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
      await axios.put(`${BASE_URL}/api/locations/${item.id || item.Id}/restore`);
      fetchDeletedData();
      fetchData();
      MySwal.fire({
        title: 'Başarılı!',
        text: 'Lokasyon başarıyla geri alındı.',
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
      [name]: name === 'IsActive'
        ? value === 'true'
        : (type === 'checkbox' ? e.target.checked : value)
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError('');

    try {
      if (editMode) {
        await axios.put(`${BASE_URL}/api/locations/${form.id}`, form);
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Lokasyon başarıyla güncellendi.',
          icon: 'success',
          confirmButtonText: 'Tamam'
        });
      } else {
        await axios.post(`${BASE_URL}/api/locations`, form);
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Lokasyon başarıyla eklendi.',
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
      text: `"${item.name || item.Name}" adlı lokasyonu silmek istediğinize emin misiniz?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Sil',
      cancelButtonText: 'İptal'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/locations/${item.id || item.Id}`);
        fetchData();
        MySwal.fire({
          title: 'Başarılı!',
          text: 'Lokasyon başarıyla silindi.',
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
      WarehouseId: row.warehouseId || row.WarehouseId || '',
      LocationType: row.locationType || row.LocationType || '',
      Aisle: row.aisle || row.Aisle || '',
      Rack: row.rack || row.Rack || '',
      Level: row.level || row.Level || '',
      Position: row.position || row.Position || '',
      Capacity: row.capacity || row.Capacity || '',
      Length: row.length || row.Length || '',
      Width: row.width || row.Width || '',
      Height: row.height || row.Height || '',
      WeightCapacity: row.weightCapacity || row.WeightCapacity || '',
      Temperature: row.temperature || row.Temperature || '',
      Humidity: row.humidity || row.Humidity || '',
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
    setSelectedLocation(row);
    setShowDetail(true);
    setDetailFade(true);
  };

  const handleDeleteRow = (row) => {
    handleDelete(row);
  };

  // Tablo kolon başlıkları
  const columns = [
    { header: "#", accessor: "rowNumber" },
    { header: "Kod", accessor: "code" },
    { header: "Ad", accessor: "name" },
    { header: "Depo", accessor: "warehouseName" },
    { header: "Tip", accessor: "locationType" },
    { header: "Kapasite", accessor: "capacity" },
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

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Lokasyonlar</h5>
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
              onClick={() => navigate('/location-trash')}
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
        <LocationModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          title={editMode ? 'Lokasyon Düzenle' : 'Lokasyon Ekle'}
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
            onClick={() => { setShowDetail(false); setSelectedLocation(null); }}
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
                  <h5 className="modal-title">Lokasyon Detay</h5>
                  <button type="button" className="btn-close" onClick={() => { setShowDetail(false); setSelectedLocation(null); }}></button>
                </div>
                <div className="modal-body" style={{padding: '16px 8px 8px 8px'}}>
                  <div style={{maxHeight: '70vh', overflowY: 'auto', padding: 0}}>
                    {selectedLocation && (
                      <div>
                        {[
                          ['Kod', selectedLocation.code],
                          ['Ad', selectedLocation.name],
                          ['Depo', selectedLocation.warehouseName],
                          ['Tip', selectedLocation.locationType],
                          ['Koridor', selectedLocation.aisle],
                          ['Raf', selectedLocation.rack],
                          ['Seviye', selectedLocation.level],
                          ['Pozisyon', selectedLocation.position],
                          ['Kapasite', selectedLocation.capacity],
                          ['Uzunluk', selectedLocation.length],
                          ['Genişlik', selectedLocation.width],
                          ['Yükseklik', selectedLocation.height],
                          ['Ağırlık Kapasitesi', selectedLocation.weightCapacity],
                          ['Sıcaklık', selectedLocation.temperature],
                          ['Nem', selectedLocation.humidity],
                          ['Açıklama', selectedLocation.description],
                          ['Aktif mi?', selectedLocation.isActive ? 'Aktif' : 'Pasif'],
                          ['Oluşturulma', formatDateTime(selectedLocation.createdDate)],
                          ['Güncellenme', formatDateTime(selectedLocation.updatedDate)],
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
                  <button className="btn btn-soft-primary" onClick={() => handleEdit(selectedLocation)}>Düzenle</button>
                  <button className="btn btn-soft-danger" onClick={() => handleDelete(selectedLocation)}>Sil</button>
                  <button className="btn btn-secondary" onClick={() => { setShowDetail(false); setSelectedLocation(null); }}>Kapat</button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default LocationTable; 