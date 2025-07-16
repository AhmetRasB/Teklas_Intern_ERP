import React, { useEffect, useState } from "react";
import axios from "axios";

const RecentStockEntriesWidget = () => {
  const [entries, setEntries] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get("https://localhost:7178/api/stockentries")
      .then(res => {
        const data = Array.isArray(res.data) ? res.data : [];
        setEntries(data.slice(-5).reverse());
      })
      .catch(() => setError("Stok girişleri alınamadı!"))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="col-xxl-6 col-md-12">
      <div className="card h-100">
        <div className="card-body">
          <h6 className="fw-bold mb-3">Son Stok Girişleri</h6>
          {loading ? (
            <span>Yükleniyor...</span>
          ) : error ? (
            <span className="text-danger">{error}</span>
          ) : (
            <table className="table table-sm">
              <thead>
                <tr>
                  <th>Giriş No</th>
                  <th>Tarih</th>
                  <th>Depo</th>
                  <th>Malzeme</th>
                  <th>Miktar</th>
                </tr>
              </thead>
              <tbody>
                {entries.map(e => (
                  <tr key={e.id}>
                    <td>{e.entryNumber}</td>
                    <td>{e.entryDate ? new Date(e.entryDate).toLocaleDateString('tr-TR') : ''}</td>
                    <td>{e.warehouseName}</td>
                    <td>{e.materialName}</td>
                    <td>{e.quantity}</td>
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

export default RecentStockEntriesWidget; 