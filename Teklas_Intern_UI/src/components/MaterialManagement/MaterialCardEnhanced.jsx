import React, { useEffect, useState, useMemo, useCallback } from 'react';
import axios from 'axios';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import { Icon } from '@iconify/react';
import { useNavigate } from 'react-router-dom';
import MaterialCardModal from './MaterialCardModal';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const MaterialCardEnhanced = () => {
  // States
  const [materials, setMaterials] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  // Modal states
  const [showModal, setShowModal] = useState(false);
  const [editingMaterial, setEditingMaterial] = useState(null);
  
  // Filtering & Search states
  const [searchTerm, setSearchTerm] = useState('');
  const [filters, setFilters] = useState({
    categoryCode: '',
    materialType: '',
    brand: '',
    minPrice: '',
    maxPrice: '',
    isDiscontinued: '',
  });
  
  // Pagination states
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(20);
  const [totalCount, setTotalCount] = useState(0);
  const [sortBy, setSortBy] = useState('name');
  const [sortDirection, setSortDirection] = useState('asc');
  
  // Selection states
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectAll, setSelectAll] = useState(false);
  
  // View states
  const [viewMode, setViewMode] = useState('table'); // table, grid, list
  const [showDeleted, setShowDeleted] = useState(false);
  
  const navigate = useNavigate();

  // API Functions
  const fetchMaterials = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const params = {
        page: currentPage,
        pageSize,
        sortBy,
        ascending: sortDirection === 'asc',
        ...filters,
        searchTerm: searchTerm || undefined
      };
      
      // Remove empty parameters
      Object.keys(params).forEach(key => {
        if (params[key] === '' || params[key] === undefined || params[key] === null) {
          delete params[key];
        }
      });
      
      const endpoint = showDeleted 
        ? `${BASE_URL}/api/v1/materials?deleted=true`
        : Object.keys(params).length > 3 
          ? `${BASE_URL}/api/v1/materials/filter`
          : `${BASE_URL}/api/v1/materials/paged`;
      
      const response = await axios.get(endpoint, { params });
      
      if (showDeleted) {
        setMaterials(response.data);
        setTotalCount(response.data.length);
      } else {
        setMaterials(response.data.items || response.data);
        setTotalCount(response.data.totalCount || response.data.length);
      }
    } catch (err) {
      setError('Failed to fetch materials: ' + (err.response?.data?.error || err.message));
      setMaterials([]);
      setTotalCount(0);
    } finally {
      setLoading(false);
    }
  }, [currentPage, pageSize, sortBy, sortDirection, filters, searchTerm, showDeleted]);

  const fetchCategories = useCallback(async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/v1/categories`);
      setCategories(response.data);
    } catch (err) {
      console.error('Failed to fetch categories:', err);
      setCategories([]);
    }
  }, []);

  // Effects
  useEffect(() => {
    fetchCategories();
  }, [fetchCategories]);

  useEffect(() => {
    fetchMaterials();
  }, [fetchMaterials]);

  // Debounced search
  useEffect(() => {
    const timeoutId = setTimeout(() => {
      if (currentPage !== 1) {
        setCurrentPage(1);
      } else {
        fetchMaterials();
      }
    }, 500);

    return () => clearTimeout(timeoutId);
  }, [searchTerm]);

  // Filter computed values
  const materialTypes = useMemo(() => {
    const types = [...new Set(materials.map(m => m.materialType).filter(Boolean))];
    return types.sort();
  }, [materials]);

  const brands = useMemo(() => {
    const brands = [...new Set(materials.map(m => m.brand).filter(Boolean))];
    return brands.sort();
  }, [materials]);

  // Handler functions
  const handleSearch = (value) => {
    setSearchTerm(value);
  };

  const handleFilterChange = (key, value) => {
    setFilters(prev => ({
      ...prev,
      [key]: value
    }));
    setCurrentPage(1);
  };

  const clearFilters = () => {
    setFilters({
      categoryCode: '',
      materialType: '',
      brand: '',
      minPrice: '',
      maxPrice: '',
      isDiscontinued: '',
    });
    setSearchTerm('');
    setCurrentPage(1);
  };

  const handleSort = (column) => {
    if (sortBy === column) {
      setSortDirection(prev => prev === 'asc' ? 'desc' : 'asc');
    } else {
      setSortBy(column);
      setSortDirection('asc');
    }
    setCurrentPage(1);
  };

  const handleSelectItem = (id) => {
    setSelectedItems(prev => 
      prev.includes(id) 
        ? prev.filter(item => item !== id)
        : [...prev, id]
    );
  };

  const handleSelectAll = () => {
    if (selectAll) {
      setSelectedItems([]);
    } else {
      setSelectedItems(materials.map(m => m.id));
    }
    setSelectAll(!selectAll);
  };

  const handleEdit = (material) => {
    setEditingMaterial(material);
    setShowModal(true);
  };

  const handleCreate = () => {
    setEditingMaterial(null);
    setShowModal(true);
  };

  const handleDelete = async (material) => {
    const result = await MySwal.fire({
      title: 'Are you sure?',
      text: `Delete material "${material.name}"?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/v1/materials/${material.id}`);
        await fetchMaterials();
        MySwal.fire('Deleted!', 'Material has been deleted.', 'success');
      } catch (err) {
        MySwal.fire('Error!', 'Failed to delete material.', 'error');
      }
    }
  };

  const handleBulkDelete = async () => {
    if (selectedItems.length === 0) return;

    const result = await MySwal.fire({
      title: 'Delete Multiple Materials?',
      text: `Delete ${selectedItems.length} selected materials?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete them!',
      cancelButtonText: 'Cancel'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/v1/materials/bulk`, {
          data: selectedItems
        });
        await fetchMaterials();
        setSelectedItems([]);
        setSelectAll(false);
        MySwal.fire('Deleted!', `${selectedItems.length} materials have been deleted.`, 'success');
      } catch (err) {
        MySwal.fire('Error!', 'Failed to delete materials.', 'error');
      }
    }
  };

  const handleRestore = async (material) => {
    try {
      await axios.put(`${BASE_URL}/api/v1/materials/${material.id}/restore`);
      await fetchMaterials();
      MySwal.fire('Restored!', 'Material has been restored.', 'success');
    } catch (err) {
      MySwal.fire('Error!', 'Failed to restore material.', 'error');
    }
  };

  const handleExport = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/v1/materials/export`, {
        responseType: 'blob'
      });
      
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `materials-${new Date().toISOString().split('T')[0]}.xlsx`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch (err) {
      MySwal.fire('Error!', 'Failed to export materials.', 'error');
    }
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleModalClose = () => {
    setShowModal(false);
    setEditingMaterial(null);
  };

  const handleModalSuccess = () => {
    setShowModal(false);
    setEditingMaterial(null);
    fetchMaterials();
  };

  // Computed values
  const totalPages = Math.ceil(totalCount / pageSize);
  const startIndex = (currentPage - 1) * pageSize + 1;
  const endIndex = Math.min(currentPage * pageSize, totalCount);

  return (
    <div className="card">
      <div className="card-header border-bottom">
        <div className="row align-items-center">
          <div className="col">
            <h4 className="card-title mb-0">
              <Icon icon="mdi:package-variant" className="me-2" />
              Material Management
              {showDeleted && <span className="badge bg-warning ms-2">Deleted Items</span>}
            </h4>
          </div>
          <div className="col-auto">
            <div className="btn-group" role="group">
              <button
                type="button"
                className={`btn ${!showDeleted ? 'btn-primary' : 'btn-outline-primary'}`}
                onClick={() => setShowDeleted(false)}
              >
                Active
              </button>
              <button
                type="button"
                className={`btn ${showDeleted ? 'btn-warning' : 'btn-outline-warning'}`}
                onClick={() => setShowDeleted(true)}
              >
                Deleted
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="card-body">
        {/* Search and Filter Bar */}
        <div className="row mb-4">
          <div className="col-lg-4">
            <div className="input-group">
              <span className="input-group-text">
                <Icon icon="mdi:magnify" />
              </span>
              <input
                type="text"
                className="form-control"
                placeholder="Search materials..."
                value={searchTerm}
                onChange={(e) => handleSearch(e.target.value)}
              />
            </div>
          </div>
          <div className="col-lg-8">
            <div className="row g-2">
              <div className="col-md-3">
                <select
                  className="form-select"
                  value={filters.categoryCode}
                  onChange={(e) => handleFilterChange('categoryCode', e.target.value)}
                >
                  <option value="">All Categories</option>
                  {categories.map(cat => (
                    <option key={cat.id} value={cat.code}>{cat.name}</option>
                  ))}
                </select>
              </div>
              <div className="col-md-3">
                <select
                  className="form-select"
                  value={filters.materialType}
                  onChange={(e) => handleFilterChange('materialType', e.target.value)}
                >
                  <option value="">All Types</option>
                  {materialTypes.map(type => (
                    <option key={type} value={type}>{type}</option>
                  ))}
                </select>
              </div>
              <div className="col-md-3">
                <select
                  className="form-select"
                  value={filters.brand}
                  onChange={(e) => handleFilterChange('brand', e.target.value)}
                >
                  <option value="">All Brands</option>
                  {brands.map(brand => (
                    <option key={brand} value={brand}>{brand}</option>
                  ))}
                </select>
              </div>
              <div className="col-md-3">
                <button
                  type="button"
                  className="btn btn-outline-secondary w-100"
                  onClick={clearFilters}
                >
                  <Icon icon="mdi:filter-off" className="me-1" />
                  Clear
                </button>
              </div>
            </div>
          </div>
        </div>

        {/* Action Bar */}
        <div className="row mb-3">
          <div className="col">
            <div className="d-flex align-items-center gap-2">
              {!showDeleted && (
                <>
                  <button
                    type="button"
                    className="btn btn-primary"
                    onClick={handleCreate}
                  >
                    <Icon icon="mdi:plus" className="me-1" />
                    Add Material
                  </button>
                  {selectedItems.length > 0 && (
                    <button
                      type="button"
                      className="btn btn-danger"
                      onClick={handleBulkDelete}
                    >
                      <Icon icon="mdi:delete" className="me-1" />
                      Delete Selected ({selectedItems.length})
                    </button>
                  )}
                </>
              )}
              <button
                type="button"
                className="btn btn-outline-success"
                onClick={handleExport}
              >
                <Icon icon="mdi:file-excel" className="me-1" />
                Export
              </button>
            </div>
          </div>
          <div className="col-auto">
            <div className="d-flex align-items-center gap-2">
              <span className="text-muted">
                Showing {startIndex}-{endIndex} of {totalCount}
              </span>
              <select
                className="form-select form-select-sm"
                style={{ width: 'auto' }}
                value={pageSize}
                onChange={(e) => {
                  setPageSize(Number(e.target.value));
                  setCurrentPage(1);
                }}
              >
                <option value={10}>10</option>
                <option value={20}>20</option>
                <option value={50}>50</option>
                <option value={100}>100</option>
              </select>
            </div>
          </div>
        </div>

        {/* Error Display */}
        {error && (
          <div className="alert alert-danger" role="alert">
            <Icon icon="mdi:alert-circle" className="me-2" />
            {error}
          </div>
        )}

        {/* Loading */}
        {loading && (
          <div className="text-center py-4">
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
          </div>
        )}

        {/* Table */}
        {!loading && materials.length > 0 && (
          <>
            <div className="table-responsive">
              <table className="table table-hover">
                <thead className="table-light">
                  <tr>
                    <th style={{ width: '40px' }}>
                      {!showDeleted && (
                        <input
                          type="checkbox"
                          className="form-check-input"
                          checked={selectAll}
                          onChange={handleSelectAll}
                        />
                      )}
                    </th>
                    <th 
                      style={{ cursor: 'pointer' }}
                      onClick={() => handleSort('code')}
                    >
                      Code
                      {sortBy === 'code' && (
                        <Icon 
                          icon={sortDirection === 'asc' ? 'mdi:arrow-up' : 'mdi:arrow-down'} 
                          className="ms-1" 
                        />
                      )}
                    </th>
                    <th 
                      style={{ cursor: 'pointer' }}
                      onClick={() => handleSort('name')}
                    >
                      Name
                      {sortBy === 'name' && (
                        <Icon 
                          icon={sortDirection === 'asc' ? 'mdi:arrow-up' : 'mdi:arrow-down'} 
                          className="ms-1" 
                        />
                      )}
                    </th>
                    <th>Category</th>
                    <th>Type</th>
                    <th>Brand</th>
                    <th className="text-end">Price</th>
                    <th>Status</th>
                    <th style={{ width: '120px' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {materials.map((material) => (
                    <tr key={material.id}>
                      <td>
                        {!showDeleted && (
                          <input
                            type="checkbox"
                            className="form-check-input"
                            checked={selectedItems.includes(material.id)}
                            onChange={() => handleSelectItem(material.id)}
                          />
                        )}
                      </td>
                      <td>
                        <code className="text-primary">{material.code}</code>
                      </td>
                      <td>
                        <div>
                          <div className="fw-semibold">{material.name}</div>
                          {material.description && (
                            <small className="text-muted">
                              {material.description.length > 50 
                                ? `${material.description.substring(0, 50)}...`
                                : material.description
                              }
                            </small>
                          )}
                        </div>
                      </td>
                      <td>
                        <span className="badge bg-light text-dark">
                          {material.category?.name || 'N/A'}
                        </span>
                      </td>
                      <td>{material.materialType || 'N/A'}</td>
                      <td>{material.brand || 'N/A'}</td>
                      <td className="text-end">
                        {material.standardSalesPrice ? (
                          <span className="fw-semibold text-success">
                            ${material.standardSalesPrice.toFixed(2)}
                          </span>
                        ) : (
                          <span className="text-muted">-</span>
                        )}
                      </td>
                      <td>
                        {showDeleted ? (
                          <span className="badge bg-danger">Deleted</span>
                        ) : material.isDiscontinued ? (
                          <span className="badge bg-warning">Discontinued</span>
                        ) : (
                          <span className="badge bg-success">Active</span>
                        )}
                      </td>
                      <td>
                        <div className="btn-group btn-group-sm">
                          {showDeleted ? (
                            <button
                              type="button"
                              className="btn btn-outline-success"
                              onClick={() => handleRestore(material)}
                              title="Restore"
                            >
                              <Icon icon="mdi:restore" />
                            </button>
                          ) : (
                            <>
                              <button
                                type="button"
                                className="btn btn-outline-primary"
                                onClick={() => handleEdit(material)}
                                title="Edit"
                              >
                                <Icon icon="mdi:pencil" />
                              </button>
                              <button
                                type="button"
                                className="btn btn-outline-danger"
                                onClick={() => handleDelete(material)}
                                title="Delete"
                              >
                                <Icon icon="mdi:delete" />
                              </button>
                            </>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Pagination */}
            {totalPages > 1 && (
              <nav className="mt-4">
                <ul className="pagination justify-content-center">
                  <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
                    <button
                      className="page-link"
                      onClick={() => handlePageChange(currentPage - 1)}
                      disabled={currentPage === 1}
                    >
                      <Icon icon="mdi:chevron-left" />
                    </button>
                  </li>
                  
                  {[...Array(totalPages)].map((_, index) => {
                    const page = index + 1;
                    const showPage = page === 1 || page === totalPages || 
                                   (page >= currentPage - 2 && page <= currentPage + 2);
                    
                    if (!showPage) {
                      if (page === currentPage - 3 || page === currentPage + 3) {
                        return (
                          <li key={page} className="page-item disabled">
                            <span className="page-link">...</span>
                          </li>
                        );
                      }
                      return null;
                    }
                    
                    return (
                      <li key={page} className={`page-item ${currentPage === page ? 'active' : ''}`}>
                        <button
                          className="page-link"
                          onClick={() => handlePageChange(page)}
                        >
                          {page}
                        </button>
                      </li>
                    );
                  })}
                  
                  <li className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
                    <button
                      className="page-link"
                      onClick={() => handlePageChange(currentPage + 1)}
                      disabled={currentPage === totalPages}
                    >
                      <Icon icon="mdi:chevron-right" />
                    </button>
                  </li>
                </ul>
              </nav>
            )}
          </>
        )}

        {/* Empty State */}
        {!loading && materials.length === 0 && (
          <div className="text-center py-5">
            <Icon icon="mdi:package-variant-closed" size={64} className="text-muted mb-3" />
            <h5 className="text-muted">No materials found</h5>
            <p className="text-muted mb-3">
              {searchTerm || Object.values(filters).some(f => f) 
                ? "Try adjusting your search or filters."
                : "Get started by adding your first material."
              }
            </p>
            {!showDeleted && (
              <button
                type="button"
                className="btn btn-primary"
                onClick={handleCreate}
              >
                <Icon icon="mdi:plus" className="me-1" />
                Add Material
              </button>
            )}
          </div>
        )}
      </div>

      {/* Modal */}
      {showModal && (
        <MaterialCardModal
          isOpen={showModal}
          onClose={handleModalClose}
          onSuccess={handleModalSuccess}
          editingItem={editingMaterial}
          categories={categories}
        />
      )}
    </div>
  );
};

export default MaterialCardEnhanced; 