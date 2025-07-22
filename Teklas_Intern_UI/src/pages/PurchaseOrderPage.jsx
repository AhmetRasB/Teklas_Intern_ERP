import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import PurchaseOrderTable from '../components/PurchasingManagement/PurchaseOrderTable';

const PurchaseOrderPage = () => {
  return (
    <MasterLayout>
      <PurchaseOrderTable />
    </MasterLayout>
  );
};

export default PurchaseOrderPage; 