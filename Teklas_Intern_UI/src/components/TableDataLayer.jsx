import React, { useState, useMemo } from 'react';
import { Icon } from '@iconify/react';

const TableDataLayer = ({
  data,
  columns,
  onView = () => {},
  onEdit = () => {},
  onDelete = () => {},
  selectable = false,
  selectedRows = [],
  onSelectRow = () => {},
  onSelectAll = () => {},
  showActions = true,
  actions = ['view', 'edit', 'delete'],
  pageSizeOptions = [5, 10, 20, 50, 100],
  ...rest
}) => {
  // Local state for search, page, pageSize, sort
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [sortCol, setSortCol] = useState(null);
  const [sortDir, setSortDir] = useState('asc');

  // Search & sort & pagination
  const filteredData = useMemo(() => {
    let filtered = data;
    if (search) {
      filtered = filtered.filter(row =>
        columns.some(col =>
          String(row[col.accessor] ?? '')
            .toLowerCase()
            .includes(search.toLowerCase())
        )
      );
    }
    if (sortCol) {
      filtered = [...filtered].sort((a, b) => {
        const aVal = a[sortCol];
        const bVal = b[sortCol];
        if (aVal === bVal) return 0;
        if (sortDir === 'asc') return aVal > bVal ? 1 : -1;
        return aVal < bVal ? 1 : -1;
      });
    }
    return filtered;
  }, [data, search, columns, sortCol, sortDir]);

  const totalCount = filteredData.length;
  const pageCount = Math.ceil(totalCount / pageSize);
  const pagedData = filteredData.slice((page - 1) * pageSize, page * pageSize);

  // Sıralama ikonları
  const getSortIcon = col => {
    if (sortCol !== col) return <span style={{ opacity: 0.3, fontSize: 16, marginLeft: 4 }}>⇅</span>;
    return (
      <span style={{ marginLeft: 4 }}>
        <span style={{ color: sortDir === 'asc' ? '#3b82f6' : '#bbb', fontSize: 14, position: 'relative', top: -2 }}>▲</span>
        <span style={{ color: sortDir === 'desc' ? '#3b82f6' : '#bbb', fontSize: 14, position: 'relative', top: 2, left: -2 }}>▼</span>
      </span>
    );
  };

  // Alt bar için gösterim aralığı
  const from = totalCount === 0 ? 0 : (page - 1) * pageSize + 1;
  const to = Math.min(page * pageSize, totalCount);

  // isActive badge render fonksiyonu
  const renderBadge = val => (
    <span className={val ? "bg-success-focus text-success-main px-32 py-4 rounded-pill fw-medium text-sm" : "bg-lilac-100 text-lilac-600 px-32 py-4 rounded-pill fw-medium text-sm"}>
      {val ? "Aktif" : "Pasif"}
    </span>
  );

    return (
        <div className="card basic-data-table">
            <div className="card-body">
        {/* Üst bar */}
        <div className="d-flex justify-content-between align-items-center mb-3">
          <div>
            <select
              className="form-select d-inline-block w-auto"
              value={pageSize}
              onChange={e => { setPageSize(Number(e.target.value)); setPage(1); }}
            >
              {pageSizeOptions.map(size => (
                <option key={size} value={size}>{size}</option>
              ))}
            </select>
            <span className="ms-2">sayfa başı kayıt</span>
          </div>
          <div>
            <span className="me-2">Ara:</span>
            <input
              type="text"
              className="form-control d-inline-block w-auto"
              style={{ minWidth: 200 }}
              value={search}
              onChange={e => { setSearch(e.target.value); setPage(1); }}
              placeholder="Arama..."
            />
          </div>
        </div>
        {/* Tablo */}
        <div className="table-responsive" style={{overflowX: 'auto', maxWidth: '100%'}}>
          <table className="table bordered-table mb-0" style={{ minWidth: 900 }}>
                    <thead>
                        <tr>
                {selectable && (
                  <th>
                    <input
                      type="checkbox"
                      checked={pagedData.length > 0 && pagedData.every(row => selectedRows.includes(row.id || row.Id))}
                      onChange={e => onSelectAll(e.target.checked)}
                    />
                  </th>
                )}
                {columns.map((col, idx) => (
                  <th
                    key={idx}
                    style={{ cursor: 'pointer', whiteSpace: 'nowrap' }}
                    onClick={() => {
                      if (sortCol === col.accessor) {
                        setSortDir(sortDir === 'asc' ? 'desc' : 'asc');
                      } else {
                        setSortCol(col.accessor);
                        setSortDir('asc');
                      }
                    }}
                  >
                    {col.header}
                    {getSortIcon(col.accessor)}
                            </th>
                ))}
                {showActions && (
                  <th>
                    İşlem
                  </th>
                )}
                        </tr>
                    </thead>
                    <tbody>
              {pagedData.map((row, i) => (
                <tr key={row.id || row.Id || i}>
                  {selectable && (
                    <td>
                      <input
                        type="checkbox"
                        checked={selectedRows.includes(row.id || row.Id)}
                        onChange={e => onSelectRow(row, e.target.checked)}
                      />
                            </td>
                  )}
                  {columns.map((col, idx) => (
                    <td key={idx} style={{ whiteSpace: 'normal' }}>
                      {col.accessor === 'isActive'
                        ? renderBadge(row[col.accessor])
                        : col.render
                          ? col.render(row[col.accessor], row)
                          : String(row[col.accessor] ?? '')}
                            </td>
                  ))}
                  {showActions && (
                            <td>
                      {actions.includes('view') && (
                        <button
                                    className="w-32-px h-32-px me-8 bg-primary-light text-primary-600 rounded-circle d-inline-flex align-items-center justify-content-center"
                          onClick={() => onView(row)}
                          title="Görüntüle"
                                >
                                    <Icon icon="iconamoon:eye-light" />
                        </button>
                      )}
                      {actions.includes('edit') && (
                        <button
                                    className="w-32-px h-32-px me-8 bg-success-focus text-success-main rounded-circle d-inline-flex align-items-center justify-content-center"
                          onClick={() => onEdit(row)}
                          title="Düzenle"
                                >
                                    <Icon icon="lucide:edit" />
                        </button>
                      )}
                      {actions.includes('delete') && (
                        <button
                                    className="w-32-px h-32-px me-8 bg-danger-focus text-danger-main rounded-circle d-inline-flex align-items-center justify-content-center"
                          onClick={() => onDelete(row)}
                          title="Sil"
                                >
                          <Icon icon="ic:round-delete" />
                        </button>
                      )}
                      {actions.includes('restore') && (
                        <button
                          className="w-32-px h-32-px me-8 bg-success-focus text-success-main rounded-circle d-inline-flex align-items-center justify-content-center"
                          onClick={() => rest.onRestore(row)}
                          title="Geri Al"
                        >
                          <Icon icon="ri:reply-line" />
                        </button>
                      )}
                      {actions.includes('permanentDelete') && (
                        <button
                          className="w-32-px h-32-px me-8 bg-danger-focus text-danger-main rounded-circle d-inline-flex align-items-center justify-content-center"
                          onClick={() => rest.onPermanentDelete(row)}
                          title="Kalıcı Sil"
                        >
                          <Icon icon="ri:delete-bin-7-line" />
                        </button>
                      )}
                            </td>
                  )}
                        </tr>
              ))}
                    </tbody>
                </table>
            </div>
        {/* Alt bar */}
        <div className="d-flex justify-content-between align-items-center mt-3">
          <div>
            {`${from} - ${to} arası gösteriliyor, toplam ${totalCount} kayıt`}
          </div>
          <nav>
            <ul className="pagination d-flex flex-wrap align-items-center gap-2 justify-content-center mb-0">
              <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                <button className="page-link" onClick={() => setPage(1)} disabled={page === 1}>&laquo;</button>
              </li>
              <li className={`page-item ${page === 1 ? 'disabled' : ''}`}>
                <button className="page-link" onClick={() => setPage(page - 1)} disabled={page === 1}>&lsaquo;</button>
              </li>
              {Array.from({ length: pageCount }, (_, i) => i + 1).map(num => (
                <li key={num} className={`page-item ${page === num ? 'active' : ''}`}>
                  <button className="page-link" onClick={() => setPage(num)}>{num}</button>
                </li>
              ))}
              <li className={`page-item ${page === pageCount ? 'disabled' : ''}`}>
                <button className="page-link" onClick={() => setPage(page + 1)} disabled={page === pageCount}>&rsaquo;</button>
              </li>
              <li className={`page-item ${page === pageCount ? 'disabled' : ''}`}>
                <button className="page-link" onClick={() => setPage(pageCount)} disabled={page === pageCount}>&raquo;</button>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </div>
  );
};

export default TableDataLayer;