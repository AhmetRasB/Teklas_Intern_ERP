import React from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
import MaterialCardEnhanced from '../components/MaterialManagement/MaterialCardEnhanced';

const MaterialCardEnhancedPage = () => {
  return (
    <MasterLayout>
      <div className="container-fluid">
        <div className="row">
          <div className="col-12">
            <div className="page-title-box d-sm-flex align-items-center justify-content-between mb-3">
              <h4 className="mb-sm-0">Material Management</h4>
              <div className="page-title-right">
                <ol className="breadcrumb m-0">
                  <li className="breadcrumb-item">
                    <a href="#!">ERP</a>
                  </li>
                  <li className="breadcrumb-item">
                    <a href="#!">Material Management</a>
                  </li>
                  <li className="breadcrumb-item active">Materials</li>
                </ol>
              </div>
            </div>
          </div>
        </div>
        
        <div className="row">
          <div className="col-12">
            <MaterialCardEnhanced />
          </div>
        </div>
      </div>
    </MasterLayout>
  );
};

export default MaterialCardEnhancedPage; 