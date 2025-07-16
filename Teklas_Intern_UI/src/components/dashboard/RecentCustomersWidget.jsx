import React, { useEffect, useState } from "react";
import axios from "axios";

const RecentCustomersWidget = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get("https://localhost:7178/api/customers")
      .then(res => {
        const data = Array.isArray(res.data) ? res.data : [];
        setCustomers(data.slice(-5).reverse());
      })
      .catch(() => setError("Müşteriler alınamadı!"))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="col-xxl-6 col-md-12">
      <div className="card h-100">
        <div className="card-body">
          <h6 className="fw-bold mb-3">Son Müşteriler</h6>
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
                {customers.map(c => (
                  <tr key={c.id}>
                    <td>{c.name}</td>
                    <td>{c.code}</td>
                    <td>{c.phone}</td>
                    <td>{c.email}</td>
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

export default RecentCustomersWidget; 