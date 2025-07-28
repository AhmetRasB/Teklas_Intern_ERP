import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MaterialCategoryModal from './MaterialCategoryModal';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';
import TableDataLayer from '../TableDataLayer';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';
import Modal from 'react-bootstrap/Modal';
import { useAuth } from '../../contexts/AuthContext';

const BASE_URL = 'https://localhost:7178';

const initialForm = {
  Code: '',
  Name: '',
  Description: '',
  ParentCategoryId: '',
  Status: 1,
};

const MySwal = withReactContent(Swal);

const COLUMN_OPTIONS = [
  { key: 'id', label: 'ID' },
  { key: 'code', label: 'Kod' },
  { key: 'name', label: 'Ad' },
  { key: 'description', label: 'Açıklama' },
  { key: 'parentCategoryName', label: 'Üst Kategori' },
  { key: 'status', label: 'Durum' },
  { key: 'createDate', label: 'Oluşturulma' }
];

const TABLE_KEY = 'MaterialCategoryTable';

const MaterialCategoryTable = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [form, setForm] = useState(initialForm);
  const [formLoading, setFormLoading] = useState(false);
  const [formError, setFormError] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [showDeleted, setShowDeleted] = useState(false);
  const [deletedData, setDeletedData] = useState([]);
  const [restoreLoading, setRestoreLoading] = useState(false);
  const [parentCategories, setParentCategories] = useState([]);
  const navigate = useNavigate();
  const [showDetail, setShowDetail] = useState(false);
  const [detailData, setDetailData] = useState(null);
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

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    setLoading(true);
    try {
      console.log('Fetching active categories from:', `${BASE_URL}/api/categories?deleted=false`);
      const res = await axios.get(`${BASE_URL}/api/categories?deleted=false`);
      console.log('Active categories response:', res.data);
      setData(res.data);
      setParentCategories(res.data);
    } catch (err) {
      console.error('Error fetching active categories:', err);
      setData([]);
      setParentCategories([]);
    } finally {
      setLoading(false);
    }
  };

  const fetchDeletedData = async () => {
    setRestoreLoading(true);
    try {
      const res = await axios.get(`${BASE_URL}/api/categories/deleted`);
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
      await axios.put(`${BASE_URL}/api/categories/${item.id || item.Id}/restore`);
      fetchDeletedData();
      fetchCategories();
      MySwal.fire({ title: 'Geri Alındı!', text: 'Kategori başarıyla geri alındı.', icon: 'success' });
    } catch {
      MySwal.fire({ title: 'Hata', text: 'Geri alma başarısız!', icon: 'error' });
    } finally {
      setRestoreLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    console.log('Category form input change:', { name, value, currentForm: form });
    setForm({
      ...form,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormLoading(true);
    setFormError(null);
    const payload = {
      Code: form.Code,
      Name: form.Name,
      Description: form.Description,
      ParentCategoryId: form.ParentCategoryId ? Number(form.ParentCategoryId) : null,
      Status: Number(form.Status),
    };
    console.log('Category form submission:', { editMode, form, payload });
    try {
      if (editMode && form.id) {
        console.log('Updating category:', `${BASE_URL}/api/categories/${form.id}`, payload);
        await axios.put(`${BASE_URL}/api/categories/${form.id}`, payload);
      } else {
        console.log('Adding new category:', `${BASE_URL}/api/categories`, payload);
        await axios.post(`${BASE_URL}/api/categories`, payload);
      }
      setShowModal(false);
      setEditMode(false);
      setForm(initialForm);
      fetchCategories();
    } catch (err) {
      console.error('Category submission error:', err);
      setFormError(editMode ? 'Güncelleme başarısız!' : 'Ekleme başarısız!');
    } finally {
      setFormLoading(false);
    }
  };

  const handleEdit = (row) => {
    setEditMode(true);
    setForm({
      id: row.id,
      Code: row.code,
      Name: row.name,
      Description: row.description || '',
      ParentCategoryId: row.parentCategoryId || '',
      Status: row.status,
    });
    setShowModal(true);
  };

  const handleDelete = async (item) => {
    if (!window.confirm('Bu kategoriyi silmek istediğinize emin misiniz?')) return;
    try {
      console.log('Deleting category:', item.id);
      await axios.delete(`${BASE_URL}/api/categories/${item.id}`);
      console.log('Category deleted successfully');
      fetchCategories();
      MySwal.fire({ title: 'Başarılı!', text: 'Kategori silindi.', icon: 'success' });
    } catch (err) {
      console.error('Delete category error:', err);
      MySwal.fire({ title: 'Hata', text: 'Silme başarısız!', icon: 'error' });
    }
  };

  const handlePermanentDelete = async (item) => {
    if (!window.confirm('Bu kategoriyi kalıcı olarak silmek istediğinize emin misiniz?')) return;
    try {
      await axios.delete(`${BASE_URL}/api/categories/${item.id}/permanent`);
      fetchDeletedData();
    } catch {
      MySwal.fire({ title: 'Hata', text: 'Kalıcı silme başarısız!', icon: 'error' });
    }
  };

  const handleView = (row) => {
    setDetailData(row);
    setShowDetail(true);
  };

  const openModal = () => {
    console.log('Opening category modal with initial form:', initialForm);
    setEditMode(false);
    setForm(initialForm);
    setShowModal(true);
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
    ...COLUMN_OPTIONS.filter(col => selectedColumns.includes(col.key)).map(col => ({
      header: col.label,
      accessor: col.key
    }))
  ];

  if (loading) return <div className="text-center p-4">Yükleniyor...</div>;

  const displayData = showDeleted ? deletedData : data;

  return (
    <div className="col-lg-12">
      <div className="card h-100">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">Malzeme Kategorileri</h5>
          <div className="d-flex gap-2 align-items-center">
            {/* Sütun Seç Butonu */}
            <button
              className="btn btn-outline-secondary"
              onClick={() => setColumnSelectorOpen(true)}
              title="Sütunları Seç"
            >
              <Icon icon="mdi:table-column" className="me-1" /> Sütun Seç
            </button>
            <button className="btn rounded-pill btn-primary-100 text-primary-600 px-20 py-11" onClick={openModal}>
              <i className="ri-add-line"></i> Yeni Ekle
            </button>
            <button
              className="btn rounded-pill btn-soft-danger text-danger px-20 py-11"
              style={{ fontWeight: 600 }}
              title="Silinenleri Göster"
              onClick={() => navigate('/material-category-trash')}
            >
              <i className="ri-delete-bin-6-line" style={{ marginRight: 6 }} />
              Çöp Kutusu
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
              data={displayData}
              columns={columns}
              onEdit={handleEdit}
              onDelete={handleDelete}
              onView={handleView}
              showActions={true}
              actions={['view', 'edit', 'delete']}
              rest={{ onRestore: handleRestore, onPermanentDelete: handlePermanentDelete }}
              loading={loading}
            />
          </div>
        </div>
        <MaterialCategoryModal
          open={showModal}
          onClose={() => { setShowModal(false); setEditMode(false); setForm(initialForm); }}
          onSubmit={handleSubmit}
          form={form}
          onChange={handleInputChange}
          loading={formLoading}
          error={formError}
          parentCategories={parentCategories}
          fetchCategories={fetchCategories}
        />
        {/* Detay Modalı */}
        <Modal show={showDetail} onHide={() => setShowDetail(false)} centered>
          <Modal.Header closeButton>
            <Modal.Title>Kategori Detay</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            {detailData && (
              <div>
                <div><b>ID:</b> {detailData.id}</div>
                <div><b>Kod:</b> {detailData.code}</div>
                <div><b>Ad:</b> {detailData.name}</div>
                <div><b>Açıklama:</b> {detailData.description}</div>
                <div><b>Üst Kategori:</b> {detailData.parentCategoryName}</div>
                <div><b>Durum:</b> {detailData.status === 1 ? 'Aktif' : 'Pasif'}</div>
                <div><b>Oluşturulma:</b> {detailData.createDate ? new Date(detailData.createDate).toLocaleDateString('tr-TR') : '-'}</div>
              </div>
            )}
          </Modal.Body>
        </Modal>
      </div>
    </div>
  );
};

export default MaterialCategoryTable; 