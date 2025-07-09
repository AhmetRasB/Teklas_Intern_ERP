import React, { useEffect, useState, useMemo, useCallback } from 'react';
import axios from 'axios';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import { Icon } from '@iconify/react';
import MaterialCategoryModal from './MaterialCategoryModal';

const BASE_URL = 'https://localhost:7178';
const MySwal = withReactContent(Swal);

const MaterialCategoryEnhanced = () => {
  // States
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  
  // Modal states
  const [showModal, setShowModal] = useState(false);
  const [editingCategory, setEditingCategory] = useState(null);
  
  // View states
  const [viewMode, setViewMode] = useState('tree'); // tree, table, grid
  const [showDeleted, setShowDeleted] = useState(false);
  const [expandedNodes, setExpandedNodes] = useState(new Set());
  
  // Search and filter states
  const [searchTerm, setSearchTerm] = useState('');
  const [filters, setFilters] = useState({
    level: '',
    manager: '',
    costCenter: '',
    hasChildren: '',
    hasMaterials: ''
  });
  
  // Selection states
  const [selectedItems, setSelectedItems] = useState([]);
  const [selectAll, setSelectAll] = useState(false);

  // API Functions
  const fetchCategories = useCallback(async () => {
    setLoading(true);
    setError(null);
    
    try {
      const endpoint = showDeleted 
        ? `${BASE_URL}/api/v1/categories?deleted=true`
        : viewMode === 'tree'
          ? `${BASE_URL}/api/v1/categories/tree`
          : `${BASE_URL}/api/v1/categories/with-material-counts`;
      
      const response = await axios.get(endpoint);
      setCategories(response.data);
    } catch (err) {
      setError('Failed to fetch categories: ' + (err.response?.data?.error || err.message));
      setCategories([]);
    } finally {
      setLoading(false);
    }
  }, [showDeleted, viewMode]);

  // Effects
  useEffect(() => {
    fetchCategories();
  }, [fetchCategories]);

  // Debounced search
  useEffect(() => {
    const timeoutId = setTimeout(() => {
      // Search logic here if needed
    }, 500);

    return () => clearTimeout(timeoutId);
  }, [searchTerm]);

  // Computed values
  const filteredCategories = useMemo(() => {
    let filtered = categories;

    // Search filter
    if (searchTerm) {
      const search = searchTerm.toLowerCase();
      filtered = filtered.filter(cat => 
        cat.name?.toLowerCase().includes(search) ||
        cat.code?.toLowerCase().includes(search) ||
        cat.description?.toLowerCase().includes(search)
      );
    }

    // Other filters
    if (filters.manager) {
      filtered = filtered.filter(cat => cat.managerName === filters.manager);
    }
    
    if (filters.costCenter) {
      filtered = filtered.filter(cat => cat.costCenter === filters.costCenter);
    }
    
    if (filters.hasChildren !== '') {
      const hasChildren = filters.hasChildren === 'true';
      filtered = filtered.filter(cat => 
        hasChildren ? (cat.subCategories && cat.subCategories.length > 0) : (!cat.subCategories || cat.subCategories.length === 0)
      );
    }
    
    if (filters.hasMaterials !== '') {
      const hasMaterials = filters.hasMaterials === 'true';
      filtered = filtered.filter(cat => 
        hasMaterials ? (cat.materialCount > 0) : (cat.materialCount === 0)
      );
    }

    return filtered;
  }, [categories, searchTerm, filters]);

  // Helper functions
  const buildCategoryTree = useCallback((cats, parentId = null) => {
    return cats
      .filter(cat => cat.parentCategoryId === parentId)
      .map(cat => ({
        ...cat,
        children: buildCategoryTree(cats, cat.id)
      }));
  }, []);

  const flattenTree = useCallback((tree) => {
    const result = [];
    const traverse = (nodes, level = 0) => {
      nodes.forEach(node => {
        result.push({ ...node, level });
        if (node.children && expandedNodes.has(node.id)) {
          traverse(node.children, level + 1);
        }
      });
    };
    traverse(tree);
    return result;
  }, [expandedNodes]);

  // Handler functions
  const handleSearch = (value) => {
    setSearchTerm(value);
  };

  const handleFilterChange = (key, value) => {
    setFilters(prev => ({
      ...prev,
      [key]: value
    }));
  };

  const clearFilters = () => {
    setFilters({
      level: '',
      manager: '',
      costCenter: '',
      hasChildren: '',
      hasMaterials: ''
    });
    setSearchTerm('');
  };

  const handleNodeToggle = (nodeId) => {
    setExpandedNodes(prev => {
      const newSet = new Set(prev);
      if (newSet.has(nodeId)) {
        newSet.delete(nodeId);
      } else {
        newSet.add(nodeId);
      }
      return newSet;
    });
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
      setSelectedItems(filteredCategories.map(c => c.id));
    }
    setSelectAll(!selectAll);
  };

  const handleCreate = (parentId = null) => {
    setEditingCategory(parentId ? { parentCategoryId: parentId } : null);
    setShowModal(true);
  };

  const handleEdit = (category) => {
    setEditingCategory(category);
    setShowModal(true);
  };

  const handleDelete = async (category) => {
    const result = await MySwal.fire({
      title: 'Are you sure?',
      text: `Delete category "${category.name}"?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/v1/categories/${category.id}`);
        await fetchCategories();
        MySwal.fire('Deleted!', 'Category has been deleted.', 'success');
      } catch (err) {
        const errorMsg = err.response?.data?.details || 'Failed to delete category.';
        MySwal.fire('Error!', errorMsg, 'error');
      }
    }
  };

  const handleBulkDelete = async () => {
    if (selectedItems.length === 0) return;

    const result = await MySwal.fire({
      title: 'Delete Multiple Categories?',
      text: `Delete ${selectedItems.length} selected categories?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Yes, delete them!',
      cancelButtonText: 'Cancel'
    });

    if (result.isConfirmed) {
      try {
        await axios.delete(`${BASE_URL}/api/v1/categories/bulk`, {
          data: selectedItems
        });
        await fetchCategories();
        setSelectedItems([]);
        setSelectAll(false);
        MySwal.fire('Deleted!', `${selectedItems.length} categories have been deleted.`, 'success');
      } catch (err) {
        MySwal.fire('Error!', 'Failed to delete categories.', 'error');
      }
    }
  };

  const handleRestore = async (category) => {
    try {
      await axios.put(`${BASE_URL}/api/v1/categories/${category.id}/restore`);
      await fetchCategories();
      MySwal.fire('Restored!', 'Category has been restored.', 'success');
    } catch (err) {
      MySwal.fire('Error!', 'Failed to restore category.', 'error');
    }
  };

  const handleMove = async (categoryId, newParentId) => {
    try {
      await axios.put(`${BASE_URL}/api/v1/categories/${categoryId}/move?newParentId=${newParentId || ''}`);
      await fetchCategories();
      MySwal.fire('Moved!', 'Category has been moved successfully.', 'success');
    } catch (err) {
      const errorMsg = err.response?.data?.details || 'Failed to move category.';
      MySwal.fire('Error!', errorMsg, 'error');
    }
  };

  const handleExport = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/v1/categories/export`, {
        responseType: 'blob'
      });
      
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `categories-${new Date().toISOString().split('T')[0]}.xlsx`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch (err) {
      MySwal.fire('Error!', 'Failed to export categories.', 'error');
    }
  };

  const handleModalClose = () => {
    setShowModal(false);
    setEditingCategory(null);
  };

  const handleModalSuccess = () => {
    setShowModal(false);
    setEditingCategory(null);
    fetchCategories();
  };

  // Tree View Component
  const renderTreeNode = (node, level = 0) => {
    const hasChildren = node.children && node.children.length > 0;
    const isExpanded = expandedNodes.has(node.id);
    const isSelected = selectedItems.includes(node.id);

    return (
      <div key={node.id}>
        <div 
          className={`d-flex align-items-center py-2 px-2 border-bottom ${isSelected ? 'bg-light' : ''}`}
          style={{ paddingLeft: `${level * 20 + 10}px` }}
        >
          <div className="flex-shrink-0 me-2" style={{ width: '20px' }}>
            {hasChildren ? (
              <button
                type="button"
                className="btn btn-sm p-0 border-0"
                onClick={() => handleNodeToggle(node.id)}
              >
                <Icon 
                  icon={isExpanded ? 'mdi:chevron-down' : 'mdi:chevron-right'} 
                  size={16}
                />
              </button>
            ) : (
              <span style={{ width: '16px', display: 'inline-block' }}></span>
            )}
          </div>
          
          <div className="flex-shrink-0 me-2">
            {!showDeleted && (
              <input
                type="checkbox"
                className="form-check-input"
                checked={isSelected}
                onChange={() => handleSelectItem(node.id)}
              />
            )}
          </div>
          
          <div className="flex-grow-1">
            <div className="d-flex align-items-center">
              <Icon 
                icon={hasChildren ? 'mdi:folder' : 'mdi:folder-outline'} 
                className="me-2 text-primary" 
              />
              <div className="flex-grow-1">
                <div className="fw-semibold">{node.name}</div>
                <small className="text-muted">
                  {node.code} {node.materialCount > 0 && `• ${node.materialCount} materials`}
                </small>
              </div>
            </div>
          </div>
          
          <div className="flex-shrink-0">
            <div className="btn-group btn-group-sm">
              {showDeleted ? (
                <button
                  type="button"
                  className="btn btn-outline-success btn-sm"
                  onClick={() => handleRestore(node)}
                  title="Restore"
                >
                  <Icon icon="mdi:restore" />
                </button>
              ) : (
                <>
                  <button
                    type="button"
                    className="btn btn-outline-secondary btn-sm"
                    onClick={() => handleCreate(node.id)}
                    title="Add Subcategory"
                  >
                    <Icon icon="mdi:plus" />
                  </button>
                  <button
                    type="button"
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => handleEdit(node)}
                    title="Edit"
                  >
                    <Icon icon="mdi:pencil" />
                  </button>
                  <button
                    type="button"
                    className="btn btn-outline-danger btn-sm"
                    onClick={() => handleDelete(node)}
                    title="Delete"
                  >
                    <Icon icon="mdi:delete" />
                  </button>
                </>
              )}
            </div>
          </div>
        </div>
        
        {hasChildren && isExpanded && node.children.map(child => 
          renderTreeNode(child, level + 1)
        )}
      </div>
    );
  };

  // Table View Component
  const renderTableView = () => (
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
            <th>Code</th>
            <th>Name</th>
            <th>Parent</th>
            <th>Level</th>
            <th>Materials</th>
            <th>Manager</th>
            <th>Status</th>
            <th style={{ width: '120px' }}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {filteredCategories.map((category) => (
            <tr key={category.id}>
              <td>
                {!showDeleted && (
                  <input
                    type="checkbox"
                    className="form-check-input"
                    checked={selectedItems.includes(category.id)}
                    onChange={() => handleSelectItem(category.id)}
                  />
                )}
              </td>
              <td>
                <code className="text-primary">{category.code}</code>
              </td>
              <td>
                <div className="d-flex align-items-center">
                  <Icon 
                    icon="mdi:folder" 
                    className="me-2 text-primary" 
                  />
                  <div>
                    <div className="fw-semibold">{category.name}</div>
                    {category.description && (
                      <small className="text-muted">
                        {category.description.length > 50 
                          ? `${category.description.substring(0, 50)}...`
                          : category.description
                        }
                      </small>
                    )}
                  </div>
                </div>
              </td>
              <td>
                {category.parentCategory ? (
                  <span className="badge bg-light text-dark">
                    {category.parentCategory.name}
                  </span>
                ) : (
                  <span className="text-muted">Root</span>
                )}
              </td>
              <td>
                <span className="badge bg-info">
                  {category.categoryLevel || 'L1'}
                </span>
              </td>
              <td>
                <span className="badge bg-success">
                  {category.materialCount || 0}
                </span>
              </td>
              <td>{category.managerName || '-'}</td>
              <td>
                {showDeleted ? (
                  <span className="badge bg-danger">Deleted</span>
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
                      onClick={() => handleRestore(category)}
                      title="Restore"
                    >
                      <Icon icon="mdi:restore" />
                    </button>
                  ) : (
                    <>
                      <button
                        type="button"
                        className="btn btn-outline-secondary"
                        onClick={() => handleCreate(category.id)}
                        title="Add Subcategory"
                      >
                        <Icon icon="mdi:plus" />
                      </button>
                      <button
                        type="button"
                        className="btn btn-outline-primary"
                        onClick={() => handleEdit(category)}
                        title="Edit"
                      >
                        <Icon icon="mdi:pencil" />
                      </button>
                      <button
                        type="button"
                        className="btn btn-outline-danger"
                        onClick={() => handleDelete(category)}
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
  );

  const treeData = useMemo(() => {
    if (viewMode === 'tree') {
      return buildCategoryTree(filteredCategories);
    }
    return filteredCategories;
  }, [viewMode, filteredCategories, buildCategoryTree]);

  return (
    <div className="card">
      <div className="card-header border-bottom">
        <div className="row align-items-center">
          <div className="col">
            <h4 className="card-title mb-0">
              <Icon icon="mdi:folder-multiple" className="me-2" />
              Category Management
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
                placeholder="Search categories..."
                value={searchTerm}
                onChange={(e) => handleSearch(e.target.value)}
              />
            </div>
          </div>
          <div className="col-lg-8">
            <div className="row g-2">
              <div className="col-md-2">
                <select
                  className="form-select"
                  value={filters.hasChildren}
                  onChange={(e) => handleFilterChange('hasChildren', e.target.value)}
                >
                  <option value="">All</option>
                  <option value="true">With Children</option>
                  <option value="false">No Children</option>
                </select>
              </div>
              <div className="col-md-2">
                <select
                  className="form-select"
                  value={filters.hasMaterials}
                  onChange={(e) => handleFilterChange('hasMaterials', e.target.value)}
                >
                  <option value="">All</option>
                  <option value="true">With Materials</option>
                  <option value="false">Empty</option>
                </select>
              </div>
              <div className="col-md-2">
                <button
                  type="button"
                  className="btn btn-outline-secondary w-100"
                  onClick={clearFilters}
                >
                  <Icon icon="mdi:filter-off" className="me-1" />
                  Clear
                </button>
              </div>
              <div className="col-md-6">
                <div className="btn-group w-100" role="group">
                  <button
                    type="button"
                    className={`btn ${viewMode === 'tree' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setViewMode('tree')}
                  >
                    <Icon icon="mdi:file-tree" className="me-1" />
                    Tree
                  </button>
                  <button
                    type="button"
                    className={`btn ${viewMode === 'table' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setViewMode('table')}
                  >
                    <Icon icon="mdi:table" className="me-1" />
                    Table
                  </button>
                </div>
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
                    onClick={() => handleCreate()}
                  >
                    <Icon icon="mdi:plus" className="me-1" />
                    Add Category
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

        {/* Content */}
        {!loading && (
          <>
            {treeData.length > 0 ? (
              <div className="border rounded">
                {viewMode === 'tree' ? (
                  <div>
                    {treeData.map(node => renderTreeNode(node))}
                  </div>
                ) : (
                  renderTableView()
                )}
              </div>
            ) : (
              <div className="text-center py-5">
                <Icon icon="mdi:folder-outline" size={64} className="text-muted mb-3" />
                <h5 className="text-muted">No categories found</h5>
                <p className="text-muted mb-3">
                  {searchTerm || Object.values(filters).some(f => f) 
                    ? "Try adjusting your search or filters."
                    : "Get started by adding your first category."
                  }
                </p>
                {!showDeleted && (
                  <button
                    type="button"
                    className="btn btn-primary"
                    onClick={() => handleCreate()}
                  >
                    <Icon icon="mdi:plus" className="me-1" />
                    Add Category
                  </button>
                )}
              </div>
            )}
          </>
        )}
      </div>

      {/* Modal */}
      {showModal && (
        <MaterialCategoryModal
          isOpen={showModal}
          onClose={handleModalClose}
          onSuccess={handleModalSuccess}
          editingItem={editingCategory}
          categories={categories}
        />
      )}
    </div>
  );
};

export default MaterialCategoryEnhanced; 