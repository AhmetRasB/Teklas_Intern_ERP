import React, { useEffect, useState } from "react";
import axios from "axios";

const TotalUsersWidget = () => {
  const [count, setCount] = useState(null);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios.get("https://localhost:7178/api/users?page=1&pageSize=5")
      .then(res => {
        const total = res.data?.pagination?.totalCount || res.data?.totalCount || 0;
        setCount(total);
        setUsers(res.data?.data || []);
      })
      .catch(() => setError("Kullanıcılar alınamadı!"))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="col-xxl-3 col-xl-4 col-sm-6">
      <div className="px-20 py-16 shadow-none radius-8 h-100 gradient-deep-1 left-line line-bg-primary position-relative overflow-hidden">
        <div className="d-flex flex-wrap align-items-center justify-content-between gap-1 mb-8">
          <div>
            <span className="mb-2 fw-medium text-secondary-light text-md">Toplam Kullanıcı</span>
            <h6 className="fw-semibold mb-1">{loading ? '...' : error ? <span className="text-danger">{error}</span> : count}</h6>
            {!loading && !error && users.length > 0 && (
              <ul className="mb-0 ps-3 text-sm text-secondary-light">
                {users.map(u => <li key={u.id}>{u.userName || u.name || u.fullName || u.email}</li>)}
              </ul>
            )}
          </div>
          <span className="w-44-px h-44-px radius-8 d-inline-flex justify-content-center align-items-center text-2xl mb-12 bg-primary-100 text-primary-600">
            <i className="ri-user-3-fill" />
          </span>
        </div>
      </div>
    </div>
  );
};

export default TotalUsersWidget; 