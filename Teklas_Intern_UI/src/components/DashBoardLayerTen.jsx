import React from "react";
import TotalUsersWidget from "./dashboard/TotalUsersWidget";
import TotalWarehousesWidget from "./dashboard/TotalWarehousesWidget";
import TotalCustomersWidget from "./dashboard/TotalCustomersWidget";
import TotalSuppliersWidget from "./dashboard/TotalSuppliersWidget";
import TotalStockEntriesWidget from "./dashboard/TotalStockEntriesWidget";
import RecentStockEntriesWidget from "./dashboard/RecentStockEntriesWidget";
import RecentCustomersWidget from "./dashboard/RecentCustomersWidget";
import RecentSuppliersWidget from "./dashboard/RecentSuppliersWidget";

const DashBoardLayerTen = () => {
  return (
    <div className='row gy-4'>
      <TotalUsersWidget />
      <TotalWarehousesWidget />
      <TotalCustomersWidget />
      <TotalSuppliersWidget />
      <TotalStockEntriesWidget />
      <RecentStockEntriesWidget />
      <RecentCustomersWidget />
      <RecentSuppliersWidget />
    </div>
  );
};

export default DashBoardLayerTen;
