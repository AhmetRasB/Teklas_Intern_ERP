import React, { useEffect, useState } from "react";
import axios from "axios";

const TotalSuppliersWidget = () => {
  const [count, setCount] = useState(null);
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get("https://localhost:7178/api/suppliers")
      .then(res => {
        const data = Array.isArray(res.data) ? res.data : [];
        setCount(data.length);
        setSuppliers(data.slice(0, 5));
      })
      .catch(() => setError("Tedarikçiler alınamadı!"))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="col-xxl-3 col-xl-4 col-sm-6">
      <div className="px-20 py-16 shadow-none radius-8 h-100 gradient-deep-4 left-line line-bg-warning position-relative overflow-hidden">
        <div className="d-flex flex-wrap align-items-center justify-content-between gap-1 mb-8">
          <div>
            <span className="mb-2 fw-medium text-secondary-light text-md">Toplam Tedarikçi</span>
            <h6 className="fw-semibold mb-1">{loading ? '...' : error ? <span className="text-danger">{error}</span> : count}</h6>
            {!loading && !error && suppliers.length > 0 && (
              <ul className="mb-0 ps-3 text-sm text-secondary-light">
                {suppliers.map(s => <li key={s.id}>{s.supplierName || s.name}</li>)}
              </ul>
            )}
          </div>
          <span className="w-44-px h-44-px radius-8 d-inline-flex justify-content-center align-items-center text-2xl mb-12 bg-warning-focus text-warning-600">
            <i className="ri-truck-fill" />
          </span>
        </div>
      </div>
    </div>
  );
};

export default TotalSuppliersWidget; 