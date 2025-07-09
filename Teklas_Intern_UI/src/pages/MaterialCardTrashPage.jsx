import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import MasterLayout from '../masterLayout/MasterLayout';
import TableDataLayer from '../components/TableDataLayer';

const BASE_URL = 'https://localhost:7178';

const MaterialCardTrashPage = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [restoreLoading, setRestoreLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchDeletedData();
  }, []);

  const fetchDeletedData = async () => {
    setLoading(true);
    try {
      const res = await axios.get(`${BASE_URL}/api/materials/deleted`);
      setData(res.data);
    } catch (err) {
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const handleRestore = async (item) => {
    setRestoreLoading(true);
    try {
      await axios.put(`${BASE_URL}/api/materials/${item.id || item.Id}/restore`);
      fetchDeletedData();
    } catch {
      alert('Geri alma başarısız!');
    } finally {
      setRestoreLoading(false);
    }
  };

  const handlePermanentDelete = async (item) => {
    if (!window.confirm('Bu kartı kalıcı olarak silmek istediğinize emin misiniz?')) return;
    setRestoreLoading(true);
    try {
      await axios.delete(`${BASE_URL}/api/materials/${item.id || item.Id}/permanent`);
      fetchDeletedData();
    } catch {
      alert('Kalıcı silme başarısız!');
    } finally {
      setRestoreLoading(false);
    }
  };

  const columns = [
    { header: '#', accessor: 'rowNumber' },
    { header: 'Kod', accessor: 'code' },
    { header: 'Ad', accessor: 'name' },
    { header: 'Tip', accessor: 'materialType' },
    { header: 'Birim', accessor: 'unitOfMeasure' },
    { header: 'Silinme Tarihi', accessor: 'updatedDate', render: val => val ? new Date(val).toLocaleString('tr-TR') : '' },
  ];

  const dataWithRowNumber = data.map((item, idx) => ({ ...item, rowNumber: idx + 1 }));

  return (
    <MasterLayout>
      <div className="col-lg-12">
        <div className="card h-100">
          <div className="card-header d-flex justify-content-between align-items-center">
            <h5 className="card-title mb-0">Silinen Malzeme Kartları</h5>
            <button className="btn btn-secondary" onClick={() => navigate(-1)}>
              <i className="ri-arrow-go-back-line"></i> Geri
            </button>
          </div>
          <div className="card-body">
            <TableDataLayer
              data={dataWithRowNumber}
              columns={columns}
              showActions={true}
              actions={['restore', 'permanentDelete']}
              onRestore={handleRestore}
              onPermanentDelete={handlePermanentDelete}
              loading={loading || restoreLoading}
            />
          </div>
        </div>
      </div>
    </MasterLayout>
  );
};

export default MaterialCardTrashPage; 