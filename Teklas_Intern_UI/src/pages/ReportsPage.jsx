import React, { useState, useEffect, useRef } from 'react';
import MasterLayout from '../masterLayout/MasterLayout';
// Removed unused widgets
// import StockReportOne from '../components/child/StockReportOne';
// import RevenueReportOne from '../components/child/RevenueReportOne';
// import OverallReport from '../components/child/OverallReport';
import axios from 'axios';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

const ALL_REPORTS = [
  { key: 'sales', label: 'Satış Raporları' },
  { key: 'purchases', label: 'Satın Alma Raporları' },
  { key: 'inventory', label: 'Stok Raporları' },
  { key: 'production', label: 'Üretim Raporları' },
  { key: 'financial', label: 'Finansal Raporlar' },
  { key: 'activity', label: 'Son Aktiviteler' },
];

const DEFAULT_REPORT = 'sales';
const BASE_URL = 'https://localhost:7178';

const ReportsPage = () => {
  const [selectedReport, setSelectedReport] = useState(DEFAULT_REPORT);
  // Data states for each report
  const [salesData, setSalesData] = useState([]);
  const [purchasesData, setPurchasesData] = useState([]);
  const [inventoryData, setInventoryData] = useState([]);
  const [productionData, setProductionData] = useState([]);
  const [financialData, setFinancialData] = useState([]);
  const [activityData, setActivityData] = useState([]);
  // Loading and error states
  const [salesLoading, setSalesLoading] = useState(false);
  const [salesError, setSalesError] = useState(null);
  const [purchasesLoading, setPurchasesLoading] = useState(false);
  const [purchasesError, setPurchasesError] = useState(null);
  const [inventoryLoading, setInventoryLoading] = useState(false);
  const [inventoryError, setInventoryError] = useState(null);
  const [productionLoading, setProductionLoading] = useState(false);
  const [productionError, setProductionError] = useState(null);
  const [financialLoading, setFinancialLoading] = useState(false);
  const [financialError, setFinancialError] = useState(null);
  const [activityLoading, setActivityLoading] = useState(false);
  const [activityError, setActivityError] = useState(null);
  // Refs for print/export
  const salesRef = useRef();
  const purchasesRef = useRef();
  const inventoryRef = useRef();
  const productionRef = useRef();
  const financialRef = useRef();
  const activityRef = useRef();

  // Fetch data for each report when selected
  useEffect(() => {
    if (selectedReport === 'sales') {
      setSalesLoading(true);
      setSalesError(null);
      axios.get(`${BASE_URL}/api/customerorder`)
        .then(res => setSalesData(res.data))
        .catch(() => setSalesError('Satış verileri alınamadı!'))
        .finally(() => setSalesLoading(false));
    }
    if (selectedReport === 'purchases') {
      setPurchasesLoading(true);
      setPurchasesError(null);
      axios.get(`${BASE_URL}/api/purchaseorder`)
        .then(res => setPurchasesData(res.data))
        .catch(() => setPurchasesError('Satın alma verileri alınamadı!'))
        .finally(() => setPurchasesLoading(false));
    }
    if (selectedReport === 'inventory') {
      setInventoryLoading(true);
      setInventoryError(null);
      axios.get(`${BASE_URL}/api/materials`)
        .then(res => setInventoryData(res.data))
        .catch(() => setInventoryError('Stok verileri alınamadı!'))
        .finally(() => setInventoryLoading(false));
    }
    if (selectedReport === 'production') {
      setProductionLoading(true);
      setProductionError(null);
      axios.get(`${BASE_URL}/api/workorder`)
        .then(res => setProductionData(res.data))
        .catch(() => setProductionError('Üretim verileri alınamadı!'))
        .finally(() => setProductionLoading(false));
    }
    if (selectedReport === 'financial') {
      setFinancialLoading(true);
      setFinancialError(null);
      axios.get(`${BASE_URL}/api/invoice`)
        .then(res => setFinancialData(res.data))
        .catch(() => setFinancialError('Finansal veriler alınamadı!'))
        .finally(() => setFinancialLoading(false));
    }
    if (selectedReport === 'activity') {
      setActivityLoading(true);
      setActivityError(null);
      axios.get(`${BASE_URL}/api/activity`)
        .then(res => setActivityData(res.data))
        .catch(() => setActivityError('Aktivite verileri alınamadı!'))
        .finally(() => setActivityLoading(false));
    }
  }, [selectedReport]);

  // Export CSV utility
  const exportCSV = (data, filename) => {
    if (!data.length) return;
    const keys = Object.keys(data[0]);
    const csvRows = [keys.join(',')];
    data.forEach(row => {
      csvRows.push(keys.map(k => JSON.stringify(row[k] ?? '')).join(','));
    });
    const blob = new Blob([csvRows.join('\n')], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
  };

  // Export PDF utility with robust image loading and fallback
  const exportPDF = (ref, filename) => {
    const doc = new jsPDF('p', 'pt', 'a4');
    const img = new window.Image();
    img.src = '/assets/logo.png';

    const generate = () => {
      try {
        doc.addImage(img, 'PNG', 40, 30, 80, 40);
      } catch (e) {
        // If image fails, skip logo
      }
      doc.setFontSize(22);
      doc.setTextColor(22, 160, 133);
      doc.text('TEKLAS ERP RAPORU', 140, 55);
      doc.setFontSize(14);
      doc.setTextColor(40, 40, 40);
      doc.text(filename.replace('_', ' ').toUpperCase(), 140, 75);
      doc.setFontSize(12);
      doc.setTextColor(100, 100, 100);
      doc.text('Tarih: ' + new Date().toLocaleDateString('tr-TR'), 140, 95);

      autoTable(doc, {
        html: ref.current.querySelector('table'),
        startY: 120,
        headStyles: { fillColor: [22, 160, 133], textColor: 255, fontSize: 12 },
        bodyStyles: { fontSize: 10 },
        alternateRowStyles: { fillColor: [240, 240, 240] },
        margin: { left: 40, right: 40 },
        didDrawPage: (data) => {
          doc.setFontSize(10);
          doc.setTextColor(150);
          doc.text('Sayfa ' + doc.internal.getNumberOfPages(), 500, 820);
          doc.text('Generated by Teklas ERP', 40, 820);
        }
      });

      doc.save(filename + '.pdf');
    };

    img.onload = generate;
    img.onerror = generate;
    if (img.complete) {
      generate();
    }
  };

  // Print utility with ERP-style header (logo, title, subtitle, date)
  const printSection = (ref, filename) => {
    const printContents = ref.current.innerHTML;
    const win = window.open('', '', 'height=700,width=900');
    win.document.write('<html><head><title>Yazdır</title>');
    win.document.write('<style>@media print { .no-print { display: none; } .print-header { display: block; margin-bottom: 24px; } body { background: #fff; color: #222; } .erp-title { color: #16a085; font-size: 22px; font-weight: bold; } .erp-subtitle { font-size: 15px; color: #222; font-weight: bold; } .erp-date { font-size: 12px; color: #666; } }</style>');
    win.document.write('</head><body >');
    win.document.write('<div class="print-header" style="display:block;margin-bottom:24px;">');
    win.document.write('<img src="/assets/logo.png" alt="Logo" style="height:40px;vertical-align:middle;margin-right:16px;float:left;" />');
    win.document.write('<div style="margin-left:100px;">');
    win.document.write('<div class="erp-title">TEKLAS ERP RAPORU</div>');
    win.document.write('<div class="erp-subtitle">' + filename.replace('_', ' ').toUpperCase() + '</div>');
    win.document.write('<div class="erp-date">Tarih: ' + new Date().toLocaleDateString('tr-TR') + '</div>');
    win.document.write('</div>');
    win.document.write('</div>');
    win.document.write(printContents);
    win.document.write('</body></html>');
    win.document.close();
    win.print();
  };

  // Dropdown for report selection
  const ReportDropdown = () => (
    <div className="mb-4">
      <label className="form-label fw-bold">Rapor Seçiniz</label>
      <select
        className="form-select"
        value={selectedReport}
        onChange={e => setSelectedReport(e.target.value)}
        style={{ maxWidth: 320 }}
      >
        {ALL_REPORTS.map(r => (
          <option key={r.key} value={r.key}>{r.label}</option>
        ))}
      </select>
    </div>
  );

  return (
    <MasterLayout>
      <div className="container-fluid py-4">
        <h2 className="mb-4">ERP Raporları</h2>
        <ReportDropdown />
        <div className="row g-4">
          <div className="col-12">
            {/* Sales Report */}
            {selectedReport === 'sales' && (
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">Satış Raporları</h5>
                  <div>
                    <button className="btn btn-outline-secondary me-2" onClick={() => printSection(salesRef, 'satis_raporu')}>Yazdır</button>
                    <button className="btn btn-outline-success me-2" onClick={() => exportCSV(salesData, 'satis_raporu.csv')}>CSV</button>
                    <button className="btn btn-outline-danger" onClick={() => exportPDF(salesRef, 'satis_raporu')}>PDF</button>
                  </div>
                </div>
                <div className="card-body" ref={salesRef}>
                  {salesLoading && <div>Yükleniyor...</div>}
                  {salesError && <div className="text-danger">{salesError}</div>}
                  {!salesLoading && !salesError && (
                    <>
                      {/* Removed <RevenueReportOne data={salesData} /> */}
                      <div className="table-responsive mt-4">
                        <table className="table table-bordered table-sm">
                          <thead>
                            <tr>
                              <th>Sipariş No</th>
                              <th>Müşteri</th>
                              <th>Tarih</th>
                              <th>Durum</th>
                              <th>Tutar</th>
                            </tr>
                          </thead>
                          <tbody>
                            {salesData.map((row, i) => (
                              <tr key={i}>
                                <td>{row.orderNumber}</td>
                                <td>{row.customerName}</td>
                                <td>{row.orderDate ? new Date(row.orderDate).toLocaleDateString('tr-TR') : ''}</td>
                                <td>{row.status}</td>
                                <td>{row.totalAmount}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </>
                  )}
                </div>
              </div>
            )}
            {/* Purchases Report */}
            {selectedReport === 'purchases' && (
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">Satın Alma Raporları</h5>
                  <div>
                    <button className="btn btn-outline-secondary me-2" onClick={() => printSection(purchasesRef, 'satinalma_raporu')}>Yazdır</button>
                    <button className="btn btn-outline-success me-2" onClick={() => exportCSV(purchasesData, 'satinalma_raporu.csv')}>CSV</button>
                    <button className="btn btn-outline-danger" onClick={() => exportPDF(purchasesRef, 'satinalma_raporu')}>PDF</button>
                  </div>
                </div>
                <div className="card-body" ref={purchasesRef}>
                  {purchasesLoading && <div>Yükleniyor...</div>}
                  {purchasesError && <div className="text-danger">{purchasesError}</div>}
                  {!purchasesLoading && !purchasesError && (
                    <>
                      {/* Removed <OverallReport data={purchasesData} /> */}
                      <div className="table-responsive mt-4">
                        <table className="table table-bordered table-sm">
                          <thead>
                            <tr>
                              <th>Sipariş No</th>
                              <th>Tedarikçi</th>
                              <th>Tarih</th>
                              <th>Durum</th>
                              <th>Tutar</th>
                            </tr>
                          </thead>
                          <tbody>
                            {purchasesData.map((row, i) => (
                              <tr key={i}>
                                <td>{row.orderNumber}</td>
                                <td>{row.supplierName}</td>
                                <td>{row.orderDate ? new Date(row.orderDate).toLocaleDateString('tr-TR') : ''}</td>
                                <td>{row.status}</td>
                                <td>{row.totalAmount}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </>
                  )}
                </div>
              </div>
            )}
            {/* Inventory Report */}
            {selectedReport === 'inventory' && (
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">Stok Raporları</h5>
                  <div>
                    <button className="btn btn-outline-secondary me-2" onClick={() => printSection(inventoryRef, 'stok_raporu')}>Yazdır</button>
                    <button className="btn btn-outline-success me-2" onClick={() => exportCSV(inventoryData, 'stok_raporu.csv')}>CSV</button>
                    <button className="btn btn-outline-danger" onClick={() => exportPDF(inventoryRef, 'stok_raporu')}>PDF</button>
                  </div>
                </div>
                <div className="card-body" ref={inventoryRef}>
                  {inventoryLoading && <div>Yükleniyor...</div>}
                  {inventoryError && <div className="text-danger">{inventoryError}</div>}
                  {!inventoryLoading && !inventoryError && inventoryData.length === 0 && (
                    <div>Veri bulunamadı.</div>
                  )}
                  {!inventoryLoading && !inventoryError && inventoryData.length > 0 && (
                    <>
                      <div className="table-responsive mt-4">
                        <table className="table table-bordered table-sm">
                          <thead>
                            <tr>
                              <th>Malzeme Kodu</th>
                              <th>Ad</th>
                              <th>Kategori</th>
                              <th>Birim</th>
                              <th>Stok</th>
                            </tr>
                          </thead>
                          <tbody>
                            {inventoryData.map((row, i) => (
                              <tr key={i}>
                                <td>{row.code}</td>
                                <td>{row.name}</td>
                                <td>{row.categoryName}</td>
                                <td>{row.unitOfMeasure}</td>
                                <td>{row.stockLevel}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </>
                  )}
                </div>
              </div>
            )}
            {/* Production Report */}
            {selectedReport === 'production' && (
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">Üretim Raporları</h5>
                  <div>
                    <button className="btn btn-outline-secondary me-2" onClick={() => printSection(productionRef, 'uretim_raporu')}>Yazdır</button>
                    <button className="btn btn-outline-success me-2" onClick={() => exportCSV(productionData, 'uretim_raporu.csv')}>CSV</button>
                    <button className="btn btn-outline-danger" onClick={() => exportPDF(productionRef, 'uretim_raporu')}>PDF</button>
                  </div>
                </div>
                <div className="card-body" ref={productionRef}>
                  {productionLoading && <div>Yükleniyor...</div>}
                  {productionError && <div className="text-danger">{productionError}</div>}
                  {!productionLoading && !productionError && productionData.length === 0 && (
                    <div>Veri bulunamadı.</div>
                  )}
                  {!productionLoading && !productionError && productionData.length > 0 && (
                    <div className="table-responsive mt-4">
                      <table className="table table-bordered table-sm">
                        <thead>
                          <tr>
                            <th>İş Emri No</th>
                            <th>Ürün</th>
                            <th>Başlangıç</th>
                            <th>Bitiş</th>
                            <th>Durum</th>
                          </tr>
                        </thead>
                        <tbody>
                          {productionData.map((row, i) => (
                            <tr key={i}>
                              <td>{row.workOrderNumber}</td>
                              <td>{row.productName}</td>
                              <td>{row.startDate ? new Date(row.startDate).toLocaleDateString('tr-TR') : ''}</td>
                              <td>{row.endDate ? new Date(row.endDate).toLocaleDateString('tr-TR') : ''}</td>
                              <td>{row.status}</td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  )}
                </div>
              </div>
            )}
            {/* Financial Report */}
            {selectedReport === 'financial' && (
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">Finansal Raporlar</h5>
                  <div>
                    <button className="btn btn-outline-secondary me-2" onClick={() => printSection(financialRef, 'finansal_rapor')}>Yazdır</button>
                    <button className="btn btn-outline-success me-2" onClick={() => exportCSV(financialData, 'finansal_rapor.csv')}>CSV</button>
                    <button className="btn btn-outline-danger" onClick={() => exportPDF(financialRef, 'finansal_rapor')}>PDF</button>
                  </div>
                </div>
                <div className="card-body" ref={financialRef}>
                  {financialLoading && <div>Yükleniyor...</div>}
                  {financialError && <div className="text-danger">{financialError}</div>}
                  {!financialLoading && !financialError && financialData.length === 0 && (
                    <div>Veri bulunamadı.</div>
                  )}
                  {!financialLoading && !financialError && financialData.length > 0 && (
                    <div className="table-responsive mt-4">
                      <table className="table table-bordered table-sm">
                        <thead>
                          <tr>
                            <th>Fatura No</th>
                            <th>Müşteri/Tedarikçi</th>
                            <th>Tarih</th>
                            <th>Tutar</th>
                            <th>Durum</th>
                          </tr>
                        </thead>
                        <tbody>
                          {financialData.map((row, i) => (
                            <tr key={i}>
                              <td>{row.invoiceNumber}</td>
                              <td>{row.partnerName}</td>
                              <td>{row.date ? new Date(row.date).toLocaleDateString('tr-TR') : ''}</td>
                              <td>{row.amount}</td>
                              <td>{row.status}</td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  )}
                </div>
              </div>
            )}
            {/* Activity Report */}
            {selectedReport === 'activity' && (
              <div className="card h-100">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">Son Aktiviteler</h5>
                  <div>
                    <button className="btn btn-outline-secondary me-2" onClick={() => printSection(activityRef, 'aktivite_raporu')}>Yazdır</button>
                    <button className="btn btn-outline-success me-2" onClick={() => exportCSV(activityData, 'aktivite_raporu.csv')}>CSV</button>
                    <button className="btn btn-outline-danger" onClick={() => exportPDF(activityRef, 'aktivite_raporu')}>PDF</button>
                  </div>
                </div>
                <div className="card-body" ref={activityRef}>
                  {activityLoading && <div>Yükleniyor...</div>}
                  {activityError && <div className="text-danger">{activityError}</div>}
                  {!activityLoading && !activityError && (
                    <>
                      <div>Son işlemler ve kullanıcı aktiviteleri buraya gelecek.</div>
                      <div className="table-responsive mt-4">
                        <table className="table table-bordered table-sm">
                          <thead>
                            <tr>
                              <th>Tarih</th>
                              <th>Kullanıcı</th>
                              <th>Modül</th>
                              <th>İşlem</th>
                            </tr>
                          </thead>
                          <tbody>
                            {activityData.map((row, i) => (
                              <tr key={i}>
                                <td>{row.date ? new Date(row.date).toLocaleDateString('tr-TR') : ''}</td>
                                <td>{row.userName}</td>
                                <td>{row.module}</td>
                                <td>{row.action}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </>
                  )}
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </MasterLayout>
  );
};

export default ReportsPage; 