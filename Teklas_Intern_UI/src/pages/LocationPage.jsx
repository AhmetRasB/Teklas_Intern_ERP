import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import LocationTable from '../components/WarehouseManagement/LocationTable';

const LocationPage = () => {
  return (
    <MasterLayout>
      <div className="row">
        <div className="col-12">
          <div className="page-title-box d-flex align-items-center justify-content-between">
            <h4 className="mb-0">Lokasyon Yönetimi</h4>
          </div>
        </div>
      </div>
      <div className="row">
        <LocationTable />
      </div>
    </MasterLayout>
  );
};

export default LocationPage; 