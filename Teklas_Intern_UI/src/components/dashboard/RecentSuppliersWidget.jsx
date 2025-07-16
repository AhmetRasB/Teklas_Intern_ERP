import React, { useEffect, useState } from "react";
import axios from "axios";

const RecentSuppliersWidget = () => {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get("https://localhost:7178/api/suppliers")
      .then(res => {
        const data = Array.isArray(res.data) ? res.data : [];
        setSuppliers(data.slice(-5).reverse());
      })
      .catch(() => setError("Tedarikçiler alınamadı!"))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="col-xxl-6 col-md-12">
      <div className="card h-100">
        <div className="card-body">
          <h6 className="fw-bold mb-3">Son Tedarikçiler</h6>
          {loading ? (
            <span>Yükleniyor...</span>
          ) : error ? (
            <span className="text-danger">{error}</span>
          ) : (
            <table className="table table-sm">
              <thead>
                <tr>
                  <th>Ad</th>
                  <th>Kod</th>
                  <th>Telefon</th>
                  <th>E-posta</th>
                </tr>
              </thead>
              <tbody>
                {suppliers.map(s => (
                  <tr key={s.id}>
                    <td>{s.name}</td>
                    <td>{s.code}</td>
                    <td>{s.phone}</td>
                    <td>{s.email}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecentSuppliersWidget; 