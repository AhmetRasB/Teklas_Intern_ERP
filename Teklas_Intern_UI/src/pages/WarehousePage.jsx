import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import WarehouseTable from '../components/WarehouseManagement/WarehouseTable';

const WarehousePage = () => {
  return (
    <MasterLayout>
      <div className="row">
        <div className="col-12">
          <div className="page-title-box d-flex align-items-center justify-content-between">
            <h4 className="mb-0">Depo Yönetimi</h4>
          </div>
        </div>
      </div>
      <div className="row">
        <WarehouseTable />
      </div>
    </MasterLayout>
  );
};

export default WarehousePage; 