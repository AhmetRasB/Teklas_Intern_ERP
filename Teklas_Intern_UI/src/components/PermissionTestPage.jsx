import React from 'react';
import { useModulePermissions } from '../hooks/useModulePermissions';
import { Icon } from '@iconify/react';

const PermissionTestPage = () => {
  const { 
    modulePermissions, 
    getModuleActions, 
    canCreateInModule, 
    hasModuleCRUDAccess 
  } = useModulePermissions();

  const modules = [
    { key: 'material-management', name: 'Malzeme Yönetimi', icon: 'mdi:package-variant-closed' },
    { key: 'production-management', name: 'Üretim Yönetimi', icon: 'mdi:factory' },
    { key: 'purchasing-management', name: 'Satınalma Yönetimi', icon: 'mdi:cart-plus' },
    { key: 'sales-management', name: 'Satış Yönetimi', icon: 'mdi:cart-arrow-up' },
    { key: 'warehouse-management', name: 'Depo/Stok Yönetimi', icon: 'mdi:warehouse' },
    { key: 'report-management', name: 'Raporlar', icon: 'mdi:chart-line' },
    { key: 'user-management', name: 'Kullanıcı Yönetimi', icon: 'mdi:account-group' }
  ];

  const renderActionButtons = (moduleKey) => {
    const actions = getModuleActions(moduleKey);
    
    return (
      <div className="d-flex gap-2">
        {/* Create Button */}
        {canCreateInModule(moduleKey) && (
          <button className="btn rounded-pill btn-primary-100 text-primary-600 radius-8 px-16 py-8 fw-medium">
            <Icon icon="mdi:plus" className="me-1" />
            Yeni
          </button>
        )}
        
        {/* Action buttons for table rows */}
        <div className="d-flex gap-1">
          {actions.includes('view') && (
            <button
              className="w-32-px h-32-px bg-primary-light text-primary-600 rounded-circle d-inline-flex align-items-center justify-content-center"
              title="Görüntüle"
            >
              <Icon icon="iconamoon:eye-light" />
            </button>
          )}
          {actions.includes('edit') && (
            <button
              className="w-32-px h-32-px bg-success-focus text-success-main rounded-circle d-inline-flex align-items-center justify-content-center"
              title="Düzenle"
            >
              <Icon icon="lucide:edit" />
            </button>
          )}
          {actions.includes('delete') && (
            <button
              className="w-32-px h-32-px bg-danger-focus text-danger-main rounded-circle d-inline-flex align-items-center justify-content-center"
              title="Sil"
            >
              <Icon icon="ic:round-delete" />
            </button>
          )}
        </div>
      </div>
    );
  };

  return (
    <div className="card h-100">
      <div className="card-header">
        <h5 className="card-title mb-0">Modül İzin Kontrolü Test Sayfası</h5>
        <p className="text-secondary-light mb-0">
          Bu sayfa modül izinlerine göre butonların nasıl gösterildiğini test etmek için oluşturulmuştur.
        </p>
      </div>
      <div className="card-body">
        <div className="alert alert-info">
          <Icon icon="mdi:information" className="me-2" />
          <strong>Nasıl Test Edilir:</strong>
          <ol className="mb-0 mt-2">
            <li>Profil sayfasında "Modül Erişim İzinleri" sekmesine gidin</li>
            <li>Modüller için "Tam Erişim" veya "Sadece Görüntüleme" seçin</li>
            <li>İzinleri kaydedin ve bu sayfaya dönün</li>
            <li>Butonların nasıl değiştiğini gözlemleyin</li>
            <li>Malzeme Yönetimi sayfasına gidin ve işlem yapmayı deneyin</li>
          </ol>
        </div>

        <div className="alert alert-success">
          <Icon icon="mdi:shield-check" className="me-2" />
          <strong>Güvenlik Koruması:</strong>
          <p className="mb-1">
            Sistem çok katmanlı güvenlik koruması kullanır:
          </p>
          <ul className="mb-0">
            <li><strong>UI Koruması:</strong> Yetkisiz butonlar gizlenir</li>
            <li><strong>Fonksiyon Koruması:</strong> CRUD fonksiyonları izin kontrollü</li>
            <li><strong>Uyarı Sistemi:</strong> Yetkisiz erişimlerde uyarı gösterilir</li>
          </ul>
        </div>

        <div className="row gy-4">
          {modules.map((module) => (
            <div key={module.key} className="col-md-6 col-lg-4">
              <div className="card border">
                <div className="card-body">
                  <div className="d-flex align-items-center gap-3 mb-3">
                    <div className="w-40-px h-40-px bg-primary-50 rounded-circle d-flex align-items-center justify-content-center">
                      <Icon icon={module.icon} className="text-primary-600" />
                    </div>
                    <div>
                      <h6 className="mb-0">{module.name}</h6>
                      <small className="text-secondary-light">
                        {hasModuleCRUDAccess(module.key) ? 'Tam Erişim' : 'Sadece Görüntüleme'}
                      </small>
                    </div>
                  </div>
                  
                  <div className="mb-3">
                    <strong>Mevcut Butonlar:</strong>
                    <div className="mt-2">
                      {renderActionButtons(module.key)}
                    </div>
                  </div>
                  
                  <div className="small text-secondary-light">
                    <strong>İzin Detayları:</strong>
                    <ul className="mb-0 mt-1">
                      <li>Oluşturma: {canCreateInModule(module.key) ? '✅' : '❌'}</li>
                      <li>Görüntüleme: ✅</li>
                      <li>Düzenleme: {getModuleActions(module.key).includes('edit') ? '✅' : '❌'}</li>
                      <li>Silme: {getModuleActions(module.key).includes('delete') ? '✅' : '❌'}</li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        <div className="mt-4">
          <h6>Mevcut İzin Durumu:</h6>
          <div className="bg-light p-3 rounded">
            <pre>{JSON.stringify(modulePermissions, null, 2)}</pre>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PermissionTestPage; 