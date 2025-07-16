import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import WorkOrderTable from '../components/ProductionManagement/WorkOrderTable';

const WorkOrderPage = () => {
  return (
    <MasterLayout>
      <WorkOrderTable />
    </MasterLayout>
  );
};

export default WorkOrderPage; 