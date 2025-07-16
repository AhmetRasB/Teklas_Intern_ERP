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

const WarehouseTrashPage = () => {
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
      const res = await axios.get(`${BASE_URL}/api/warehouse/warehouse/deleted`);
      console.log('Deleted Warehouse data:', res.data); // Debug log
      setData(res.data);
    } catch (err) {
      console.error('Error fetching deleted Warehouse data:', err); // Debug log
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
      title: 'Geri Yükleme Onayı',
      text: `Bu depoyu geri yüklemek istediğinizden emin misiniz?`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Evet, Geri Yükle',
      cancelButtonText: 'İptal',
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
        await axios.put(`${BASE_URL}/api/warehouse/warehouse/${item.id}/restore`);
        await fetchDeletedData();
        MySwal.fire({
          title: 'Geri Yüklendi!',
          text: 'Depo başarıyla geri yüklendi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (err) {
        console.error('Restore error:', err);
        MySwal.fire({
          title: 'Hata',
          text: 'Geri yükleme başarısız!',
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
      text: 'Bu depoyu kalıcı olarak silmek istediğinizden emin misiniz? Bu işlem geri alınamaz!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Evet, Kalıcı Sil',
      cancelButtonText: 'İptal',
      reverseButtons: true,
      confirmButtonColor: '#dc3545',
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
        await axios.delete(`${BASE_URL}/api/warehouse/warehouse/${item.id}/permanent`);
        await fetchDeletedData();
        MySwal.fire({
          title: 'Kalıcı Olarak Silindi!',
          text: 'Depo kalıcı olarak silindi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (err) {
        console.error('Permanent delete error:', err);
        MySwal.fire({
          title: 'Hata',
          text: 'Kalıcı silme başarısız!',
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
    { header: 'Depo Kodu', accessor: 'warehouseCode' },
    { header: 'Depo Adı', accessor: 'warehouseName' },
    { header: 'Adres', accessor: 'address' },
    { header: 'Telefon', accessor: 'phone' },
    { header: 'Silinme Tarihi', accessor: 'deleteDate', render: val => val ? new Date(val).toLocaleString('tr-TR') : '' },
  ];

  const dataWithRowNumber = data.map((item, idx) => ({ 
    ...item, 
    rowNumber: idx + 1,
    warehouseCode: item.warehouseCode || 'N/A',
    warehouseName: item.warehouseName || 'N/A',
    address: item.address || 'Adres yok',
    phone: item.phone || 'Telefon yok',
    deleteDate: item.deleteDate || item.updateDate || 'Tarih bilgisi yok'
  }));

  return (
    <MasterLayout>
      <div className="col-lg-12">
        <div className="card h-100">
          <div className="card-header d-flex justify-content-between align-items-center">
            <h5 className="card-title mb-0">Silinen Depolar</h5>
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

export default WarehouseTrashPage; 