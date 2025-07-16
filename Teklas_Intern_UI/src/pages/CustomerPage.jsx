import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import CustomerTable from '../components/SalesManagement/CustomerTable';

const CustomerPage = () => {
  return (
    <MasterLayout>
      <div className="row">
        <div className="col-12">
          <div className="page-title-box d-flex align-items-center justify-content-between">
            <h4 className="mb-0">Müşteri Yönetimi</h4>
          </div>
        </div>
      </div>
      <div className="row">
        <CustomerTable />
      </div>
    </MasterLayout>
  );
};

export default CustomerPage; 