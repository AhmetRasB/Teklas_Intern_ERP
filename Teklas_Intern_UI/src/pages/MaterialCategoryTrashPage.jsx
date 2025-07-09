import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MasterLayout from '../masterLayout/MasterLayout';
import TableDataLayer from '../components/TableDataLayer';
import { useNavigate } from 'react-router-dom';

const BASE_URL = 'https://localhost:7178';

const columns = [
  { header: 'ID', accessor: 'id' },
  { header: 'Kod', accessor: 'code' },
  { header: 'Ad', accessor: 'name' },
  { header: 'Açıklama', accessor: 'description' },
  { header: 'Üst Kategori', accessor: 'parentCategoryName' },
  { header: 'Durum', accessor: 'status', render: (val) => val === 1 ? <span className="badge bg-success">Aktif</span> : <span className="badge bg-danger">Pasif</span> },
  { header: 'Oluşturulma', accessor: 'createDate', render: (val) => val ? new Date(val).toLocaleDateString('tr-TR') : '-' },
];

const MaterialCategoryTrashPage = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [restoreLoading, setRestoreLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchDeletedData();
  }, []);

  const fetchDeletedData = async () => {
    setLoading(true);
    try {
      const res = await axios.get(`${BASE_URL}/api/categories/deleted`);
      setData(res.data);
    } catch (err) {
      setError('Silinen kategoriler alınamadı!');
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const handleRestore = async (item) => {
    setRestoreLoading(true);
    try {
      await axios.put(`${BASE_URL}/api/categories/${item.id || item.Id}/restore`);
      fetchDeletedData();
    } catch {
      alert('Geri alma başarısız!');
    } finally {
      setRestoreLoading(false);
    }
  };

  const handlePermanentDelete = async (item) => {
    if (!window.confirm('Bu kategoriyi kalıcı olarak silmek istediğinize emin misiniz?')) return;
    setRestoreLoading(true);
    try {
      await axios.delete(`${BASE_URL}/api/categories/${item.id || item.Id}/permanent`);
      fetchDeletedData();
    } catch {
      alert('Kalıcı silme başarısız!');
    } finally {
      setRestoreLoading(false);
    }
  };

  const dataWithRowNumber = data.map((item, idx) => ({ ...item, rowNumber: idx + 1 }));

  return (
    <MasterLayout>
      <div className="col-lg-12">
        <div className="card h-100">
          <div className="card-header d-flex justify-content-between align-items-center">
            <h5 className="card-title mb-0">Silinen Kategoriler</h5>
            <button className="btn btn-secondary" onClick={() => navigate(-1)}>
              <i className="ri-arrow-go-back-line"></i> Geri
            </button>
          </div>
          <div className="card-body">
            {loading ? (
              <div className="text-center p-4">Yükleniyor...</div>
            ) : error ? (
              <div className="alert alert-danger mb-3">{error}</div>
            ) : (
              <TableDataLayer
                data={dataWithRowNumber}
                columns={columns}
                showActions={true}
                actions={['restore', 'permanentDelete']}
                onRestore={handleRestore}
                onPermanentDelete={handlePermanentDelete}
                loading={restoreLoading}
              />
            )}
          </div>
        </div>
      </div>
    </MasterLayout>
  );
};

export default MaterialCategoryTrashPage; 