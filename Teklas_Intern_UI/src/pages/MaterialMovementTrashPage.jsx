import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import MasterLayout from '../masterLayout/MasterLayout';
import TableDataLayer from '../components/TableDataLayer';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import 'sweetalert2/dist/sweetalert2.min.css';

const BASE_URL = 'https://localhost:7178';

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

const MaterialMovementTrashPage = () => {
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
      const res = await axios.get(`${BASE_URL}/api/MaterialMovement/deleted`);
      setData(res.data);
    } catch (err) {
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const handleRestore = async (item) => {
    // Dark mode detection
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    
    const result = await MySwal.fire({
      title: 'Geri Alma Onayı',
      text: 'Bu malzeme hareketini geri almak istediğinize emin misiniz?',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Evet, Geri Al',
      cancelButtonText: 'Vazgeç',
      reverseButtons: true,
      customClass: {
        popup: isDark ? 'swal2-dark' : '',
        confirmButton: 'btn btn-success mx-2',
        cancelButton: 'btn btn-secondary mx-2',
      },
      buttonsStyling: false,
    });
    
    if (result.isConfirmed) {
      setRestoreLoading(true);
      try {
        await axios.post(`${BASE_URL}/api/MaterialMovement/${item.id || item.Id}/restore`);
        fetchDeletedData();
        MySwal.fire({
          title: 'Geri Alındı!',
          text: 'Malzeme hareketi başarıyla geri alındı.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (err) {
        MySwal.fire({
          title: 'Hata',
          text: 'Geri alma işlemi başarısız!',
          icon: 'error',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } finally {
        setRestoreLoading(false);
      }
    }
  };

  const handlePermanentDelete = async (item) => {
    // Dark mode detection
    const isDark = document.body.classList.contains('dark') || document.documentElement.classList.contains('dark');
    if (isDark && !document.getElementById('swal-dark-style')) {
      const style = document.createElement('style');
      style.id = 'swal-dark-style';
      style.innerHTML = swalDarkStyles;
      document.head.appendChild(style);
    }
    
    const result = await MySwal.fire({
      title: 'Kalıcı Silme Onayı',
      text: 'Bu malzeme hareketini kalıcı olarak silmek istediğinize emin misiniz? Bu işlem geri alınamaz!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Kalıcı Sil',
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
      setRestoreLoading(true);
      try {
        await axios.delete(`${BASE_URL}/api/MaterialMovement/${item.id || item.Id}/permanent`);
        fetchDeletedData();
        MySwal.fire({
          title: 'Kalıcı Silindi!',
          text: 'Malzeme hareketi kalıcı olarak silindi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (err) {
        MySwal.fire({
          title: 'Hata',
          text: 'Kalıcı silme işlemi başarısız!',
          icon: 'error',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } finally {
        setRestoreLoading(false);
      }
    }
  };

  const columns = [
    { header: '#', accessor: 'rowNumber' },
    { header: 'Malzeme', accessor: 'materialCardName' },
    { header: 'Hareket Tipi', accessor: 'movementType' },
    { header: 'Miktar', accessor: 'quantity' },
    { header: 'Tarih', accessor: 'movementDate', render: val => val ? new Date(val).toLocaleString('tr-TR') : '' },
    { header: 'Silinme Tarihi', accessor: 'updatedDate', render: val => val ? new Date(val).toLocaleString('tr-TR') : '' },
  ];

  const dataWithRowNumber = data.map((item, idx) => ({ ...item, rowNumber: idx + 1 }));

  return (
    <MasterLayout>
      <div className="col-lg-12">
        <style>{swalDarkStyles}</style>
        <div className="card h-100">
          <div className="card-header d-flex justify-content-between align-items-center">
            <h5 className="card-title mb-0">Silinen Malzeme Hareketleri</h5>
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

export default MaterialMovementTrashPage; 