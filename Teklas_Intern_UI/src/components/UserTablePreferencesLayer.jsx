import React, { useState, useEffect } from "react";
import axios from "axios";

const MODULES = [
  { key: "MaterialManagement", label: "Malzeme Yönetimi" },
  { key: "PurchaseOrders", label: "Satın Alma Siparişleri" },
  { key: "SupplierTypes", label: "Tedarikçi Tipleri" },
  { key: "CustomerOrders", label: "Müşteri Siparişleri" },
  // Add more modules as needed
];


const MODULE_COLUMNS = {
  // Material Management
  MaterialCard: [
    { key: "code", label: "Kod" },
    { key: "name", label: "Ad" },
    { key: "materialType", label: "Tip" },
    { key: "unitOfMeasure", label: "Birim" },
    { key: "barcode", label: "Barkod" },
    { key: "brand", label: "Marka" },
    { key: "model", label: "Model" },
    { key: "purchasePrice", label: "Alış Fiyatı" },
    { key: "salesPrice", label: "Satış Fiyatı" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  MaterialMovement: [
    { key: "materialCardName", label: "Malzeme" },
    { key: "movementType", label: "Hareket Tipi" },
    { key: "quantity", label: "Miktar" },
    { key: "movementDate", label: "Tarih" },
    { key: "referenceNumber", label: "Referans No" },
    { key: "locationFrom", label: "Lokasyon (Çıkış)" },
    { key: "locationTo", label: "Lokasyon (Varış)" },
    { key: "description", label: "Açıklama" }
  ],
  MaterialCategory: [
    { key: "id", label: "ID" },
    { key: "code", label: "Kod" },
    { key: "name", label: "Ad" },
    { key: "description", label: "Açıklama" },
    { key: "parentCategoryName", label: "Üst Kategori" },
    { key: "status", label: "Durum" },
    { key: "createDate", label: "Oluşturulma" }
  ],

  // Purchasing Management
  PurchaseOrder: [
    { key: "orderNumber", label: "Sipariş No" },
    { key: "supplierName", label: "Tedarikçi" },
    { key: "orderDate", label: "Tarih" },
    { key: "status", label: "Durum" },
    { key: "totalAmount", label: "Tutar" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  Supplier: [
    { key: "name", label: "Ad" },
    { key: "address", label: "Adres" },
    { key: "phone", label: "Telefon" },
    { key: "email", label: "E-posta" },
    { key: "taxNumber", label: "Vergi No" },
    { key: "contactPerson", label: "İletişim Kişisi" },
    { key: "supplierTypeName", label: "Tedarikçi Tipi" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  SupplierType: [
    { key: "name", label: "Tip Adı" },
    { key: "description", label: "Açıklama" },
    { key: "isActive", label: "Aktif mi?" }
  ],

  // Sales Management
  CustomerOrder: [
    { key: "orderNumber", label: "Sipariş No" },
    { key: "customerName", label: "Müşteri" },
    { key: "orderDate", label: "Tarih" },
    { key: "status", label: "Durum" },
    { key: "totalAmount", label: "Tutar" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  Customer: [
    { key: "name", label: "Ad" },
    { key: "address", label: "Adres" },
    { key: "phone", label: "Telefon" },
    { key: "email", label: "E-posta" },
    { key: "taxNumber", label: "Vergi No" },
    { key: "contactPerson", label: "İletişim Kişisi" },
    { key: "isActive", label: "Aktif mi?" }
  ],

  // Warehouse Management
  StockEntry: [
    { key: "entryNumber", label: "Giriş No" },
    { key: "entryDate", label: "Tarih" },
    { key: "warehouseName", label: "Depo" },
    { key: "locationName", label: "Lokasyon" },
    { key: "materialName", label: "Malzeme" },
    { key: "entryType", label: "Tip" },
    { key: "quantity", label: "Miktar" },
    { key: "unitPrice", label: "Birim Fiyat" },
    { key: "totalValue", label: "Toplam" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  Warehouse: [
    { key: "code", label: "Kod" },
    { key: "name", label: "Ad" },
    { key: "description", label: "Açıklama" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  Location: [
    { key: "code", label: "Kod" },
    { key: "name", label: "Ad" },
    { key: "warehouseName", label: "Depo" },
    { key: "locationType", label: "Tip" },
    { key: "capacity", label: "Kapasite" },
    { key: "isActive", label: "Aktif mi?" }
  ],

  // Production Management
  WorkOrder: [
    { key: "bomName", label: "BOM Adı" },
    { key: "materialCardName", label: "Ürün" },
    { key: "plannedQuantity", label: "Planlanan Miktar" },
    { key: "plannedStartDate", label: "Başlangıç Tarihi" },
    { key: "status", label: "Durum" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  BillOfMaterial: [
    { key: "version", label: "BOM Kodu" },
    { key: "notes", label: "Açıklama" },
    { key: "parentMaterialCardName", label: "Ürün" },
    { key: "status", label: "Durum" },
    { key: "isActive", label: "Aktif mi?" }
  ],
  ProductionConfirmation: [
    { key: "confirmationId", label: "Teyit ID" },
    { key: "workOrderId", label: "İş Emri ID" },
    { key: "confirmationDate", label: "Teyit Tarihi" },
    { key: "quantityProduced", label: "Üretilen Miktar" },
    { key: "performedBy", label: "Yapan Kişi" }
  ]
};

const BASE_URL = "https://localhost:7178";
const USER_ID = 1; // Replace with real user id from auth context

const UserTablePreferencesLayer = () => {
  const [selectedModule, setSelectedModule] = useState(MODULES[0].key);
  const [columns, setColumns] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const token = localStorage.getItem('token');

  useEffect(() => {
    // Load preferences for selected module
    setLoading(true);
    setError("");
    axios
      .get(`${BASE_URL}/api/user-table-preferences`, {
        params: { userId: USER_ID, moduleName: selectedModule },
      })
      .then((res) => {
        const pref = res.data;
        if (pref && pref.columnsJson) {
          const config = JSON.parse(pref.columnsJson);
          setColumns(
            MODULE_COLUMNS[selectedModule].map((col) => ({
              ...col,
              visible:
                config.columns.find((c) => c.key === col.key)?.visible ?? true,
            }))
          );
        } else {
          setColumns(
            MODULE_COLUMNS[selectedModule].map((col) => ({ ...col, visible: true }))
          );
        }
        setLoading(false);
      })
      .catch(() => {
        setColumns(
          MODULE_COLUMNS[selectedModule].map((col) => ({ ...col, visible: true }))
        );
        setLoading(false);
      });
  }, [selectedModule]);

  const handleToggle = (key) => {
    setColumns((cols) =>
      cols.map((col) =>
        col.key === key ? { ...col, visible: !col.visible } : col
      )
    );
  };

  const handleSave = () => {
    setLoading(true);
    setError("");
    const columnsConfig = { columns: columns.map(({ key, visible }) => ({ key, visible })) };
    axios
      .post(`${BASE_URL}/api/user-table-column-preferences`, {
        tableKey: selectedModule, // or whatever variable holds the table key
        columnsJson: JSON.stringify(columnsConfig) // or your columns structure
      }, {
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}` // ensure token is set
        }
      })
      .then(() => {
        setLoading(false);
        alert("Tercihler kaydedildi!");
      })
      .catch(() => {
        setLoading(false);
        setError("Tercihler kaydedilemedi!");
      });
  };

  return (
    <div className="card">
      <div className="card-header">
        <h5>Kullanıcı Tablo Kolon Tercihleri</h5>
      </div>
      <div className="card-body">
        <div className="mb-3">
          <label>Modül Seçin:</label>
          <select
            className="form-select"
            value={selectedModule}
            onChange={(e) => setSelectedModule(e.target.value)}
          >
            {MODULES.map((mod) => (
              <option key={mod.key} value={mod.key}>
                {mod.label}
              </option>
            ))}
          </select>
        </div>
        {loading ? (
          <div>Yükleniyor...</div>
        ) : (
          <table className="table">
            <thead>
              <tr>
                <th>Kolon</th>
                <th>Görünür</th>
              </tr>
            </thead>
            <tbody>
              {columns.map((col) => (
                <tr key={col.key}>
                  <td>{col.label}</td>
                  <td>
                    <input
                      type="checkbox"
                      checked={col.visible}
                      onChange={() => handleToggle(col.key)}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
        {error && <div className="text-danger">{error}</div>}
        <button className="btn btn-primary" onClick={handleSave} disabled={loading}>
          Kaydet
        </button>
      </div>
    </div>
  );
};

export default UserTablePreferencesLayer; 