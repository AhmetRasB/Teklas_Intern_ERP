import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import SupplierTable from '../components/PurchasingManagement/SupplierTable';

const SupplierPage = () => {
  return (
    <MasterLayout>
      <div className="row">
        <div className="col-12">
          <div className="page-title-box d-flex align-items-center justify-content-between">
            <h4 className="mb-0">Tedarikçi Yönetimi</h4>
          </div>
        </div>
      </div>
      <div className="row">
        <SupplierTable />
      </div>
    </MasterLayout>
  );
};

export default SupplierPage; 