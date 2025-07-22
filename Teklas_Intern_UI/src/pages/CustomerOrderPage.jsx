import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import CustomerOrderTable from '../components/SalesManagement/CustomerOrderTable';

const CustomerOrderPage = () => {
  return (
    <MasterLayout>
      <CustomerOrderTable />
    </MasterLayout>
  );
};

export default CustomerOrderPage; 