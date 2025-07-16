import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import MasterLayout from '../masterLayout/MasterLayout';
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

const CustomerTrashPage = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchDeletedData();
  }, []);

  const fetchDeletedData = async () => {
    try {
      setLoading(true);
      const res = await axios.get(`${BASE_URL}/api/Customer/deleted`);
      setData(res.data);
    } catch (err) {
      console.error('Error fetching deleted customers:', err);
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
      text: `Bu müşteriyi geri yüklemek istediğinizden emin misiniz?`,
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
      try {
        await axios.put(`${BASE_URL}/api/Customer/${item.id || item.Id}/restore`);
        await fetchDeletedData();
        MySwal.fire({
          title: 'Geri Yüklendi!',
          text: 'Müşteri başarıyla geri yüklendi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (error) {
        console.error('Restore error:', error);
        MySwal.fire({
          title: 'Hata',
          text: 'Geri yükleme başarısız!',
          icon: 'error',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
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
      text: 'Bu müşteriyi kalıcı olarak silmek istediğinizden emin misiniz? Bu işlem geri alınamaz!',
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
      try {
        await axios.delete(`${BASE_URL}/api/Customer/${item.id || item.Id}/permanent`);
        await fetchDeletedData();
        MySwal.fire({
          title: 'Kalıcı Olarak Silindi!',
          text: 'Müşteri kalıcı olarak silindi.',
          icon: 'success',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      } catch (error) {
        console.error('Permanent delete error:', error);
        MySwal.fire({
          title: 'Hata',
          text: 'Kalıcı silme başarısız!',
          icon: 'error',
          customClass: { popup: isDark ? 'swal2-dark' : '' },
        });
      }
    }
  };

  const formatDateTime = (val) => {
    if (!val) return '';
    return new Date(val).toLocaleString('tr-TR');
  };

  if (loading) return <div>Yükleniyor...</div>;

  return (
    <MasterLayout>
      <div className="row">
        <div className="col-12">
          <div className="page-title-box d-flex align-items-center justify-content-between">
            <h4 className="mb-0">Silinen Müşteriler</h4>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-lg-12">
          <div className="card h-100">
            <div className="card-header d-flex justify-content-between align-items-center">
              <h5 className="card-title mb-0">Silinen Müşteriler</h5>
              <button
                className="btn rounded-pill btn-primary px-20 py-11"
                onClick={() => navigate('/customer')}
              >
                <i className="ri-arrow-left-line me-2"></i>
                Geri Dön
              </button>
            </div>
            <div className="card-body">
              <div className="table-responsive">
                <table className="table table-hover">
                  <thead>
                    <tr>
                      <th>#</th>
                      <th>Ad</th>
                      <th>Adres</th>
                      <th>Telefon</th>
                      <th>E-posta</th>
                      <th>Vergi No</th>
                      <th>İletişim Kişisi</th>
                      <th>Silinme Tarihi</th>
                      <th>İşlemler</th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.length === 0 ? (
                      <tr>
                        <td colSpan="9" className="text-center py-4">
                          <div className="text-muted">
                            <i className="ri-delete-bin-6-line fs-1 mb-3 d-block"></i>
                            Silinen müşteri bulunamadı
                          </div>
                        </td>
                      </tr>
                    ) : (
                      data.map((item, index) => (
                        <tr key={item.id || item.Id}>
                          <td>{index + 1}</td>
                          <td>{item.name || item.Name}</td>
                          <td>{item.address || item.Address}</td>
                          <td>{item.phone || item.Phone}</td>
                          <td>{item.email || item.Email}</td>
                          <td>{item.taxNumber || item.TaxNumber}</td>
                          <td>{item.contactPerson || item.ContactPerson}</td>
                          <td>{formatDateTime(item.deletedDate || item.DeletedDate)}</td>
                          <td>
                            <div className="d-flex gap-2">
                              <button
                                className="btn btn-sm btn-soft-success"
                                onClick={() => handleRestore(item)}
                                title="Geri Al"
                              >
                                <i className="ri-restore-line"></i>
                              </button>
                              <button
                                className="btn btn-sm btn-soft-danger"
                                onClick={() => handlePermanentDelete(item)}
                                title="Kalıcı Sil"
                              >
                                <i className="ri-delete-bin-6-line"></i>
                              </button>
                            </div>
                          </td>
                        </tr>
                      ))
                    )}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </MasterLayout>
  );
};

export default CustomerTrashPage; 