import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import StockEntryTable from '../components/WarehouseManagement/StockEntryTable';

const StockEntryPage = () => {
  return (
    <MasterLayout>
      <div className="row">
        <div className="col-12">
          <div className="page-title-box d-flex align-items-center justify-content-between">
            <h4 className="mb-0">Stok Girişleri Yönetimi</h4>
          </div>
        </div>
      </div>
      <div className="row">
        <StockEntryTable />
      </div>
    </MasterLayout>
  );
};

export default StockEntryPage; 